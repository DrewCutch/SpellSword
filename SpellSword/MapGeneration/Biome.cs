using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Xml.XPath;
using GoRogue;
using GoRogue.GameFramework;
using GoRogue.MapGeneration;
using GoRogue.MapViews;
using GoRogue.Random;
using SpellSword.Engine;
using SpellSword.Engine.Components;
using SpellSword.Game;
using SpellSword.MapGeneration.Sources;
using SpellSword.MapGeneration.Structure;
using SpellSword.Render;
using SpellSword.Render.Lighting;
using SpellSword.Speech;
using SpellSword.Util;
using SpellSword.Util.Collections;
using Troschuetz.Random;
using static System.Linq.Enumerable;
using Rectangle = GoRogue.Rectangle;

namespace SpellSword.MapGeneration
{
    class Biome: IBiome
    {
        private const int DECORATION_RETRYS = 10;

        private WeightedRandomBag<IAreaDecorator> _decorators;

        private Source<GenerationContext, IRoomGenerator> _rootRoomSource;

        private Source<GenerationContext, IHallGenerator> _hallGenerators;

        public Biome(WeightedRandomBag<IAreaDecorator> decorators, Source<GenerationContext, IRoomGenerator> rootRoomSource, Source<GenerationContext, IHallGenerator> hallGenerators)
        {
            _decorators = decorators;
            _rootRoomSource = rootRoomSource;
            _hallGenerators = hallGenerators;
        }

        public IEnumerable<MapInfo> GenerateOn(Floor floor, Rectangle area, ResettableRandom rng)
        {
            GenerationContext context = new GenerationContext(rng);
            MapInfo mapInfo = floor.MapInfo;

            Bag<RoomConnection> pendingConnections = new Bag<RoomConnection>(rng);
            List<IRoom> rooms = new List<IRoom>();

            Rectangle initialRoomRect = new Rectangle(new Coord(20, 20), 3, 3);
            SourceCursor<IRoomGenerator> initialSourceCursor = _rootRoomSource.Pull(context);
            IRoomGenerator initialRoomGenerator = initialSourceCursor.Value;
            initialSourceCursor.Use();

            IRoom initialRoom = initialRoomGenerator.Generate(floor, new RoomConnection(new Coord(area.Width / 4, area.Height / 2), Direction.NONE, null, false), rng);

            floor.Entrance = initialRoom.Area.Bounds.Center;

            pendingConnections.PutRange(initialRoom.PotentialConnections);

            yield return mapInfo;

            int n = 0;
            while (pendingConnections.Count > 0 && n < 1000)
            {
                RoomConnection nextConnection = pendingConnections.Get(true);

                Coord hallEnd = nextConnection.Position +
                                new Coord(nextConnection.Direction.DeltaX, nextConnection.Direction.DeltaY) * rng.Next(2, 5);

                RoomConnection hallConnection = new RoomConnection(hallEnd, nextConnection.Direction, null, false);

                if (nextConnection.Possibilities.IsEmpty())
                {
                    n += 1;
                    continue;
                }

                SourceCursor<IRoomGenerator> generatorCursor = nextConnection.Possibilities.Pull(context);

                IRoomGenerator generator = generatorCursor.Value;

                int[] rngState = rng.GetState();

                if (!generator.CanGenerate(floor, hallConnection, rng))
                {
                    pendingConnections.Put(nextConnection);
                    n += 1;
                    continue;
                }

                rng.LoadState(rngState);

                IRoom room = generator.Generate(floor, hallConnection, rng);
                rooms.Add(room);

                generatorCursor.Use();

                pendingConnections.PutRange(room.PotentialConnections);

                yield return mapInfo;


                SourceCursor<IHallGenerator> hallGeneratorCursor = _hallGenerators.Pull(context);;
                IHallGenerator hallGenerator = hallGeneratorCursor.Value;

                if (!hallGenerator.CanGenerate(floor, nextConnection.Position, hallEnd, rng))
                    continue;

                IRoom hall = hallGenerator.Generate(floor, nextConnection.Position, hallEnd, rng);
                hallGeneratorCursor.Use();

                yield return mapInfo;

                n += 1;
            }

            while (pendingConnections.Count > 0)
            {
                RoomConnection nextConnection = pendingConnections.Get(true);
                Coord hit = Trace(mapInfo.Map, nextConnection.Position, nextConnection.Direction);

                if (hit == Coord.NONE)
                    continue;

                if(Distance.MANHATTAN.Calculate(nextConnection.Position, hit) > 10)
                    continue;

                if(mapInfo.Map.AStar.ShortestPath(nextConnection.Position, hit).Length < 40)
                    continue;

                SourceCursor<IHallGenerator> hallGeneratorCursor = _hallGenerators.Pull(context); ;
                IHallGenerator hallGenerator = hallGeneratorCursor.Value;

                if (!hallGenerator.CanGenerate(floor, nextConnection.Position, hit, rng))
                    continue;

                IRoom hall = hallGenerator.Generate(floor, nextConnection.Position, hit, rng);
                hallGeneratorCursor.Use();


                yield return mapInfo;
            }

            foreach (Coord pos in area.Positions())
            {
                if (mapInfo.Map.GetTerrain(pos) == null)
                    mapInfo.Map.SetTerrain(SolidRock(pos));
            }

            Console.WriteLine($"Next random: {rng.Next()}");

            Console.WriteLine($"num rooms: {rooms.Count}");

            foreach (IRoom room in rooms)
            {
                room.GeneratedBy.Populate(floor, room, rng);
                int nextRNG = rng.Next();
                Console.WriteLine($"Next random: {nextRNG}");
            }

            

            // place exit
            while (true)
            {
                Coord randomPos = new Coord(rng.Next(0, area.Width), rng.Next(0, area.Height));
                if (mapInfo.Map.GetTerrain(randomPos) is IGameObject terrain && terrain.IsWalkable)
                {
                    mapInfo.Map.SetTerrain(Exit(floor, randomPos));
                    floor.Exit = randomPos;
                    break;
                }
            }

            Console.WriteLine($"leftover connections  = {pendingConnections.Count}");
        }

        private static Coord Trace(Map map, Coord origin, Direction direction)
        {
            Coord pos = origin + direction;

            while (map.Terrain.Contains(pos) && (map.Terrain[pos] == null || !map.WalkabilityView[pos]))
            {
                pos += direction;
            }

            if(!map.Terrain.Contains(pos))
                return Coord.NONE;

            return pos;
        }

        private static IGameObject SolidRock(Coord pos)
        {
            IGameObject wall = new GameObject(pos, Layers.Terrain, null, true, false, false);
            wall.AddComponent(new GlyphComponent(new Glyph(Characters.POUND_SIGN, Color.Black, Color.Black)));
            wall.AddComponent(new NameComponent(new Title("", "solid rock")));

            return wall;
        }

        private static IGameObject Exit(Floor floor, Coord pos)
        {
            IGameObject exit = new GameObject(pos, Layers.Terrain, null, true, true, true);
            exit.AddComponent(new GlyphComponent(new Glyph(Characters.GREATER_THAN, Color.Yellow, Color.DarkGoldenrod)));
            exit.AddComponent(new NameComponent(new Title("the", "exit")));
            exit.AddComponent(new TriggerComponent((gameobject, dir) => { floor.MessageBus.Send(new FloorTransitionEvent(gameobject, floor.Index, floor.Index + 1)); }, (_, __) => { }));
            exit.AddComponent(new LightSourceComponent(floor.MapInfo.LightMap, new Light(Color.Yellow, new Coord(0,0), 3, 5)));
            return exit;

        }

        public void Populate(Floor floor, MapArea area, IGenerator rng)
        {
            int areaCoefficient = (area.Count * rng.Next(10, 20)) / 1000;

            int numDecorators = Math.Max(1, areaCoefficient);

            foreach (int _ in Range(0, numDecorators))
            {
                IAreaDecorator decorator = _decorators.Get(rng);

                // Retry getting decorator if it cannot decorate the area
                for (int i = 0; !decorator.CanDecorate(floor, area) && i < DECORATION_RETRYS; i++)
                    decorator = _decorators.Get();

                decorator.Decorate(floor, area, rng);
            }
        }
    }
}
