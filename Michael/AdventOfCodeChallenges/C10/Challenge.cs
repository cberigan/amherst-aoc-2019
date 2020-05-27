using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCodeChallenges.C10
{
    public static partial class Challenge
    {
        public static bool[,] Parse(string input)
        {
            var lines = input.Split("\r\n");
            var output = new bool[lines.Length, lines[0].Length];
            for (int y = 0; y <= lines.GetUpperBound(0); y++)
            {
                for (int x = 0; x < lines[0].Length; x++)
                {
                    output[y, x] = lines[y][x] == '#';
                }
            }

            return output;
        }


        public class Pt1
        {
            /// <summary>
            /// Key: count of visible items. 
            /// Values: Each asteroid and which ones it can see.
            /// </summary>
            public Dictionary<int, IEnumerable<(Coordinate asteroid, IEnumerable<Coordinate> visibles)>> 
                Run(string input)
            {
                var map = Parse(input);
                var coordinateMap = new CoordinateMap(map);
                var asteroids = coordinateMap.AsteroidLocations;

                var allProjections =
                    from asteroid in asteroids
                    select (asteroid, visibles: coordinateMap.VisibleAsteroids(asteroid));

                var bests = allProjections.Select(x => (x, count: x.visibles.Count()))
                    .GroupBy(xx => xx.count)
                    .ToDictionary(g=> g.Key, g => g.Select(x => x.x));

                return bests;
            }
        }
    }
}
