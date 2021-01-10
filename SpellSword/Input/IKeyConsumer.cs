using System;
using System.Collections.Generic;
using System.Text;

namespace SpellSword.Input
{
    interface IKeyConsumer
    {
        public bool Consume(int keyCode);
    }
}
