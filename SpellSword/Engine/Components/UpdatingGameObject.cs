using System;
using GoRogue;
using GoRogue.GameFramework;
using SpellSword.Logging;
using SpellSword.Time;
using SpellSword.Update;

namespace SpellSword.Engine.Components
{
    class UpdatingGameObject: GameObject
    {
        public static Timeline MainTimeline { get; set; }
        
        private event Action<int> OnUpdate;

        public static UpdatingGameObject CreateUpdatingGameObject(Coord position, int layer, IGameObject parentObject, bool isStatic = false, bool isWalkable = true, bool isTransparent = true)
        {
            return new UpdatingGameObject(position, layer, parentObject, MainTimeline, isStatic, isWalkable, isTransparent);
        }

        public UpdatingGameObject(Coord position, int layer, IGameObject parentObject, Timeline timeline, bool isStatic = false, bool isWalkable = true, bool isTransparent = true) : base(position, layer, parentObject, isStatic, isWalkable, isTransparent)
        {
            timeline.OnAdvance += OnAdvance;
        }

        public UpdatingGameObject(Coord position, int layer, IGameObject parentObject, bool isStatic = false, bool isWalkable = true, bool isTransparent = true) : base(position, layer, parentObject, isStatic, isWalkable, isTransparent)
        {
            MainTimeline.OnAdvance += OnAdvance;
        }

        private void OnAdvance(int ticks)
        {
            OnUpdate?.Invoke(ticks);
        }

        public override void AddComponent(object component)
        {
            base.AddComponent(component);
            if (component is IUpdate updatable)
                OnUpdate += updatable.Update;
        }
    }
}
