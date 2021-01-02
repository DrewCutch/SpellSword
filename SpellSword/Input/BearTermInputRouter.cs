using System;
using System.Collections.Generic;
using System.Text;
using BearLib;
using GoRogue;

namespace SpellSword.Input
{
    class BearTermInputRouter
    {
        private IInputListener _listener;

        private IInputListener _focus;

        private Coord _mouseLast;

        public BearTermInputRouter(IInputListener listener)
        {
            _listener = listener;
            _focus = listener;
            _mouseLast = MousePos();
        }

        public void HandleInput()
        {
            while (Terminal.HasInput())
            {
                int input = Terminal.Read();

                if (input == Terminal.TK_MOUSE_LEFT)
                {
                    _focus = _listener.Focus(MousePos());
                    _listener.OnMouseClick(MousePos()); 
                }
                else if (input == Terminal.TK_MOUSE_MOVE)
                {
                    _listener.OnMouseMove(_mouseLast, MousePos());
                    _mouseLast = MousePos();
                }
                else if (input < Terminal.TK_MOUSE_LEFT) // All key presses
                {
                    if ((input & Terminal.TK_KEY_RELEASED) == Terminal.TK_KEY_RELEASED)
                        _focus.OnKeyUp(input);
                    else
                        _focus.OnKeyDown(input);
                }
            }
        }

        private Coord MousePos()
        {
            return new Coord(Terminal.State(Terminal.TK_MOUSE_X), Terminal.State(Terminal.TK_MOUSE_Y));
        }
    }
}
