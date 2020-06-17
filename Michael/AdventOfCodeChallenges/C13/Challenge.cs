using AdventOfCodeChallenges.C13.Inputs;
using AdventOfCodeChallenges.C13.Views;
using AdventOfCodeChallenges.C9;
using System;
using System.Linq;
using System.Numerics;

namespace AdventOfCodeChallenges.C13
{
    public static partial class Challenge
    {
        public class Pt1
        {
            public int Run(string input = Input)
            {
                var memory = Input.Split(',').Select(s => BigInteger.Parse(s));
                // probably not the best origin of control, but I don't
                // have to make any changes, sooooo, here's my excuse.
                var cpu = new IntCodeStateMachine(memory);
                var view = new InMemoryGameView();
                var game = new ArcadeProgram(cpu, view);
                cpu.RunAll();

                var blockCount = view.Memory.Values.Count(t => t == TileType.Block);

                return blockCount;
            }
        }

        public class Pt2
        {
            public int Run(string input = Input)
            {
                var memory = Input.Split(',').Select(s => BigInteger.Parse(s)).ToArray();
                memory[0] = 2; // to play for free!
                var cpu = new IntCodeStateMachine(memory);

                var scoreView = new ScoreView();
                var gameView = new SegmentDisplay(scoreView, new ConsoleGameView());
                var gameInput = new AiGameInput(cpu);
                
                var game = new ArcadeProgram(cpu, gameView, gameInput);

                cpu.RunAll();

                Console.Clear();

                return scoreView.FinalScore;
            }
        }
    }
}
