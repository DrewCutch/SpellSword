using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using BearLib;
using GoRogue;
using GoRogue.Messaging;
using SpellSword.Actors;
using SpellSword.Engine;
using SpellSword.Input;
using SpellSword.RPG;
using SpellSword.RPG.Items;
using SpellSword.Util;

namespace SpellSword.Render.Panes
{
    class InventoryDisplayPane: Pane
    {
        private readonly StackPane _stackPane;
        private readonly EquipmentSlotPane _equipmentSlotPane;
        private readonly InventoryPane _inventoryPane;
        private readonly MessageBus _mainBus;

        private readonly JoystickConfig _joystickConfig;

        public override bool Dirty
        {
            get => _stackPane.Dirty;
            protected set { }
        }


        public InventoryDisplayPane(EquipmentSlotSet equipmentSlotSet, Inventory inventory,
            JoystickConfig joystickConfig, MessageBus mainBus)
        {
            _equipmentSlotPane = new EquipmentSlotPane(equipmentSlotSet);
            _inventoryPane = new InventoryPane(inventory);

            _stackPane = new StackPane(StackPane.StackDirection.Horizontal);
            _stackPane.AddChild(_equipmentSlotPane, 1);
            _stackPane.AddChild(_inventoryPane, 1);

            _mainBus = mainBus;

            _joystickConfig = joystickConfig;
        }


        public override void SuggestHeight(int height)
        {
            base.SuggestHeight(height);
            _stackPane.SuggestHeight(height);
        }

        public override void SuggestWidth(int width)
        {
            base.SuggestWidth(width);
            _stackPane.SuggestWidth(width);
        }

        public override bool Paint(Writeable writeContext)
        {
            return _stackPane.Paint(writeContext);
        }

        public override void OnKeyDown(int keyCode)
        {
            if (keyCode == _joystickConfig.DownCode)
            {
                if(_equipmentSlotPane.CurrentlyEquipping)
                    _equipmentSlotPane.IncrementEquipmentIndex();
                else
                    _inventoryPane.IncrementSelectedItemIndex();
                    
            }
            else if (keyCode == _joystickConfig.UpCode)
            {
                if (_equipmentSlotPane.CurrentlyEquipping)
                    _equipmentSlotPane.DecrementEquipmentIndex();
                else
                    _inventoryPane.DecrementSelectedItemIndex();
            }
            else if (keyCode == _joystickConfig.Equip)
            {
                if (_equipmentSlotPane.CurrentlyEquipping)
                    Equip();
                else
                    _equipmentSlotPane.BeginEquipping(_inventoryPane.CurrentlySelectedItem);
            }
        }

        private void Equip()
        {
            Item itemToEquip = _inventoryPane.CurrentlySelectedItem;
            _inventoryPane.Inventory.Remove(itemToEquip);

            EquipmentSlot slot = _equipmentSlotPane.GetCurrentlySelectedSlot();

            Item replacedItem = _equipmentSlotPane.EquipmentSlotSet.Equip(itemToEquip, slot);

            if (replacedItem != null)
                _inventoryPane.Inventory.Add(replacedItem);

            _equipmentSlotPane.ResetEquipping();
        }

        public override IInputListener Focus(Coord pos)
        {
            if (!HasFocus)
            {
                _inventoryPane.ResetSelectingItem();
                _equipmentSlotPane.ResetEquipping();
            }

            return base.Focus(pos);
        }

        public override void LoseFocus()
        {
            base.LoseFocus();
        }

        class EquipmentSlotPane : Pane
        {
            public EquipmentSlotSet EquipmentSlotSet { get; }

            public bool CurrentlyEquipping => _currentlyEquipping != null;

            private int _equipmentIndex;
            private Item _currentlyEquipping;

            public EquipmentSlotPane(EquipmentSlotSet equipmentSlotSet)
            {
                EquipmentSlotSet = equipmentSlotSet;
                equipmentSlotSet.OnEquipped += OnEquipped;
                equipmentSlotSet.OnUnequipped += OnUnEquipped;

                _equipmentIndex = -1;
                _currentlyEquipping = null;

                Dirty = true;
            }

            private void OnEquipped(EquipmentSlot arg1, Item arg2)
            {
                Dirty = true;
            }

            private void OnUnEquipped(EquipmentSlot arg1, Item arg2)
            {
                Dirty = true;
            }

            public override bool Paint(Writeable writeContext)
            {
                if (!Dirty)
                    return false;

                Coord nextMessage = new Coord(0, 0);
                writeContext.Clear();

                Coord equippedLineEnd = StringPrinter.Print("Equipped:", writeContext, nextMessage.X, nextMessage.Y);
                nextMessage = new Coord(0, equippedLineEnd.Y + 1);

                foreach ((EquipmentSlot equipmentSlot, int i) in EquipmentSlotSet.Slots.WithIndex())
                {
                    bool slotSelected = _equipmentIndex == i;
                    Color textColor = slotSelected ? Color.Black : Color.White;
                    Color backgroundColor = slotSelected ? Color.White : Color.Black;

                    Coord lineEnd = StringPrinter.Print($"{equipmentSlot.Name}: {EquipmentSlotSet.Equipped.GetValueOrDefault(equipmentSlot)?.Title.Name ?? "Nothing"}", writeContext, textColor, backgroundColor, nextMessage.X, nextMessage.Y);

                    nextMessage = new Coord(0, lineEnd.Y + 1);
                }

                Dirty = false;

                return true;
            }

            public void ResetEquipping()
            {
                _currentlyEquipping = null;
                _equipmentIndex = -1;

                Dirty = true;
            }

            public void BeginEquipping(Item item)
            {
                if (!item.CanBeEquipped)
                    return;

                _currentlyEquipping = item;
                IncrementEquipmentIndex();
            }

            public EquipmentSlot GetCurrentlySelectedSlot()
            {
                if (_currentlyEquipping == null)
                    return null;

                return EquipmentSlotSet.Slots[_equipmentIndex];
            }

            public void IncrementEquipmentIndex()
            {
                EquipmentSlotKind slotKind = _currentlyEquipping.SlotKind;

                int i = (_equipmentIndex + 1) % EquipmentSlotSet.Slots.Count;

                while (i != _equipmentIndex)
                {
                    if (EquipmentSlotSet.Slots[i].Kind == slotKind)
                        break;

                    i = (i + 1) % EquipmentSlotSet.Slots.Count;
                }

                _equipmentIndex = i;

                Dirty = true;
            }

            public void DecrementEquipmentIndex()
            {
                EquipmentSlotKind slotKind = _currentlyEquipping.SlotKind;
                int i = _equipmentIndex == 0 ? EquipmentSlotSet.Slots.Count - 1 : _equipmentIndex - 1;

                while (i != _equipmentIndex)
                {
                    if (EquipmentSlotSet.Slots[i].Kind == slotKind)
                        break;

                    i = i == 0 ? EquipmentSlotSet.Slots.Count - 1 : i - 1;
                }

                _equipmentIndex = i;

                Dirty = true;
            }
        }

        class InventoryPane : Pane
        {
            public Inventory Inventory { get; }

            public int SelectedItemIndex { get; protected set; }

            public Item CurrentlySelectedItem => SelectedItemIndex != -1 ? Inventory.Items.Keys.AtIndex(SelectedItemIndex) : null;

            public InventoryPane(Inventory inventory)
            {
                Inventory = inventory;
                SelectedItemIndex = -1;

                Dirty = true;
                Inventory.OnAdd += InventoryOnAdd;
                Inventory.OnRemove += InventoryOnRemove;

            }
            private void InventoryOnAdd(Item obj)
            {
                Dirty = true;
            }

            private void InventoryOnRemove(Item obj)
            {
                Dirty = true;
            }

            public void ResetSelectingItem()
            {
                SelectedItemIndex = 0;
                Dirty = true;
            }

            public void IncrementSelectedItemIndex()
            {
                SelectedItemIndex = Math.Min(SelectedItemIndex + 1, Inventory.Items.Count - 1);
                Dirty = true;
            }

            public void DecrementSelectedItemIndex()
            {
                SelectedItemIndex = Math.Max(SelectedItemIndex - 1, 0);
                Dirty = true;
            }

            public override bool Paint(Writeable writeContext)
            {
                if (!Dirty)
                    return false;

                Coord nextMessage = new Coord(0, 0);

                Coord inventoryLineEnd = StringPrinter.Print("Inventory:", writeContext, nextMessage.X, nextMessage.Y);
                nextMessage = new Coord(0, inventoryLineEnd.Y + 1);

                foreach ((Item item, int i) in Inventory.Items.Keys.WithIndex())
                {
                    bool itemSelected = i == SelectedItemIndex;
                    Color textColor = itemSelected ? Color.Black : Color.White;
                    Color backgroundColor = itemSelected ? Color.White : Color.Black;

                    Coord lineEnd = StringPrinter.Print($"{item.Title.Name} (x{Inventory.Items[item]})", writeContext, textColor, backgroundColor, nextMessage.X, nextMessage.Y);

                    nextMessage = new Coord(0, lineEnd.Y + 1);
                }

                Dirty = false;

                return true;
            }
        }
    }
}
