using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using GoRogue;
using GoRogue.MapGeneration;
using SpellSword.Game;
using SpellSword.MapGeneration.Decorators;
using SpellSword.MapGeneration.Sources;
using SpellSword.Util;
using Troschuetz.Random;

namespace SpellSword.MapGeneration.Structure
{
    class RectCompositeRoomGenerator: IRoomGenerator
    {
        public Source<GenerationContext, IRoomGenerator> NeighborPossibilities { get; set; }
        public Source<GenerationContext, IAreaDecorator> Decorators { get; set; }

        public int MinRects { get; }
        public int MaxRects { get; }
        public Rectangle MinBounds { get; }
        public Rectangle MaxBounds { get; }

        private IPlaceable _wall;

        private IPlaceable _floor;
        

        public RectCompositeRoomGenerator(int minRects, int maxRects, Rectangle minBounds, Rectangle maxBounds, IPlaceable wall, IPlaceable floor)
        {
            MinRects = minRects;
            MaxRects = maxRects;
            MinBounds = minBounds;
            MaxBounds = maxBounds;

            _wall = wall;
            _floor = floor;

            Decorators = new PrioritySource<GenerationContext, IAreaDecorator>(true);
            NeighborPossibilities = new PrioritySource<GenerationContext, IRoomGenerator>(true);
        }

        public IRoom Generate(Floor floor, RoomConnection connectAt, IGenerator rng)
        {
            MapArea area = GenerateArea(floor.MapInfo, connectAt, rng);

            HashSet<Coord> perimeter = new HashSet<Coord>(area.Perimeter());

            foreach (Coord pos in area.Positions)
            {
                if (perimeter.Contains(pos))
                    _wall.Place(floor, pos, rng);
                else
                    _floor.Place(floor, pos, rng);
            }

            List<Coord> perimeterList = new List<Coord>(perimeter);
            List<RoomConnection> connections = new List<RoomConnection>();
            HashSet<Direction> usedDirections = new HashSet<Direction>();

            for (int n = 0; n < 20 && connections.Count < 4; n++)
            {
                Coord connectionPos = perimeterList[rng.Next(perimeterList.Count)];
                
                if (IsCorner(connectionPos, area))
                    continue;

                Direction dir = GetDirection(connectionPos, area);

                if (usedDirections.Contains(dir))
                    continue;

                connections.Add(new RoomConnection(connectionPos, dir, NeighborPossibilities, true));
                usedDirections.Add(dir);
            }

            return new BasicRoom(area, connections, this);
        }

        private bool IsCorner(Coord pos, MapArea area)
        {
            Coord left = pos + Direction.LEFT;
            Coord right = pos + Direction.RIGHT;
            Coord up = pos + Direction.UP;
            Coord down = pos + Direction.DOWN;

            return !((area.Contains(left) && area.Contains(right)) || (area.Contains(up) && area.Contains(down)));
        }

        private Direction GetDirection(Coord pos, MapArea area)
        {
            Coord left = pos + Direction.LEFT;
            Coord right = pos + Direction.RIGHT;
            Coord up = pos + Direction.UP;
            Coord down = pos + Direction.DOWN;

            if (area.Contains(up) && area.Contains(down))
                return area.Contains(right) ? Direction.LEFT : Direction.RIGHT;
            else
                return area.Contains(up) ? Direction.DOWN : Direction.UP;
        }

        public bool CanGenerate(Floor floor, RoomConnection connectAt, IGenerator rng)
        {
            MapArea mapArea = GenerateArea(floor.MapInfo, connectAt, rng);

            return floor.MapInfo.Map.Terrain.AllEmpty(mapArea.Positions);
        }

        private MapArea GenerateArea(MapInfo mapInfo, RoomConnection connectAt, IGenerator rng)
        {
            MapArea area = new MapArea();
            int numRects = rng.Next(MinRects, MaxRects + 1);

            List<Rectangle> rects = new List<Rectangle>();

            Rectangle firstRect = new Rectangle(0, 0, rng.Next(MinBounds.Width, MaxBounds.Width), rng.Next(MinBounds.Height, MaxBounds.Height));

            Coord offset = connectAt.Direction.Type switch
            {
                Direction.Types.LEFT => new Coord(-firstRect.Width + 1, -firstRect.Height / 2),
                Direction.Types.RIGHT => new Coord(0, -firstRect.Height / 2),
                Direction.Types.UP => new Coord(-firstRect.Width / 2, -firstRect.Height + 1),
                Direction.Types.DOWN => new Coord(-firstRect.Width / 2, 0),
                Direction.Types.NONE => new Coord(-firstRect.Height / 2, -firstRect.Width / 2),
                _ => throw new ArgumentOutOfRangeException()
            };

            firstRect = firstRect.WithPosition(offset + connectAt.Position);
           
            rects.Add(firstRect);
            area.Add(firstRect);

            int fails = 0;
            while (rects.Count < numRects && fails < 200)
            {
                Rectangle previousRectangle = rects[^1];
                Rectangle nextRectangle = new Rectangle(previousRectangle.MinExtentX, previousRectangle.MinExtentY, rng.Next(MinBounds.Width, MaxBounds.Width), rng.Next(MinBounds.Height, MaxBounds.Height));

                Coord prevPos = previousRectangle.RandomPosition(rng);
                Coord rectPos = nextRectangle.RandomPosition(rng);

                Coord translate = rectPos - prevPos;
                nextRectangle = nextRectangle.Translate(translate);

                if (!mapInfo.Map.Terrain.AllEmpty(nextRectangle.Positions()))
                {
                    fails += 1;
                    continue;
                }

                area.Add(nextRectangle);
                rects.Add(nextRectangle);
            }

            return area;
        }


        public List<Hook> Populate(Floor floor, IRoom room, IGenerator rng)
        {
            Source<GenerationContext, IAreaDecorator> individualSource = Decorators.Clone();

            int numDecorators = Math.Max(room.Area.Count / 20 + rng.Next(0, 2), 3);
            List<Hook> hooks = new List<Hook>();

            for (int n = 0; n < numDecorators; n++)
            {
                if (individualSource.IsEmpty())
                    return hooks;

                SourceCursor<IAreaDecorator> decoratorCursor = individualSource.Pull(new GenerationContext(rng));

                if (!decoratorCursor.Value.CanDecorate(floor, room.Area))
                    continue;

                decoratorCursor.Value.Decorate(floor, room.Area, rng);
                decoratorCursor.Use();
            }

            return hooks;
        }
    }
}
