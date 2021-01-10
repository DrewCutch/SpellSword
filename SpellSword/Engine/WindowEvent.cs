using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using GoRogue.MapGeneration.Connectors;
using SpellSword.Render.Panes;
using Rectangle = GoRogue.Rectangle;

namespace SpellSword.Engine
{
    class WindowEvent
    {
        public Pane Root { get; }
        public Rectangle Bounds { get; }

        public Color? BackgroundColor { get; }

        public bool OpenEvent { get; }

        public static WindowEvent Open(Pane root, Rectangle bounds, Color? backgroundColor = null)
        {
            return new WindowEvent(root, bounds, backgroundColor, true);
        }

        public static WindowEvent Close(Pane root)
        {
            return new WindowEvent(root, new Rectangle(0, 0, 0,0), null,false);
        }

        private WindowEvent(Pane root, Rectangle bounds, Color? backgroundColor, bool openEvent = false)
        {
            Root = root;
            Bounds = bounds;
            BackgroundColor = backgroundColor;
            OpenEvent = openEvent;
        }
    }
}
