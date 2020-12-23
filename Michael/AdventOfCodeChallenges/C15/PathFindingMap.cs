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
            foreach (var cell in stackalloc Coordinate[] {
                c.MoveUp(),
                c.MoveDown(),
                c.MoveLeft(),
                c.MoveRight()
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

        public int ShortestPathSteps(Coordinate dest) => _data[dest];

        public int MinutesToFill(Coordinate startPoint)
        {
            // from a start point with oxygen filling at 1 cell per minute,
            // how many minutes will it take to fill everywhere

            var filledCells = new HashSet<Coordinate>(_data.Count) { startPoint };

            var c = startPoint;

            var curr = new List<Coordinate>();
            var next = new List<Coordinate>();

            SetNeighbors(c, curr, _data); // with my challenge, should add the neighbor to the left
            unsafe static void SetNeighbors(Coordinate coord, List<Coordinate> l, Dictionary<Coordinate, int> data)
            {
                l.Clear();
                Span<Coordinate> neighbors = stackalloc Coordinate[4];
                neighbors[0] = coord.MoveUp();
                neighbors[1] = coord.MoveDown();
                neighbors[2] = coord.MoveLeft();
                neighbors[3] = coord.MoveRight();

                foreach (var c in neighbors)
                {
                    if (data.ContainsKey(c))
                        l.Add(c);
                }
            }

            int minutes = 0;

            // goes over each neighbor's neighbors that haven't been viside/filled and adds them to the next list of neighbors
            // to go over
            static void CheckNeighbors(List<Coordinate> coords, HashSet<Coordinate> visited, Dictionary<Coordinate, int> data,
                List<Coordinate> next)
            {
                for (int i = 0; i < coords.Count; i++)
                {
                    var c = coords[i];

                    if (data.ContainsKey(c) && visited.Add(c))
                    {
                        CheckExists(c.MoveUp(), visited, data, next);
                        CheckExists(c.MoveLeft(), visited, data, next);
                        CheckExists(c.MoveDown(), visited, data, next);
                        CheckExists(c.MoveRight(), visited, data, next);
                    }
                }

                void CheckExists(Coordinate x, HashSet<Coordinate> v, Dictionary<Coordinate, int> d, List<Coordinate> n)
                {
                    if (d.ContainsKey(x) && !v.Contains(x))
                        n.Add(x);
                }
            }

            while (filledCells.Count < _data.Count)
            {
                CheckNeighbors(curr, filledCells, _data, next);

                curr.Clear();
                (curr, next) = (next, curr);

                minutes++;
            }

            return minutes;
        }
    }
}
