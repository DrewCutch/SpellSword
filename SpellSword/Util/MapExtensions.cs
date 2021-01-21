using System;
using System.Collections.Generic;
using System.Text;
using GoRogue;
using GoRogue.GameFramework;
using GoRogue.MapViews;

namespace SpellSword.Util
{
    static class MapExtensions
    {
        public static bool AllEmpty(this IMapView<IGameObject> map, IEnumerable<Coord> area)
        {
            foreach (Coord pos in area)
            {
                if (!map.Contains(pos) || map[pos] != null)
                    return false;
            }

            return true;
        }
    }
}
