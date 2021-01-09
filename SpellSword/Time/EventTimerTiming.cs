using System;
using System.Collections.Generic;
using System.Text;

namespace SpellSword.Time
{
    class EventTimerTiming
    {
        public static EventTimerTiming Instant = new EventTimerTiming(0, 0);

        public int Leadup { get; }
        public int Cooldown { get; }

        public EventTimerTiming(int leadup, int cooldown)
        {
            Leadup = leadup;
            Cooldown = cooldown;
        }
    }
}
