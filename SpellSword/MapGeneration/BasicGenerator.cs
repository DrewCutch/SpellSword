﻿using System;
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
using SpellSword.Engine.Components;
using SpellSword.Render;
using Rectangle = GoRogue.Rectangle;

namespace SpellSword.MapGeneration
{
    class BasicGenerator: IMapGenerator
    {
        public Map Generate(int width, int height)
        {
            return new Map(CreateTerrain(width, height), 3, Distance.MANHATTAN);
        }

        private ISettableMapView<IGameObject> CreateTerrain(int width, int height)
        {
            ISettableMapView<IGameObject> terrain = new ArrayMap<IGameObject>(width, height);
                

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
            }
            //MapArea area1 = CreateArea(terrain, new Rectangle(0, 0, 10, 20));
            //MapArea area2 = CreateArea(terrain, new Rectangle(100, 10, 10, 10));

            /*
            areas.Sort((a1, a2) => ManMag(a1.Bounds.Center).CompareTo(ManMag(a2.Bounds.Center)));
            for (int i = 0; i < areas.Count - 2; i++)
            {
                CarveTunnel(areas[i], areas[i + 1], terrain);
                CarveTunnel(areas[i], areas[i + 2], terrain);
            }
            */

            foreach (MapArea area in areas)
            {
                List<MapArea> closest = areas.ToList();
                closest.Sort((a1, a2) => 
                    Distance.MANHATTAN.Calculate(a1.Bounds.Position, area.Bounds.Position)
                        .CompareTo(Distance.MANHATTAN.Calculate(a2.Bounds.Position, area.Bounds.Position)));

                for (int i = 0; i < 2; i++)
                {
                    CarveTunnel(area, closest.Skip(i + 1).First(), terrain);
                }
            }

            //CarveTunnel(area1, area2, terrain);

            return terrain;
        }

        private static int ManMag(Coord coord)
        {
            return coord.X + coord.Y;
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
                IGameObject obstacle = new GameObject(position, Layer.Low.MapNum, null, true, false, false);
                obstacle.AddComponent(new GlyphComponent(new Glyph(Color.DimGray)));

                mapView[position] = obstacle;
            }

            area.Add(new Rectangle(dimensions.MinExtent + new Coord(1, 1), dimensions.MaxExtent - new Coord(1, 1)).Positions());

            return area;
        }

        private static IGameObject CreateWall(Coord pos)
        {
            IGameObject wall = new GameObject(pos, Layer.Low.MapNum, null, true, false, false);
            wall.AddComponent(new GlyphComponent(new Glyph(Color.DimGray)));
            return wall;
        }

        private static IGameObject CreateFloor(Coord pos)
        {
            IGameObject floor = new GameObject(pos, Layer.Low.MapNum, null, true);


            int color = SingletonRandom.DefaultRNG.Next(160, 170);
            Color tileColor = Color.FromArgb(color, color, color);

            GlyphComponent glyphComponent = new GlyphComponent(new Glyph('█', tileColor, tileColor));
            floor.AddComponent(glyphComponent);
            floor.AddComponent(new DecalComponent(glyphComponent));

            return floor;
        }
    }
}