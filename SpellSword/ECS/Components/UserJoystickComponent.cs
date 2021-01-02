using System;
using System.Collections.Generic;
using System.Text;
using Artemis.Interface;

namespace SpellSword.ECS.Components
{
    class UserJoystickComponent: IComponent
    {
        public int UpCode { get; set; }
        public int RightCode { get; set; }
        public int LeftCode { get; set; }
        public int DownCode { get; set; }
    }
}
