using System;
using System.Collections.Generic;
using System.Text;
using GoRogue;
using SpellSword.Time;

namespace SpellSword.Actors.Action
{
    interface IInteractable
    {
        void Interact(Actor by);

        public int Range { get; }

        public Distance RangeDistanceType { get; }

        public EventTimerTiming InteractTiming(Actor by);
    }
}
