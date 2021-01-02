using System;
using System.Collections.Generic;
using System.Text;
using GoRogue.GameFramework;
using GoRogue.MapViews;
using GoRogue.Pathing;

namespace SpellSword.Actors.Pathing
{
    class GoalMapTranslator : TranslationMap<IEnumerable<IGameObject>, GoalState>
    {
        private IGameObject _target;

        public GoalMapTranslator(IMapView<IEnumerable<IGameObject>> baseMap, IGameObject target) : base(baseMap)
        {
            _target = target;
        }

        protected override GoalState TranslateGet(IEnumerable<IGameObject> value)
        {
            foreach (IGameObject gameObject in value)
            {
                if (gameObject == _target)
                    return GoalState.Goal;

                if (!gameObject.IsWalkable)
                    return GoalState.Obstacle;
            }

            return GoalState.Clear;
        }
    }
}
