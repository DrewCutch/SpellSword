using System;
using System.Collections.Generic;
using System.Drawing;
using GoRogue.MapViews;
using SpellSword.Actors.Action;

namespace SpellSword.Render
{
    public class TextTexture: IWriteable
    {
        public int Width { get; }
        public int Height { get; }

        public int Layers { get; }

        public readonly Glyph[][][] Contents;

        private uint _dirtyMask;

        public TextTexture(int width, int height, int layers = Layer.Layers)
        {
            Width = width;
            Height = height;
            Layers = layers;
            _dirtyMask = 0;

            Contents = new Glyph[layers][][];
            for (int i = 0; i < layers; i++)
            {
                Contents[i] = new Glyph[height][];
                for (int j = 0; j < height; j++)
                {
                    Contents[i][j] = new Glyph[width];
                }
            }
        }

        public void SetGlyph(int row, int col, Layer layer, Glyph g)
        {
            if (Contents[layer.Num][row][col] == g)
                return;

            _dirtyMask |= layer.Mask;
            Contents[layer.Num][row][col] = g;
        }

        public IEnumerable<Tuple<Layer, Glyph[][]>> DirtyLayers()
        {
            foreach (Layer layer in Layer.AllLayers())
            {
                if ((_dirtyMask & layer.Mask) == layer.Mask)
                    yield return new Tuple<Layer, Glyph[][]>(layer, Contents[layer.Num]);
            }
        }

        public void Clean()
        {
            _dirtyMask = 0;
        }
    }
}