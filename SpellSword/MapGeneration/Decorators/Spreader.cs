using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using GoRogue;
using GoRogue.GameFramework;
using GoRogue.Pathing;
using GoRogue.Random;
using Priority_Queue;
using SpellSword.Game;
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


        private IPlaceable _generator;

        public Spreader(IPlaceable generator, int startingEnergy, float spreadChance, float deadChance)
        {
            _generator = generator;

            StartingEnergy = startingEnergy;
            SpreadChance = spreadChance;
            DeadChance = deadChance;
        }

        public bool Place(Floor floor, Coord pos, IGenerator rng)
        {
            int energy = StartingEnergy;

            Queue<Coord> frontier = new Queue<Coord>();
            HashSet<Coord> explored = new HashSet<Coord>();

            frontier.Enqueue(pos);

            while (frontier.Count > 0 && energy > 0)
            {
                Coord next = frontier.Dequeue();

                if (rng.NextDouble() > SpreadChance)
                {
                    energy--;

                    if (rng.NextDouble() < DeadChance)
                        explored.Add(next);

                    continue;
                }

                if (!floor.MapInfo.Map.IsWalkable(next) || !_generator.CanPlace(floor, next, rng)) 
                    continue;

                _generator.Place(floor, next, rng);

                explored.Add(next);

                energy--;
                foreach (Coord neighbor in AdjacencyRule.CARDINALS.Neighbors(next))
                {
                    if(!explored.Contains(neighbor))
                        frontier.Enqueue(neighbor);
                }
            }

            return true;
        }

        public bool CanPlace(Floor floor, Coord pos, IGenerator rng)
        {
            return _generator.CanPlace(floor, pos, rng);
        }
    }
}
