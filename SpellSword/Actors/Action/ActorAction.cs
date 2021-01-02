using SpellSword.Logging;
using SpellSword.Time;

namespace SpellSword.Actors.Action
{
    abstract class ActorAction
    {
        public EventTimer Timer { get; protected set; }

        public Actor Actor { get; }

        protected ActorAction(Actor actor, EventTimer timer)
        {
            Actor = actor;
            Timer = timer;
        }
        public abstract bool Resolve();
    }
}