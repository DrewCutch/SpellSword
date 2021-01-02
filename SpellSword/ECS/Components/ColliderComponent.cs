using System;
using System.Collections.Generic;
using System.Text;
using Artemis.Interface;
using GoRogue;

namespace SpellSword.ECS.Components
{
    class ColliderComponent: IComponent
    {
        public Rectangle Bounds { get; }

        public ColliderComponent(Rectangle bounds)
        {
            Bounds = bounds;
        }
    }
}
