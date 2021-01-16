using System;
using System.Collections.Generic;
using System.Text;
using GoRogue;
using GoRogue.GameFramework;
using SpellSword.Engine.Components;

namespace SpellSword.Engine
{
    class TriggerRouter
    {
        public Map Map { get; }

        public TriggerRouter(Map map)
        {
            map.ObjectMoved += MapOnObjectMoved;

            Map = map;
        }

        private void MapOnObjectMoved(object? sender, ItemMovedEventArgs<IGameObject> e)
        {
            Direction direction = Direction.GetDirection(e.OldPosition, e.NewPosition);

            foreach (IGameObject gameObject in Map.GetObjects(e.OldPosition))
            {
                if (gameObject.GetComponent<TriggerComponent>() is TriggerComponent triggerComponent)
                {
                    triggerComponent.Exit(e.Item, direction);
                }
            }

            foreach (IGameObject gameObject in Map.GetObjects(e.NewPosition))
            {
                if (gameObject.GetComponent<TriggerComponent>() is TriggerComponent triggerComponent)
                {
                    triggerComponent.Enter(e.Item, direction);
                }
            }
            
        }
    }
}
