using System;
using System.Collections.Generic;
using System.Text;
using GoRogue;
using GoRogue.GameFramework;
using GoRogue.MapViews;
using GoRogue.Messaging;
using SpellSword.Engine.Components;
using SpellSword.Input;
using SpellSword.Render.Panes;
using SpellSword.Speech;

namespace SpellSword.Engine
{
    class Describer : ISubscriber<MouseMoveEvent>
    {
        public Map Map { get; set; }

        private readonly TextPane _descriptionPane;

        public Describer(TextPane descriptionPane)
        {
            _descriptionPane = descriptionPane;
        }

        public void Handle(MouseMoveEvent message)
        {
            StringBuilder builder = new StringBuilder();

            if (!Map.Contains(message.NewPos))
                return;

            Coord gridPos = new Coord(message.NewPos.X / 2, message.NewPos.Y);

            bool first = true;

            foreach (IGameObject gameObject in Map.GetObjects(gridPos))
            {
                if (gameObject.GetComponent<DecalComponent>() is DecalComponent decalComponent && decalComponent.Decal != null)
                {
                    if (!first)
                    {
                        builder.Append($" {decalComponent.Decal.Title.Preposition} ");
                    }

                    builder.Append(decalComponent.Decal.Title.Article.WithTrailingSpace() + decalComponent.Decal.Title.Name);
                    first = false;
                }

                if (gameObject.GetComponent<NameComponent>() is NameComponent nameComponent)
                {
                    if (!first)
                    {
                        builder.Append($" {nameComponent.Title.Preposition} ");
                    }

                    builder.Append(nameComponent.Title.Article.WithTrailingSpace() + nameComponent.Title.Name);
                    first = false;
                }
            }

            _descriptionPane.Text = builder.ToString();
        }
    }
}
