using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace SpellSword.MapGeneration
{
    class BiomeBuilder
    {
        private readonly List<(IAreaDecorator, int)> _decoratorWeights;

        public BiomeBuilder()
        {
            _decoratorWeights = new List<(IAreaDecorator, int)>();
        }

        public BiomeBuilder WithDecorator(IAreaDecorator decorator, int weight)
        {
            _decoratorWeights.Add((decorator, weight));
            return this;
        }

        public Biome GetBiome()
        {
            WeightedRandomBag<IAreaDecorator> decorators = new WeightedRandomBag<IAreaDecorator>(_decoratorWeights);
            return new Biome(decorators);
        }
    }
}
