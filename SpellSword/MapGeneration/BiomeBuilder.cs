using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using SpellSword.MapGeneration.Sources;
using SpellSword.MapGeneration.Structure;

namespace SpellSword.MapGeneration
{
    class BiomeBuilder
    {
        private readonly List<(IAreaDecorator, int)> _decoratorWeights;

        private Source<GenerationContext, IRoomGenerator> _rootRoomSource;

        private Source<GenerationContext, IHallGenerator> _hallGeneratorSource;

        public BiomeBuilder()
        {
            _decoratorWeights = new List<(IAreaDecorator, int)>();
        }

        public BiomeBuilder WithDecorator(IAreaDecorator decorator, int weight)
        {
            _decoratorWeights.Add((decorator, weight));
            return this;
        }

        public BiomeBuilder WithRoomSource(Source<GenerationContext, IRoomGenerator> rootRoomSource)
        {
            _rootRoomSource = rootRoomSource;

            return this;
        }

        public BiomeBuilder WithHallSource(Source<GenerationContext, IHallGenerator> hallSource)
        {
            _hallGeneratorSource = hallSource;

            return this;
        }

        public Biome GetBiome()
        {
            WeightedRandomBag<IAreaDecorator> decorators = new WeightedRandomBag<IAreaDecorator>(_decoratorWeights);
            return new Biome(decorators, _rootRoomSource, _hallGeneratorSource);
        }
    }
}
