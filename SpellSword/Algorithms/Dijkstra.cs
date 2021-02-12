using System;
using System.Collections.Generic;
using System.Text;
using GoRogue;
using GoRogue.MapViews;
using GoRogue.Pathing;
using Priority_Queue;

namespace SpellSword.Algorithms
{
    class Dijkstra
    {
        public Distance Distance { get; }

        public IMapView<bool> AvailabilityMap { get; }
        public IMapView<bool> WalkabilityMap { get; }

        public Dijkstra(IMapView<bool> availabilityMap, IMapView<bool> walkabilityMap) : this(availabilityMap, walkabilityMap, Distance.EUCLIDEAN) { }

        public Dijkstra(IMapView<bool> availabilityMap, IMapView<bool> walkabilityMap, Distance distance)
        {
            AvailabilityMap = availabilityMap;
            WalkabilityMap = walkabilityMap;
            Distance = distance;
        }

        public Coord GetClosestAvailablePoint(Coord coord)
        {
            FastPriorityQueue<DijkstraNode> frontier = new FastPriorityQueue<DijkstraNode>(AvailabilityMap.Height * AvailabilityMap.Width);
            DijkstraNode[] explored = new DijkstraNode[AvailabilityMap.Height * AvailabilityMap.Width];

            AdjacencyRule adjacencyRule = Distance;

            DijkstraNode initial = new DijkstraNode(coord, null);
            initial.Distance = 0;

            frontier.Enqueue(initial, 0);

            while (frontier.Count > 0)
            {
                DijkstraNode current = frontier.Dequeue();

                if (AvailabilityMap[current.Position])
                    return current.Position;

                foreach (Coord neighbor in adjacencyRule.Neighbors(current.Position))
                {
                    if(!WalkabilityMap[neighbor])
                        continue;

                    int neighborIndex = neighbor.ToIndex(AvailabilityMap.Width);
                    bool neighborExplored = explored[neighborIndex] != null;
                    float neighborDistance = (float)Distance.Calculate(current.Position, neighbor) + current.Distance;

                    DijkstraNode neighborNode =
                        neighborExplored ? explored[neighborIndex] : new DijkstraNode(neighbor, current);

                    explored[neighborIndex] = neighborNode;

                    if (neighborExplored && neighborDistance < neighborNode.Distance)
                    {
                        neighborNode.Distance = neighborDistance;
                        frontier.UpdatePriority(neighborNode, neighborNode.Distance);
                    }
                    else
                    {
                        neighborNode.Distance = neighborDistance;
                        frontier.Enqueue(neighborNode, neighborDistance);
                    }
                        
                }
            }

            return Coord.NONE;
        }

        internal class DijkstraNode : FastPriorityQueueNode
        {
            public readonly Coord Position;

            // (Known) distance from start to this node, by shortest known path
            public float Distance;

            // Previous node
            public DijkstraNode Parent;
            
            public DijkstraNode(Coord position, DijkstraNode parent = null)
            {
                Position = position;
                Parent = parent;
                Position = position;
                Distance = float.MaxValue;
            }
        }
    }
}
