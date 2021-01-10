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
            Placeable.Place(mapInfo, area.RandomPosition(), rng);
        }

        public bool CanDecorate(MapInfo mapInfo, MapArea area)
        {
            return area.Count > 0;
        }
    }
}
