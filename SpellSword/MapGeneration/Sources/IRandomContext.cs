using System;
using System.Collections.Generic;
using System.Text;
using Troschuetz.Random;

namespace SpellSword.MapGeneration.Sources
{
    interface IRandomContext
    {
        public IGenerator Generator { get; }
    }
}
