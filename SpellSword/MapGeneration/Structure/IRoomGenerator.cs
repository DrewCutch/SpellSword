using System.Collections.Generic;
using GoRogue;
using SpellSword.MapGeneration.Decorators;
using SpellSword.MapGeneration.Sources;
using Troschuetz.Random;

namespace SpellSword.MapGeneration.Structure
{
    interface IRoomGenerator
    {
        Source<IRoomGenerator> NeighborPossibilities { get; set; }

        Source<IAreaDecorator> Decorators { get; set; }

        public IRoom Generate(MapInfo mapInfo, RoomConnection connectAt, IGenerator rng);

        public bool CanGenerate(MapInfo mapInfo, RoomConnection connectAt, IGenerator rng);

        public List<Hook> Populate(MapInfo mapInfo, IRoom room, IGenerator rng);
    }
}
