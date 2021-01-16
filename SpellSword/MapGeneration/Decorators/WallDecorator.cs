using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using GoRogue;
using GoRogue.GameFramework;
using GoRogue.MapGeneration;
using Troschuetz.Random;

namespace SpellSword.MapGeneration
{
    class WallDecorator: IAreaDecorator
    {
        private IPlaceable _placeable;

        public WallDecorator(IPlaceable placeable)
        {
            _placeable = placeable;
        }

        public void Decorate(MapInfo mapInfo, MapArea area, IGenerator rng)
        {
            IEnumerable<Coord> wallCoords = area.Bounds.PerimeterPositions();

            Coord roomTop = area.Bounds.MinExtent;
            Coord roomMax = area.Bounds.MaxExtent;

            int xSkip = 4;
            int ySkip = 4;

            foreach (Coord pos in wallCoords)
            {
                // only place on walls
                if(mapInfo.Map.WalkabilityView[pos])
                    continue;

                Coord offset = pos - roomTop;

                if(offset.X % xSkip != 0 && pos.X != roomMax.X)
                    continue;

                if (offset.Y % ySkip != 0 && pos.Y != roomMax.Y)
                    continue;

                _placeable.Place(mapInfo, pos, rng);
            }
            area.Bounds.PerimeterPositions();
        }

        public bool CanDecorate(MapInfo mapInfo, MapArea area)
        {
            return area.Count > 0;
        }
    }
}
