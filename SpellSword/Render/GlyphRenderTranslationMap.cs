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
    class GlyphRenderTranslationMap: TranslationMap<IEnumerable<IGameObject>, IEnumerable<Glyph>>
    {
        public GlyphRenderTranslationMap(IMapView<IEnumerable<IGameObject>> baseMap) : base(baseMap)
        {
            
        }

        protected override IEnumerable<Glyph> TranslateGet(IEnumerable<IGameObject> value)
        {
            foreach (IGameObject gameObject in value.Reverse())
            {
                if (gameObject.HasComponent<GlyphComponent>())
                    yield return gameObject.GetComponent<GlyphComponent>().Glyph;
            }
        }
    }
}
