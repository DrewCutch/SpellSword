using System;
using System.Collections.Generic;
using System.Text;
using GoRogue;
using SpellSword.Game;
using Troschuetz.Random;

namespace SpellSword.MapGeneration.Structure
{
    interface IHallGenerator
    {
        public IRoom Generate(Floor floor, Coord connectFrom, Coord connectTo, IGenerator rng);

        public bool CanGenerate(Floor floor, Coord connectFrom, Coord connectTo, IGenerator rng);
    }
}
