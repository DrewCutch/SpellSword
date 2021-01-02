using System;
using System.Collections.Generic;
using System.Text;
using GoRogue;

namespace SpellSword.Render.Particles
{
    class Particle
    {
        public Glyph Glyph { get; }

        public Coord Pos { get; }

        public Particle(Glyph glyph, Coord pos)
        {
            Glyph = glyph;
            Pos = pos;
        }
    }
}
