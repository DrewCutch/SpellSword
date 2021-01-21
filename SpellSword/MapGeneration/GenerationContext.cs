using System;
using System.Collections.Generic;
using System.Text;
using SpellSword.MapGeneration.Sources;
using Troschuetz.Random;

namespace SpellSword.MapGeneration
{
    class GenerationContext: IRandomContext
    {
        public IGenerator Generator { get; }

        public GenerationContext(IGenerator rng)
        {
            Generator = rng;
        }
    }
}
