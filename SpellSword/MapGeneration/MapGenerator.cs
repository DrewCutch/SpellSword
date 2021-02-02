using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using GoRogue;
using GoRogue.GameFramework;
using GoRogue.Messaging;
using GoRogue.Random;
using SpellSword.Actors.Pathing;
using SpellSword.Engine;
using SpellSword.Engine.Components;
using SpellSword.Game;
using SpellSword.MapGeneration.Resources;
using SpellSword.MapGeneration.Sources;
using SpellSword.MapGeneration.Structure;
using SpellSword.Render;
using SpellSword.Render.Lighting;
using SpellSword.Speech;
using SpellSword.Time;
using SpellSword.Util;
using SpellSword.Util.Collections;
using Troschuetz.Random;
using Troschuetz.Random.Generators;
using Rectangle = GoRogue.Rectangle;

namespace SpellSword.MapGeneration
{
    class MapGenerator: IMapGenerator
    {
        private IBiome _biome = Biomes.TestBiome;

        private Timeline _timeline;
        private MessageBus _messageBus;

        public MapGenerator(Timeline timeline, MessageBus messageBus)
        {
            _timeline = timeline;
            _messageBus = messageBus;
        }

        public IEnumerable<MapInfo> GenerationSteps(int width, int height, string seed)
        {
            Map map = new Map(width, height, Layers.Effects + 1, Distance.MANHATTAN);

            LightMap lightMap = new LightMap(width, height, map.TransparencyView);

            MapInfo mapInfo = new MapInfo(map, lightMap);

            ResettableRandom rng = new ResettableRandom(seed);

            return _biome.GenerateOn(new Floor(0, _timeline, mapInfo, new Spawner(map), new GoalMapStore(map), _messageBus), new Rectangle(0, 0, width, height), rng);
        }

        public MapInfo Generate(int width, int height, string seed)
        {
            return GenerationSteps(width, height, seed).Last();
        }

        private static IGameObject SolidRock(Coord pos)
        {
            IGameObject wall = new GameObject(pos, Layers.Terrain, null, true, false, false);
            wall.AddComponent(new GlyphComponent(new Glyph(Characters.POUND_SIGN, Color.Black, Color.Black)));
            wall.AddComponent(new NameComponent(new Title("", "solid rock")));

            return wall;
        }
    }
}
