using System;
using System.Collections.Generic;
using System.Text;
using GoRogue.GameFramework;
using GoRogue.GameFramework.Components;
using SpellSword.Render;

namespace SpellSword.Engine.Components
{
    class DecalComponent: IDecalable, IGameObjectComponent
    { 
        public IGameObject Parent { get; set; }

        public Glyph Decal { get; private set; }

        private GlyphComponent _glyphComponent;

        public DecalComponent(GlyphComponent glyphComponent)
        {
            _glyphComponent = glyphComponent;
            Decal = null;
        }

        public void SetDecal(Glyph glyph)
        {
            _glyphComponent.Glyph = new Glyph(glyph.Character, glyph.Color, _glyphComponent.Glyph.BackgroundColor);
        }

    }
}
