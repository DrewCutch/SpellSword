using System;
using System.Collections.Generic;
using System.Text;

namespace SpellSword.Logging
{
    class LinkDebugger: ILinkable
    {
        public void OnLinkHover()
        {
            Console.WriteLine("Link hovered");
        }

        public void OnLinkClick()
        {
            Console.WriteLine("Link clicked");
        }

        public void OnLinkExit()
        {
            Console.WriteLine("Link exited");
        }
    }
}
