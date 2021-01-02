using System;
using System.Collections.Generic;
using System.Text;
using GoRogue;
using SpellSword.Render.Particles;

namespace SpellSword.Actors.Action
{
    interface IAimVisualization: IParticleEffect
    {
        public void UpdateTarget(Coord newTarget);

        public void Stop();
    }
}
