using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemis;
using Artemis.Manager;
using Artemis.System;
using SpellSword.ECS.Components;
using SpellSword.Time;

namespace SpellSword.ECS.Systems
{
    [Artemis.Attributes.ArtemisEntitySystem(ExecutionType = ExecutionType.Synchronous, GameLoopType = GameLoopType.Update, Layer = 2)]
    class MotionSystem:EntityComponentProcessingSystem<GameObjectComponent, VelocityComponent>
    {
        public override void Process(Entity entity, GameObjectComponent gameObjectComponent, VelocityComponent velocity)
        {
            Console.WriteLine("Motion happening");

            int count = 0;
            foreach (EventTimer.EventTimerMoment moment in velocity.Timer.PendingMoments)
            {
                if (moment == EventTimer.EventTimerMoment.Action)
                {
                    gameObjectComponent.GameObject.Position += velocity.Direction;
                    Console.WriteLine($"Move {count++}");
                }
            }
        }
    }
}
