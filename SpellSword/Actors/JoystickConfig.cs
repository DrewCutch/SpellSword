using System;
using System.Collections.Generic;
using System.Text;

namespace SpellSword.Actors
{
    class JoystickConfig
    {
        public int UpCode { get; set; }
        public int RightCode { get; set; }
        public int LeftCode { get; set; }
        public int DownCode { get; set; }

        public int MapUp { get; set; }
        public int MapDown { get; set; }
        public int MapLeft { get; set; }
        public int MapRight { get; set; }

        public int Inventory { get; set; }
        public int Equip { get; set; }
    }
}
