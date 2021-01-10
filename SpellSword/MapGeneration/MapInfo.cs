using System;
using System.Collections.Generic;
using System.Text;
using GoRogue.GameFramework;
using SpellSword.Render.Lighting;

namespace SpellSword.MapGeneration
{
    class MapInfo
    {
        public Map Map { get; }
        public LightMap LightMap { get; }

        public MapInfo(Map map, LightMap lightMap)
        {
            Map = map;
            LightMap = lightMap;
        }
    }
}
