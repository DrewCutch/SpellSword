using System;
using System.Collections.Generic;
using System.Text;
using GoRogue.MapGeneration;
using SpellSword.Game;
using Troschuetz.Random;

namespace SpellSword.MapGeneration
{
    interface IAreaDecorator
    {
        public void Decorate(Floor floor, MapArea area, IGenerator rng);

        public bool CanDecorate(Floor floor, MapArea area);
    }
}
