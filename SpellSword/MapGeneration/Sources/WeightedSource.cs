using System;
using System.Collections.Generic;
using System.Text;
using SpellSword.Util;
using Troschuetz.Random;

namespace SpellSword.MapGeneration.Sources
{
    class WeightedSource<T>: Source<T>
    {
        public int Count => _choices.Count;

        private List<Choice> _choices;
        private int _accumulatedWeight;

        public WeightedSource(bool shared): base(shared)
        {
            _choices = new List<Choice>();
            _accumulatedWeight = 0;
        }

        public void Add(Source<T> source, int weight)
        {
            _accumulatedWeight += weight;
            Choice choice = new Choice() { Weight = weight, Value = source };
            _choices.Add(choice);
        }

        public override SourceCursor<T> Pull(IGenerator rng)
        {
            double r = rng.NextDouble() * _accumulatedWeight;
            int weightSoFar = 0;

            foreach ((Choice choice, int i) in _choices.WithIndex())
            {
                weightSoFar += choice.Weight;
                if (weightSoFar >= r)
                {
                    SourceCursor<T> choiceCursor = choice.Value.Pull(rng);

                    choiceCursor.OnUsed += (val) => { OnUseChoice(i);};

                    return choiceCursor;
                }
            }


            return default;
        }

        private void OnUseChoice(int i)
        {
            Choice choice = _choices[i];

            if (!choice.Value.IsEmpty())
                return;

            _accumulatedWeight -= choice.Weight;

            _choices.RemoveAt(i);
        }

        public override bool IsEmpty()
        {
            return Count == 0;
        }

        public override Source<T> Clone()
        {
            if (Shared)
                return this;

            WeightedSource<T> clone = new WeightedSource<T>(false);

            foreach (Choice choice in _choices)
            {
                clone.Add(choice.Value.Clone(), choice.Weight);
            }

            return clone;
        }

        private class Choice
        {
            public int Weight;
            public Source<T> Value;
        }
    }
}
