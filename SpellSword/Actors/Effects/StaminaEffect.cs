using System;
using System.Collections.Generic;
using System.Text;
using SpellSword.Time;

namespace SpellSword.Actors.Effects
{
    class StaminaEffect: IEffect<Actor>
    {
        public EffectKind Kind { get; }
        public EventTimerTiming Timing { get; }
        public string Name => "Stamina Drain";

        public int Amount { get; }

        public StaminaEffect(int amount): this(amount, EffectKind.OneShot, EventTimerTiming.Instant)
        {
        }

        public StaminaEffect(int amount, EffectKind kind, EventTimerTiming timing)
        {
            Amount = amount;

            Kind = kind;

            Timing = timing;
        }

        public void Apply(Actor target)
        {
            if(Amount < 0)
                target.Being.Stamina.Expend(-Amount);
            else
                target.Being.Stamina.Charge(Amount);
        }

        public void UnApply(Actor target)
        {
            if (Amount < 0)
                target.Being.Stamina.Charge(-Amount);
            else
                target.Being.Stamina.Expend(Amount);
        }
    }
}
