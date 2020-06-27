using AdventOfCodeChallenges.C10;
using AdventOfCodeChallenges.C9;
using AdventOfCodeChallenges.Core;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace AdventOfCodeChallenges.C11
{
    public static partial class Challenge
    {
        private enum CodeState { Paint, Move }
        public enum SquareColor { Black, White }
        private enum Direction { Left, Right }
        private enum Orientation { Left, Up, Right, Down }

        public class Pt1
        {
            public int Run(string input = Input)
            {
                var paintedSquares = new Dictionary<Coordinate, SquareColor>();
                RunImpl(input, 0, paintedSquares);

                var totalPaintedSquares = paintedSquares.Count;

                return totalPaintedSquares;
            }
        }

        public class Pt2
        {
            private readonly Visualizer _visualizer;

            public Pt2() => _visualizer = new Visualizer();

            public object Run(string input = Input)
            {
                var paintedSquares = new Dictionary<Coordinate, SquareColor>();
                RunImpl(input, 1, paintedSquares);

                _visualizer.Visualize(paintedSquares);

                return null;
            }
        }

        private static int RunImpl(string input, int startValue, Dictionary<Coordinate, SquareColor> paintedSquares)
        {
            var cpu = new IntCodeStateMachine(input.Split(',').Select(BigInteger.Parse));
            var paintingRobotState = new MessageState(paintedSquares, cpu);
            cpu.OnOutput += paintingRobotState.Run;
            cpu.SetInput(startValue);

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
                switch (_currentState)
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
                            Orientation.Up => _currentPosition.MoveUp(),
                            Orientation.Left => _currentPosition.MoveLeft(),
                            Orientation.Down => _currentPosition.MoveDown(),
                            Orientation.Right => _currentPosition.MoveRight()
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
