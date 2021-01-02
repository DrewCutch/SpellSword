using System;
using System.Collections.Generic;
using System.Text;
using GoRogue;
using GoRogue.GameFramework;
using GoRogue.GameFramework.Components;

namespace SpellSword.Engine.Components
{
    class FOVExplorerComponent: IGameObjectComponent
    {
        private IGameObject _parent;

        public IGameObject Parent
        {
            get => _parent;
            set
            {
                _parent = value;
                _parent.Moved += ParentOnMoved;
            }
        }

        public FOVExplorerComponent()
        {
        }

        private void ParentOnMoved(object? sender, ItemMovedEventArgs<IGameObject> e)
        {
            Parent.CurrentMap?.CalculateFOV(Parent.Position);
        }
    }
}
