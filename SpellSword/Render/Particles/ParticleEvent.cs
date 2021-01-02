using System;
using System.Collections.Generic;
using System.Text;

namespace SpellSword.Render.Particles
{
    class ParticleEvent
    {
        public bool Start { get; }
        public bool End => !Start;

        public IParticleEffect Effect { get; }

        public ParticleEvent(IParticleEffect effect, bool start = true)
        {
            Effect = effect;
            Start = start;
        }
    }
}
