using AdventOfCodeChallenges.C10;

namespace AdventOfCodeChallenges.Core
{
    public static class CoordinateExtensions
    {
        public static Coordinate MoveUp(this Coordinate c, int distance = 1) => c.Offset(0, -distance);
        public static Coordinate MoveRight(this Coordinate c, int distance = 1) => c.Offset(distance, 0);
        public static Coordinate MoveDown(this Coordinate c, int distance = 1) => c.Offset(0, distance);
        public static Coordinate MoveLeft(this Coordinate c, int distance = 1) => c.Offset(-distance, 0);
    }
}
