using System;
using System.Collections.Generic;
using System.Text;
using GoRogue;
using GoRogue.GameFramework;
using Troschuetz.Random;

namespace SpellSword.MapGeneration
{
    interface IPlaceable
    {
        bool Place(MapInfo mapInfo, Coord pos, IGenerator rng);
    }
}
