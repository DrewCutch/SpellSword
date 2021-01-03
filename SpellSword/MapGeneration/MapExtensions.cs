using System;
using System.Collections.Generic;
using System.Text;
using GoRogue;
using GoRogue.GameFramework;

namespace SpellSword.MapGeneration
{
    static class MapExtensions
    {
        public static bool IsWalkable(this Map map, Coord pos)
        {
            return map.WalkabilityView[pos];
        }
    }
}
