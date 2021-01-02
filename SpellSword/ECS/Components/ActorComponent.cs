using System;
using System.Collections.Generic;
using System.Text;
using Artemis.Interface;
using SpellSword.Actors;

namespace SpellSword.ECS.Components
{
    class ActorComponent: IComponent
    {
        public ActorComponent(Actor actor)
        {
            Actor = actor;
        }

        public Actor Actor { get; }
    }
}
