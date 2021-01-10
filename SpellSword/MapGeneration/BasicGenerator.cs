using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using GoRogue;
using GoRogue.GameFramework;
using GoRogue.MapGeneration;
using GoRogue.MapGeneration.Connectors;
using GoRogue.MapViews;
using GoRogue.Pathing;
using GoRogue.Random;
using SpellSword.Engine;
using SpellSword.Engine.Components;
using SpellSword.Render;
using SpellSword.Render.Lighting;
using SpellSword.Speech;
using Rectangle = GoRogue.Rectangle;

namespace SpellSword.MapGeneration
{
    class BasicGenerator: IMapGenerator
    {
        public IEnumerable<MapInfo> GenerationSteps(int width, int height)
        {
            return CreateTerrain(width, height);
        }

        public MapInfo Generate(int width, int height)
        {
            return GenerationSteps(width, height).Last();
        }

        private IEnumerable<MapInfo> CreateTerrain(int width, int height)
        {
            Map map = new Map(width, height, Layers.Effects + 1, Distance.MANHATTAN);
            LightMap lightMap = new LightMap(width, height, map.TransparencyView);

            MapInfo mapInfo = new MapInfo(map, lightMap);

            ISettableMapView<IGameObject> terrain = new LambdaSettableMapView<IGameObject>(width, height, (pos) => map.GetTerrain(pos),
                (pos, gameObject) => { gameObject.Position = pos; map.SetTerrain(gameObject);});
            //ISettableMapView<IGameObject> terrain = new ArrayMap<IGameObject>(width, height);
            
            List<MapArea> areas = new List<MapArea>();

            areas.Add(CreateArea(terrain, new Rectangle(5, 5, 10, 10)));

            for (int i = 0; i < 1000; i++)
            {
                int xPos = SingletonRandom.DefaultRNG.Next(width);
                int yPos = SingletonRandom.DefaultRNG.Next(height);
                int rectWidth = SingletonRandom.DefaultRNG.Next(3, 10);
                int rectHeight = SingletonRandom.DefaultRNG.Next(3, 10);
                Rectangle roomRect = new Rectangle(new Coord(xPos, yPos), rectWidth, rectHeight);

                if (!roomRect.Positions().All(pos => terrain.Contains(pos) && terrain[pos] == null))
                    continue;

                areas.Add(CreateArea(terrain, roomRect));
                yield return mapInfo;
            }

            foreach (MapArea area in areas)
            {
                List<MapArea> closest = areas.ToList();
                closest.Sort((a1, a2) => 
                    Distance.MANHATTAN.Calculate(a1.Bounds.Position, area.Bounds.Position)
                        .CompareTo(Distance.MANHATTAN.Calculate(a2.Bounds.Position, area.Bounds.Position)));

                for (int i = 0; i < 2; i++)
                {
                    CarveTunnel(area, closest.Skip(i + 1).First(), terrain);
                    yield return mapInfo;
                }
            }

            //CarveTunnel(area1, area2, terrain);
            

            foreach (MapArea area in areas)
            {
                Decorate(mapInfo, area);

                yield return mapInfo;
            }

            yield return mapInfo;
        }

        private static void CarveTunnel(MapArea a, MapArea b, ISettableMapView<IGameObject> terrain)
        {
            Tuple<Coord, Coord> closest = new ClosestConnectionPointSelector(Distance.MANHATTAN).SelectConnectionPoints(a, b);
            terrain[closest.Item1] = CreateFloor(closest.Item1);
            terrain[closest.Item2] = CreateFloor(closest.Item2);

            List<Coord> tunnel = HorizontalVerticalPath(closest.Item1, closest.Item2).ToList(); //Lines.Get(closest.Item1, closest.Item2, Lines.Algorithm.ORTHO).ToList();

            for (int i = 1; i < tunnel.Count - 1; i++)
            {
                Coord current = tunnel[i];
                Coord next = tunnel[i + 1];
                Coord prev = tunnel[i - 1];

                if(terrain[current]!= null && terrain[current].IsWalkable)
                    continue;

                terrain[current] = CreateFloor(current);

                HashSet<Coord> wallOptions = new HashSet<Coord>{ current + Direction.UP, current + Direction.RIGHT, current + Direction.DOWN, current + Direction.LEFT};
                wallOptions.Remove(prev);
                wallOptions.Remove(next);

                foreach (Coord pos in wallOptions)
                {
                    if(terrain[pos] == null)
                        terrain[pos] = CreateWall(pos);
                }
            }
        }

        private static IEnumerable<Coord> HorizontalVerticalPath(Coord start, Coord end)
        {
            foreach (Coord c in Lines.Get(start.X, start.Y, end.X, start.Y, Lines.Algorithm.ORTHO))
                yield return c;

            foreach (Coord c in Lines.Get(end.X, start.Y, end.X, end.Y, Lines.Algorithm.ORTHO).Skip(1))
                yield return c;
        }

        private MapArea CreateArea(ISettableMapView<IGameObject> mapView, Rectangle dimensions)
        {
            MapArea area = new MapArea();

            foreach (Coord position in dimensions.Positions())
            {
                IGameObject floor = CreateFloor(position);
                mapView[position] = floor;


            }

            foreach (Coord position in dimensions.PerimeterPositions())
            {
                IGameObject obstacle = CreateWall(position);

                mapView[position] = obstacle;
            }

            area.Add(new Rectangle(dimensions.MinExtent + new Coord(1, 1), dimensions.MaxExtent - new Coord(1, 1)).Positions());

            return area;
        }

        private void Decorate(MapInfo mapInfo, MapArea area)
        {
            AreaDecorator testDecorator = new AreaDecorator();

            testDecorator.Decorate(mapInfo, area);
        }

        private static IGameObject CreateWall(Coord pos)
        {
            IGameObject wall = new GameObject(pos, Layers.Terrain, null, true, false, false);
            wall.AddComponent(new GlyphComponent(new Glyph('#', Color.DimGray, Color.DimGray)));
            wall.AddComponent(new NameComponent(new Title("a", "stone wall")));

            return wall;
        }

        private static IGameObject CreateFloor(Coord pos)
        {
            IGameObject floor = new GameObject(pos, Layers.Terrain, null, true);


            int color = SingletonRandom.DefaultRNG.Next(160, 170);
            Color tileColor = Color.FromArgb(color, color, color);

            GlyphComponent glyphComponent = new GlyphComponent(new Glyph(' ', tileColor, tileColor));
            floor.AddComponent(glyphComponent);
            floor.AddComponent(new DecalComponent());
            floor.AddComponent(new NameComponent(new Title("a", "stone floor")));

            return floor;
        }
    }
}
