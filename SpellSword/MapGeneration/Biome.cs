using System;
using System.Collections.Generic;
using System.Text;
using GoRogue.GameFramework;
using GoRogue.MapGeneration;
using GoRogue.MapViews;
using Troschuetz.Random;
using static System.Linq.Enumerable;

namespace SpellSword.MapGeneration
{
    class Biome: IBiome
    {
        private const int DECORATION_RETRYS = 10;

        private WeightedRandomBag<IAreaDecorator> _decorators;

        public Biome(WeightedRandomBag<IAreaDecorator> decorators)
        {
            _decorators = decorators;
        }

        public IMapView<GameObject> GenerateTerrain(IGenerator rng)
        {
            throw new NotImplementedException();
        }

        public void Populate(MapInfo mapInfo, MapArea area, IGenerator rng)
        {
            double areaCoefficient = area.Count * rng.NextDouble(1.0, 2.0);

            int numDecorators = Math.Max(1, (int) (areaCoefficient / 100));

            foreach (int _ in Range(0, numDecorators))
            {
                IAreaDecorator decorator = _decorators.Get(rng);

                // Retry getting decorator if it cannot decorate the area
                for (int i = 0; !decorator.CanDecorate(mapInfo, area) && i < DECORATION_RETRYS; i++)
                    decorator = _decorators.Get();

                decorator.Decorate(mapInfo, area, rng);
            }
        }
    }
}
