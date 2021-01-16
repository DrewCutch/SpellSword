using System;
using System.Collections.Generic;
using System.Text;
using GoRogue;
using GoRogue.GameFramework;
using GoRogue.MapGeneration;
using GoRogue.MapViews;
using SpellSword.Game;
using SpellSword.Util;
using Troschuetz.Random;

namespace SpellSword.MapGeneration
{
    interface IBiome
    {
        public IEnumerable<MapInfo> GenerateOn(Floor floor, Rectangle area, ResettableRandom rng);
        public void Populate(Floor floor, MapArea area, IGenerator rng);
    }
}
