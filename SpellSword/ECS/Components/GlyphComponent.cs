using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using Artemis;
using Artemis.Interface;
using SpellSword.Render;

namespace SpellSword.ECS.Components
{
    class GlyphComponent: IComponent
    {
        public char Character { get; set; }
        public Color Color { get; set; }

        public GlyphComponent(char character, Color color)
        {
            Character = character;
            Color = color;
        }

        public GlyphComponent(Glyph glyph)
        {
            Character = glyph.Character;
            Color = glyph.Color;
        }
    }
}
