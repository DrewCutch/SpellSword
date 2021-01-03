using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoRogue;
using GoRogue.GameFramework;
using GoRogue.Messaging;
using SpellSword.MapGeneration;
using SpellSword.Util;

namespace SpellSword.Engine
{
    class Spawner: ISubscriber<SpawnEvent>
    {
        public Map Map { get; }

        private List<Direction> _tumbleDirections;

        public Spawner(Map map)
        {
            Map = map;

            _tumbleDirections = AdjacencyRule.EIGHT_WAY.DirectionsOfNeighbors().ToList();

            _tumbleDirections.Shuffle();
        }

        public void Handle(SpawnEvent spawn)
        {
            if(Map.GetObject(spawn.GameObject.Position, Map.LayerMasker.Mask(spawn.GameObject.Layer)) == null || !spawn.Tumble)
            {
                Map.AddEntity(spawn.GameObject);
                return;
            }

            foreach (Direction direction in _tumbleDirections)
            {
                if (Map.GetObject(spawn.GameObject.Position + direction, Map.LayerMasker.Mask(spawn.GameObject.Layer)) != null || !Map.IsWalkable(spawn.GameObject.Position + direction)) 
                    continue;

                spawn.GameObject.Position += direction;
                Map.AddEntity(spawn.GameObject);

                _tumbleDirections.Shuffle();
                return;
            }
        }
    }
}
