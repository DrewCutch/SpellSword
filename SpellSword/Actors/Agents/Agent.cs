using SpellSword.Actors.Action;

namespace SpellSword.Actors
{
    abstract class Agent
    {
        public abstract ActorAction GetNextAction(Actor actor);
    }
}
