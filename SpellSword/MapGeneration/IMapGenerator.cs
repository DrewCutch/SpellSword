using System;
using System.Collections.Generic;
using System.Text;
using GoRogue.GameFramework;
using GoRogue.MapViews;

namespace SpellSword.MapGeneration
{
    interface IMapGenerator
    {
        public Map Generate(int width, int height);
    }
}
