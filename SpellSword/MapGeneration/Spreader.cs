using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using GoRogue;
using GoRogue.GameFramework;
using GoRogue.Pathing;
using GoRogue.Random;
using Priority_Queue;
using Troschuetz.Random;

namespace SpellSword.MapGeneration
{
    class Spreader: IPlaceable
    {
        // The number of spread tries
        public int StartingEnergy { get; }

        // The chance that a spread attempt to succeed
        public float SpreadChance { get; }

        // The chance that a failed spread attempt will result in a "dead" space
        // (a space that will not be attempted again
        public float DeadChance { get; }

        private IGenerator _rng;


        private Func<Coord, GameObject> _generator;

        public Spreader(Func<Coord, GameObject> generator, IGenerator rng, int startingEnergy, float spreadChance, float deadChance)
        {
            _generator = generator;
            _rng = rng;

            StartingEnergy = startingEnergy;
            SpreadChance = spreadChance;
            DeadChance = deadChance;
        }

        public bool Place(Map map, Coord pos)
        {
            int energy = StartingEnergy;

            Queue<Coord> frontier = new Queue<Coord>();
            HashSet<Coord> explored = new HashSet<Coord>();

            frontier.Enqueue(pos);

            while (frontier.Count > 0 && energy > 0)
            {
                Coord next = frontier.Dequeue();
                explored.Add(next);

                if (_rng.NextDouble() > SpreadChance)
                {
                    energy--;

                    if (_rng.NextDouble() < DeadChance)
                        explored.Add(next);
                    //frontier.Enqueue(next);

                    continue;
                }

                GameObject obj = _generator(next);

                if (!map.IsWalkable(next) || !map.AddEntity(obj)) 
                    continue;


                energy--;
                foreach (Coord neighbor in AdjacencyRule.CARDINALS.Neighbors(next))
                {
                    if(!explored.Contains(neighbor))
                        frontier.Enqueue(neighbor);
                }
            }

            return true;
        }
    }
}
