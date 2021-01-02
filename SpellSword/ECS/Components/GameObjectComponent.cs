using System;
using System.Collections.Generic;
using System.Text;
using Artemis.Interface;
using GoRogue.GameFramework;

namespace SpellSword.ECS.Components
{
    class GameObjectComponent: IComponent
    {
        public GameObject GameObject { get; }

        public GameObjectComponent(GameObject gameObject)
        {
            GameObject = gameObject;
        }
    }
}
