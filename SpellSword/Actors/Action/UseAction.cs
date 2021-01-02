using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using GoRogue;
using SpellSword.Logging;
using SpellSword.Time;

namespace SpellSword.Actors.Action
{
    class UseAction: ActorAction
    {
        private Coord _target;
        private IUsable _usable;

        public UseAction(Actor actor, Coord target, IUsable usable) : base(actor, new EventTimer(usable.UseTiming(actor)))
        {
            _target = target;
            _usable = usable;
        }

        public override bool Resolve()
        {
            if (Timer.PendingMoments.Contains(EventTimer.EventTimerMoment.Action))
                _usable.Use(Actor, _target);

            return Timer.PendingMoments.Contains(EventTimer.EventTimerMoment.End);
        }
    }
}
