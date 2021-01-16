using System;
using System.Collections.Generic;
using System.Text;
using GoRogue.GameFramework;

namespace SpellSword.Engine
{
    class FloorTransitionEvent
    {
        public IGameObject Target { get; }
        public int FromFloor { get; }
        public int ToFloor { get; }

        public FloorTransitionEvent(IGameObject target, int fromFloor, int toFloor)
        {
            Target = target;
            FromFloor = fromFloor;
            ToFloor = toFloor;
        }
    }
}
