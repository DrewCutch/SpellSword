using System;
using System.Collections.Generic;
using System.Text;
using GoRogue.GameFramework;
using GoRogue.GameFramework.Components;

namespace SpellSword.Engine.Components
{
    abstract class Component: IGameObjectComponent
    {
        public virtual IGameObject Parent { get; set; }
        
        public virtual Component CloneTo(IGameObject gameObject)
        {
            Component clone = (Component) this.MemberwiseClone();

            gameObject.AddComponent(clone);

            return clone;
        }
    }
}
