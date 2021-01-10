using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BearLib;
using GoRogue;
using GoRogue.Messaging;
using SpellSword.Engine;
using SpellSword.Render.Panes;
using SpellSword.Util;

namespace SpellSword.Input
{
    class BearTermInputRouter: ISubscriber<WindowEvent>
    {
        //private IInputListener _listener => _windowListeners.Peek();

        private readonly IKeyConsumer _gameControlConsumer;

        private OrderedDictionary<PaneBounds, IInputListener> _windowListeners;

        private IInputListener _focus;

        private Coord _mouseLast;

        public BearTermInputRouter(IKeyConsumer gameControlConsumer)
        {
            _gameControlConsumer = gameControlConsumer;

            _mouseLast = MousePos();
            _windowListeners = new OrderedDictionary<PaneBounds, IInputListener>();
        }

        public void HandleInput()
        {
            while (Terminal.HasInput())
            {
                int input = Terminal.Read();

                if (input == Terminal.TK_MOUSE_LEFT)
                {
                    PaneBounds target = GetTopPaneBounds(MousePos());
                    IInputListener newFocus = target.Pane.Focus(MousePos());

                    _windowListeners[target] = newFocus;

                    if (newFocus != _focus)
                    {
                        _focus.LoseFocus();
                        _focus = newFocus;
                    }

                    target.Pane.OnMouseClick(MousePos());
                }
                else if (input == Terminal.TK_MOUSE_MOVE)
                {
                    _windowListeners.Values.Last().OnMouseMove(_mouseLast, MousePos());
                    _mouseLast = MousePos();
                }
                else if (input < Terminal.TK_MOUSE_LEFT) // All key presses
                {
                    if(_gameControlConsumer.Consume(input))
                        continue;

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

        public void Handle(WindowEvent message)
        {
            if (message.OpenEvent)
            {
                _focus = message.Root.Focus(new Coord(0, 0));

                _windowListeners.Add(new PaneBounds(message.Root, message.Bounds), _focus);
            }
            else
            {
                _windowListeners.Remove(new PaneBounds(message.Root, message.Bounds));

                IInputListener newFocus = _windowListeners.Values.Last();

                if(_focus != newFocus)
                    _focus.LoseFocus();
                
                _focus = _windowListeners.Values.Last();
            }
        }

        private PaneBounds GetTopPaneBounds(Coord pos)
        {
            foreach (PaneBounds paneBounds in _windowListeners.Keys.Reverse())
            {
                if (!paneBounds.Bounds.Contains(pos))
                    continue;

                return paneBounds;
            }

            return null;
        }

        private class PaneBounds
        {
            public Pane Pane { get; }
            public Rectangle Bounds { get; }

            public PaneBounds(Pane pane, Rectangle bounds)
            {
                Pane = pane;
                Bounds = bounds;
            }

            protected bool Equals(PaneBounds other)
            {
                return Equals(Pane, other.Pane);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((PaneBounds)obj);
            }

            public override int GetHashCode()
            {
                return Pane.GetHashCode();
            }
        }
    }
}
