using System;
using System.Collections.Generic;
using System.Text;

namespace SpellSword.RPG
{
    interface IEquippable
    {
        public string Name { get; }
        public EquipmentSlotKind SlotKind { get; }
    }
}
