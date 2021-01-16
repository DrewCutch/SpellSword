using System;
using System.Collections.Generic;
using System.Text;
using GoRogue;
using GoRogue.GameFramework;

namespace SpellSword.Engine.Components
{
    class TriggerComponent: Component
    {
        private Action<IGameObject, Direction> _onEnter;

        private Action<IGameObject, Direction> _onExit;

        public TriggerComponent(Action<IGameObject, Direction> onEnter, Action<IGameObject, Direction> onExit)
        {
            _onEnter = onEnter;
            _onExit = onExit;
        }

        public void Enter(IGameObject gameObject, Direction direction)
        {
            _onEnter.Invoke(gameObject, direction);
        }

        public void Exit(IGameObject gameObject, Direction direction)
        {
            _onExit.Invoke(gameObject, direction);
        }
    }
}
