using System;
using System.Collections.Generic;
using System.Text;
using GoRogue;

namespace SpellSword.Input
{
    class MouseMoveEvent
    {
        public Coord OldPos { get; } 
        public Coord NewPos { get; }

        public MouseMoveEvent(Coord oldPos, Coord newPos)
        {
            OldPos = oldPos;
            NewPos = newPos;
        }
    }
}
