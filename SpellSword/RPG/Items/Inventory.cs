using System;
using System.Collections.Generic;
using System.Text;

namespace SpellSword.RPG.Items
{
    class Inventory
    {
        public IReadOnlyDictionary<Item, int> Items => _items;
        private SortedDictionary<Item, int> _items;

        public event Action<Item> OnAdd;
        public event Action<Item> OnRemove;

        public Inventory()
        {
            _items = new SortedDictionary<Item, int>();
        }

        public void Add(Item item)
        {
            Add(item, 1);
        }

        public void Add(Item item, int amount)
        {
            int currentAmount = _items.GetValueOrDefault(item, 0);
            _items[item] = currentAmount + amount;

            OnAdd?.Invoke(item);
        }

        public void Remove(Item item)
        {
            Remove(item, 1);
        }

        public void Remove(Item item, int amount)
        {
            _items[item] -= amount;

            OnRemove?.Invoke(item);
        }
    }
}
