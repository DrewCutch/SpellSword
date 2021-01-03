using System;
using System.Collections.Generic;
using System.Text;
using SpellSword.Render;
using SpellSword.Speech;

namespace SpellSword.Engine
{
    class Decal
    {
        public Title Title { get; }
        public Glyph Glyph { get; }

        public Decal(Title title, Glyph glyph)
        {
            Title = title;
            Glyph = glyph;
        }
    }
}
