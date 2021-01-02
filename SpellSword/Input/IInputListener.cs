using System;
using System.Collections.Generic;
using System.Text;
using GoRogue;

namespace SpellSword.Input
{
    interface IInputListener
    {
        public bool HasFocus { get; }

        public void OnMouseClick(Coord pos);

        public void OnMouseMove(Coord last, Coord current);

        public void OnKeyDown(int keyCode);

        public void OnKeyUp(int keyCode);

        public IInputListener Focus(Coord pos);
    }
}
