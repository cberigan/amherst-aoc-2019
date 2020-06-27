using AdventOfCodeChallenges.C10;
using System;

namespace AdventOfCodeChallenges.C13.Views
{
    public class ScoreView : IGameView
    {
        private string _emptyLine = new string(' ', Console.WindowWidth);

        public int FinalScore { get; private set; }

        public void Draw(Coordinate coordinate, int drawCode)
        {
            Console.SetCursorPosition(0, 0);
            if (_emptyLine.Length != Console.WindowWidth)
                _emptyLine = new string(' ', Console.WindowWidth);
            Console.Write(_emptyLine);
            Console.SetCursorPosition(0, 0);
            Console.Write(drawCode);
            FinalScore = drawCode;
        }
    }
}
