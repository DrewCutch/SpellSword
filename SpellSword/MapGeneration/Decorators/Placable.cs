using System;
using System.Collections.Generic;
using System.Text;
using GoRogue;
using GoRogue.GameFramework;
using SpellSword.Game;
using Troschuetz.Random;

namespace SpellSword.MapGeneration
{
    class Placable: IPlaceable
    {
        private readonly Func<Floor, Coord, IGameObject> _generator;
        private readonly int _layer;
        private readonly bool _canOverwrite;

        public Placable(Func<Floor, Coord, IGameObject> generator, int layer, bool canOverwrite = false)
        {
            _generator = generator;
            _layer = layer;
            _canOverwrite = canOverwrite;
        }

        public bool Place(Floor floor, Coord pos, IGenerator rng)
        {
            if (!CanPlace(floor, pos, rng))
                return false;

            IGameObject gameObject = _generator(floor, pos);

            if (gameObject.Layer == 0)
            {
                floor.MapInfo.Map.SetTerrain(gameObject);
                return true;
            }

            if(!floor.MapInfo.Map.AddEntity(gameObject))
                Console.WriteLine("Place failed!");

            return floor.MapInfo.Map.AddEntity(gameObject);
        }

        public bool CanPlace(Floor floor, Coord pos, IGenerator rng)
        {
            return floor.MapInfo.Map.GetObject(pos, floor.MapInfo.Map.LayerMasker.Mask(_layer)) == null || _canOverwrite;
        }
    }
}
