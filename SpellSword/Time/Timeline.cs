using System;
using System.Collections.Generic;
using System.Text;
using SpellSword.ECS.Components;

namespace SpellSword.Time
{
    public delegate void TimeAdvance(int ticks);

    class Timeline
    {
        public event TimeAdvance OnAdvance;

        public EventTimer CreateTimer(int leadup, int cooldown)
        {
            return new EventTimer(leadup, cooldown, this);
        }

        public void Advance(int time)
        {
            OnAdvance?.Invoke(time);
        }
    }
}
