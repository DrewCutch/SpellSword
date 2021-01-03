using System;
using System.Collections.Generic;
using System.Text;
using GoRogue.GameFramework;
using GoRogue.GameFramework.Components;
using SpellSword.RPG.Items;

namespace SpellSword.Engine.Components
{
    class ItemComponent: IGameObjectComponent
    {
        public IGameObject Parent { get; set; }

        public Item Item { get; }

        public ItemComponent(Item item)
        {
            Item = item;
        }
    }
}
