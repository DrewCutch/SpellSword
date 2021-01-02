using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace SpellSword.Render.Lighting
{
    class UnboundedColor
    {
        public int R { get; private set; }
        public int G { get; private set; }
        public int B { get; private set; }

        public bool IsValidColor => R >= 0 && R <= 255 && G >= 0 && G <= 255 && B >= 0 && B <= 255;

        public UnboundedColor()
        {
            R = 0;
            G = 0;
            B = 0;
        }

        public UnboundedColor(int r, int g, int b)
        {
            R = r;
            G = g;
            B = b;
        }

        public UnboundedColor(Color color)
        {
            R = color.R;
            G = color.G;
            B = color.B;
        }

        public void Add(Color color)
        {
            R += color.R;
            G += color.G;
            B += color.B;
        }

        public void Add(UnboundedColor color)
        {
            R += color.R;
            G += color.G;
            B += color.B;
        }

        public void Subtract(Color color)
        {
            R -= color.R;
            G -= color.G;
            B -= color.B;
        }

        public void Subtract(UnboundedColor color)
        {
            R -= color.R;
            G -= color.G;
            B -= color.B;
        }

        public static implicit operator Color(UnboundedColor unboundedColor)
        {
            return Color.FromArgb(
                Math.Clamp(unboundedColor.R, 0, 255),
                Math.Clamp(unboundedColor.G, 0, 255),
                Math.Clamp(unboundedColor.B, 0, 255)
            );
        }

        public static implicit operator UnboundedColor(Color color)
        {
            return new UnboundedColor(color);
        }
    }
}
