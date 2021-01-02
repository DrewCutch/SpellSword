using System;
using System.Collections.Generic;
using System.Text;
using GoRogue.Messaging;

namespace SpellSword.Logging
{
    class Logger: ISubscriber<LogMessage>
    {
        public event Action<LogMessage> MessageLogged;

        private LinkedList<LogMessage> _messageLog;

        public Logger()
        {
            _messageLog = new LinkedList<LogMessage>();
        }

        public void Log(LogMessage message)
        {
            _messageLog.AddLast(message);

            MessageLogged?.Invoke(message);
        }


        public IEnumerable<LogMessage> GetPrevious(int n)
        {
            LinkedListNode<LogMessage> current = _messageLog.Last;

            if (current is null)
                yield break;

            for (int i = 0; i < n && i < _messageLog.Count; i++)
            {
                yield return current.Value;
                current = current.Previous;
            }
        }

        public IEnumerable<LogMessage> MostRecent()
        {
            LinkedListNode<LogMessage> current = _messageLog.Last;

            while (current != null)
            {
                yield return current.Value;
                current = current.Previous;
            }
        }

        public void Handle(LogMessage message)
        {
            Log(message);
        }
    }
}
