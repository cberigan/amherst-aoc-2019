using AdventOfCodeChallenges.C10;
using AdventOfCodeChallenges.C9;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;

namespace AdventOfCodeChallenges.C11
{
    public static partial class Challenge
    {
        private enum CodeState { Paint, Move }
        private enum SquareColor { Black, White }
        private enum Direction { Left, Right }
        private enum Orientation { Left, Up, Right, Down }

        public class Pt1
        {
            public int Run(string input = Input)
            {
                var paintedSquares = new Dictionary<Coordinate, SquareColor>();
                var cpu = new IntCodeStateMachine(input.Split(',').Select(BigInteger.Parse));
                var paintingRobotState = new MessageState(paintedSquares, cpu);
                cpu.OnOutput += paintingRobotState.Run;
                cpu.SetInput(0);

                cpu.RunAll();

                var totalPaintedSquares = paintedSquares.Count;

                return totalPaintedSquares;
            }

            private class MessageState
            {
                private CodeState _currentState = CodeState.Paint;
                private Coordinate _currentPosition = Coordinate.Origin;
                private Orientation _orientation = Orientation.Up;
                private Dictionary<Coordinate, SquareColor> paintedSquares;
                private readonly IntCodeStateMachine cpu;

                public MessageState(Dictionary<Coordinate, SquareColor> paintedSquares, IntCodeStateMachine cpu)
                {
                    this.paintedSquares = paintedSquares;
                    this.cpu = cpu;
                }

                public void Run(object sender, BigInteger value)
                {
                    switch(_currentState)
                    {
                        case CodeState.Paint:
                            var colorToPaint = value == 0 ? SquareColor.Black : SquareColor.White;
                            paintedSquares[_currentPosition] = colorToPaint;

                            cpu.SetInput(value == 0 ? 0 : 1);

                            _currentState = CodeState.Move;
                            break;
                        case CodeState.Move:
                            var turnDirection = value == 0 ? Direction.Left : Direction.Right;
                            _orientation = _orientation switch
                            {
                                Orientation.Up => turnDirection == Direction.Left ? Orientation.Left : Orientation.Right,
                                Orientation.Left => turnDirection == Direction.Left ? Orientation.Down : Orientation.Up,
                                Orientation.Down => turnDirection == Direction.Left ? Orientation.Right : Orientation.Left,
                                Orientation.Right => turnDirection == Direction.Left ? Orientation.Up : Orientation.Down
                            };

                            _currentPosition = _orientation switch
                            {
                                Orientation.Up => _currentPosition.Offset(0, -1),
                                Orientation.Left => _currentPosition.Offset(-1, 0),
                                Orientation.Down => _currentPosition.Offset(0, 1),
                                Orientation.Right => _currentPosition.Offset(1, 0)
                            };

                            if (!paintedSquares.TryGetValue(_currentPosition, out var currentPanelColor))
                                currentPanelColor = SquareColor.Black;

                            var nextInput = currentPanelColor == SquareColor.Black ? 0 : 1;

                            cpu.SetInput(nextInput);

                            _currentState = CodeState.Paint;

                            break;
                    }
                }
            }

        }
    }
}
