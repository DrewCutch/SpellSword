using System;
using System.Collections.Generic;
using System.Text;
using GoRogue.GameFramework;

namespace SpellSword.Engine
{
    class SpawnEvent
    {
        public IGameObject GameObject { get; }

        public bool Tumble { get; }

        public SpawnEvent(IGameObject gameObject, bool tumble = false)
        {
            GameObject = gameObject;
            Tumble = tumble;
        }
    }
}
