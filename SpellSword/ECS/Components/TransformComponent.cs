using System;
using System.Collections.Generic;
using System.Text;
using Artemis;
using Artemis.Interface;
using GoRogue;

namespace SpellSword.ECS.Components
{
    class TransformComponentX : IComponent
    {
        public Coord Position { get; set; }
    }
}
