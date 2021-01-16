using System;
using System.Collections.Generic;
using System.Text;
using GoRogue;
using GoRogue.MapGeneration;
using GoRogue.MapGeneration.Connectors;
using SpellSword.MapGeneration.Decorators;
using SpellSword.MapGeneration.Sources;
using SpellSword.Util;
using Troschuetz.Random;

namespace SpellSword.MapGeneration.Structure
{
    class RectRoomGenerator: IRoomGenerator
    {
        public Source<IRoomGenerator> NeighborPossibilities { get; set; }
        public Source<IAreaDecorator> Decorators { get; set; }

        private Rectangle _minBounds;
        private Rectangle _maxBounds;

        private IPlaceable _wall;
        private IPlaceable _floor;

        public RectRoomGenerator(Rectangle minBounds, Rectangle maxBounds, IPlaceable wall, IPlaceable floor)
        {
            _minBounds = minBounds;
            _maxBounds = maxBounds;

            _wall = wall;
            _floor = floor;

            Decorators = new PrioritySource<IAreaDecorator>(false);
            NeighborPossibilities = new PrioritySource<IRoomGenerator>(false);
        }

        public IRoom Generate(MapInfo mapInfo, RoomConnection connectAt, IGenerator rng)
        {
            Rectangle roomBounds = new Rectangle(0, 0, rng.Next(_minBounds.Width, _maxBounds.Width), rng.Next(_minBounds.Height, _maxBounds.Height));

            Coord offset = connectAt.Direction.Type switch
            {
                Direction.Types.LEFT => new Coord(-roomBounds.Width + 1, -roomBounds.Height / 2),
                Direction.Types.RIGHT => new Coord(0, -roomBounds.Height / 2),
                Direction.Types.UP => new Coord(-roomBounds.Width / 2, -roomBounds.Height + 1),
                Direction.Types.DOWN => new Coord(-roomBounds.Width / 2, 0),
                Direction.Types.NONE => new Coord(-roomBounds.Height / 2, -roomBounds.Width / 2),
                _ => throw new ArgumentOutOfRangeException()
            };

            roomBounds = roomBounds.WithPosition(offset + connectAt.Position);


            foreach (Coord pos in roomBounds.PerimeterPositions())
            {
                _wall.Place(mapInfo, pos, rng);
            }

            foreach (Coord pos in roomBounds.ChangeSize(-2, -2).WithCenter(roomBounds.Center).Positions())
            {
                _floor.Place(mapInfo, pos, rng);
            }

            MapArea area = new MapArea();
            area.Add(roomBounds);

            List<RoomConnection> connections = new List<RoomConnection>();

            foreach (Direction dir in AdjacencyRule.CARDINALS.DirectionsOfNeighbors())
            {
                if (dir.DeltaX == -connectAt.Direction.DeltaX && dir.DeltaY == -connectAt.Direction.DeltaY)
                    continue;

                int xOffset = dir.DeltaX * (roomBounds.Width - 1) / 2;
                int yOffset = dir.DeltaY * (roomBounds.Height - 1) / 2;

                if (dir.DeltaX == 0)
                    xOffset += rng.Next(-roomBounds.Width / 2 + 1, roomBounds.Width / 2 - 1);

                if (dir.DeltaY == 0)
                    yOffset += rng.Next(-roomBounds.Height / 2 + 1, roomBounds.Height / 2 - 1);

                Coord door = roomBounds.Center + new Coord(xOffset, yOffset);
                connections.Add(new RoomConnection(door, dir, NeighborPossibilities, true));
                //_floor.Place(mapInfo, door, rng);
            }

            if(connectAt.Position != Coord.NONE)
                _floor.Place(mapInfo, connectAt.Position, rng);

            return new BasicRoom(area, connections, this);
        }

        public bool CanGenerate(MapInfo mapInfo, RoomConnection connectAt, IGenerator rng)
        {
            Rectangle roomBounds = new Rectangle(0, 0, rng.Next(_minBounds.Width, _maxBounds.Width), rng.Next(_minBounds.Height, _maxBounds.Height));

            Coord offset = connectAt.Direction.Type switch
            {
                Direction.Types.LEFT => new Coord(-roomBounds.Width + 1, -roomBounds.Height / 2),
                Direction.Types.RIGHT => new Coord(0, -roomBounds.Height / 2),
                Direction.Types.UP => new Coord(-roomBounds.Width / 2, -roomBounds.Height + 1),
                Direction.Types.DOWN => new Coord(-roomBounds.Width / 2, 0),
                _ => throw new ArgumentOutOfRangeException()
            };

            roomBounds = roomBounds.WithPosition(offset + connectAt.Position);

            return mapInfo.Map.Terrain.AllEmpty(roomBounds);
        }

        public List<Hook> Populate(MapInfo mapInfo, IRoom room, IGenerator rng)
        {
            Source<IAreaDecorator> individualSource = Decorators.Clone();

            int numDecorators = Math.Max(room.Area.Count / 20 + rng.Next(0, 2), 3);
            List<Hook> hooks = new List<Hook>();

            for (int n = 0; n < numDecorators; n++)
            {
                if (individualSource.IsEmpty())
                    return hooks;

                SourceCursor<IAreaDecorator> decoratorCursor = individualSource.Pull(rng);

                if (!decoratorCursor.Value.CanDecorate(mapInfo, room.Area)) 
                    continue;

                decoratorCursor.Value.Decorate(mapInfo, room.Area, rng);
                decoratorCursor.Use();
            }

            return hooks;
        }
    }
}
