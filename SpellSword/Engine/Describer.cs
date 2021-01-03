using System;
using System.Collections.Generic;
using System.Text;
using GoRogue.GameFramework;
using GoRogue.MapViews;
using GoRogue.Messaging;
using SpellSword.Engine.Components;
using SpellSword.Input;
using SpellSword.Render.Panes;

namespace SpellSword.Engine
{
    class Describer : ISubscriber<MouseMoveEvent>
    {
        public Map Map { get; }

        private readonly TextPane _descriptionPane;

        public Describer(TextPane descriptionPane, Map map)
        {
            _descriptionPane = descriptionPane;
            Map = map;
        }

        public void Handle(MouseMoveEvent message)
        {
            StringBuilder builder = new StringBuilder();

            if (!Map.Contains(message.NewPos))
                return;

            bool first = true;

            foreach (IGameObject gameObject in Map.GetObjects(message.NewPos))
            {
                if (gameObject.GetComponent<NameComponent>() is NameComponent nameComponent)
                {
                    if (!first)
                    {
                        builder.Append(" on ");
                    }

                    builder.Append(nameComponent.Name);
                    first = false;
                }
            }

            _descriptionPane.Text = builder.ToString();
        }
    }
}
