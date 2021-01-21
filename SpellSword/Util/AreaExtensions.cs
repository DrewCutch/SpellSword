using System;
using System.Collections.Generic;
using System.Text;
using GoRogue;
using GoRogue.MapGeneration;

namespace SpellSword.Util
{
    static class AreaExtensions
    {
        public static IEnumerable<Coord> Perimeter(this MapArea area)
        {
            foreach (Coord pos in area.Positions)
            {
                foreach (Coord neighbor in AdjacencyRule.EIGHT_WAY.Neighbors(pos))
                {
                    if (!area.Contains(neighbor))
                    {
                        yield return pos;
                        break;
                    }
                }
            }
        }
    }
}
