using System;
using System.Collections.Generic;
using System.Text;
using GoRogue;

namespace SpellSword.Render.Particles
{
    class GlyphFlash: IParticleEffect
    {
        public bool Changed { get; private set; }
        public bool Finished { get; private set; }

        public long Duration { get; }

        public Glyph Glyph => _particle.Glyph;

        private readonly Particle _particle;

        private long _aliveFor;

        public GlyphFlash(Glyph glyph, int duration, Coord pos)
        {
            _particle = new Particle(glyph, pos);
            Duration = duration;

            Changed = true;
            Finished = false;

            _aliveFor = 0;
        }

        public IEnumerable<Particle> Particles()
        {
            yield return _particle;
        }

        public void Update(long deltaTime)
        {
            _aliveFor += deltaTime;

            if (_aliveFor >= Duration)
            {
                Finished = true;
                Changed = true;
            }
            else if(_aliveFor != deltaTime)
            {
                Changed = false;
            }
        }
    }
}
