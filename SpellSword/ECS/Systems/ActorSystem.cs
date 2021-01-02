using System;
using System.Collections.Generic;
using System.Text;
using Artemis;
using Artemis.Manager;
using Artemis.System;
using SpellSword.ECS.Components;

namespace SpellSword.ECS.Systems
{
    [Artemis.Attributes.ArtemisEntitySystem(ExecutionType = ExecutionType.Synchronous, GameLoopType = GameLoopType.Update, Layer = 1)]
    class ActorSystem : EntityComponentProcessingSystem<ActorComponent>
    {
        public override void Process(Entity entity, ActorComponent actor)
        {
            //actor.Actor.Update();
        }
    }
}
