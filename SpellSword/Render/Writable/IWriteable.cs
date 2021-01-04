using System;
using System.Collections.Generic;

namespace SpellSword.Render
{
    interface IWriteable
    {
        int Width { get; }
        int Height { get; }

        void SetGlyph(int row, int col, Glyph glyph);

        void WriteGlyph(int row, int col, Glyph glyph);

        public void Clear()
        {
            for (int i = 0; i < Height; i++)
                for (int j = 0; j < Width; j++)
                    SetGlyph(i, j, Glyph.Blank);
        }
    }
}
