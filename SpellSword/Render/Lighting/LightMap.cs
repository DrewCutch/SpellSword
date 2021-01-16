using System;
using System.Collections.Generic;
using System.Drawing;
using GoRogue;
using GoRogue.MapViews;
using Rectangle = GoRogue.Rectangle;

namespace SpellSword.Render.Lighting
{
    class LightMap : IMapView<Color> {
        private ArrayMap2D<UnboundedColor> map;
        private IMapView<bool> _transparencyMap;

        public LightMap(int width, int height, IMapView<bool> transparencyMap)
        {
            map = new ArrayMap2D<UnboundedColor>(width, height);
            _transparencyMap = transparencyMap;
            for (int i = 0; i < map.Width; i++)
            {
                for (int j = 0; j < map.Height; j++)
                {
                    map[i, j] = Color.FromArgb(50, 50, 50);
                }
            }
        }

        public void AddLight(Light light)
        {
            ModifyLight(light, true);
        }

        public void RemoveLight(Light light)
        {
            ModifyLight(light, false);
        }


        private void ModifyLight(Light light, bool add)
        {
            //HashSet<Coord> litSpaces = new HashSet<Coord>();

            if (!_transparencyMap.Contains(light.Pos))
                return;

            FOV litFov = new FOV(_transparencyMap);
            litFov.Calculate(light.Pos, light.Range);
            IEnumerable<Coord> litSpaces = litFov.CurrentFOV;

            foreach (Coord litSpace in litSpaces)
            {
                //double falloff = litFov[litSpace];

                double dist = Distance.EUCLIDEAN.Calculate(litSpace, light.Pos);
                double sqrDist = Math.Max(dist, .1); ; //Math.Max(dist * dist, .1);

                if (!map.Contains(litSpace))
                    continue;

                if (add)
                    map[litSpace].Add(Brightness(light.Color, (int)(light.Brightness - sqrDist)));
                else
                    map[litSpace].Subtract(Brightness(light.Color, (int)(light.Brightness - sqrDist)));
            }
        }

        private UnboundedColor Brightness(Color color, int bright)
        {
            if (bright > 255)
                return color;

            return new UnboundedColor((color.R * bright) / 255, (color.G * bright) /255, (color.B * bright) / 255);
        }

        public Color this[Coord pos] => map[pos];

        public Color this[int index1D] => map[index1D];

        public Color this[int x, int y] => map[x, y];

        public int Height => map.Height;

        public int Width => map.Width;
    }
}
