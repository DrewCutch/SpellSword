using System.Linq;
using SpellSword.Render;

namespace SpellSword.RPG.Items
{
    abstract class Item
    {
        private static readonly string[] RarityTitles = new[] {"Trash", "Common", "Uncommon", "Rare", "Epic", "Legendary"};

        public bool CanBeEquipped => SlotKind != EquipmentSlotKind.None;
        public EquipmentSlotKind SlotKind { get; }
        public string Name { get; }
        public string Description { get; }
        public Glyph Glyph { get; }
        public int Rarity { get; }

        public string RarityTitle => Rarity < RarityTitles.Length ? RarityTitles[Rarity] : RarityTitles.Last();

        public Item(string name, string description, Glyph glyph, EquipmentSlotKind slotKind, int rarity)
        {
            Name = name;
            Description = description;
            Glyph = glyph;
            SlotKind = slotKind;
            Rarity = rarity;
        }
    }
}
