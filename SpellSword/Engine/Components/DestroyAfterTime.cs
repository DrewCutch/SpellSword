using GoRogue.GameFramework;
using GoRogue.GameFramework.Components;
using SpellSword.Update;

namespace SpellSword.Engine.Components
{
    class DestroyAfterTime: Component, IUpdate
    {
        public int Time { get; }
        
        private int _count;

        public DestroyAfterTime(int time)
        {
            Time = time;
            _count = 0;
        }

        public void Update(int ticks)
        {
            _count += ticks;

            if (_count >= Time)
                Parent.CurrentMap.RemoveEntity(Parent);
        }

        public override Component CloneTo(IGameObject gameObject)
        {
            DestroyAfterTime clone = new DestroyAfterTime(Time);
            gameObject.AddComponent(clone);

            return clone;
        }
    }
}
