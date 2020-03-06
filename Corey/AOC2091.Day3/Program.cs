using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace AOC2091.Day3
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt");

            var moves1 = lines[0].Split(',');
            var moves2 = lines[1].Split(',');

            var path1 = BuildPath(moves1);
            var path2 = BuildPath(moves2);


            var min = int.MaxValue;
            var minSteps = int.MaxValue;
            foreach (var pair in path2)
            {
                var newPoint = pair.Key;
                var steps2 = pair.Value.Item2;

                if (path1.ContainsKey(newPoint))
                {
                    var totalSteps = path1[newPoint].Item2 + steps2;
                    if (path1[newPoint].Item1 < min) min = path1[newPoint].Item1;
                    if (totalSteps < minSteps) minSteps = totalSteps;
                }
            }
            Console.WriteLine($"min steps: {minSteps}");
            Console.WriteLine($"shortest distance: {min}");
            Console.Read();
        }

        private static Dictionary<(int, int), (int, int)> BuildPath(string[] moves)
        {
            var path = new Dictionary<(int, int), (int, int)>();
            int steps1 = 0;
            var o = (0, 0);
            foreach (var move in moves)
            {
                var dir = move[0];
                var steps = int.Parse(move.Substring(1));
                for (int i = 0; i < steps; i++)
                {
                    steps1++;
                    var newPoint = Move(o, dir);
                    var distance = Math.Abs((0 - newPoint.Item1)) + Math.Abs((0 - newPoint.Item2));
                    if (!path.ContainsKey(newPoint))
                        path.Add(newPoint, (distance, steps1));
                    o = newPoint;
                }

            }
            return path;
        }

        private static (int, int) Move((int, int) c2, char dir)
        {
            var newPoint = (c2.Item1, c2.Item2);
            if (dir == 'L') newPoint.Item1 -= 1;
            if (dir == 'R') newPoint.Item1 += 1;
            if (dir == 'U') newPoint.Item2 += 1;
            if (dir == 'D') newPoint.Item2 -= 1;
            return newPoint;
        }
    }
}
