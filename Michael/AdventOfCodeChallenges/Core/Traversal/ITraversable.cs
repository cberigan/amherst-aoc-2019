using AdventOfCodeChallenges.C10;
using System.Collections.Generic;

namespace AdventOfCodeChallenges.Core.Traversal
{
    public interface ITraversable<T> 
    {
        T this [Coordinate c] { get; }

        Coordinate OriginCoordinates { get; }
        T OriginTile { get; }

    }
}