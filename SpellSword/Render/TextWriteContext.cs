using System;

namespace SpellSword.Render
{
    class TextWriteContext: IWriteable
    {
        public int Width { get; }
        public int Height { get; }

        public int Layers { get; }
        private readonly int _colOffset
            ;
        private readonly int _rowOffset;

        private readonly IWriteable _writeable;

        public TextWriteContext(IWriteable writeable)
        {
            _writeable = writeable;
            Width = writeable.Width;
            Height = writeable.Height;
            Layers = writeable.Layers;
        }

        public TextWriteContext(IWriteable writeable, int width, int height)
        {
            _writeable = writeable;
            Width = width;
            Height = height;
        }

        public TextWriteContext(IWriteable writeable, int width, int height, int col, int row)
        {
            _writeable = writeable;
            Width = width;
            Height = height;
            _colOffset = col;
            _rowOffset = row;
        }

        public void SetGlyph(int row, int col, Layer layer, Glyph g)
        {
            if(row >= Height || row < 0)
                throw new ArgumentOutOfRangeException(nameof(row));

            if (col >= Width || col < 0)
                throw new ArgumentOutOfRangeException(nameof(col));

            _writeable.SetGlyph(_rowOffset + row, _colOffset + col, layer, g);
        }
    }
}
