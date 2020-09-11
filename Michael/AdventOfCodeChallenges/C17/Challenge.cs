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
        public enum Direction { L = 'L', R = 'R' }
        public enum Orientation { U = '^', L = '<', R = '>', D = 'v' }

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

                var isValid = IsValid(grid, validSquares);

                var up = c.MoveUp();
                if (isValid(up))
                    neighbors++;

                var down = c.MoveDown();
                if (isValid(down))
                    neighbors++;

                var left = c.MoveLeft();
                if (isValid(left))
                    neighbors++;

                var right = c.MoveRight();
                if (isValid(right))
                    neighbors++;

                return neighbors;

                //[MethodImpl(MethodImplOptions.AggressiveInlining)]
                static bool IsInRange(Coordinate x, List<List<char>> grid) =>
                    x.X >= 0 && x.Y >= 0 && x.Y < grid.Count && x.X < grid[x.Y].Count;
                //[MethodImpl(MethodImplOptions.AggressiveInlining)]
                static bool IsNeighbor(Coordinate x, List<List<char>> grid, char[] validSquares) =>
                    validSquares.Contains(grid[x.Y][x.X]);
                //[MethodImpl(MethodImplOptions.AggressiveInlining)]
                static Func<Coordinate, bool> IsValid(List<List<char>> grid, char[] validSquares) =>
                    x => IsInRange(x, grid) && IsNeighbor(x, grid, validSquares);
            }
        }

        public class Pt2
        {
            public int Run()
            {
                var input = Input.ToArray();
                var (map, start, orientation) = GetMap(Input);

                // first lets print out the path by choosing a direction and 
                // running until a wall is hit, ending once there's no where to turn
                foreach (var path in FindPaths(map, (start, orientation)))
                {
                    Console.WriteLine("{0},{1}", path.dir, path.steps);
                }

                // todo: found all of the paths.
                // There seems to be patetrns of 3-4.

                // plan for next time:
                // start with windows of 4
                // remove matches from list and make a function (max 3)
                // then windows of 3 and make a function
                // hopefully there's one left

                return 0;
                //input[0] = 2;

                //var cpu = new IntCodeStateMachine(input);
            }

            IEnumerable<(Direction dir, int steps, Coordinate stop)> FindPaths(List<List<char>> map, (Coordinate c, Orientation o) bot)
            {
                var (turn, newOrientation, found) = NextTurn(map, bot);
                if (!found)
                {
                    yield break;
                }
                else
                {
                    var dist = NextDistance(map, (bot.c, newOrientation));
                    var stop = Move(bot.c, newOrientation, dist);

                    yield return (turn, dist, stop);

                    foreach (var path in FindPaths(map, (stop, newOrientation)))
                    {
                        yield return path;
                    }
                }
            }

            (Direction dir, Orientation o, bool found) NextTurn(List<List<char>> map, (Coordinate c, Orientation o) bot)
            {
                var isValid = IsValid(map);

                if (isValid(LeftOf(bot)))
                {
                    return (Direction.L, bot.o switch
                    {
                        Orientation.U => Orientation.L,
                        Orientation.L => Orientation.D,
                        Orientation.R => Orientation.U,
                        Orientation.D => Orientation.R,
                    }, true);
                }
                if (isValid(RightOf(bot)))
                {
                    return (Direction.R, bot.o switch
                    {
                        Orientation.U => Orientation.R,
                        Orientation.L => Orientation.U,
                        Orientation.R => Orientation.D,
                        Orientation.D => Orientation.L,
                    }, true);
                }

                // this helped me find the end
                //Console.SetCursorPosition(bot.c.X, bot.c.Y);
                //Console.Write('X');
                //throw new Exception($"Cound not find a direction for ({bot.c.X}, {bot.c.Y})");

                return (default, default, false);

                static Coordinate LeftOf((Coordinate c, Orientation o) bot)
                {
                    return bot.o switch
                    {
                        Orientation.U => bot.c.MoveLeft(),
                        Orientation.L => bot.c.MoveDown(),
                        Orientation.D => bot.c.MoveRight(),
                        Orientation.R => bot.c.MoveUp()
                    };
                }

                static Coordinate RightOf((Coordinate c, Orientation o) bot)
                {
                    return bot.o switch
                    {
                        Orientation.U => bot.c.MoveRight(),
                        Orientation.L => bot.c.MoveUp(),
                        Orientation.D => bot.c.MoveLeft(),
                        Orientation.R => bot.c.MoveDown()
                    };
                }

                static bool IsInRange(Coordinate x, List<List<char>> grid) =>
                    x.X >= 0 && x.Y >= 0 && x.Y < grid.Count && x.X < grid[x.Y].Count;
                static Func<Coordinate, bool> IsValid(List<List<char>> grid) =>
                    x => IsInRange(x, grid) && grid[x.Y][x.X] == '#';
            }

            int NextDistance(List<List<char>> map, (Coordinate c, Orientation o) bot)
            {
                Func<Coordinate, Coordinate> move = null;

                switch (bot.o)
                {
                    case Orientation.U:
                        move = c => c.MoveUp();
                        break;
                    case Orientation.L:
                        move = c => c.MoveLeft();
                        break;
                    case Orientation.R:
                        move = c => c.MoveRight();
                        break;
                    case Orientation.D:
                        move = c => c.MoveDown();
                        break;
                }

                int count = -1;
                Coordinate curr = bot.c;
                do
                {
                    curr = move(curr);
                    count++;
                } while (curr.Y > -1 && curr.X > -1 && map[curr.Y][curr.X] == '#');

                return count;
            }

            Coordinate Move(Coordinate c, Orientation dir, int dist) => dir switch
            {
                Orientation.U => c.MoveUp(dist),
                Orientation.L => c.MoveLeft(dist),
                Orientation.R => c.MoveRight(dist),
                Orientation.D => c.MoveDown(dist),
            };
        }

        private static (List<List<char>> map, Coordinate start, Orientation o) GetMap(int[] program)
        {
            var botSymbols = new[] { '^', '<', '>', 'v' };
            var camera = new List<List<char>>() { new List<char>() };
            Coordinate start = default;
            var orientation = Orientation.U;
            var cpu = new IntCodeStateMachine(program);

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
                        if (botSymbols.Contains(c))
                        {
                            start = new Coordinate(camera.Last().Count - 1, camera.Count - 1);
                            orientation = (Orientation)c;
                        }
                        break;
                }
            };

            cpu.RunAll();

            // ensure they are all the same length
            for (int i = camera.Count - 1; i > 0; i--)
            {
                if (camera[i].Count > 0)
                    break;
                camera[i].AddRange(Enumerable.Repeat((char)0, camera[0].Count));
            }

            return (camera, start, orientation);
        }
    }
}
