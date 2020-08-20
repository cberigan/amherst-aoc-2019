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
            // find a path to a point in an unknown environment. Then find the shortest path
            // from the origin to that point.
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
    }
}
