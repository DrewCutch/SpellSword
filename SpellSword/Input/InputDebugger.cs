using System;
using System.Collections.Generic;
using System.Text;
using GoRogue;

namespace SpellSword.Input
{
    class InputDebugger: IInputListener
    {
        public bool HasFocus { get; }
        public void OnMouseClick(Coord pos)
        {
            Console.WriteLine($"Mouse clicked at {pos}");
        }

        public void OnMouseMove(Coord last, Coord current)
        {
            Console.WriteLine($"Mouse moved from {last} to {current}");
        }

        public void OnKeyDown(int keyCode)
        {
            Console.WriteLine($"Key down with key code: {keyCode}");
        }

        public void OnKeyUp(int keyCode)
        {
            Console.WriteLine($"Key up with key code: {keyCode}");
        }

        public IInputListener Focus(Coord pos)
        {
            Console.WriteLine("Asked for new focus");
            return this;
        }

        public void LoseFocus()
        {
            Console.WriteLine("Lost focus");
        }
    }
}
