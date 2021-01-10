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
    class GlyphRenderTranslationMap: TranslationMap<IEnumerable<IGameObject>, IEnumerable<GlyphRenderTranslationMap.MapGlyph>>
    {
        public class MapGlyph
        {
            public Glyph Glyph;
            public bool SelfLit;
        }

        public GlyphRenderTranslationMap(IMapView<IEnumerable<IGameObject>> baseMap) : base(baseMap)
        {
            
        }

        protected override IEnumerable<MapGlyph> TranslateGet(IEnumerable<IGameObject> value)
        {
            foreach (IGameObject gameObject in value.Reverse())
            {
                if (gameObject.HasComponent<GlyphComponent>())
                    yield return new MapGlyph() {Glyph= gameObject.GetComponent<GlyphComponent>().Glyph, SelfLit = gameObject.HasComponent<LightSourceComponent>()};
            }
        }
    }
}
