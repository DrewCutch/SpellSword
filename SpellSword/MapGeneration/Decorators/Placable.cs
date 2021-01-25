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

        public Placable(Func<Floor, Coord, IGameObject> generator)
        {
            _generator = generator;
        }

        public bool Place(Floor floor, Coord pos, IGenerator rng)
        {
            IGameObject gameObject = _generator(floor, pos);

            if (gameObject.Layer == 0)
            {
                floor.MapInfo.Map.SetTerrain(gameObject);
                return true;
            }

            return floor.MapInfo.Map.AddEntity(gameObject);
        }
    }
}
