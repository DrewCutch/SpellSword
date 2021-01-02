using System;
using System.Collections.Generic;
using System.Text;

namespace SpellSword.Render.Particles
{
    interface IParticleEffect
    {
        public bool Changed { get; }

        public bool Finished { get; }

        public IEnumerable<Particle> Particles();

        public void Update(long deltaTime);
    }
}
