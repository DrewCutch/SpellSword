using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace SpellSword.Logging
{
    public class LogMessage
    {
        public string Message { get; }
        public LogLink[] Links { get; }

        public string ColorFormatted => string.Format(Message, Links);

        public Color[] Colors => (Links.Select(link => link.Color).Prepend(Color.White)).ToArray();

        public LogMessage(string message, params LogLink[] logLinks)
        {
            Message = message;
            Links = logLinks;
        }
    }
}