using Artemis.Interface;
using GoRogue;
using SpellSword.Time;

namespace SpellSword.ECS.Components
{
    class VelocityComponent: IComponent
    {
        public Direction Direction { get; set; }
        public EventTimer Timer { get; set; }
    }
}
