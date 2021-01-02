using System.Collections.Generic;
using GoRogue;
using SpellSword.Input;

namespace SpellSword.Render.Panes
{
    class HPane: Pane
    {
        private class HPaneChild
        {
            public readonly Pane Pane;
            public readonly int Grow;

            public HPaneChild(Pane pane, int grow)
            {
                Pane = pane;
                Grow = grow;
            }
        }

        private readonly List<HPaneChild> _children;

        public HPane()
        {
            _children = new List<HPaneChild>();
        }

        public void AddChild(Pane pane, int grow)
        {
            _children.Add(new HPaneChild(pane, grow));
        }

        public override bool Paint(IWriteable writeContext)
        {
            Height = writeContext.Height;
            Width = writeContext.Width;

            int growSum = 0;
            foreach (HPaneChild child in _children)
                growSum += child.Grow;

            int offset = 0;

            bool childPainted = false;
            foreach (HPaneChild child in _children)
            {
                childPainted |= child.Pane.Paint(new TextWriteContext(writeContext, (writeContext.Width * child.Grow) / growSum, writeContext.Height, offset, 0));
                offset += (writeContext.Width * child.Grow) / growSum;
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
            HPaneChild fromPane = GetChild(last);
            HPaneChild toPane = GetChild(current);

            if (fromPane != toPane)
            {
                fromPane.Pane.OnMouseMove(last - new Coord(GetXOffset(fromPane), 0),
                    current - new Coord(GetXOffset(fromPane), 0));
            }

            toPane.Pane.OnMouseMove(last - new Coord(GetXOffset(fromPane), 0),
                current - new Coord(GetXOffset(fromPane), 0));
        }

        private HPaneChild GetChild(Coord pos)
        {
            int growSum = 0;
            foreach (HPaneChild child in _children)
                growSum += child.Grow;

            int offset = 0;

            foreach (HPaneChild child in _children)
            {
                offset += (Width * child.Grow) / growSum;
                if (pos.X < offset)
                    return child;
            }

            return _children[^1];
        }

        private int GetXOffset(HPaneChild pane)
        {
            int growSum = 0;
            foreach (HPaneChild child in _children)
                growSum += child.Grow;

            int offset = 0;

            foreach (HPaneChild child in _children)
            {
                if (child == pane)
                    return offset;


                offset += (Width * child.Grow) / growSum;
            }

            return offset;
        }
    }
}
