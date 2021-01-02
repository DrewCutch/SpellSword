using System;
using System.Collections.Generic;

namespace SpellSword.Render
{
    interface IWriteable
    {
        int Width { get; }
        int Height { get; }

        int Layers { get; }

        void SetGlyph(int row, int col, Layer layer, Glyph glyph);

        public void Clear(Layer layer)
        {
            for (int i = 0; i < Height; i++)
                for (int j = 0; j < Width; j++)
                    SetGlyph(i, j, layer, Glyph.Blank);
        }
    }
}
