using System;
using System.Collections.Generic;
using System.Text;

namespace SpellSword.RPG.Items
{
    class Inventory
    {
        public IReadOnlyList<Item> Items => _items;
        private List<Item> _items;

        public event Action<Item> OnAdd;
        public event Action<Item> OnRemove;

        public Inventory()
        {
            _items = new List<Item>();
        }

        public void Add(Item item)
        {
            _items.Add(item);

            OnAdd?.Invoke(item);
        }

        public void Remove(Item item)
        {
            _items.Remove(item);

            OnRemove?.Invoke(item);
        }
    }
}
