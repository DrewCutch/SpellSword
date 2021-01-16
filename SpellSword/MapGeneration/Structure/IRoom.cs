using System.Collections.Generic;
using GoRogue;
using GoRogue.MapGeneration;

namespace SpellSword.MapGeneration.Structure
{
    interface IRoom
    {
        public IRoomGenerator GeneratedBy { get; }

        public IReadOnlyList<RoomConnection> PotentialConnections { get; }
        public int OpenConnections { get; }
        public MapArea Area { get; }

        public bool UseConnection(RoomConnection connection);
    }
}
