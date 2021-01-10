using System;
using System.Collections.Generic;
using System.Text;
using GoRogue.Messaging;
using SpellSword.Engine.Components;
using SpellSword.Speech;
using SpellSword.Time;

namespace SpellSword.Actors.Effects
{
    class DamageEffect: IEffect<IDamagable>
    {
        public EventTimerTiming Timing => EventTimerTiming.Instant;

        public string Name { get; }

        public Damage Damage { get; }

        public EffectKind Kind => EffectKind.OneShot;

        public DamageEffect(Damage damage)
        {
            Damage = damage;
            Name = "Damage";
        }

        public void Apply(IDamagable target)
        {
            target.DoDamage(Damage);
        }

        public void UnApply(IDamagable target)
        {
            target.DoDamage(new Damage(-Damage.Amount));
        }
    }
}
