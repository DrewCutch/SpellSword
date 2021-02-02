using System.Drawing;
using GoRogue.GameFramework;
using GoRogue.GameFramework.Components;
using SpellSword.Render;

namespace SpellSword.Engine.Components
{
    class GlyphComponent: Component
    {

        public Glyph Glyph { get; set; }

        public GlyphComponent(Characters character, Color color)
        {
            Glyph = new Glyph(character, color);
        }

        public GlyphComponent(Glyph glyph)
        {
            Glyph = glyph;
        }
    }
}
