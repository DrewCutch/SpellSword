using System;
using System.Collections.Generic;
using System.Text;

namespace SpellSword.Render.Panes
{
    class TextPane: Pane
    {
        private bool _dirty;
        private string _text;

        public string Text
        {
            get => _text;
            set
            {
                Width = _text?.Length ?? 0;
                _text = value;
                _dirty = true;
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

        public override bool Paint(IWriteable writeContext)
        {
            if (!_dirty)
                return false;

            writeContext.Clear(Layer.Main);

            StringPrinter.Print(Text, writeContext, 0, 0);

            _dirty = false;
            return true;
        }
    }
}
