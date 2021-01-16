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

        private event Action<int> OnUpdate;

        private Timeline _timeline;

        public Timeline Timeline
        {
            get => _timeline;
            set
            {
                if (value == _timeline)
                    return;

                if(_timeline != null)
                    _timeline.OnAdvance -= OnAdvance;

                _timeline = value;
                _timeline.OnAdvance += OnAdvance;
            }
        }

        public UpdatingGameObject(Coord position, int layer, IGameObject parentObject, Timeline timeline, bool isStatic = false, bool isWalkable = true, bool isTransparent = true) : base(position, layer, parentObject, isStatic, isWalkable, isTransparent)
        {
            Timeline = timeline;
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
