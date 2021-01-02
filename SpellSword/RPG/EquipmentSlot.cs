using System;
using System.Collections.Generic;
using System.Text;

namespace SpellSword.RPG
{
    class EquipmentSlot
    {
        public string Name { get; }
        public EquipmentSlotKind Kind { get; }

        public static EquipmentSlot Head = new EquipmentSlot("Head", EquipmentSlotKind.Head);
        public static EquipmentSlot Chest = new EquipmentSlot("Chest", EquipmentSlotKind.Chest);
        public static EquipmentSlot Legs = new EquipmentSlot("Legs", EquipmentSlotKind.Legs);
        public static EquipmentSlot Feet = new EquipmentSlot("Feet", EquipmentSlotKind.Foot);
        public static EquipmentSlot LeftArm = new EquipmentSlot("Left Arm", EquipmentSlotKind.Arm);
        public static EquipmentSlot RightArm = new EquipmentSlot("Right Arm", EquipmentSlotKind.Arm);
        public static EquipmentSlot LeftHandEquip = new EquipmentSlot("Left Hand", EquipmentSlotKind.Hand);
        public static EquipmentSlot RightHandEquip = new EquipmentSlot("Right Hand", EquipmentSlotKind.Hand);

        public static EquipmentSlot[] AllSlots = new[]
            {Head, Chest, Legs, Feet, LeftArm, RightArm, LeftHandEquip, RightHandEquip};

        private EquipmentSlot(string name, EquipmentSlotKind kind)
        {
            Name = name;
            Kind = kind;
        }
    }
}
