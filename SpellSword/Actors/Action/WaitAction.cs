using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpellSword.Time;

namespace SpellSword.Actors.Action
{
    class WaitAction: ActorAction
    {
        public WaitAction(Actor actor, int time) : base(actor, new EventTimer(0, time))
        {

        }

        public override bool Resolve()
        {
            return Timer.PendingMoments.Contains(EventTimer.EventTimerMoment.End);
        }
    }
}
