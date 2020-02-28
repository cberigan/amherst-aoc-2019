using System;

namespace AdventOfCode
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Ruchi's solution to Advent of Code Challenges. To select the Solution of a specific day please enter a number out of the following range: 1-1.\n\nEnter 0 to EXIT.");
            string input = Console.ReadLine();

            while(Convert.ToInt32(input) != 0)
            {
                if (Convert.ToInt32(input) == 1)
                {
                    string[] lines = System.IO.File.ReadAllLines(@"D:\AoC-GitHub\amherst-aoc-2019\Ruchi\AdventOfCode\inputForDay1.txt");
                    double ouputPart1 = 0;
                    foreach (var line in lines)
                    {
                        ouputPart1 += Day1.CalculateFuel(Convert.ToDouble(line));
                    }

                    Console.WriteLine("\nPart 1 result: " + ouputPart1);

                    double ouputPart2 = 0;
                    foreach (string line in lines)
                    {
                        double result = Day1.CalculateFuel(Convert.ToDouble(line));
                        ouputPart2 += result > 0 ? result : 0;

                        while (result > 0)
                        {
                            result = Day1.CalculateFuel(Convert.ToDouble(result));
                            ouputPart2 += result > 0 ? result : 0;
                        }
                    }

                    Console.WriteLine("Part 2 result: " + ouputPart2);
                }
                Console.WriteLine("\n\nTo select the Solution of a specific day please enter a number out of the following range: 1-1.\n\nEnter 0 to EXIT.");
                input = Console.ReadLine();
            }
            
            
        }


    }
}