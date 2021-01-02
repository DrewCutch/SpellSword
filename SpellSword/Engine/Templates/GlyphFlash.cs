using System;
using System.Collections.Generic;
using System.Text;
using GoRogue;
using GoRogue.GameFramework;
using SpellSword.Engine.Components;
using SpellSword.Render;

namespace SpellSword.Engine.Templates
{
    class GlyphFlash: UpdatingGameObject
    {
        public GlyphFlash(Coord position, Glyph glyph, int ticks) : base(position, Layers.Effects, null, true, true)
        {
            AddComponent(new GlyphComponent(glyph));
            AddComponent(new DestroyAfterTime(10));
        }
    }
}
