using System;
using System.Drawing;
using SpellSword.Render.Fonts;
using Rectangle = GoRogue.Rectangle;

namespace SpellSword.Render
{
    class TextWriteContext: Writeable
    {
        public override bool Dirty
        {
            get => _writeable.Dirty;
            set => _writeable.Dirty = value;

        }

        private readonly int _colOffset;
        private readonly int _rowOffset;

        private readonly Typeface _typeface;

        private readonly Writeable _writeable;

        public TextWriteContext(Writeable writeable, int width, int height, int col, int row, Typeface typeface): base(width / typeface.Width, height)
        {
            _writeable = writeable;
            _colOffset = col;
            _rowOffset = row;
            _typeface = typeface;
        }

        public TextWriteContext(Writeable writeable, Typeface typeface) : this(writeable, writeable.Width, writeable.Height, 0, 0, typeface) { }

        public TextWriteContext(Writeable writeable, int width, int height, int col, int row) : this(writeable, width, height, col, row, Typeface.Text) { }


        public override void SetCharacter(int row, int col, int character, Color color, Color? backgroundColor)
        {
            if (row >= Height || row < 0)
                throw new ArgumentOutOfRangeException(nameof(row));

            if (col >= Width || col < 0)
                throw new ArgumentOutOfRangeException(nameof(col));

            col *= _typeface.Width;

            _writeable.SetCharacter(_rowOffset + row, _colOffset + col, _typeface.GetCharacter(character), color, backgroundColor);
        }

        public override void WriteCharacter(int row, int col, int character, Color color, Color? backgroundColor)
        {
            if (row >= Height || row < 0)
                throw new ArgumentOutOfRangeException(nameof(row));

            if (col >= Width || col < 0)
                throw new ArgumentOutOfRangeException(nameof(col));

            col *= _typeface.Width;

            _writeable.WriteCharacter(_rowOffset + row, _colOffset + col, _typeface.GetCharacter(character), color, backgroundColor);
        }

        public override void Clear(Rectangle bounds)
        {
            _writeable.Clear(bounds.Translate(_colOffset, _rowOffset));
        }
    }
}
