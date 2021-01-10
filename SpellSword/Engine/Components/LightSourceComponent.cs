using System;
using System.Collections.Generic;
using System.Text;
using GoRogue;
using GoRogue.GameFramework;
using GoRogue.GameFramework.Components;
using SpellSword.Render.Lighting;

namespace SpellSword.Engine.Components
{
    class LightSourceComponent: Component
    {
        private IGameObject _parent;

        public override IGameObject Parent
        {
            get => _parent;
            set
            {
                value.Moved += OnMoved;
                _parent = value;

                _lightMap.AddLight(new Light(_light.Color, Parent.Position, _light.Range, _light.Brightness));
            }
        }

        private LightMap _lightMap;

        private Light _light;

        public LightSourceComponent(LightMap lightMap, Light light)
        {
            _lightMap = lightMap;
            _light = light;
        }

        private void OnMoved(object sender, ItemMovedEventArgs<IGameObject> e)
        {
            _lightMap.RemoveLight(_light);
            _light = new Light(_light.Color, Parent.Position, _light.Range, _light.Brightness);
            _lightMap.AddLight(_light);
        }
    }
}
