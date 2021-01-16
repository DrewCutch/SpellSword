using System;
using System.Collections.Generic;
using System.Text;
using GoRogue.MapGeneration;
using Troschuetz.Random;

namespace SpellSword.MapGeneration
{
    class PlaceableDecorator: IAreaDecorator
    {
        public IPlaceable Placeable { get; }

        public PlaceableDecorator(IPlaceable placeable)
        {
            Placeable = placeable;
        }

        public void Decorate(MapInfo mapInfo, MapArea area, IGenerator rng)
        {
            MapArea restrictedArea = new MapArea();

            restrictedArea.Add(area.Positions);
            restrictedArea.Remove(area.Bounds.PerimeterPositions());

            Placeable.Place(mapInfo, restrictedArea.RandomPosition(), rng);
        }

        public bool CanDecorate(MapInfo mapInfo, MapArea area)
        {
            return area.Count > 0;
        }
    }
}
