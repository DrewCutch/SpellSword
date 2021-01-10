using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoRogue;
using GoRogue.GameFramework;
using GoRogue.MapViews;
using GoRogue.Messaging;
using SpellSword.Algorithms;
using SpellSword.MapGeneration;
using SpellSword.Util;

namespace SpellSword.Engine
{
    class Spawner: ISubscriber<SpawnEvent>
    {
        public Map Map { get; }

        private readonly Dijkstra[] _dijkstras;

        public Spawner(Map map)
        {
            Map = map;

            _dijkstras = new Dijkstra[map.LayerMasker.NumberOfLayers];

            for (int i = 0; i < map.LayerMasker.NumberOfLayers; i++)
            {
                var i1 = i;

                // In order to tumble the space must be valid terrain, walkable and empty on the same layer.
                _dijkstras[i] = new Dijkstra(new LambdaMapView<bool>(map.Width, map.Height, 
                    (coord) => map.WalkabilityView[coord] && map.Terrain[coord] != null && map.GetObject(coord, map.LayerMasker.Mask(i1)) == null), map.WalkabilityView);
            }
        }

        public void Handle(SpawnEvent spawn)
        {
            if(Map.GetObject(spawn.GameObject.Position, Map.LayerMasker.Mask(spawn.GameObject.Layer)) == null || !spawn.Tumble)
            {
                Map.AddEntity(spawn.GameObject);
                return;
            }

            Coord closestPoint = _dijkstras[spawn.GameObject.Layer].GetClosestAvailablePoint(spawn.GameObject.Position);

            spawn.GameObject.Position = closestPoint;

            Map.AddEntity(spawn.GameObject);
        }
    }
}
