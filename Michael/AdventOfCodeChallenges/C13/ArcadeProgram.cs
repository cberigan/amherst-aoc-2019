using AdventOfCodeChallenges.C10;
using AdventOfCodeChallenges.C13.Inputs;
using AdventOfCodeChallenges.C13.Views;
using AdventOfCodeChallenges.C9;
using System.Numerics;
using System.Threading;

namespace AdventOfCodeChallenges.C13
{
    public class ArcadeProgram
    {
        public enum InstructionState
        {
            XIndex = 0,
            YIndex = 1,
            TileType = 3
        }
        
        private InstructionState _instructionState;
        private int _xCoordinate, _yCoordinate;
        private readonly IGameView _gameView;
        private readonly GameState _gameState;

        public ArcadeProgram(IntCodeStateMachine cpu, IGameView gameView, IGameInput gameInput = null)
        {
            cpu.OnOutput += Advance;
            _gameView = gameView;
            gameInput.OnInput += (_, e) => cpu.SetInput(e);
            _gameState = new GameState(gameView, gameInput);
        }


        private void Advance(object sender, BigInteger e)
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
                    _gameView.Draw(new Coordinate(_xCoordinate, _yCoordinate), (int)e);
                    _instructionState = InstructionState.XIndex;

                    _gameState.Advance(e);
                    break;
            };
        }

        sealed class GameState
        {
            private enum CurrentState { Starting, PlacedPaddle, Countdown, Playing }
            private CurrentState currentGameState;

            private int _wallsCompletedAfterPaddle;
            private readonly IGameView _gameView;
            private readonly IGameInput _gameInput;

            public GameState(IGameView gameView, IGameInput gameInput)
            {
                currentGameState = CurrentState.Starting;
                _wallsCompletedAfterPaddle = 0;
                _gameView = gameView;
                _gameInput = gameInput;
            }

            public void Advance(BigInteger e)
            {
                var tiletype = (TileType)(int)e;
                switch (currentGameState)
                {
                    case CurrentState.Starting:
                        if (e == (int)TileType.HorizontalPaddle)
                        {
                            currentGameState = CurrentState.PlacedPaddle;
                        }
                        break;
                    case CurrentState.PlacedPaddle:
                        if (tiletype == TileType.Wall)
                        {
                            _wallsCompletedAfterPaddle++;
                            if (_wallsCompletedAfterPaddle == 3)
                            {
                                currentGameState = CurrentState.Countdown;
                            }
                        }
                        break;
                    case CurrentState.Countdown:
                        _gameView.Draw((-1, 0), 3);
                        Thread.Sleep(300);
                        _gameView.Draw((-1, 0), 2);
                        Thread.Sleep(300);
                        _gameView.Draw((-1, 0), 1);
                        Thread.Sleep(300);
                        _gameView.Draw((-1, 0), 0);
                        Thread.Sleep(300);

                        currentGameState = CurrentState.Playing;
                        break;
                    case CurrentState.Playing:
                        _gameInput.PollInput();
                        break;
                }
            }
        }
    }
}
