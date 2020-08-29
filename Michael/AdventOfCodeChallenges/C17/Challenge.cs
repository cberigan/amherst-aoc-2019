using AdventOfCodeChallenges.C10;
using AdventOfCodeChallenges.C15;
using AdventOfCodeChallenges.Core;
using AdventOfCodeChallenges.Core.Cpu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace AdventOfCodeChallenges.C17
{
    public static partial class Challenge
    {
        public class Pt1
        {
            public int Run()
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                var cpu = new IntCodeStateMachine(Input.Concat(new[] { 0 }));
                var camera = new List<List<char>>() { new List<char>() };
                cpu.OnOutput += (_, e) =>
                {
                    var c = (char)e;
                    switch (c)
                    {
                        case '\n':
                            Console.WriteLine();
                            camera.Add(new List<char>());
                            break;
                        default:
                            Console.Write(c);
                            camera.Last().Add(c);
                            break;
                    }
                };

                cpu.RunAll();

                var validSquares = new char[] { '#', '^', '<', '>', 'v'};
                var intersections=
                    from y in Enumerable.Range(0, camera.Count)
                    from x in Enumerable.Range(0, camera[y].Count)
                    where validSquares.Contains(camera[y][x]) 
                          && NeighborCount((x,y), validSquares, camera) >= 3
                    select (x,y)
                    ;

                Console.ForegroundColor = ConsoleColor.Green;
                foreach (Coordinate c in intersections)
                {
                    Console.SetCursorPosition(c.X, c.Y);
                    
                    Console.Write(camera[c.Y][c.X]);
                }

                Console.SetCursorPosition(0, camera.Count + 2);
                return intersections.Sum(x => x.y * x.x);
            }

            private static int NeighborCount(Coordinate c, char[] validSquares, List<List<char>> grid)
            {
                int neighbors = 0;

                var up = c.MoveUp();
                if (IsValid(up, grid, validSquares))
                    neighbors++;

                var down = c.MoveDown();
                if (IsValid(down, grid, validSquares))
                    neighbors++;

                var left = c.MoveLeft();
                if (IsValid(left, grid, validSquares))
                    neighbors++;

                var right = c.MoveRight();
                if (IsValid(right, grid, validSquares))
                    neighbors++;

                return neighbors;

                //[MethodImpl(MethodImplOptions.AggressiveInlining)]
                static bool IsInRange(Coordinate x, List<List<char>> grid) =>
                    x.X >= 0 && x.Y >= 0 && x.Y < grid.Count && x.X < grid[x.Y].Count;
                //[MethodImpl(MethodImplOptions.AggressiveInlining)]
                static bool IsNeighbor(Coordinate x, List<List<char>> grid, char[] validSquares) =>
                    validSquares.Contains(grid[x.Y][x.X]);
                //[MethodImpl(MethodImplOptions.AggressiveInlining)]
                static bool IsValid(Coordinate x, List<List<char>> grid, char[] validSquares) =>
                    IsInRange(x, grid) && IsNeighbor(x, grid, validSquares);
            }
        }
    }
}
