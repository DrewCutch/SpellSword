using System;
using System.Collections.Generic;
using System.Text;
using Artemis;
using Artemis.Manager;
using Artemis.System;
using BearLib;
using GoRogue;
using SpellSword.Actors.Action;
using SpellSword.ECS.Components;
using SpellSword.Time;

namespace SpellSword.ECS.Systems
{
    [Artemis.Attributes.ArtemisEntitySystem(ExecutionType = ExecutionType.Synchronous, GameLoopType = GameLoopType.Update, Layer = 0)]
    class UserControlSystem: EntityComponentProcessingSystem<UserJoystickComponent, ActorComponent>
    {
        public override void Process(Entity entity, UserJoystickComponent joystick, ActorComponent actor)
        {
            if (!actor.Actor.ReadyToAct())
                return;

            Console.WriteLine("Control happening");

            int key = Terminal.Read();

            if (key == joystick.UpCode)
            {
                actor.Actor.Do(new MoveAction(actor.Actor, Direction.UP));
            }
            else if (key == joystick.RightCode)
            {
                actor.Actor.Do(new MoveAction(actor.Actor, Direction.RIGHT));
            }
            else if (key == joystick.DownCode)
            {
                actor.Actor.Do(new MoveAction(actor.Actor, Direction.DOWN));
            }
            else if (key == joystick.LeftCode)
            {
                actor.Actor.Do(new MoveAction(actor.Actor, Direction.LEFT));
            }
        }
    }
}
