using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using GoRogue;
using GoRogue.Messaging;
using SpellSword.Engine;
using SpellSword.Render;
using SpellSword.Render.Panes;
using SpellSword.Util;

namespace SpellSword.Input
{
    class WindowRouter: IInputListener, ISubscriber<WindowEvent>
    {
        private OrderedSet<PaneBounds> _paneBounds;

        public WindowRouter()
        {
            _paneBounds = new OrderedSet<PaneBounds>();
        }

        public bool HasFocus { get; }
        public void OnMouseClick(Coord pos)
        {
            PaneBounds target = GetTopPaneBounds(pos);

            target?.Pane.OnMouseClick(pos - target.Bounds.MinExtent);
        }

        public void OnMouseMove(Coord last, Coord current)
        {
            PaneBounds from = GetTopPaneBounds(last);
            PaneBounds to = GetTopPaneBounds(current);

            from?.Pane.OnMouseMove(last - from.Bounds.MinExtent, current - from.Bounds.MinExtent);

            if (!to.Equals(from))
            {
                to?.Pane.OnMouseMove(last - to.Bounds.MinExtent, current - to.Bounds.MinExtent);
            }
        }

        public IInputListener Focus(Coord pos)
        {
            PaneBounds target = GetTopPaneBounds(pos);

            return target?.Pane.Focus(pos - target.Bounds.MinExtent) ?? this;
        }

        private PaneBounds GetTopPaneBounds(Coord pos)
        {
            foreach (PaneBounds paneBounds in _paneBounds.Reverse())
            {
                if (!paneBounds.Bounds.Contains(pos))
                    continue;

                return paneBounds;
            }

            return null;
        }

        public void Handle(WindowEvent message)
        {
            if (message.OpenEvent)
            {
                _paneBounds.Add(new PaneBounds(message.Root, message.Bounds));
            }
            else
            {
                _paneBounds.Remove(new PaneBounds(message.Root, message.Bounds));
            }
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
                return Equals(Pane, other.Pane) && Bounds.Equals(other.Bounds);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((PaneBounds) obj);
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(Pane, Bounds);
            }
        }
    }
}
