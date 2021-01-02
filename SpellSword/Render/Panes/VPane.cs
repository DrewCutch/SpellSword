using System.Collections.Generic;
using GoRogue;
using SpellSword.Input;

namespace SpellSword.Render.Panes
{
    class VPane : Pane
    {
        private class VPaneChild
        {
            public readonly Pane Pane;
            public readonly int Grow;

            public VPaneChild(Pane pane, int grow)
            {
                Pane = pane;
                Grow = grow;
            }
        }

        private readonly List<VPaneChild> _children;

        public VPane()
        {
            _children = new List<VPaneChild>();
        }

        public void AddChild(Pane pane, int grow)
        {
            _children.Add(new VPaneChild(pane, grow));

            ResizeChildren();
        }

        public override bool Paint(IWriteable writeContext)
        {
            Height = writeContext.Height;
            Width = writeContext.Width;

            ResizeChildren();

            int offset = 0;

            bool childPainted = false;
            foreach (VPaneChild child in _children)
            {
                childPainted |=
                    child.Pane.Paint(new TextWriteContext(writeContext, child.Pane.Width, child.Pane.Height, 0,
                        offset));
                offset += child.Pane.Height;
            }

            return childPainted;
        }

        public override IInputListener Focus(Coord pos)
        {
            if (_children.Count == 0)
                return this;

            return GetChild(pos).Pane;
        }

        public override void OnMouseMove(Coord last, Coord current)
        {
            VPaneChild fromPane = GetChild(last);
            VPaneChild toPane = GetChild(current);

            if (fromPane != toPane)
            {
                fromPane.Pane.OnMouseMove(last - new Coord(0, GetYOffset(fromPane)),
                    current - new Coord(0, GetYOffset(fromPane)));
            }

            toPane.Pane.OnMouseMove(last - new Coord(0, GetYOffset(toPane)),
                current - new Coord(0, GetYOffset(toPane)));
        }

        private VPaneChild GetChild(Coord pos)
        {
            int offset = 0;

            foreach (VPaneChild child in _children)
            {
                offset += child.Pane.Height;
                if (pos.Y < offset)
                    return child;
            }

            return _children[^1];
        }

        private int GetYOffset(VPaneChild pane)
        {
            int growSum = 0;
            foreach (VPaneChild child in _children)
                growSum += child.Grow;

            int offset = 0;

            foreach (VPaneChild child in _children)
            {
                if (child == pane)
                    return offset;


                offset += (Height * child.Grow) / growSum;
            }

            return offset;
        }

        private void ResizeChildren()
        {
            int growSum = 0;
            foreach (VPaneChild child in _children)
                growSum += child.Grow;

            int heightUsed = 0;
            int childrenLeft = _children.Count;

            foreach (VPaneChild child in _children)
            {
                int availableHeight = Height - heightUsed;

                int height = (int) (availableHeight * (child.Grow / (growSum * 1.0f)));

                child.Pane.SuggestWidth(Width);
                child.Pane.SuggestHeight(height);

                growSum -= child.Grow;
                childrenLeft -= 1;
                heightUsed += child.Pane.Height;
            }
        }
    }
}
