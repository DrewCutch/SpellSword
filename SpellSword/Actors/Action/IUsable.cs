using System;
using System.Collections.Generic;
using System.Text;
using GoRogue;
using SpellSword.Render.Particles;
using SpellSword.Time;

namespace SpellSword.Actors.Action
{
    interface IUsable
    {
        public string Name { get; }
        
        public int Range { get; }
        
        public Distance RangeDistanceType { get; }

        public void Use(Actor by, Coord target);

        public bool CanUse(Actor by, Coord target);

        public EventTimerTiming UseTiming(Actor by);

        public IAimVisualization GetVisualization(Actor by, Coord target);
    }
}
