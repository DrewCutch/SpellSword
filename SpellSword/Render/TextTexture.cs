using System;
using System.Collections.Generic;
using System.Drawing;
using GoRogue.MapViews;
using SpellSword.Actors.Action;

namespace SpellSword.Render
{
    public class TextTexture: IWriteable
    {
        public int Width { get; }
        public int Height { get; }

        public readonly Glyph[][] Contents;

        public bool Dirty { get; private set; }

        public TextTexture(int width, int height)
        {
            Width = width;
            Height = height;
            Dirty = false;

            Contents = new Glyph[height][];
            for (int j = 0; j < height; j++)
            {
                Contents[j] = new Glyph[width];
            }
        }

        public void WriteGlyph(int row, int col, Glyph glyph)
        {
            SetGlyph(row, col, glyph);
        }

        public void SetGlyph(int row, int col, Glyph g)
        {
            if (Contents[row][col] == g)
                return;

            Dirty = true;
            Contents[row][col] = g;
        }


        public void Clean()
        {
            Dirty = false;
        }
    }
}