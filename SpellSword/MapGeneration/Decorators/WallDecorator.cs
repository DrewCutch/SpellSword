using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using GoRogue;
using GoRogue.GameFramework;
using GoRogue.MapGeneration;
using SpellSword.Game;
using SpellSword.Util;
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

        public void Decorate(Floor floor, MapArea area, IGenerator rng)
        {
            IEnumerable<Coord> wallCoords = area.Perimeter();

            Coord roomTop = area.Bounds.MinExtent;
            Coord roomMax = area.Bounds.MaxExtent;

            int xSkip = 4;
            int ySkip = 4;

            foreach (Coord pos in wallCoords)
            {
                // only place on walls
                if(floor.MapInfo.Map.WalkabilityView[pos])
                    continue;

                Coord offset = pos - roomTop;

                if(offset.X % xSkip != 0 && pos.X != roomMax.X)
                    continue;

                if (offset.Y % ySkip != 0 && pos.Y != roomMax.Y)
                    continue;

                _placeable.Place(floor, pos, rng);
            }
            area.Bounds.PerimeterPositions();
        }

        public bool CanDecorate(Floor floor, MapArea area)
        {
            return area.Count > 0;
        }
    }
}
