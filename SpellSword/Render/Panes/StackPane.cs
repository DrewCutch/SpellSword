using System;
using System.Collections.Generic;
using System.Text;
using GoRogue;
using SpellSword.Input;

namespace SpellSword.Render.Panes
{
    class StackPane: Pane
    {
        private class StackPaneChild
        {
            public readonly Pane Pane;
            public readonly int Grow;

            public StackPaneChild(Pane pane, int grow)
            {
                Pane = pane;
                Grow = grow;
            }
        }

        public enum StackDirection
        {
            Vertical,
            Horizontal
        }

        public StackDirection Direction { get; }

        private readonly List<StackPaneChild> _children;

        public StackPane(StackDirection direction)
        {
            Direction = direction;
            _children = new List<StackPaneChild>();
        }

        public override void SuggestWidth(int width)
        {
            base.SuggestWidth(width);

            ResizeChildren();
        }

        public override void SuggestHeight(int height)
        {
            base.SuggestHeight(height);

            ResizeChildren();
        }

        public void AddChild(Pane pane, int grow)
        {
            _children.Add(new StackPaneChild(pane, grow));

            ResizeChildren();
        }

        public override bool Paint(IWriteable writeContext)
        {
            int xOffset = 0;
            int yOffset = 0;

            bool childPainted = false;

            foreach (StackPaneChild child in _children)
            {
                childPainted |= child.Pane.Paint(new TextWriteContext(writeContext, child.Pane.Width, child.Pane.Height, xOffset, yOffset));

                xOffset += Direction == StackDirection.Horizontal ? child.Pane.Width : 0;
                yOffset += Direction == StackDirection.Vertical ? child.Pane.Height : 0;
            }

            return childPainted;
        }

        public override IInputListener Focus(Coord pos)
        {
            if (_children.Count == 0)
                return this;

            StackPaneChild child = GetChild(pos);

            return child.Pane.Focus(pos - GetOffset(child));
        }

        public override void OnMouseMove(Coord last, Coord current)
        {
            StackPaneChild fromPane = GetChild(last);
            StackPaneChild toPane = GetChild(current);

            if (fromPane != toPane)
            {
                fromPane.Pane.OnMouseMove(last - GetOffset(fromPane), current - GetOffset(fromPane));
            }

            toPane.Pane.OnMouseMove(last - GetOffset(toPane), current - GetOffset(toPane));
        }

        private StackPaneChild GetChild(Coord pos)
        {
            int offset = 0;
            int posOffset = Direction == StackDirection.Vertical ? pos.Y : pos.X;

            foreach (StackPaneChild child in _children)
            {
                offset += Direction == StackDirection.Vertical ? child.Pane.Height : child.Pane.Width;

                if (posOffset < offset)
                    return child;
            }

            return _children[^1];
        }

        private Coord GetOffset(StackPaneChild pane)
        {
            Coord offset = new Coord(0, 0);

            foreach (StackPaneChild child in _children)
            {
                if (child == pane)
                    return offset;


                offset += new Coord(Direction == StackDirection.Horizontal ? child.Pane.Width : 0, Direction == StackDirection.Vertical ? child.Pane.Height : 0);
            }

            return offset;
        }

        private void ResizeChildren()
        {
            int growSum = 0;
            foreach (StackPaneChild child in _children)
                growSum += child.Grow;

            int initialSpace = Direction == StackDirection.Vertical ? Height : Width;
            int spaceUsed = 0;
            int childrenLeft = _children.Count;

            foreach (StackPaneChild child in _children)
            {
                int availableSpace = initialSpace - spaceUsed;

                int space = (int)(availableSpace * (child.Grow / (growSum * 1.0f)));

                child.Pane.SuggestWidth(Direction == StackDirection.Horizontal ? space : Width);
                child.Pane.SuggestHeight(Direction == StackDirection.Vertical ? space : Height);

                growSum -= child.Grow;
                childrenLeft -= 1;
                spaceUsed += Direction == StackDirection.Vertical ? child.Pane.Height : child.Pane.Width;
            }
        }
    }
}
