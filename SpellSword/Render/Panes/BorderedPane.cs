using System;
using System.Drawing;

namespace SpellSword.Render.Panes
{
    class BorderedPane: Pane
    {
        private readonly Pane _inside;
        private readonly BorderSet _borderSet;

        public BorderedPane(Pane inside, BorderSet borderSet)
        {
            _inside = inside;
            _borderSet = borderSet;
        }

        public override bool Paint(Writeable writeContext)
        {
            throw new NotImplementedException();
            /*
            writeContext.SetGlyph(0, 0, new Glyph(BorderSet.ThinBorder.NorthWest, Color.White));
            _inside.Paint(new TextWriteContext(writeContext, writeContext.Width - 2, writeContext.Height - 2, 1, 1));

            return true;
            */
        }
    }
}
