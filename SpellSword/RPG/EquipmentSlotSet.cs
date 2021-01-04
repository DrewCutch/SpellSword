using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpellSword.Actors.Action;
using SpellSword.RPG.Items;

namespace SpellSword.RPG
{
    class EquipmentSlotSet
    {
        public List<EquipmentSlot> Slots { get; }

        public HashSet<EquipmentSlot> SlotsInUse { get; private set; }

        public IReadOnlyDictionary<EquipmentSlot, Item> Equipped => _equipped;

        private Dictionary<EquipmentSlot, Item> _equipped;

        public event Action<EquipmentSlot, Item> OnEquipped;
        public event Action<EquipmentSlot, Item> OnUnequipped;

        public EquipmentSlotSet()
        {
            Slots = EquipmentSlot.AllSlots.ToList();
            SlotsInUse = new HashSet<EquipmentSlot>();
            _equipped = new Dictionary<EquipmentSlot, Item>();
        }

        public EquipmentSlotSet(List<EquipmentSlot> slots)
        {
            Slots = slots;
            SlotsInUse = new HashSet<EquipmentSlot>();
            _equipped = new Dictionary<EquipmentSlot, Item>();
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

        public Item Equip(Item equippable, EquipmentSlot slot)
        {
            if (equippable.SlotKind != slot.Kind || !Slots.Contains(slot))
                return null;

            Item previousItem = null;

            if (SlotsInUse.Contains(slot))
                previousItem = _equipped[slot];
            

            _equipped[slot] = equippable;
            SlotsInUse.Add(slot);

            OnEquipped?.Invoke(slot, equippable);

            if(previousItem != null)
                OnUnequipped?.Invoke(slot, previousItem);

            return previousItem;
        }
    }
}
