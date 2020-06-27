using AdventOfCodeChallenges.C10;
using AdventOfCodeChallenges.C9;
using System;
using System.Numerics;
using static AdventOfCodeChallenges.C13.ArcadeProgram;

namespace AdventOfCodeChallenges.C13.Inputs
{
    public class AiGameInput : IGameInput
    {
        private Coordinate _lastBallPosition;
        private Coordinate _paddlePosition;

        public AiGameInput(IntCodeStateMachine cpu)
        {
            cpu.OnOutput += InterceptOutput;
        }

        private int _xCoordinate, _yCoordinate;
        private InstructionState _instructionState;

        private void InterceptOutput(object sender, BigInteger e)
        {
            switch (_instructionState)
            {
                case InstructionState.XIndex:
                    _xCoordinate = (int)e;
                    _instructionState = InstructionState.YIndex;
                    break;
                case InstructionState.YIndex:
                    _yCoordinate = (int)e;
                    _instructionState = InstructionState.TileType;
                    break;
                case InstructionState.TileType:
                    _instructionState = InstructionState.XIndex;

                    Track((_xCoordinate, _yCoordinate), (int)e);
                    break;
            };
            
        }

        public event EventHandler<int> OnInput;

        private void Track(Coordinate c, int code)
        {
            if (c == (-1, 0)) return;

            switch ((TileType)code)
            {
                case TileType.Ball:
                    _lastBallPosition = c;
                    break;
                case TileType.HorizontalPaddle:
                    _paddlePosition = c;
                    break;
            }
        }

        public void PollInput()
        {
            var ballX = _lastBallPosition.X;
            var movePaddle = ballX.CompareTo(_paddlePosition.X);
            OnInput?.Invoke(this, movePaddle);
        }
    }
}
