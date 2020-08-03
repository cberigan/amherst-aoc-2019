using AdventOfCodeChallenges.C10;
using System;

namespace AdventOfCodeChallenges.C15
{
    public class ConsoleView
    {
        private readonly Coordinate _center;

        public ConsoleView(Coordinate center) => _center = center;

        internal void Write(Coordinate relativePosition, char c)
        {
            var pos = relativePosition + _center;
            Console.SetCursorPosition(pos.X, pos.Y);
            Console.Write(c);
        }
    }
}