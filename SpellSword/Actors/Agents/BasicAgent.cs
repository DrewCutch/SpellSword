using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoRogue;
using GoRogue.GameFramework;
using GoRogue.Pathing;
using SpellSword.Actors.Action;
using SpellSword.Actors.Pathing;

namespace SpellSword.Actors
{
    class BasicAgent: Agent
    {
        private Actor _target;
        private GoalMapStore _goalMapStore;
        public BasicAgent(Actor target, GoalMapStore goalMapStore)
        {
            _target = target;
            _goalMapStore = goalMapStore;
        }

        public override ActorAction GetNextAction(Actor actor)
        {
            IUsable currentWeapon = actor.Being.Equipment.GetMain();

            if (currentWeapon.RangeDistanceType.Calculate(actor.Parent.Position, _target.Parent.Position) <= currentWeapon.Range)
            {
                return new UseAction(actor, _target.Parent.Position, currentWeapon);
            }

            foreach (Direction nextStep in _goalMapStore.GetGoalMap(_target.Parent).GetImprovingDirections(actor.Parent.Position, AdjacencyRule.CARDINALS))
            {
                // Only move if no other object occupying layer
                if (actor.Parent.CurrentMap.GetObject(actor.Parent.Position + nextStep,
                    actor.Parent.CurrentMap.LayerMasker.Mask(actor.Parent.Layer)) == null)
                {
                    return new MoveAction(actor, nextStep);
                }
            }

            return new WaitAction(actor, 1);
        }
    }
}
