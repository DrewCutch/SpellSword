using System.Drawing;
using GoRogue.GameFramework;
using GoRogue.GameFramework.Components;
using SpellSword.Render;

namespace SpellSword.Engine.Components
{
    class GlyphComponent: IGameObjectComponent
    {
        public IGameObject Parent { get; set; }

        public Glyph Glyph { get; set; }

        public GlyphComponent(char character, Color color)
        {
            Glyph = new Glyph(character, color);
        }

        public GlyphComponent(Glyph glyph)
        {
            Glyph = glyph;
        }
    }
}
