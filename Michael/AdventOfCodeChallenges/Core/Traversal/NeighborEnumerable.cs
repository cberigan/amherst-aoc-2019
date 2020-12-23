using AdventOfCodeChallenges.C10;
using AdventOfCodeChallenges.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeChallenges.Core.Traversal
{
    
    public struct NeighborEnumerable<T>
    {
        private readonly ITraversable<T> traversable;
        private Coordinate c;
        private T terrain;
        private MovementCommand _direction;
        private int i;

        public NeighborEnumerable(ITraversable<T> traversable, Coordinate c)
        {
            this.traversable = traversable;
            this.c = c;
            _direction = MovementCommand.North;
            i = 0;
            terrain = default;
        }

        public bool MoveNext() {
            if (i < 4) // 4 potential neighbors
            {
                i++;
                _direction = NextState(_direction);
                this.c = GetNewLocation(this.c, _direction);
                terrain = this.traversable[this.c];
                return true;
            }
            return false;
        }

        public (T, Coordinate) Current => (terrain, c);

        public NeighborEnumerable<T> GetEnumerator() => this;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Coordinate GetNewLocation(Coordinate p, MovementCommand c) => c switch
        {
            MovementCommand.North => p.MoveUp(),
            MovementCommand.South => p.MoveDown(),
            MovementCommand.East => p.MoveRight(),
            MovementCommand.West => p.MoveLeft()
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private MovementCommand NextState(MovementCommand state) => state switch
        {
            MovementCommand.East => MovementCommand.South,
            MovementCommand.North => MovementCommand.East,
            MovementCommand.West => MovementCommand.North,
            MovementCommand.South => MovementCommand.West
        };
    }
}
