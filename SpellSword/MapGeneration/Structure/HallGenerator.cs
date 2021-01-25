using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoRogue;
using GoRogue.MapGeneration;
using GoRogue.MapViews;
using SpellSword.Game;
using SpellSword.MapGeneration.Sources;
using Troschuetz.Random;

namespace SpellSword.MapGeneration.Structure
{
    class HallGenerator: IHallGenerator
    {
        private IPlaceable _wall;
        private IPlaceable _floor;
        private Source<GenerationContext, IRoomGenerator> _endPossibilities;

        public HallGenerator(IPlaceable wall, IPlaceable floor, Source<GenerationContext, IRoomGenerator> endPossibilities)
        {
            _wall = wall;
            _floor = floor;
            _endPossibilities = endPossibilities;
        }

        public IRoom Generate(Floor floor, Coord connectFrom, Coord connectTo, IGenerator rng)
        {
            _floor.Place(floor, connectFrom, rng);
            _floor.Place(floor, connectTo, rng);

            List<Coord> path = HorizontalVerticalPath(connectFrom, connectTo).ToList();

            MapArea area = new MapArea();

            for (int i = 1; i < path.Count - 1; i++)
            {
                Coord current = path[i];
                Coord next = path[i + 1];
                Coord prev = path[i - 1];

                area.Add(current);

                if (floor.MapInfo.Map.Terrain[current] != null && floor.MapInfo.Map.Terrain[current].IsWalkable)
                    continue;

                _floor.Place(floor, current, rng);


                HashSet<Coord> wallOptions = new HashSet<Coord> { current + Direction.UP, current + Direction.RIGHT, current + Direction.DOWN, current + Direction.LEFT };
                wallOptions.Remove(prev);
                wallOptions.Remove(next);

                foreach (Coord pos in wallOptions)
                {
                    if (floor.MapInfo.Map.Terrain[pos] == null)
                        _wall.Place(floor, pos, rng);
                }
            }

            Direction finalDirection = Direction.GetCardinalDirection(path[path.Count - 2], path[path.Count - 1]);
            RoomConnection finalConnection = new RoomConnection(connectTo, finalDirection, _endPossibilities, false);


            return new BasicRoom(area, new List<RoomConnection>{finalConnection}, null);
        }

        public bool CanGenerate(Floor floor, Coord connectFrom, Coord connectTo, IGenerator rng)
        {
            return floor.MapInfo.Map.Terrain.Contains(connectFrom) && floor.MapInfo.Map.Terrain.Contains(connectTo);
        }

        private static IEnumerable<Coord> HorizontalVerticalPath(Coord start, Coord end)
        {
            foreach (Coord c in Lines.Get(start.X, start.Y, end.X, start.Y, Lines.Algorithm.ORTHO))
                yield return c;

            foreach (Coord c in Lines.Get(end.X, start.Y, end.X, end.Y, Lines.Algorithm.ORTHO).Skip(1))
                yield return c;
        }
    }
}
