using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace SpellSword.Render
{
    static class GlyphExtensions
    {
        public static Glyph MultipliedByColor(this Glyph glyph, Color color)
        {
            return new Glyph(glyph.Character, glyph.Color.Multiply(color), glyph.BackgroundColor?.Multiply(color));
        }

        public static Glyph GreyScale(this Glyph glyph)
        {
            return new Glyph(glyph.Character, glyph.Color.GreyScale(), glyph.BackgroundColor?.GreyScale());
        }

        public static Glyph BlackAndWhite(this Glyph glyph)
        {
            return glyph.BackgroundColor != null ? new Glyph(glyph.Character, Color.White, Color.Black) : new Glyph(glyph.Character, Color.White);
        }
    }
}
