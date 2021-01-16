using System;
using GoRogue;

namespace SpellSword.Render
{
    class TextWriteContext: IWriteable
    {
        public int Width { get; }
        public int Height { get; }

        public bool Dirty
        {
            get => _writeable.Dirty;
            set => _writeable.Dirty = Dirty;

        }

        private readonly int _colOffset;
        private readonly int _rowOffset;

        private readonly IWriteable _writeable;

        public TextWriteContext(IWriteable writeable)
        {
            _writeable = writeable;
            Width = writeable.Width;
            Height = writeable.Height;
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

        public void SetGlyph(int row, int col, Glyph g)
        {
            if(row >= Height || row < 0)
                throw new ArgumentOutOfRangeException(nameof(row));

            if (col >= Width || col < 0)
                throw new ArgumentOutOfRangeException(nameof(col));

            _writeable.SetGlyph(_rowOffset + row, _colOffset + col, g);
        }

        public void WriteGlyph(int row, int col, Glyph g)
        {
            if (row >= Height || row < 0)
                throw new ArgumentOutOfRangeException(nameof(row));

            if (col >= Width || col < 0)
                throw new ArgumentOutOfRangeException(nameof(col));

            _writeable.WriteGlyph(_rowOffset + row, _colOffset + col, g);
        }

        public void Clear(Rectangle bounds)
        {
            _writeable.Clear(bounds.Translate(_colOffset, _rowOffset));
        }
    }
}
