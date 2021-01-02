using System;
using System.Collections.Generic;
using System.Text;
using SpellSword.Render;

namespace SpellSword.Engine.Components
{
    interface IDecalable
    {
        public Glyph Decal { get; }

        public void SetDecal(Glyph glyph);
    }
}
