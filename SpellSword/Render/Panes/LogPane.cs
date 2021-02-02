using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using GoRogue;
using SpellSword.Logging;

namespace SpellSword.Render.Panes
{
    class LogPane: Pane
    {
        private Logger _logger;


        private Dictionary<Coord, ILinkable> _linkMask;

        public LogPane(Logger logger)
        {
            _logger = logger;
            logger.MessageLogged += NewMessage;
            Dirty = true;
            _linkMask = new Dictionary<Coord, ILinkable>();
        }

        private void NewMessage(LogMessage message)
        {
            Dirty = true;
        }

        public override bool Paint(Writeable writeContext)
        {
            if (!Dirty)
                return false;

            Coord nextMessage = new Coord(0,0);
            _linkMask.Clear();

            writeContext.Clear();

            foreach (LogMessage message in _logger.MostRecent())
            {
                LinkPrint linkPrint = StringPrinter.PrintLinkedText(message, writeContext, 255 - nextMessage.Y * 255 / writeContext.Height, nextMessage.X, nextMessage.Y);

                foreach (var (key, value) in linkPrint.LinkMask)
                    _linkMask.Add(key, value);

                nextMessage = new Coord(0, linkPrint.End.Y + 1);

                if (nextMessage.Y >= writeContext.Height)
                    break;
            }

            Dirty = false;
            return true;
        }

        private ILinkable lastLink = null;

        public override void OnMouseMove(Coord last, Coord current)
        {
            if (_linkMask.ContainsKey(last) && (!_linkMask.ContainsKey(current) || _linkMask[current] != lastLink))
            {
                _linkMask[last].OnLinkExit();
                lastLink = null;
            }

            if (_linkMask.ContainsKey(current) && _linkMask[current] != lastLink)
            {
                _linkMask[current].OnLinkHover();
                lastLink = _linkMask[current];
            }
        }
    }
}
