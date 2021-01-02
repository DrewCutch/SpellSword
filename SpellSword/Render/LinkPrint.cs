using System;
using System.Collections.Generic;
using System.Text;
using GoRogue;
using SpellSword.Logging;

namespace SpellSword.Render
{
    class LinkPrint
    {
        public Dictionary<Coord, ILinkable> LinkMask { get; }
        public Coord End { get; }

        public LinkPrint(Dictionary<Coord, ILinkable> linkMask, Coord end)
        {
            LinkMask = linkMask;
            End = end;
        }
    }
}
