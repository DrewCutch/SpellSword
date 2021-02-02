using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoRogue;
using SpellSword.Input;

namespace SpellSword.Render.Panes
{
    class OverlapPane: Pane
    {
        private class OverlapPaneChild
        {
            public readonly Pane Pane;
            public readonly Coord Offset;

            public Rectangle Bounds => new Rectangle(Offset, Offset + new Coord(Pane.Width, Pane.Height));

            public OverlapPaneChild(Pane pane, Coord offset)
            {
                Pane = pane;
                Offset = offset;
            }
        }

        private readonly List<OverlapPaneChild> _children;

        public OverlapPane()
        {
            _children = new List<OverlapPaneChild>();
        }

        public void AddChild(Pane pane, Coord offset)
        {
            _children.Add(new OverlapPaneChild(pane, offset));
        }

        public override void SuggestHeight(int height)
        {
            foreach (OverlapPaneChild child in _children)
            {
                child.Pane.SuggestHeight(height - child.Offset.Y);
            }

            base.SuggestHeight(height);
        }

        public override void SuggestWidth(int width)
        {
            foreach (OverlapPaneChild child in _children)
            {
                child.Pane.SuggestWidth(width - child.Offset.X);
            }

            base.SuggestWidth(width);
        }

        public override bool Paint(Writeable writeContext)
        {
            bool childPainted = false;

            foreach (OverlapPaneChild child in _children)
            {
                // TODO: child.Pane.Dirty = true;
                childPainted |= child.Pane.Paint(new TextWriteContext(writeContext, child.Pane.Width, child.Pane.Height, child.Offset.X, child.Offset.Y));
            }

            return childPainted;
        }

        public override void OnMouseMove(Coord last, Coord current)
        {
            OverlapPaneChild fromPane = GetChild(last);
            OverlapPaneChild toPane = GetChild(current);

            if (fromPane != toPane)
            {
                fromPane?.Pane.OnMouseMove(last - fromPane.Offset, current - fromPane.Offset);
            }

            toPane?.Pane.OnMouseMove(last - toPane.Offset, current - toPane.Offset);
        }

        public override IInputListener Focus(Coord pos)
        {
            OverlapPaneChild child = GetChild(pos);

            return child?.Pane.Focus(pos - child.Offset) ?? this;
        }

        private OverlapPaneChild GetChild(Coord pos)
        {
            for (int i = _children.Count - 1; i >= 0; i--)
            {
                OverlapPaneChild child = _children[i];

                if (child.Bounds.Contains(pos))
                    return child;
            }

            return null;
        }
    }
}
