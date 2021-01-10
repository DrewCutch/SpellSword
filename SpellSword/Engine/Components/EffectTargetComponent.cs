using System;
using System.Collections.Generic;
using System.Text;
using SpellSword.Actors.Effects;
using SpellSword.Update;

namespace SpellSword.Engine.Components
{
    class EffectTargetComponent: Component, IUpdate
    {
        public EffectTarget EffectTarget { get; }
        public EffectTargetComponent()
        {
            EffectTarget = new EffectTarget();
        }

        public void Update(int ticks)
        {
            EffectTarget.UpdateActiveEffects(ticks);
        }
    }
}
