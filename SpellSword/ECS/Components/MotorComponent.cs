using System;
using System.Collections.Generic;
using System.Text;
using Artemis.Interface;
using GoRogue;
using SpellSword.Time;

namespace SpellSword.ECS.Components
{
    class MotorComponentX: IComponent
    {
        public int MoveLeadUp { get; set; }

        public int MoveCoolDown { get; set; }

        public void SetTiming(EventTimerTiming timing)
        {
            MoveLeadUp = timing.Leadup;
            MoveCoolDown = timing.Cooldown;
        }
    }
}
