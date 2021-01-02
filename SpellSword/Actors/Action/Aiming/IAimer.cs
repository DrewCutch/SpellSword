using System;
using System.Collections.Generic;
using System.Text;
using GoRogue;

namespace SpellSword.Actors.Action.Aiming
{
    interface IAimer
    {
        public IEnumerable<Coord> Path(Coord source, Coord target);
    }
}
