using System;
using System.Collections.Generic;
using System.Text;
using GoRogue;
using Troschuetz.Random;

namespace SpellSword.MapGeneration.Structure
{
    interface IHallGenerator
    {
        public IRoom Generate(MapInfo mapInfo, Coord connectFrom, Coord connectTo, IGenerator rng);

        public bool CanGenerate(MapInfo mapInfo, Coord connectFrom, Coord connectTo, IGenerator rng);
    }
}
