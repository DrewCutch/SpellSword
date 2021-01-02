using System;
using System.Collections.Generic;
using System.Text;

namespace SpellSword.Time
{
    class EventTimerTiming
    {
        public int Leadup { get; }
        public int Cooldown { get; }

        public EventTimerTiming(int leadup, int cooldown)
        {
            Leadup = leadup;
            Cooldown = cooldown;
        }
    }
}
