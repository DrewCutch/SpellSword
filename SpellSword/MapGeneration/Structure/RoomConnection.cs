using System;
using System.Collections.Generic;
using System.Text;
using GoRogue;
using SpellSword.MapGeneration.Sources;

namespace SpellSword.MapGeneration.Structure
{
    class RoomConnection
    {
        public Coord Position { get; }
        public Direction Direction { get; }

        public Source<IRoomGenerator> Possibilities { get; }

        public bool UseHall { get; }

        public RoomConnection(Coord position, Direction direction, Source<IRoomGenerator> possibilities, bool useHall)
        {
            Position = position;
            Direction = direction;
            Possibilities = possibilities;
            UseHall = useHall;
        }
    }
}
