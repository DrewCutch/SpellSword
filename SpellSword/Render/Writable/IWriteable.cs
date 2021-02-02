using System;
using System.Collections.Generic;
using System.Drawing;
using Rectangle = GoRogue.Rectangle;

namespace SpellSword.Render
{
    public abstract class Writeable
    {
        public int Width { get; }
        public int Height { get; }

        public virtual bool Dirty { get; set; }

        protected Writeable(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public void SetGlyph(int row, int col, Glyph glyph)
        {
            SetCharacter(row, col, (int) glyph.Character, glyph.Color, glyph.BackgroundColor);
        }

        public abstract void SetCharacter(int row, int col, int character, Color color, Color? backgroundColor);

        public void WriteGlyph(int row, int col, Glyph glyph)
        {
            WriteCharacter(row, col, (int) glyph.Character, glyph.Color, glyph.BackgroundColor);
        }

        public abstract void WriteCharacter(int row, int col, int character, Color color, Color? backgroundColor);

        public void Clear()
        {
            Clear(new Rectangle(0,0,Width, Height));
        }

        public abstract void Clear(Rectangle bounds);
    }
}
