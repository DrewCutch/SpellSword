using System;
using System.Collections.Generic;
using System.Text;
using BearLib;
using GoRogue;
using GoRogue.Messaging;

namespace SpellSword.Input
{
    class BearTermInputRouter
    {
        private readonly IInputListener _listener;

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
                    IInputListener newFocus = _listener.Focus(MousePos());

                    if (newFocus != _focus)
                    {
                        _focus.LoseFocus();
                        _focus = newFocus;
                    }

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
