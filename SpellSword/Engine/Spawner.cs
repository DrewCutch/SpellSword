using System;
using System.Collections.Generic;
using System.Text;
using GoRogue.GameFramework;
using GoRogue.Messaging;

namespace SpellSword.Engine
{
    class Spawner: ISubscriber<SpawnEvent>
    {
        public Map Map { get; }

        public Spawner(Map map)
        {
            Map = map;
        }

        public void Handle(SpawnEvent spawn)
        {
            Map.AddEntity(spawn.GameObject);
        }
    }
}
