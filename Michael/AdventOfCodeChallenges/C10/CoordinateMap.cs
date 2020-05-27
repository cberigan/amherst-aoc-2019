using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace AdventOfCodeChallenges.C10
{
    public class CoordinateMap
    {
        private readonly bool[,] map;

        public CoordinateMap(bool[,] map) => this.map = map;

        public bool IsEmpty
        {
            get
            {
                int count = 0;
                foreach (var x in AsteroidLocations)
                {
                    count++;
                    if (count == 2) return false;
                }
                return true;
            }
        }

        public IEnumerable<Coordinate> AsteroidLocations =>
            from y in Enumerable.Range(0, map.GetUpperBound(0) + 1)
            from x in Enumerable.Range(0, map.GetUpperBound(1) + 1)
            let hasAsteroid = map[y, x]
            where hasAsteroid
            select new Coordinate(x, y);

        public IEnumerable<(Coordinate target, double degrees)> VisibleAsteroids(Coordinate point)
        {
            var res = from target in AsteroidLocations
                      where point != target
                      let degrees = Math.Atan2(target.Y - point.Y, target.X - point.X) * 180 / Math.PI
                      select (target, degrees);
            var r = res
                .OrderBy(r => point.DistanceTo(r.target))
                .DistinctBy(x => x.degrees) //  only shows the first one along the slope
                .ToList();
            return r;
        }

        public void RemoveAll(IEnumerable<Coordinate> targets)
        {
            foreach (var target in targets)
                map[target.Y, target.X] = false;
        }

        internal void Remove(Coordinate target) => map[target.Y, target.X] = false;

        // ************** This is where actually knowing math pays off :\ I'm leaving it lol **********************************
        //public IEnumerable<Coordinate> VisibleAsteroids(Coordinate point)
        //{
        //    var start = point;

        //    var offsetsToAsteroids =
        //        (from y in Enumerable.Range(0, map.GetUpperBound(0)+ 1)
        //        from x in Enumerable.Range(0, map.GetUpperBound(1)+ 1)
        //        where  (x,y) != point && map[y, x]
        //        select (x, y) - point)
        //        .ToArray();

        //    var coordinateMultiples =
        //        from a in offsetsToAsteroids
        //        join b in offsetsToAsteroids on a + a equals b
        //        where a != b
        //        select b;

        //    var blockedAsteroidsInCompassDirections = _();
        //    IEnumerable<Coordinate> _()
        //    {
        //        // return blocked coordinates in the 8 cardinal directions. 
        //        // using the multiples doesn't work on straight angles b/c those  jump
        //        // across the map and don't account for asteroids that are right behind each other in these 8 directions
        //        Coordinate c = point;
        //        var arr = new[] { (-1, -1), (0, -1), (1,-1), (1, 0), (1,1), (0, 1), (-1, 1), (-1, 0) };
        //        var blocked = new bool[8];
        //        var outOfmap = new bool[8];
        //        int i  = 0, round = 1;
        //        while (true)
        //        {
        //            if (!outOfmap[i])
        //            {
        //                var (x, y) = arr[i];
        //                c = point + (x * round, y * round);
        //                if (MapContains(c))
        //                {
        //                    if (HasAsteroid(c))
        //                        if (blocked[i])
        //                            yield return c;
        //                        else
        //                            blocked[i] = true;
        //                }
        //                else
        //                {
        //                    outOfmap[i] = true;
        //                    if (outOfmap.All(b => b)) break;
        //                }
        //            }

        //            i++;
        //            if (i == arr.Length)
        //            {
        //                i = 0;
        //                round++;
        //            }
        //        }
        //    };

        //    var offsetAbsolute = offsetsToAsteroids.Select(o => o + point);
        //    var coordinateMultipleAbsolute = coordinateMultiples.Select(o => o + point);

        //    var visibleAsteroidAbsoluteCoordinates =
        //        offsetAbsolute
        //        .Except(coordinateMultipleAbsolute
        //                .Concat(blockedAsteroidsInCompassDirections))
        //        .ToList();

        //    return visibleAsteroidAbsoluteCoordinates;
        //}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasAsteroid(Coordinate c) => map[c.Y, c.X];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool MapContains(Coordinate coordinate) =>
            coordinate.X >= 0 && coordinate.X <= map.GetUpperBound(1)
                && coordinate.Y >= 0 && coordinate.Y <= map.GetUpperBound(0);
    }
}
