using System;
using System.Collections.Generic;
using System.Text;

namespace SpellSword.Render.Panes
{
    class TextPane: Pane
    {
        private string _text;

        public string Text
        {
            get => _text;
            set
            {
                Width = _text?.Length ?? 0;
                _text = value;
                Dirty = true;
            }
        }

        public TextPane(string text = null)
        {
            Text = text;
            Height = 1;
        }

        public override void SuggestHeight(int height)
        {
            
        }

        public override bool Paint(Writeable writeContext)
        {
            if (!Dirty)
                return false;

            writeContext.Clear();

            StringPrinter.Print(Text, writeContext, 0, 0);

            Dirty = false;
            return true;
        }
    }
}
