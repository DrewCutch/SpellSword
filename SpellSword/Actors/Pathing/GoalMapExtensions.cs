using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoRogue;
using GoRogue.MapViews;

namespace SpellSword.Actors.Pathing
{
    static class GoalMapExtensions
    {
        public static IEnumerable<Direction> GetImprovingDirections(this IMapView<double?> goalMap, Coord position,
            AdjacencyRule adjacencyRule)
        {
            double current = goalMap[position].HasValue ? goalMap[position].Value : double.MaxValue;

            List<(Direction, double)> improvements = new List<(Direction, double)>();
            improvements.Add((Direction.NONE, current));

            foreach (Direction dir in adjacencyRule.DirectionsOfNeighbors())
            {
                Coord newPosition = position + dir;
                if (!goalMap[newPosition].HasValue)
                    continue;

                // Consider orthogonal movement in order to "slip" around obstacles (less than 1 extra distance)
                if (goalMap[newPosition].Value  - current < 1)
                {
                    improvements.Add((dir, goalMap[newPosition].Value));
                }
            }

            improvements.Sort((a, b) => a.Item2.CompareTo(b.Item2));
            return improvements.Select((improvement) => improvement.Item1);
        }

    }
}
