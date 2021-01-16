using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoRogue.Random;
using SpellSword.Util.Collections;
using Troschuetz.Random;

namespace SpellSword.MapGeneration
{
    public class WeightedRandomBag<T>: IReadOnlyBag<T>
    {
        public int Count => _choices.Count;

        private List<Choice> _choices;
        private int _accumulatedWeight;
        private readonly IGenerator _defaultRng;

        public WeightedRandomBag(IGenerator defaultRng)
        {
            _defaultRng = defaultRng;
            _choices = new List<Choice>();
        }

        public WeightedRandomBag(IEnumerable<(T, int)> choices, IGenerator rng = null): this(rng)
        {
            foreach ((T value, int weight) in choices)
            {
                AddChoice(value, weight);
            }
        }

        public void AddChoice(T value, int weight)
        {
            _accumulatedWeight += weight;
            Choice choice = new Choice() {AccumulatedWeight = _accumulatedWeight, Value = value};
            _choices.Add(choice);
        }

        public T Get(IGenerator rng, bool remove = false)
        {
            double r = rng.NextDouble() * _accumulatedWeight;

            foreach (Choice choice in _choices)
            {
                if (choice.AccumulatedWeight >= r)
                {
                    return choice.Value;
                }
            }

            return default; //should only happen when there are no entries
        }

        public T Get(bool remove = false)
        {
            return Get(_defaultRng, remove);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _choices.Select(choice => choice.Value).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private class Choice
        {
            public int AccumulatedWeight;
            public T Value;
        }
    }

}
