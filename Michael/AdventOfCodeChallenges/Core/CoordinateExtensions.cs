using AdventOfCodeChallenges.C10;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCodeChallenges.Core
{
    public static class CoordinateExtensions
    {
        public static Coordinate MoveUp(this Coordinate c) => c.Offset(0, -1);
        public static Coordinate MoveRight(this Coordinate c) => c.Offset(1, 0);
        public static Coordinate MoveDown(this Coordinate c) => c.Offset(0, 1);
        public static Coordinate MoveLeft(this Coordinate c) => c.Offset(-1, 0);
    }
}
