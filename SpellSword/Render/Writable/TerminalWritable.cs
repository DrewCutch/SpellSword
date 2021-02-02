using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection.PortableExecutable;
using System.Text;
using BearLib;
using GoRogue.Random;

namespace SpellSword.Render
{
    class TerminalWritable: Writeable
    {
        public override bool Dirty { get; set; }
        public int Layer { get; }

        public TerminalWritable(int width, int height, int layer): base(width, height)
        {
            Layer = layer;
            Dirty = true;
        }

        public override void SetCharacter(int row, int col, int character, Color color, Color? backgroundColor)
        {
            Terminal.Layer(Layer);

            Terminal.Composition(false);

            if (backgroundColor is Color _backgroundColor)
            {
                Terminal.Color(_backgroundColor);
                Terminal.Put(col, row, '█');
                Terminal.Composition(true);
            }

            Terminal.Color(color);
            Terminal.Put(col, row, character);

            Dirty = true;
        }

        public override void WriteCharacter(int row, int col, int character, Color color, Color? backgroundColor)
        {
            Terminal.Layer(Layer);

            Terminal.Composition(true);

            if (backgroundColor is Color _backgroundColor)
            {
                Terminal.Color(_backgroundColor);
                if(character > 0xE000)
                    Terminal.Put(col, row, 'a'  + 122 + 0xE000);
                else
                    Terminal.Put(col, row, '█');
            }

            Terminal.Color(color);

            Terminal.Put(col, row, character);

            Dirty = true;
        }

        public override void Clear(GoRogue.Rectangle bounds)
        {
            Terminal.Layer(Layer);
            Terminal.ClearArea(bounds);
        }
    }
}
