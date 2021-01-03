using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using GoRogue;
using GoRogue.GameFramework;
using GoRogue.GameFramework.Components;
using SpellSword.Actors;
using SpellSword.Actors.Action;
using SpellSword.Logging;
using SpellSword.RPG.Items;
using SpellSword.Speech;
using SpellSword.Time;

namespace SpellSword.Engine.Components
{
    class ItemComponent: Component, IInteractable
    {
        public Item Item { get; }
        public int Range => 1;
        public Distance RangeDistanceType => Distance.MANHATTAN;

        public ItemComponent(Item item)
        {
            Item = item;
        }

        public void Interact(Actor by)
        {
            Parent.CurrentMap.RemoveEntity(Parent);
            by.Being.Inventory.Add(Item);

            by.MainBus.Send(new LogMessage($"{{0}} picked up {Item.Title.Article.WithTrailingSpace()}{Item.Title.Name}", new LogLink(by.Being.Name, Color.Aquamarine, by)));
        }

        public EventTimerTiming InteractTiming(Actor by)
        {
            return new EventTimerTiming(100,0);
        }
    }
}
