using AdventOfCodeChallenges.C10;
using AdventOfCodeChallenges.Core.Cpu;
using AdventOfCodeChallenges.Core.Input;
using System;
using System.Linq;

namespace AdventOfCodeChallenges.C15
{
    public static partial class Challenge
    {
        public class Pt1
        {
            public int Run(string input = Input)
            {
                var memory = input.Split(',').Select(int.Parse);
                var cpu = new IntCodeStateMachine(memory);

                //ICommandInput commandInput = new ConsoleInput();
                ICommandInput commandInput = new AiInput(cpu);
                var repairDroid = new RepairDroid(cpu, commandInput);

                var consoleView = new ConsoleView((40,40));
                var rcp = new RemoteControlProgram(repairDroid, commandInput, consoleView);

                var finder = new RemoteDroidPathFinder(repairDroid, rcp);

                int shortestPath = -1;
                finder.ShortestPathFound += (_, steps) =>
                {
                    shortestPath = steps;
                    rcp.Stop();
                };

                rcp.FoundOxygenSystem += (_, coord) =>
                {
                    Console.SetCursorPosition(0, 2);
                    Console.WriteLine("Found oxygen tank at {0}. Navigating back to origin.", coord);
                };

                Console.WriteLine("1=N, 2=S, 3=W, 4=E");

                rcp.Run();

                return shortestPath;
            }
        }

        public class Pt2
        {
            // how many minutes does it take for the all cells to fill with oxygen if
            // the oxygen spreads to all neighbors in 1 minute
            public int Run(string input = Input)
            {
                var memory = input.Split(',').Select(int.Parse);
                var cpu = new IntCodeStateMachine(memory);

                ICommandInput commandInput = new AiInput(cpu);
                var repairDroid = new RepairDroid(cpu, commandInput);

                var view = new NullView();

                var rcp = new RemoteControlProgram(repairDroid, commandInput, view);
                var finder = new RemoteDroidPathFinder(repairDroid, rcp);


                int minutes = 0;
                Coordinate o2Container = Coordinate.Origin;
                rcp.FoundOxygenSystem += (_, coord) =>
                {
                    Console.SetCursorPosition(0, 2);
                    Console.WriteLine("Found oxygen tank at {0}. Navigating back to origin.", coord);

                    o2Container = coord;
                };

                // this should really be renamed to something like "finished plotting map"
                finder.ShortestPathFound += (_, __) =>
                {
                    rcp.Stop();
                    Console.WriteLine("Found shortest path");

                    minutes = finder.MinutesToFill(o2Container);
                };

                rcp.Run();

                return minutes;
            }
        }
    }
}
