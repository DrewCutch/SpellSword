using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpellSword.Time;

namespace SpellSword.Actors.Action
{
    class InteractAction: ActorAction
    {
        private readonly IInteractable _interactable;

        public InteractAction(Actor actor, IInteractable interactable) : base(actor, new EventTimer(interactable.InteractTiming(actor)))
        {
            _interactable = interactable;
        }

        public override bool Resolve()
        {
            if (Timer.PendingMoments.Contains(EventTimer.EventTimerMoment.Action))
                _interactable.Interact(Actor);

            return Timer.PendingMoments.Contains(EventTimer.EventTimerMoment.End);
        }
    }
}
