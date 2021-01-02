using System;
using System.Collections.Generic;
using System.Text;
using GoRogue.GameFramework;

namespace SpellSword.Engine
{
    class SpawnEvent
    {
        public IGameObject GameObject { get; }

        public SpawnEvent(IGameObject gameObject)
        {
            GameObject = gameObject;
        }
    }
}
