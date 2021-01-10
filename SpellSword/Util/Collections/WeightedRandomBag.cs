using System;
using System.Collections.Generic;
using System.Text;
using SpellSword.Util.Collections;
using Troschuetz.Random;

namespace SpellSword.MapGeneration
{
    public class WeightedRandomBag<T>: IReadOnlyBag<T>
    {
        private class Choice
        {
            public double AccumulatedWeight;
            public T Value;
        }

        private List<Choice> _choices;
        private double _accumulatedWeight;
        private IGenerator _rng;

        public WeightedRandomBag(IGenerator rng)
        {
            _rng = rng;
        }

        public void AddChoice(T value, float weight)
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
    }

}
