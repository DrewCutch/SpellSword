using System;
using System.Collections.Generic;
using System.Text;
using GoRogue;
using GoRogue.GameFramework;
using GoRogue.Pathing;

namespace SpellSword.Actors.Pathing
{
    class GoalMapStore
    {
        private readonly Dictionary<IGameObject, GoalMap> _goalMaps;

        private event Func<bool> _updateMaps;

        private bool _dirty;

        private readonly Map _baseMap;

        public GoalMapStore(Map baseMap)
        {
            _baseMap = baseMap;
            _baseMap.ObjectAdded += BaseMapOnObjectAdded;
            _baseMap.ObjectMoved += BaseMapOnObjectMoved;
            _baseMap.ObjectRemoved += BaseMapOnObjectRemoved;

            _dirty = false;

            _goalMaps = new Dictionary<IGameObject, GoalMap>();
        }

        private void BaseMapOnObjectRemoved(object? sender, ItemEventArgs<IGameObject> e)
        {
            if (!e.Item.IsWalkable)
            {
                _dirty = true;
            }
        }

        private void BaseMapOnObjectMoved(object? sender, ItemMovedEventArgs<IGameObject> e)
        {
            if (!e.Item.IsWalkable || _goalMaps.ContainsKey(e.Item))
            {
                _dirty = true;
            }
        }

        private void BaseMapOnObjectAdded(object? sender, ItemEventArgs<IGameObject> e)
        {
            if (!e.Item.IsWalkable)
            {
                _dirty = true;
            }
        }

        public void UpdateMaps()
        {
            if(_dirty)
                _updateMaps?.Invoke();

            _dirty = false;
        }

        public GoalMap GetGoalMap(IGameObject target)
        {
            if (_goalMaps.ContainsKey(target))
                return _goalMaps[target];

            return CreateGoalMap(target);
        }

        private GoalMap CreateGoalMap(IGameObject target)
        {
            GoalMap goalMap = new GoalMap(new GoalMapTranslator(_baseMap, target), Distance.EUCLIDEAN);
            _updateMaps += goalMap.Update;
            _goalMaps[target] = goalMap;
            return goalMap;
        }
    }
}
