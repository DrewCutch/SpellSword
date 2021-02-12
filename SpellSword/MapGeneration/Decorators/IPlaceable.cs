using System;
using System.Collections.Generic;
using System.Text;
using GoRogue;
using GoRogue.GameFramework;
using SpellSword.Game;
using Troschuetz.Random;

namespace SpellSword.MapGeneration
{
    interface IPlaceable
    {
        bool Place(Floor floor, Coord pos, IGenerator rng);
        bool CanPlace(Floor floor, Coord pos, IGenerator rng);
    }
}
