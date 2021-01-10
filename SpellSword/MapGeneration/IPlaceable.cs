using System;
using System.Collections.Generic;
using System.Text;
using GoRogue;
using GoRogue.GameFramework;

namespace SpellSword.MapGeneration
{
    interface IPlaceable
    {
        bool Place(Map map, Coord pos);
    }
}
