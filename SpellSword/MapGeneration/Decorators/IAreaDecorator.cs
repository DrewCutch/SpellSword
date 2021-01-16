using System;
using System.Collections.Generic;
using System.Text;
using GoRogue.MapGeneration;
using Troschuetz.Random;

namespace SpellSword.MapGeneration
{
    interface IAreaDecorator
    {
        public void Decorate(MapInfo mapInfo, MapArea area, IGenerator rng);

        public bool CanDecorate(MapInfo mapInfo, MapArea area);
    }
}
