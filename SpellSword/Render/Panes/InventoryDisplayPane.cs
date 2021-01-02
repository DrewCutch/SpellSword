using System;
using System.Collections.Generic;
using System.Text;
using GoRogue;
using SpellSword.RPG;
using SpellSword.Util;

namespace SpellSword.Render.Panes
{
    class InventoryDisplayPane: Pane
    {
        public EquipmentSlotSet EquipmentSlotSet { get; }

        private bool _dirty;

        public InventoryDisplayPane(EquipmentSlotSet equipmentSlotSet)
        {
            EquipmentSlotSet = equipmentSlotSet;
            _dirty = true;

            EquipmentSlotSet.OnEquipped += OnEquipped;
        }

        private void OnEquipped(EquipmentSlot arg1, IEquippable arg2)
        {
            _dirty = true;
        }

        public override bool Paint(IWriteable writeContext)
        {
            if (!_dirty)
                return false;

            Coord nextMessage = new Coord(0,0);
            writeContext.Clear(Layer.Main);

            foreach (EquipmentSlot equipmentSlot in EquipmentSlotSet.Slots)
            {
                Coord lineEnd = StringPrinter.Print($"{equipmentSlot.Name}: {EquipmentSlotSet.Equipped.GetValueOrDefault(equipmentSlot)?.Name ?? "Nothing"}", writeContext, nextMessage.X, nextMessage.Y); 

                nextMessage = new Coord(0, lineEnd.Y + 1);
            }

            _dirty = false;

            return true;
        }
    }
}
