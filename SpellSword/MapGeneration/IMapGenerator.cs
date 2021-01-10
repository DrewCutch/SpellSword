using System;
using System.Collections.Generic;
using System.Text;
using GoRogue.GameFramework;
using GoRogue.MapViews;

namespace SpellSword.MapGeneration
{
    interface IMapGenerator
    {
        public IEnumerable<MapInfo> GenerationSteps(int width, int height);
        public MapInfo Generate(int width, int height);
    }
}
