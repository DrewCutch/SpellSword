using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using GoRogue.GameFramework;
using GoRogue.MapViews;
using SpellSword.Engine.Components;

namespace SpellSword.Render
{
    class GlyphRenderTranslationMap: TranslationMap<IEnumerable<IGameObject>, Glyph[]>
    {
        public GlyphRenderTranslationMap(IMapView<IEnumerable<IGameObject>> baseMap) : base(baseMap)
        {
            
        }

        protected override Glyph[] TranslateGet(IEnumerable<IGameObject> value)
        {
            Glyph[] glyphs = new Glyph[3] {Glyph.Blank, Glyph.Blank, Glyph.Blank};

            foreach (IGameObject gameObject in value)
            {
                if (gameObject.HasComponent<GlyphComponent>())
                    glyphs[gameObject.Layer] = gameObject.GetComponent<GlyphComponent>().Glyph;
            }

            return glyphs;
        }
    }
}
