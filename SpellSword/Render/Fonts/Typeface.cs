using System;
using System.Collections.Generic;
using System.Text;

namespace SpellSword.Render.Fonts
{
    public abstract class Typeface
    {
        public static Typeface Text = new TextTypeface();
        public static Typeface Grid = new GridTypeface();

        public abstract int Width { get; }
        public abstract int GetCharacter(int c);

        private class TextTypeface : Typeface
        {
            public override int Width => 1;

            public override int GetCharacter(int c)
            {
                return c;
            }
        }

        private class GridTypeface : Typeface
        {
            public override int Width => 2;

            public override int GetCharacter(int c)
            {
                return c + 0xE000;
            }
        }
    }
}
