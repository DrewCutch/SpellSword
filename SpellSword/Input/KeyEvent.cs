using System;
using System.Collections.Generic;
using System.Text;

namespace SpellSword.Input
{
    class KeyEvent
    {
        public int KeyCode { get; }
        public bool Released { get; }

        public KeyEvent(int keyCode, bool released = false)
        {
            KeyCode = keyCode;
            Released = released;
        }
    }
}
