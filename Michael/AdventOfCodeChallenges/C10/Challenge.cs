using AdventOfCodeChallenges.Core;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Linq;

namespace AdventOfCodeChallenges.C10
{
    public static partial class Challenge
    {
        public static (bool[,] map, (int x, int y) station) Parse(string input)
        {
            var lines = input.Split("\r\n");
            var output = new bool[lines.Length, lines[0].Length];
            var station = (x:-1, y:-1);
            for (int y = 0; y <= lines.GetUpperBound(0); y++)
            {
                for (int x = 0; x < lines[0].Length; x++)
                {
                    output[y, x] = lines[y][x] == '#';
                    if (lines[y][x] == 'X')
                        station = (x, y);
                }
            }

            return (output, station);
        }


        public class Pt1
        {
            /// <summary>
            /// Key: count of visible items. 
            /// Values: Each asteroid and which ones it can see.
            /// </summary>
            public (Coordinate asteroid, IEnumerable<Coordinate> visibles) Run(string input)
            {
                var map = Parse(input);
                var coordinateMap = new CoordinateMap(map.map);
                var asteroids = coordinateMap.AsteroidLocations;

                var allProjections =
                    from asteroid in asteroids
                    select (asteroid, visibles: coordinateMap.VisibleAsteroids(asteroid).Select(x => x.target));

                var best = allProjections.Select(x => (x, count: x.visibles.Count()))
                    .GroupBy(xx => xx.count)
                    .MaxBy(x => x.Key)
                    .First()
                    .Single()
                    .x;

                return best;
            }
        }

        public class Pt2
        {
            public int Run(string input, (int x, int y)? station = null)
            {
                if (station == null) {
                    var c = new Pt1().Run(input).asteroid;
                    station = (c.X, c.Y);
                }
                var coord = RunImpl(input,station).ElementAt(199);
                var res = coord.target.X * 100 + coord.target.Y;
                return res;
            }

            public IEnumerable<(Coordinate target, double degrees)> RunImpl(string input, (int x, int y)? station = null)
            {
                var map = Parse(input);
                var coordinateMap = new CoordinateMap(map.map);

                while (!coordinateMap.IsEmpty)
                {
                    var destroyedAsteroids = coordinateMap.VisibleAsteroids(station ?? map.station)
                        .Select(x =>
                        {
                            var deg = x.degrees;
                            //rotate left to start at the top
                            if (deg < 0) deg += 360;
                            deg += 90;

                            if (deg >= 360) deg -= 360;
                            
                            return (x.target, deg);
                        })
                        .OrderBy(x => x.deg)
                        .ToArray();

                    foreach (var item in destroyedAsteroids)
                    {
                        coordinateMap.Remove(item.target);
                        yield return item;
                    }
                }
            }

            public void Visualize(string input)
            {
                Console.Write(input);
                Console.SetCursorPosition(0, 0);
                Console.SetCursorPosition(11,13);
                Console.Write('X');

                foreach (var x in RunImpl(input, (11,13)))
                {
                    Console.SetCursorPosition(x.target.X, x.target.Y);
                    Console.Write(',');
                    Thread.Sleep(100);
                }
            }
        }
    }
}
