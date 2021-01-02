using System;
using System.Drawing;
using System.Linq;
using Artemis.System;
using GoRogue;
using SpellSword.Logging;
using SpellSword.Time;

namespace SpellSword.Actors.Action
{
    class MoveAction: ActorAction
    {
        public Direction Direction { get; }
        public MoveAction(Actor actor, Direction direction): base(actor, new EventTimer())
        {
            Direction = direction;

            Timer.SetUp(Actor.Being.GetMovementTiming());
        }

        public override bool Resolve()
        {
            if (Timer.PendingMoments.Contains(EventTimer.EventTimerMoment.Action))
                Actor.Parent.Position += Direction;

            return Timer.PendingMoments.Contains(EventTimer.EventTimerMoment.End);
        }
    }
}
