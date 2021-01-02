using System;
using System.Collections.Generic;
using System.Text;
using GoRogue;
using SpellSword.RPG;
using SpellSword.RPG.Items;
using SpellSword.Util;

namespace SpellSword.Render.Panes
{
    class InventoryDisplayPane: Pane
    {
        public EquipmentSlotSet EquipmentSlotSet { get; }

        public InventoryDisplayPane(EquipmentSlotSet equipmentSlotSet)
        {
            EquipmentSlotSet = equipmentSlotSet;
            Dirty = true;

            EquipmentSlotSet.OnEquipped += OnEquipped;
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

            foreach (EquipmentSlot equipmentSlot in EquipmentSlotSet.Slots)
            {
                Coord lineEnd = StringPrinter.Print($"{equipmentSlot.Name}: {EquipmentSlotSet.Equipped.GetValueOrDefault(equipmentSlot)?.Name ?? "Nothing"}", writeContext, nextMessage.X, nextMessage.Y); 

                nextMessage = new Coord(0, lineEnd.Y + 1);
            }

            Dirty = false;

            return true;
        }
    }
}
