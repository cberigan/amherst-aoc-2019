using AdventOfCodeChallenges.C10;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeChallenges.Core.Traversal
{
    public class StickRightTraversalStrategy : ITraversalStrategy<char>
    {
        private readonly ITerrainMetadata<char> _terrainMetadata;
        private readonly List<(Predicate<char> execute, Action<char, Coordinate, int> callback)> _events = new List<(Predicate<char> execute, Action<char, Coordinate, int> callback)>();

        public StickRightTraversalStrategy(ITerrainMetadata<char> terrainMetadata)
        {
            this._terrainMetadata = terrainMetadata;
        }

        public void OnFound(Predicate<char> predicate, Action<char, Coordinate, int> callback)
        {
            _events.Add((predicate, callback));
        }

        public void Traverse(ITraversable<char> traversable, Coordinate startingPoint, Func<bool> stop, Predicate<char> resetStepCounter)
        {
            var last = startingPoint;
            var pos = startingPoint;
            MovementCommand state = MovementCommand.North;
            int steps = 0;
            while(!stop())
            {
                var (next, s, tile) = NextPosition(state, pos, traversable);
                last = pos;
                pos = next;
                state = s;

                steps++;

                Notify(tile, pos, steps);

                if (resetStepCounter(tile))
                    steps = 0;
            }
        }

        private void Notify(char tile, Coordinate pos, int steps)
        {
            for (int i = 0; i < _events.Count; i++)
            {
                var (execute, callback) = _events[i];
                if (execute(tile))
                    callback(tile, pos, steps);
            }
        }

        private (Coordinate, MovementCommand, char) NextPosition(MovementCommand state, Coordinate pos, ITraversable<char> traversable)
        {
            // try sticking to the right-hand wall
            var s = state;
            s = NextState(s);
            var nextp = GetNewLocation(pos, s);
            char tile;
            while (!_terrainMetadata.IsPassable((tile = traversable[nextp])))
            {
                s = LastState(s);
                nextp = GetNewLocation(pos, s);
            }

            return (nextp, s, tile);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private MovementCommand NextState(MovementCommand state) => state switch
        {
            MovementCommand.East => MovementCommand.South,
            MovementCommand.North => MovementCommand.East,
            MovementCommand.West => MovementCommand.North,
            MovementCommand.South => MovementCommand.West
        };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private MovementCommand LastState(MovementCommand state) => state switch
        {
            MovementCommand.East => MovementCommand.North,
            MovementCommand.North => MovementCommand.West,
            MovementCommand.West => MovementCommand.South,
            MovementCommand.South => MovementCommand.East
        };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Coordinate GetNewLocation(Coordinate p, MovementCommand c) => c switch
        {
            MovementCommand.North => p.MoveUp(),
            MovementCommand.South => p.MoveDown(),
            MovementCommand.East => p.MoveRight(),
            MovementCommand.West => p.MoveLeft()
        };
    }
}
