using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using GoRogue;
using GoRogue.GameFramework;
using SpellSword.Engine;
using SpellSword.Engine.Components;
using SpellSword.Render.Lighting;
using SpellSword.Speech;

namespace SpellSword.MapGeneration.Resources
{
    static class Biomes
    {
        private static Spreader GrassSpreader = new Spreader(
            (mapInfo, pos) =>
            {
                GameObject grass = new GameObject(pos, Layers.Floor, null);
                grass.AddComponent(new GlyphComponent('"', Color.DarkGreen));
                grass.AddComponent(new NameComponent(new Title("in", "", "grass")));

                return grass;
            },
            100, .7f, .2f);

        private static Spreader GlowingMushroomsSpreader = new Spreader(
            (mapInfo, pos) =>
            {
                GameObject grass = new GameObject(pos, Layers.Floor, null);
                grass.AddComponent(new GlyphComponent('"', Color.Cyan));
                grass.AddComponent(new LightSourceComponent(mapInfo.LightMap, new Light(Color.Cyan, Coord.NONE, 3, 5)));
                grass.AddComponent(new NameComponent(new Title("in", "", "grass")));

                return grass;
            }, 20, .8f, .8f);

        public static Biome TestBiome = 
            new BiomeBuilder()
                .WithDecorator(new PlaceableDecorator(GrassSpreader), 2)
                .WithDecorator(new PlaceableDecorator(GlowingMushroomsSpreader), 1)
                .GetBiome();


    }
}