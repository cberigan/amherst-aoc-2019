using AdventOfCodeChallenges.C10;
using AdventOfCodeChallenges.Core;
using AdventOfCodeChallenges.Core.Input;
using System;

namespace AdventOfCodeChallenges.C15
{
    public class RemoteControlProgram
    {
        private bool _run = false;
        private Coordinate _relativePosition;
        private Coordinate _lastPosition;
        private MovementCommand _movementCommand;
        private readonly RepairDroid _repairDroid;
        private readonly ICommandInput _commandInput;

        public Coordinate CurrentPosition => _relativePosition;

        public event EventHandler<Coordinate> FoundOxygenSystem;

        public RemoteControlProgram(RepairDroid repairDroid, ICommandInput commandInput, IView consoleView)
        {
            _relativePosition = Coordinate.Origin;
            _lastPosition = _relativePosition;

            _movementCommand = (MovementCommand)(-1);
            commandInput.InputReceived += (_, v) => _movementCommand = (MovementCommand)v;

            repairDroid.OnReply += (_, msg) =>
            {
                switch (msg)
                {
                    case RepairDroidStatusCode.HitWall:
                        consoleView.Write(GetNewLocation(_relativePosition), '#');
                        
                        break;

                    case RepairDroidStatusCode.Moved:
                        consoleView.Write(_relativePosition, '.');
                        Move();
                        consoleView.Write(_relativePosition, 'D');
                        break;

                    case RepairDroidStatusCode.MovedAndFoundOxygenSystem:
                        Move();
                        FoundOxygenSystem?.Invoke(this, _relativePosition);
                        _run = false;
                        break;
                };
            };
            _repairDroid = repairDroid;
            _commandInput = commandInput;
        }

        public void Stop()
        {
            _run = false;
            _repairDroid.Stop();
        }

        public void Run()
        {
            _run = true;
            while (_run)
            {
                _commandInput.AwaitInput();
                _repairDroid.ExecuteInstruction();
            }
        }

        private void Move()
        {
            Console.SetCursorPosition(0, 1);
            Console.Write("from ({0},{1} ", _relativePosition.X, _relativePosition.Y);

            _lastPosition = _relativePosition;
            _relativePosition = GetNewLocation(_relativePosition);

            Console.Write("to ({0},{1}) ", _relativePosition.X, _relativePosition.Y);
        }

        private Coordinate GetNewLocation(Coordinate p)
        {
            return _movementCommand switch
            {
                MovementCommand.North => p.MoveUp(),
                MovementCommand.South => p.MoveDown(),
                MovementCommand.East => p.MoveRight(),
                MovementCommand.West => p.MoveLeft()
            };
        }
    }
}
