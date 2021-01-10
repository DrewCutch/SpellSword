using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpellSword.Util.Collections;
using Troschuetz.Random;

namespace SpellSword.MapGeneration
{
    public class WeightedRandomBag<T>: IReadOnlyBag<T>
    {
        public int Count => _choices.Count;

        private List<Choice> _choices;
        private int _accumulatedWeight;
        private IGenerator _rng;

        public WeightedRandomBag(IGenerator rng)
        {
            _rng = rng;
        }

        public void AddChoice(T value, int weight)
        {
            _accumulatedWeight += weight;
            Choice choice = new Choice() {AccumulatedWeight = _accumulatedWeight, Value = value};
            _choices.Add(choice);
        }

        public T Get(bool remove = false)
        {
            double r = _rng.NextDouble() * _accumulatedWeight;

            foreach(Choice choice in _choices)
            {
                if (choice.AccumulatedWeight >= r)
                {
                    return choice.Value;
                }
            }

            return default; //should only happen when there are no entries
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
