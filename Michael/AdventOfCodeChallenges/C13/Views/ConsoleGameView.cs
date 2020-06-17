using AdventOfCodeChallenges.C10;
using System;

namespace AdventOfCodeChallenges.C13.Views
{
    public class ConsoleGameView : IGameView
    {
        public void Draw(Coordinate coordinate, int drawCode)
        {
            var tileType = (TileType)drawCode;
            Console.SetCursorPosition(coordinate.X, coordinate.Y);

            var writeChar = tileType switch
            {
                TileType.Empty => ' ',
                TileType.Wall => (char)219,
                TileType.Block => 'B',
                TileType.HorizontalPaddle => '_',
                TileType.Ball => 'o',
                _ => throw new NotImplementedException("Tile type " + drawCode + " is not supported")
            };
            Console.Write(writeChar);
        }
    }
}
