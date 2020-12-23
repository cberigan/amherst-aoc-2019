using AdventOfCodeChallenges.C10;

namespace AdventOfCodeChallenges.Core.Traversal
{

    public class NodeBasedMap<T, TMetadata> 
        where TMetadata : struct, ITerrainMetadata<T>
    {
        private readonly Tree<(Coordinate c, T terrain, int steps)> _paths = new();

        public void Map(ITraversable<T> traversable)
        {
            var node = _paths.GetOrAdd((traversable.OriginCoordinates, traversable.OriginTile, 0));
            Map(traversable, node, node.Value.c, node.Value.steps);
        }

        private void Map(ITraversable<T> traversable, 
            TreeNode<(Coordinate c, T terrain, int steps)> lastNode,
            Coordinate position, int runningSteps)
        {
            NeighborEnumerable<T> neighborEnumerable = new NeighborEnumerable<T>(traversable, position);

            // not walls

            TMetadata metadata = default;

            foreach ((T tile, Coordinate c) in neighborEnumerable)
            {
                if (c == lastNode.Value.c) continue; // don't go backwards! You'll never leave!
                if (metadata.IsPointOfInterest(tile))
                {
                    var newNode = lastNode.AddChild((c, tile, runningSteps));
                    Map(traversable, newNode, c, runningSteps + 1);
                }
                else if (metadata.IsPassable(tile)) // floor, no walls
                {
                    Map(traversable, lastNode, c, runningSteps + 1);
                }
            }
        }

    }
}
