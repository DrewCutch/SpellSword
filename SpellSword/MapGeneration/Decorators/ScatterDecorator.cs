using System;
using System.Collections.Generic;
using System.Text;
using GoRogue.MapGeneration;
using Troschuetz.Random;

namespace SpellSword.MapGeneration.Decorators
{
    class ScatterDecorator: IAreaDecorator
    {
        public IPlaceable Placeable { get; }

        public int MinAmount { get; }

        public int MaxAmount { get; }

        public ScatterDecorator(IPlaceable placeable, int minAmount, int maxAmount)
        {
            Placeable = placeable;
            MinAmount = minAmount;
            MaxAmount = maxAmount;
        }

        public void Decorate(MapInfo mapInfo, MapArea area, IGenerator rng)
        {
            MapArea restrictedArea = new MapArea();

            restrictedArea.Add(area.Positions);
            restrictedArea.Remove(area.Bounds.PerimeterPositions());

            int amount = Math.Min(rng.Next(MinAmount, MaxAmount + 1), restrictedArea.Count);
            int amountPlaced = 0;

            while (amountPlaced < amount)
            {
                if (Placeable.Place(mapInfo, restrictedArea.RandomPosition(rng), rng))
                    amountPlaced += 1;
            }
        }

        public bool CanDecorate(MapInfo mapInfo, MapArea area)
        {
            return area.Count > 0;
        }
    }
}
