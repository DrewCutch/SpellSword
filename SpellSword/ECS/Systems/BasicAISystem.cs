using System;
using System.Collections.Generic;
using System.Text;
using Artemis;
using Artemis.Manager;
using Artemis.System;
using GoRogue;
using SpellSword.Actors.Action;
using SpellSword.ECS.Components;

namespace SpellSword.ECS.Systems
{
    [Artemis.Attributes.ArtemisEntitySystem(ExecutionType = ExecutionType.Synchronous, GameLoopType = GameLoopType.Update, Layer = 1)]
    class BasicAISystem : EntityComponentProcessingSystem<BasicAIComponent, ActorComponent>
    {
        public override void Process(Entity entity, BasicAIComponent ai, ActorComponent actor)
        {
            if(actor.Actor.ReadyToAct())
                actor.Actor.Do(new MoveAction(actor.Actor, Direction.RIGHT));
        }
    }
}
