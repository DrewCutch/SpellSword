using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using BearLib;
using GoRogue.Messaging;
using SpellSword.Engine;
using SpellSword.Render.Panes;
using SpellSword.Render.Particles;

namespace SpellSword.Render
{
    class BearTermRenderer: ISubscriber<ParticleEvent>, ISubscriber<WindowEvent>
    {
        private Dictionary<Pane, Window> _windows;
        private Pane _root;

        public BearTermRenderer(Window root, string options = "")
        {
            if (!Terminal.Open())
                throw new Exception("Terminal instance is already open!");

            Terminal.Set(options);

            Terminal.Set("0xE000: Content\\terminal16x16_gs_ro.png, size=16x16, spacing=2x1, transparent=black");//polyducks_12x12

            //Terminal.Set("font: Content\\polyducks_12x12.png, size=12x12, resize=6x12, codepage=437, spacing=1x1, resize-filter=bicubic");

            Terminal.Refresh();

            _root = root.Root;

            _windows = new Dictionary<Pane, Window>();
            _windows[_root] = root;
        }

        public void Refresh()
        {
            bool dirty = false;
            foreach (Window window in _windows.Values)
            {
                window.Refresh();

                dirty |= window.Dirty;

                window.Clean();
            }

            if (dirty)
                Terminal.Refresh();
        }


        public void Handle(ParticleEvent message)
        {
            _windows[_root].Handle(message);
        }

        public void Handle(WindowEvent windowEvent)
        {
            if (windowEvent.OpenEvent)
            {
                Window newWindow = new Window(windowEvent.Root, windowEvent.Bounds, _windows.Count);

                if (windowEvent.BackgroundColor is Color backgroundColor)
                    newWindow.Background = backgroundColor;

                _windows[windowEvent.Root] = newWindow;
            }
            else
            {
                Window window = _windows[windowEvent.Root];
                window.Hide();
                _windows.Remove(windowEvent.Root);
            }
        }
    }
}
