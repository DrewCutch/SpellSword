using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using BearLib;
using GoRogue;
using SpellSword.Actors;
using SpellSword.Input;
using SpellSword.RPG;
using SpellSword.RPG.Items;
using SpellSword.Util;

namespace SpellSword.Render.Panes
{
    class InventoryDisplayPane: Pane
    {
        public EquipmentSlotSet EquipmentSlotSet { get; }
        public Inventory Inventory { get; }

        private readonly JoystickConfig _joystickConfig;

        private int _inventoryIndex;
        private int _equipmentIndex;

        public InventoryDisplayPane(EquipmentSlotSet equipmentSlotSet, Inventory inventory, JoystickConfig joystickConfig)
        {
            EquipmentSlotSet = equipmentSlotSet;
            Inventory = inventory;

            _joystickConfig = joystickConfig;
            _equipmentIndex = -1;

            Dirty = true;

            EquipmentSlotSet.OnEquipped += OnEquipped;
            Inventory.OnAdd += InventoryOnAdd;
            Inventory.OnRemove += InventoryOnRemove;

            _inventoryIndex = 0;
        }

        private void InventoryOnAdd(Item obj)
        {
            Dirty = true;
        }

        private void InventoryOnRemove(Item obj)
        {
            Dirty = true;
        }

        private void OnEquipped(EquipmentSlot arg1, Item arg2)
        {
            Dirty = true;
        }

        public override bool Paint(IWriteable writeContext)
        {
            if (!Dirty)
                return false;

            Coord nextMessage = new Coord(0,0);
            writeContext.Clear();

            Coord equippedLineEnd = StringPrinter.Print("Equipped:", writeContext, nextMessage.X, nextMessage.Y);
            nextMessage = new Coord(0, equippedLineEnd.Y + 1);

            foreach ((EquipmentSlot equipmentSlot, int i) in EquipmentSlotSet.Slots.WithIndex())
            {
                bool slotSelected = HasFocus && _equipmentIndex == i;
                Color textColor = slotSelected ? Color.Black : Color.White;
                Color backgroundColor = slotSelected ? Color.White : Color.Black;

                Coord lineEnd = StringPrinter.Print($"{equipmentSlot.Name}: {EquipmentSlotSet.Equipped.GetValueOrDefault(equipmentSlot)?.Title.Name ?? "Nothing"}", writeContext, textColor, backgroundColor, nextMessage.X, nextMessage.Y); 

                nextMessage = new Coord(0, lineEnd.Y + 1);
            }

            Coord inventoryLineEnd = StringPrinter.Print("Inventory:", writeContext, nextMessage.X, nextMessage.Y);
            nextMessage = new Coord(0, inventoryLineEnd.Y + 1);

            foreach ((Item item, int i) in Inventory.Items.Keys.WithIndex())
            {
                bool itemSelected = HasFocus && i == _inventoryIndex;
                Color textColor = itemSelected ? Color.Black : Color.White;
                Color backgroundColor = itemSelected ? Color.White : Color.Black;

                Coord lineEnd = StringPrinter.Print($"{item.Title.Name} (x{Inventory.Items[item]})", writeContext, textColor, backgroundColor, nextMessage.X, nextMessage.Y);

                nextMessage = new Coord(0, lineEnd.Y + 1);
            }

            Dirty = false;

            return true;
        }

        public override void OnKeyDown(int keyCode)
        {
            if (keyCode == _joystickConfig.DownCode)
            {
                if(_equipmentIndex != -1)
                    IncrementEquipmentIndex();
                else
                    _inventoryIndex = Math.Min(_inventoryIndex + 1, Inventory.Items.Count - 1);
            }

            if (keyCode == _joystickConfig.UpCode)
            {
                if (_equipmentIndex != -1)
                    DecrementEquipmentIndex();
                else
                    _inventoryIndex = Math.Max(_inventoryIndex - 1, 0);
            }

            if (keyCode == Terminal.TK_E)
            {
                if (_equipmentIndex != -1)
                    Equip();
                else
                    StartEquipping();
            }

            Dirty = true;
        }

        private void StartEquipping()
        {
            if (!Inventory.Items.Keys.AtIndex(_inventoryIndex).CanBeEquipped)
                return;

            _equipmentIndex = -1;
            IncrementEquipmentIndex();

            Dirty = true;
        }

        private void Equip()
        {
            Item itemToEquip = Inventory.Items.Keys.AtIndex(_inventoryIndex);
            Inventory.Remove(itemToEquip);

            EquipmentSlot slot = EquipmentSlotSet.Slots[_equipmentIndex];

            Item replacedItem = EquipmentSlotSet.Equip(itemToEquip, slot);

            if(replacedItem != null)
                Inventory.Add(replacedItem);

            _equipmentIndex = -1;
        }

        private void IncrementEquipmentIndex()
        {
            EquipmentSlotKind slotKind = Inventory.Items.Keys.AtIndex(_inventoryIndex).SlotKind;
            int i = (_equipmentIndex + 1) % EquipmentSlotSet.Slots.Count;

            while (i != _equipmentIndex)
            {
                if (EquipmentSlotSet.Slots[i].Kind == slotKind)
                    break;

                i = (i + 1) % EquipmentSlotSet.Slots.Count;
            }

            _equipmentIndex = i;
        }

        private void DecrementEquipmentIndex()
        {
            EquipmentSlotKind slotKind = Inventory.Items.Keys.AtIndex(_inventoryIndex).SlotKind;
            int i = _equipmentIndex == 0 ? EquipmentSlotSet.Slots.Count - 1 : _equipmentIndex - 1;

            while (i != _equipmentIndex)
            {
                if (EquipmentSlotSet.Slots[i].Kind == slotKind)
                    break;

                i = i == 0 ? EquipmentSlotSet.Slots.Count - 1 : i - 1;
            }

            _equipmentIndex = i;
        }

        public override IInputListener Focus(Coord pos)
        {
            IInputListener baseFocus = base.Focus(pos);

            _equipmentIndex = -1;
            _inventoryIndex = 0;

            Dirty = true;
            return baseFocus;
        }

        public override void LoseFocus()
        {
            base.LoseFocus();
            Dirty = true;
        }
    }
}
