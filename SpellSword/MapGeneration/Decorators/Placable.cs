using System;
using System.Collections.Generic;
using System.Text;
using GoRogue;
using GoRogue.GameFramework;
using Troschuetz.Random;

namespace SpellSword.MapGeneration
{
    class Placable: IPlaceable
    {
        private readonly Func<MapInfo, Coord, IGameObject> _generator;

        public Placable(Func<MapInfo, Coord, IGameObject> generator)
        {
            _generator = generator;
        }

        public bool Place(MapInfo mapInfo, Coord pos, IGenerator rng)
        {
            IGameObject gameObject = _generator(mapInfo, pos);

            if (gameObject.Layer == 0)
            {
                mapInfo.Map.SetTerrain(gameObject);
                return true;
            }

            return mapInfo.Map.AddEntity(gameObject);
        }
    }
}
