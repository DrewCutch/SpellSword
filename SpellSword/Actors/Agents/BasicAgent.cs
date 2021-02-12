using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoRogue;
using GoRogue.GameFramework;
using GoRogue.MapViews;
using GoRogue.Pathing;
using SpellSword.Actors.Action;
using SpellSword.Actors.Pathing;
using SpellSword.RPG.Alignment;

namespace SpellSword.Actors
{
    class BasicAgent: Agent
    {
        private Actor _target;
        private Coord _targetLastSeen;

        private GoalMapStore _goalMapStore;

        public BasicAgent(GoalMapStore goalMapStore)
        {
            _goalMapStore = goalMapStore;
        }

        public override ActorAction GetNextAction(Actor actor)
        {
            if (_target != null && _target.Parent.CurrentMap != actor.Parent.CurrentMap)
                _target = null;

            if (_target == null || _targetLastSeen == actor.Parent.Position)
                SearchForTarget(actor);

            if (_target == null)
                return new WaitAction(actor, 100);

            bool hasSightLine = _target != null && Lines.Get(actor.Parent.Position, _target.Parent.Position)
                .All((pos) => actor.Parent.CurrentMap.TransparencyView[pos] || actor.Parent.Position == pos || _target.Parent.Position == pos);
            
            if (hasSightLine)
                _targetLastSeen = _target.Parent.Position;

            if (!hasSightLine)
            {
                Path pathToLastSeen = actor.Parent.CurrentMap.AStar.ShortestPath(actor.Parent.Position, _targetLastSeen);

                if(pathToLastSeen == null)
                    return new WaitAction(actor, 50);

                Coord nextStep = pathToLastSeen.GetStep(0);
                Direction stepDirection = Direction.GetDirection(actor.Parent.Position, nextStep);

                return new MoveAction(actor, stepDirection);
            }

            IUsable currentWeapon = actor.Being.Equipment.GetMain();

            bool tooClose =
                currentWeapon.RangeDistanceType.Calculate(actor.Parent.Position, _target.Parent.Position) <= 3 &&
                currentWeapon.Range > 1;

            if (currentWeapon.RangeDistanceType.Calculate(actor.Parent.Position, _target.Parent.Position) <= currentWeapon.Range && !tooClose)
            {
                return new UseAction(actor, _target.Parent.Position, currentWeapon);
            }

            // Flee if too close with a ranged weapon
            IMapView<double?> goalMap;

            if (tooClose)
                goalMap = _goalMapStore.GetFleeMap(_target.Parent);
            else
                goalMap = _goalMapStore.GetGoalMap(_target.Parent);


            foreach (Direction nextStep in goalMap.GetImprovingDirections(actor.Parent.Position, AdjacencyRule.CARDINALS))
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

        private void SearchForTarget(Actor actor)
        {
            FOV fov = new FOV(actor.Parent.CurrentMap.TransparencyView);
            fov.Calculate(actor.Parent.Position);

            foreach (Coord pos in fov.CurrentFOV)
            {
                if (!(actor.Parent.CurrentMap.GetObject(pos).GetComponent<Actor>() is Actor otherActor)) 
                    continue;

                if (actor.Being.Alignment.GetRelation(otherActor.Being.Alignment) != AlignmentRelation.Enemy)
                    continue;
                    
                _target = otherActor;
                _targetLastSeen = otherActor.Parent.Position;

                return;
            }

            _target = null;
        }
    }
}
