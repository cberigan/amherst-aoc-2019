using AdventOfCodeChallenges.C10;
using AdventOfCodeChallenges.C13.Views;
using System;

namespace AdventOfCodeChallenges.C15
{
    public class ConsoleView : IView
    {
        private readonly Coordinate _center;

        public ConsoleView(Coordinate center) => _center = center;

        public void Write(Coordinate relativePosition, char c)
        {
            var pos = relativePosition + _center;
            Console.SetCursorPosition(pos.X, pos.Y);
            Console.Write(c);
        }
    }
}