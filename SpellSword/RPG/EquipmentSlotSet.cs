using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpellSword.Actors.Action;

namespace SpellSword.RPG
{
    class EquipmentSlotSet
    {
        public List<EquipmentSlot> Slots { get; }

        public HashSet<EquipmentSlot> SlotsInUse { get; private set; }

        public IReadOnlyDictionary<EquipmentSlot, IEquippable> Equipped => _equipped;

        private Dictionary<EquipmentSlot, IEquippable> _equipped;

        public event Action<EquipmentSlot, IEquippable> OnEquipped;
        public event Action<EquipmentSlot, IEquippable> OnUnequipped;

        public EquipmentSlotSet()
        {
            Slots = EquipmentSlot.AllSlots.ToList();
            SlotsInUse = new HashSet<EquipmentSlot>();
            _equipped = new Dictionary<EquipmentSlot, IEquippable>();
        }

        public EquipmentSlotSet(List<EquipmentSlot> slots)
        {
            Slots = slots;
            SlotsInUse = new HashSet<EquipmentSlot>();
            _equipped = new Dictionary<EquipmentSlot, IEquippable>();
        }

        public IUsable GetMain()
        {
            if (_equipped.ContainsKey(EquipmentSlot.RightHandEquip) &&
                _equipped[EquipmentSlot.RightHandEquip] is IUsable rightHandUsable)
                return rightHandUsable;

            if (_equipped.ContainsKey(EquipmentSlot.LeftHandEquip) &&
                _equipped[EquipmentSlot.RightHandEquip] is IUsable leftHandUsable)
                return leftHandUsable;

            return null;
        }

        public bool Equip(IEquippable equippable, EquipmentSlot slot)
        {
            if (equippable.SlotKind != slot.Kind)
                return false;

            if (!Slots.Contains(slot) || SlotsInUse.Contains(slot))
                return false;

            _equipped[slot] = equippable;
            SlotsInUse.Add(slot);

            OnEquipped?.Invoke(slot, equippable);

            return true;
        }
    }
}
