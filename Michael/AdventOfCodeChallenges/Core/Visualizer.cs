using AdventOfCodeChallenges.C10;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static AdventOfCodeChallenges.C11.Challenge;

namespace AdventOfCodeChallenges.Core
{
    public class Visualizer
    {
        public void Visualize(Dictionary<Coordinate, SquareColor> colors)
        {
            var leftMost = colors.Keys.Min(k => k.X);
            var rightMost = colors.Keys.Max(k => k.X);
            var topMost = colors.Keys.Min(k => k.Y);
            var bottomMost = colors.Keys.Max(k => k.Y);

            foreach (var kvp in colors)
            {
                Console.SetCursorPosition(kvp.Key.X + -leftMost, kvp.Key.Y + -topMost);
                if (kvp.Value == SquareColor.White)
                    Console.Write('#');
            }

            Console.SetCursorPosition(0, bottomMost + -topMost + 1);
        }
    }
}
