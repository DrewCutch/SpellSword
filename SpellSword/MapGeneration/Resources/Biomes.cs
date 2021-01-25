using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using GoRogue;
using GoRogue.GameFramework;
using GoRogue.MapViews;
using GoRogue.Random;
using SpellSword.Engine;
using SpellSword.Engine.Components;
using SpellSword.MapGeneration.Sources;
using SpellSword.MapGeneration.Structure;
using SpellSword.Render;
using SpellSword.Render.Lighting;
using SpellSword.Speech;
using SpellSword.TestUtils;
using Rectangle = GoRogue.Rectangle;

namespace SpellSword.MapGeneration.Resources
{
    static class Biomes
    {
        private static Spreader GrassSpreader = new Spreader(
            (floor, pos) =>
            {
                GameObject grass = new GameObject(pos, Layers.Floor, null);
                grass.AddComponent(new GlyphComponent('"', Color.DarkGreen));
                grass.AddComponent(new NameComponent(new Title("in", "", "grass")));

                return grass;
            },
            100, .7f, .2f);

        
        private static Spreader TallGrassSpreader = new Spreader(
            (floor, pos) =>
            {
                GameObject grass = new GameObject(pos, Layers.Floor, null, isTransparent:false);
                grass.AddComponent(new GlyphComponent('⌠', Color.DarkGreen));
                grass.AddComponent(new NameComponent(new Title("in", "", "tall grass")));

                return grass;
            },
            50, .7f, .1f);

        
        private static Spreader GlowingMushroomsSpreader = new Spreader(
            (floor, pos) =>
            {
                GameObject grass = new GameObject(pos, Layers.Floor, null);
                grass.AddComponent(new GlyphComponent('"', Color.Cyan));
                grass.AddComponent(new LightSourceComponent(floor.MapInfo.LightMap, new Light(Color.Cyan, new Coord(0,0), 3, 7)));
                grass.AddComponent(new NameComponent(new Title("in", "", "mana mushrooms")));

                return grass;
            }, 15, .8f, .8f);

        private static Spreader GlowingFungusSpreader = new Spreader(
            (floor, pos) =>
            {
                int red = SingletonRandom.DefaultRNG.Next(220, 256);
                int green = SingletonRandom.DefaultRNG.Next(50, 100);
                int blue = SingletonRandom.DefaultRNG.Next(50, 100);
                Color color = Color.FromArgb(red, green, blue);

                GameObject grass = new GameObject(pos, Layers.Floor, null);
                grass.AddComponent(new GlyphComponent('"', color));
                grass.AddComponent(new LightSourceComponent(floor.MapInfo.LightMap, new Light(color, new Coord(0, 0), 4, 10)));
                grass.AddComponent(new NameComponent(new Title("in", "", "fire fungus")));

                return grass;
            }, 15, .8f, .8f);


        private static IPlaceable TorchPlacer = new Placable(
            (floor, pos) =>
        {
            Coord wallAway = new Coord(0, 0);

            foreach (Direction dir in AdjacencyRule.CARDINALS.DirectionsOfNeighbors())
            {
                if (floor.MapInfo.Map.Contains(pos + dir) && !(floor.MapInfo.Map.Terrain[pos + dir]?.IsWalkable ?? false))
                {
                    wallAway -= new Coord(dir.DeltaX, dir.DeltaY);
                }
            }

            if (wallAway.X == 0 && wallAway.Y == 0)
            {
                foreach (Direction dir in AdjacencyRule.DIAGONALS.DirectionsOfNeighbors())
                {
                    if (floor.MapInfo.Map.Contains(pos + dir) && !(floor.MapInfo.Map.Terrain[pos + dir]?.IsWalkable ?? false))
                    {
                        wallAway -= new Coord(dir.DeltaX, dir.DeltaY);
                    }
                }
            }

            GameObject torch = new GameObject(pos, Layers.Main, null);
            torch.AddComponent(new GlyphComponent('i', Color.OrangeRed));
            torch.AddComponent(new LightSourceComponent(floor.MapInfo.LightMap, new Light(Color.LightGoldenrodYellow, wallAway, 20, 25)));
            torch.AddComponent(new NameComponent(new Title("over", "", "torch")));

            return torch;
        });

        private static IPlaceable WallPlaceable = new Placable(
            (info, pos) =>
            {
                IGameObject wall = new GameObject(pos, Layers.Terrain, null, true, false, false);
                wall.AddComponent(new GlyphComponent(new Glyph('#', Color.DimGray, Color.DimGray)));
                wall.AddComponent(new NameComponent(new Title("a", "stone wall")));

                return wall;
            });

        private static IPlaceable FloorPlacable = new Placable(
            (info, pos) =>
            {
                IGameObject floor = new GameObject(pos, Layers.Terrain, null, true);

                int red = SingletonRandom.DefaultRNG.Next(85, 95);
                int green = SingletonRandom.DefaultRNG.Next(70, 80);
                int blue = SingletonRandom.DefaultRNG.Next(15, 25);
                Color tileColor = Color.FromArgb(green, green, green);

                GlyphComponent glyphComponent = new GlyphComponent(new Glyph(' ', tileColor, tileColor));
                floor.AddComponent(glyphComponent);
                floor.AddComponent(new DecalComponent());
                floor.AddComponent(new NameComponent(new Title("a", "dirt floor")));

                return floor;
            });

        private static Source<GenerationContext, IRoomGenerator> RootSource()
        {
            IRoomGenerator closet = new RectRoomGenerator(new Rectangle(0, 0, 4, 4), new Rectangle(0, 0, 6, 6), WallPlaceable, FloorPlacable);
            IRoomGenerator basicRoom = new RectRoomGenerator(new Rectangle(0, 0, 8, 8), new Rectangle(0, 0, 10, 10), WallPlaceable, FloorPlacable);
            IRoomGenerator largeRoom = new RectRoomGenerator(new Rectangle(0, 0, 10, 10), new Rectangle(0, 0, 20, 20), WallPlaceable, FloorPlacable);

            IRoomGenerator compositeRoom = new RectCompositeRoomGenerator(2,3, new Rectangle(0, 0, 8, 8), new Rectangle(0, 0, 10, 10), WallPlaceable, FloorPlacable);

            IRoomGenerator hugeRoom = new RectRoomGenerator(new Rectangle(0, 0, 20, 20), new Rectangle(0, 0, 30, 30), WallPlaceable, FloorPlacable);

            WeightedSource<GenerationContext, IRoomGenerator> roomSource = new WeightedSource<GenerationContext, IRoomGenerator>(true);
            //roomSource.Add(Source.From(closet), 1);
            roomSource.Add(Source.From<GenerationContext, IRoomGenerator>(basicRoom), 5);
            roomSource.Add(Source.From<GenerationContext, IRoomGenerator>(largeRoom), 5);
            roomSource.Add(Source.From<GenerationContext, IRoomGenerator>(compositeRoom), 10);
            roomSource.Add(new LimitedSource<GenerationContext, IRoomGenerator>(Source.From<GenerationContext, IRoomGenerator>(hugeRoom), 1, true), 3);

            basicRoom.NeighborPossibilities = roomSource;
            largeRoom.NeighborPossibilities = roomSource;
            hugeRoom.NeighborPossibilities = roomSource;
            compositeRoom.NeighborPossibilities = roomSource;
            
            WeightedSource<GenerationContext, IAreaDecorator> lightingSource = new WeightedSource<GenerationContext, IAreaDecorator>(true);
            lightingSource.Add(Source.From<GenerationContext, IAreaDecorator>(new PlaceableDecorator(GlowingMushroomsSpreader)), 1);
            lightingSource.Add(Source.From<GenerationContext, IAreaDecorator>(new PlaceableDecorator(GlowingFungusSpreader)), 1);
            lightingSource.Add(Source.From<GenerationContext, IAreaDecorator>(new WallDecorator(TorchPlacer)), 10);


            PrioritySource<GenerationContext, IAreaDecorator> decorationSource = new PrioritySource<GenerationContext, IAreaDecorator>(false);
            
            // First, place lighting in room (always)
            decorationSource.Add(new LimitedSource<GenerationContext, IAreaDecorator>(lightingSource, 1, false));

            //Then place foliage
            WeightedSource<GenerationContext, IAreaDecorator> foliageSource = new WeightedSource<GenerationContext, IAreaDecorator>(true);
            foliageSource.Add(Source.From<GenerationContext, IAreaDecorator>(new PlaceableDecorator(GrassSpreader)), 1);
            foliageSource.Add(Source.From<GenerationContext, IAreaDecorator>(new PlaceableDecorator(TallGrassSpreader)), 1);

            decorationSource.Add(foliageSource);

            compositeRoom.Decorators = decorationSource;
            basicRoom.Decorators = decorationSource;
            largeRoom.Decorators = decorationSource;
            hugeRoom.Decorators = decorationSource;

            return Source.From<GenerationContext, IRoomGenerator>(basicRoom);
        }

        private static Source<GenerationContext, IHallGenerator> HallSource()
        {
            IHallGenerator hallGenerator = new HallGenerator(WallPlaceable, FloorPlacable, null);

            return Source.From<GenerationContext, IHallGenerator>(hallGenerator);
        }




        public static Biome TestBiome = 
            new BiomeBuilder()
                .WithDecorator(new PlaceableDecorator(GrassSpreader), 2)
                .WithDecorator(new PlaceableDecorator(GlowingMushroomsSpreader), 1)
                .WithDecorator(new PlaceableDecorator(TallGrassSpreader), 1)
                .WithDecorator(new WallDecorator(TorchPlacer), 3)
                .WithRoomSource(RootSource())
                .WithHallSource(HallSource())
                .GetBiome();


    }
}