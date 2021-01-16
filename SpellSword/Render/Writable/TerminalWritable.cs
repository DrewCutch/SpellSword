using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection.PortableExecutable;
using System.Text;
using BearLib;

namespace SpellSword.Render
{
    class TerminalWritable: IWriteable
    {
        public int Width { get; }
        public int Height { get; }
        public bool Dirty { get; set; }
        public int Layer { get; }

        public TerminalWritable(int width, int height, int layer)
        {
            Width = width;
            Height = height;
            Layer = layer;
            Dirty = true;
        }

        public void SetGlyph(int row, int col, Glyph glyph)
        {
            Terminal.Layer(Layer);

            Terminal.Composition(false);

            if (glyph.BackgroundColor is Color backgroundColor)
            {
                Terminal.Color(backgroundColor);
                Terminal.Put(col, row, '█');
                Terminal.Composition(true);
            }

            Terminal.Color(glyph.Color);
            Terminal.Put(col, row, glyph.Character);

            Dirty = true;
        }

        public void WriteGlyph(int row, int col, Glyph glyph)
        {
            Terminal.Layer(Layer);

            Terminal.Composition(true);

            if (glyph.BackgroundColor is Color backgroundColor)
            {
                Terminal.Color(backgroundColor);
                Terminal.Put(col, row, '█');
            }

            Terminal.Color(glyph.Color);
            Terminal.Put(col, row, glyph.Character);

            Dirty = true;
        }

        public void Clear(GoRogue.Rectangle bounds)
        {
            Terminal.Layer(Layer);
            Terminal.ClearArea(bounds);
        }
    }
}
