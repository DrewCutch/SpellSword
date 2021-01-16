using System;
using System.Collections.Generic;
using System.Text;
using GoRogue;
using GoRogue.GameFramework;
using SpellSword.Engine.Components;
using SpellSword.Render;
using SpellSword.Time;

namespace SpellSword.Engine.Templates
{
    class GlyphFlash: UpdatingGameObject
    {
        public GlyphFlash(Coord position, Glyph glyph, int ticks, Timeline timeline) : base(position, Layers.Effects, null, timeline, true, true)
        {
            AddComponent(new GlyphComponent(glyph));
            AddComponent(new DestroyAfterTime(10));
        }
    }
}
