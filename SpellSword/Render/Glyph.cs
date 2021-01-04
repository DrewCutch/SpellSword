using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace SpellSword.Render
{
    public class Glyph
    {
        public static Glyph Blank => new Glyph(' ', Color.Black);

        public char Character { get;}
        public Color Color { get; }

        public Color? BackgroundColor { get;}

        public int Alpha => Color.A;

        public Glyph(char character, Color color, Color? backgroundColor = null)
        {
            Character = character;
            Color = color;
            BackgroundColor = backgroundColor;
        }

        public Glyph(Color color)
        {
            Color = color;
            Character = '█';
        }
    }
}
