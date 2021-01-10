using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using GoRogue;
using GoRogue.GameFramework;
using GoRogue.MapGeneration;
using GoRogue.Random;
using SpellSword.Engine;
using SpellSword.Engine.Components;
using SpellSword.Render.Lighting;
using SpellSword.Speech;

namespace SpellSword.MapGeneration
{
    class AreaDecorator
    {
        public void Decorate(MapInfo mapInfo, MapArea area)
        {
            Spreader spreader = new Spreader((pos) =>
            {
                GameObject grass = new GameObject(pos, Layers.Floor, null);
                grass.AddComponent(new GlyphComponent('"', Color.DarkGreen));
                grass.AddComponent(new NameComponent(new Title("in","", "grass")));

                return grass;
            }, SingletonRandom.DefaultRNG, 100, .6f, .2f);

            Spreader glowingMushrooms = new Spreader((pos) =>
            {
                GameObject grass = new GameObject(pos, Layers.Floor, null);
                grass.AddComponent(new GlyphComponent('"', Color.Cyan));
                grass.AddComponent(new LightSourceComponent(mapInfo.LightMap, new Light(Color.Cyan, Coord.NONE, 3, 5)));
                grass.AddComponent(new NameComponent(new Title("in", "", "grass")));

                return grass;
            }, SingletonRandom.DefaultRNG, 100, .6f, .2f);

            glowingMushrooms.Place(mapInfo.Map, area.RandomPosition());
        }
    }
}
