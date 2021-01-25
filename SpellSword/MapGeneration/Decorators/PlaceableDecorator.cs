using System;
using System.Collections.Generic;
using System.Text;
using GoRogue.MapGeneration;
using SpellSword.Game;
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

        public void Decorate(Floor floor, MapArea area, IGenerator rng)
        {
            MapArea restrictedArea = new MapArea();

            restrictedArea.Add(area.Positions);
            restrictedArea.Remove(area.Bounds.PerimeterPositions());

            Placeable.Place(floor, restrictedArea.RandomPosition(rng), rng);
        }

        public bool CanDecorate(Floor floor, MapArea area)
        {
            return area.Count > 0;
        }
    }
}
