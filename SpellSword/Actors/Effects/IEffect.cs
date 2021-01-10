using System;
using System.Collections.Generic;
using System.Text;
using SpellSword.Speech;
using SpellSword.Time;

namespace SpellSword.Actors
{
    interface IEffect<in T>
    {
        public EffectKind Kind { get; }
        
        public EventTimerTiming Timing { get; }

        public string Name { get; }

        void Apply(T target);

        void UnApply(T target);
    }
}
