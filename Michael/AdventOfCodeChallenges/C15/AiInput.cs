using AdventOfCodeChallenges.C10;
using AdventOfCodeChallenges.Core;
using AdventOfCodeChallenges.Core.Cpu;
using AdventOfCodeChallenges.Core.Input;
using System;

namespace AdventOfCodeChallenges.C15
{
    internal class AiInput : ICommandInput
    {
        private enum Floor :byte { Unknown = 0, Passed, Wall }

        private IntCodeStateMachine cpu;
        private Floor[,] _map;
        private Coordinate _pos, _last;
        private readonly Coordinate _origin;
        private MovementCommand _lastCommand;

        public AiInput(IntCodeStateMachine cpu)
        {
            this.cpu = cpu;
            cpu.OnOutput += Intercept;
            _map = new Floor[100, 100];
            _pos = _last = _origin = new Coordinate(50, 50);
        }

        private void Intercept(object sender, int e)
        {


            var status = (RepairDroidStatusCode)e;
            switch (status)
            {
                case RepairDroidStatusCode.HitWall:
                    _map[_pos.Y, _pos.X] = Floor.Wall;
                    _pos = _last;
                    _state = LastState(_state);
                    break;
                case RepairDroidStatusCode.Moved:
                    _last = _pos;
                    _map[_pos.Y, _pos.X] = Floor.Passed;
                    break;
                case RepairDroidStatusCode.MovedAndFoundOxygenSystem:
                    // handled in the challenge.cs code
                    break;
            }
        }

        public event EventHandler<int> InputReceived;

        public void AwaitInput()
        {
            // stick to the right-most wall
            var (next, s) = NextPosition();
            _last = _pos;
            _pos = next;
            _state = s;
            InputReceived?.Invoke(this, (int)_state);
        }

        MovementCommand _state = MovementCommand.North;
        private (Coordinate, MovementCommand) NextPosition()
        {
            // try sticking to the right-hand wall
            var s = _state;
            s = NextState(s);
            var nextp = GetNewLocation(_pos, s);
            while (_map[nextp.Y, nextp.X] == Floor.Wall)
            {
                s = LastState(s);
                nextp = GetNewLocation(_pos, s);
            }

            return (nextp, s);
        }

        private MovementCommand NextState(MovementCommand state) => state switch
        {
            MovementCommand.East => MovementCommand.South,
            MovementCommand.North => MovementCommand.East,
            MovementCommand.West => MovementCommand.North,
            MovementCommand.South => MovementCommand.West
        };

        private MovementCommand LastState(MovementCommand state) => state switch
        {
            MovementCommand.East => MovementCommand.North,
            MovementCommand.North => MovementCommand.West,
            MovementCommand.West => MovementCommand.South,
            MovementCommand.South => MovementCommand.East
        };

        private Coordinate GetNewLocation(Coordinate p, MovementCommand c) => c switch
        {
            MovementCommand.North => p.MoveUp(),
            MovementCommand.South => p.MoveDown(),
            MovementCommand.East => p.MoveRight(),
            MovementCommand.West => p.MoveLeft()
        };
    }
}