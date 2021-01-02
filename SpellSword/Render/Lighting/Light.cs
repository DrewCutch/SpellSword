using System.Drawing;
using GoRogue;

namespace SpellSword.Render.Lighting
{
    public class Light
    {
        public Color Color { get; }
        public Coord Pos { get; }
        public int Range { get; }
        public int Brightness { get; }

        public Light(Color color, Coord pos, int range, int brightness)
        {
            Color = color;
            Pos = pos;
            Range = range;
            Brightness = brightness;
        }
    }
}