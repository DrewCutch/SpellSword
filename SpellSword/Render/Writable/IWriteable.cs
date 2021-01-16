using System;
using System.Collections.Generic;
using GoRogue;

namespace SpellSword.Render
{
    interface IWriteable
    {
        int Width { get; }
        int Height { get; }

        bool Dirty { get; set; }

        void SetGlyph(int row, int col, Glyph glyph);

        void WriteGlyph(int row, int col, Glyph glyph);

        public void Clear()
        {
            Clear(new Rectangle(0,0,Width, Height));
        }

        public void Clear(Rectangle bounds);
    }
}
