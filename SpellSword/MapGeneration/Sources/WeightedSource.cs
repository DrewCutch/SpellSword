using System;
using System.Collections.Generic;
using System.Text;
using SpellSword.Util;
using Troschuetz.Random;

namespace SpellSword.MapGeneration.Sources
{
    class WeightedSource<TContext, TValue>: Source<TContext, TValue> where TContext: IRandomContext 
    {
        public int Count => _choices.Count;

        private List<Choice> _choices;
        private int _accumulatedWeight;

        private bool _sticky;
        private int _stuckChoice;

        public WeightedSource(bool shared, bool sticky = false): base(shared)
        {
            _choices = new List<Choice>();
            _accumulatedWeight = 0;
            _sticky = sticky;
            _stuckChoice = -1;
        }

        public void Add(Source<TContext, TValue> source, int weight)
        {
            _accumulatedWeight += weight;
            Choice choice = new Choice() { Weight = weight, Value = source };
            _choices.Add(choice);
        }

        public override SourceCursor<TValue> Pull(TContext context)
        {
            int r = context.Generator.Next(0, _accumulatedWeight + 1);
            int weightSoFar = 0;

            int sourceIndex = _stuckChoice;

            if (sourceIndex == -1)
            {
                foreach ((Choice choice, int i) in _choices.WithIndex())
                {
                    weightSoFar += choice.Weight;
                    if (weightSoFar < r) 
                        continue;

                    sourceIndex = i;
                    break;
                }
            }

            if (_sticky)
                _stuckChoice = sourceIndex;

            SourceCursor<TValue> choiceCursor = _choices[sourceIndex].Value.Pull(context);
            choiceCursor.OnUsed += (val) => { OnUseChoice(sourceIndex); };

            return choiceCursor;
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

        public override Source<TContext, TValue> Clone()
        {
            if (Shared)
                return this;

            WeightedSource<TContext, TValue> clone = new WeightedSource<TContext, TValue>(false);

            foreach (Choice choice in _choices)
            {
                clone.Add(choice.Value.Clone(), choice.Weight);
            }

            return clone;
        }

        private class Choice
        {
            public int Weight;
            public Source<TContext, TValue> Value;
        }
    }
}
