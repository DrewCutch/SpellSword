using System;
using System.Collections.Generic;
using System.Drawing;
using GoRogue.MapViews;
using SpellSword.Actors.Action;
using Rectangle = GoRogue.Rectangle;

namespace SpellSword.Render
{
    public class TextTexture: Writeable
    {
        public readonly Glyph[][] Contents;

        public TextTexture(int width, int height): base(width, height)
        {
            Dirty = false;

            Contents = new Glyph[height][];
            for (int j = 0; j < height; j++)
            {
                Contents[j] = new Glyph[width];
            }
        }

        public override void WriteCharacter(int row, int col, int character, Color color, Color? backgroundColor)
        {
            SetCharacter(row, col, character, color, backgroundColor);
        }

        public override void Clear(Rectangle bounds)
        {
            for (int i = bounds.MinExtentY; i < bounds.MaxExtentY; i++)
                for (int j = bounds.MinExtentX; j < bounds.MaxExtentX; j++)
                    SetGlyph(i, j, Glyph.Blank);
        }

        public override void SetCharacter(int row, int col, int character, Color color, Color? backgroundColor)
        {
            Dirty = true;
            Contents[row][col] = new Glyph((Characters) character, color, backgroundColor);
        }


        public void Clean()
        {
            Dirty = false;
        }
    }
}