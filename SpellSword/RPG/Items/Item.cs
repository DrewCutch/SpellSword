using System.Linq;
using SpellSword.Render;
using SpellSword.Speech;

namespace SpellSword.RPG.Items
{
    abstract class Item
    {
        private static readonly string[] RarityTitles = new[] {"Trash", "Common", "Uncommon", "Rare", "Epic", "Legendary"};

        public bool CanBeEquipped => SlotKind != EquipmentSlotKind.None;
        public EquipmentSlotKind SlotKind { get; }
        public Title Title { get; }
        public string Description { get; }
        public Glyph Glyph { get; }
        public int Rarity { get; }

        public string RarityTitle => Rarity < RarityTitles.Length ? RarityTitles[Rarity] : RarityTitles.Last();

        public Item(Title title, string description, Glyph glyph, EquipmentSlotKind slotKind, int rarity)
        {
            Title = title;
            Description = description;
            Glyph = glyph;
            SlotKind = slotKind;
            Rarity = rarity;
        }
    }
}
