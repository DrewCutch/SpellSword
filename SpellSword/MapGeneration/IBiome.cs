using System;
using System.Collections.Generic;
using System.Text;
using GoRogue.GameFramework;
using GoRogue.MapGeneration;
using GoRogue.MapViews;
using Troschuetz.Random;

namespace SpellSword.MapGeneration
{
    interface IBiome
    {
        public IMapView<GameObject> GenerateTerrain(IGenerator rng);

        public void Populate(MapInfo mapInfo, MapArea area, IGenerator rng);
    }
}
