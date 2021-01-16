using System;
using System.Collections.Generic;
using System.Text;
using GoRogue;
using GoRogue.MapGeneration;

namespace SpellSword.MapGeneration.Structure
{
    class BasicRoom: IRoom
    {
        private readonly List<RoomConnection> _potentialConnections;
        public IRoomGenerator GeneratedBy { get; }
        public IReadOnlyList<RoomConnection> PotentialConnections => _potentialConnections;
        public int OpenConnections => PotentialConnections.Count;
        public MapArea Area { get; }

        public BasicRoom(MapArea area, List<RoomConnection> potentialConnections, IRoomGenerator generatedBy)
        {
            Area = area;
            _potentialConnections = potentialConnections;
            GeneratedBy = generatedBy;
        }

        public bool UseConnection(RoomConnection connection)
        {
            return _potentialConnections.Remove(connection);
        }
    }
}
