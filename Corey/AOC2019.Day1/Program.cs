using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2019.Day1
{
    class Program
    {
        static void Main(string[] _)
        {
            var lines = File.ReadAllLines("input.txt");
            Console.WriteLine($"Part 1: {part1(lines)}");
            Console.WriteLine($"Part 2: {part2(lines)}");

            Console.Read();
        }

        static int part2(string[] lines)
        {
            var sum = 0;
            foreach (var line in lines)
            {
                var mass = int.Parse(line);

                sum += calcFuel(mass);
            }
            return sum;
        }

        static int part1 (string[] lines)
        {
            var sum = 0;
            foreach (var line in lines)
            {
                var mass = int.Parse(line);

                var val = (int)(mass / 3.0) - 2;
                sum += val;
            }
            return sum;
        }

        static int calcFuel(int mass)
        {

            var fuel = (int)(mass / 3.0) - 2;
            if (fuel <= 0) return 0;

            return fuel + calcFuel(fuel);
        }

    }
}
