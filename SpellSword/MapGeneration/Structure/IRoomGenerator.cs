using System.Collections.Generic;
using GoRogue;
using SpellSword.Game;
using SpellSword.MapGeneration.Decorators;
using SpellSword.MapGeneration.Sources;
using Troschuetz.Random;

namespace SpellSword.MapGeneration.Structure
{
    interface IRoomGenerator
    {
        Source<GenerationContext, IRoomGenerator> NeighborPossibilities { get; set; }

        Source<GenerationContext, IAreaDecorator> Decorators { get; set; }

        public IRoom Generate(Floor floor, RoomConnection connectAt, IGenerator rng);

        public bool CanGenerate(Floor floor, RoomConnection connectAt, IGenerator rng);

        public List<Hook> Populate(Floor floor, IRoom room, IGenerator rng);
    }
}
