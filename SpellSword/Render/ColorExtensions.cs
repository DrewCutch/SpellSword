using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace SpellSword.Render
{
    static class ColorExtensions
    {
        public static Color Multiply(this Color color, Color other)
        {
            return Color.FromArgb(color.A, (color.R * other.R) / 255, (color.G * other.G)/255, (color.B * other.B)/255);
        }

        public static Color Inverted(this Color color)
        {
            return Color.FromArgb(color.ToArgb() ^ 0xffffff);
        }

        public static Color Add(this Color color, Color other)
        {
            int r = Math.Min(color.R + other.R, 255);
            int g = Math.Min(color.G + other.G, 255);
            int b = Math.Min(color.B + other.B, 255);

            return Color.FromArgb(r, g, b);
        }
    }
}
