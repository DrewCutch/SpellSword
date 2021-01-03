using System;
using System.Collections.Generic;
using System.Text;
using GoRogue.GameFramework;
using GoRogue.GameFramework.Components;
using SpellSword.Render;

namespace SpellSword.Engine.Components
{
    class DecalComponent: Component, IDecalable
    {
        private IGameObject _parent;

        public override IGameObject Parent
        {
            get => _parent;
            set
            {
                _parent = value;
                _glyphComponent = _parent.GetComponent<GlyphComponent>();

                SetDecal(Decal);
            }
        }

        public Glyph Decal { get; private set; }

        private GlyphComponent _glyphComponent;

        public DecalComponent()
        {
            _glyphComponent = null;
            Decal = null;
        }

        public void SetDecal(Glyph glyph)
        {
            if(_glyphComponent != null && glyph != null)
                _glyphComponent.Glyph = new Glyph(glyph.Character, glyph.Color, _glyphComponent.Glyph.BackgroundColor);

            Decal = glyph;
        }
    }
}
