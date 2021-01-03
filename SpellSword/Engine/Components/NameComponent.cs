using System;
using System.Collections.Generic;
using System.Text;
using GoRogue.GameFramework;
using GoRogue.GameFramework.Components;

namespace SpellSword.Engine.Components
{
    class NameComponent: IGameObjectComponent
    {
        public IGameObject Parent { get; set; }

        public string Name { get; }

        public NameComponent(string name)
        {
            Name = name;
        }
    }
}
