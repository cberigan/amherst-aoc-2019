using AdventOfCodeChallenges.C10;
using AdventOfCodeChallenges.Core;
using System;
using System.Collections.Generic;

namespace AdventOfCodeChallenges.C15
{
    /// <summary>
    /// Builds the distances from the origin for each given point, given that the points are added to an adjacent 
    /// point that has already been added.
    /// </summary>
    public class PathFindingMap
    {
        private readonly Dictionary<Coordinate, int> _data = new Dictionary<Coordinate, int>();

        unsafe public void Add(Coordinate c)
        {
            if (_data.ContainsKey(c)) return;

            var lowestNeighbor = int.MaxValue;
            foreach (var cell in stackalloc Coordinate[]
                                        {
                                            c.MoveUp(), c.MoveDown(), c.MoveLeft(), c.MoveRight()
                                        })
            {
                if (_data.TryGetValue(cell, out var dist))
                {
                    lowestNeighbor = Math.Min(lowestNeighbor, dist);
                }
            }

            if (lowestNeighbor != int.MaxValue)
                _data[c] = lowestNeighbor + 1;
            else _data[c] = 0; // origin
        }

        public int ShortestPathSteps(Coordinate dest)
        {
            return _data[dest];
        }
    }
}
