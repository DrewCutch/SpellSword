using System;
using System.Collections.Generic;
using System.Text;
using Artemis.Interface;

namespace SpellSword.ECS.Components
{
    class BeingComponent: IComponent
    {        
        public Being Being {get;}

        public bool Updated { get; }

        public BeingComponent(Being being)
        {
            Being = being;
            Updated = true;
        }
    }
}
