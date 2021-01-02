using GoRogue.GameFramework;
using GoRogue.GameFramework.Components;
using SpellSword.Update;

namespace SpellSword.Engine.Components
{
    class DestroyAfterTime: IGameObjectComponent, IUpdate
    {
        public IGameObject Parent { get; set; }

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
    }
}
