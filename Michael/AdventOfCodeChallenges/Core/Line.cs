using System.Drawing;

namespace AdventOfCodeChallenges.Core
{
    public readonly struct Line
    {
        public readonly Point A;
        public readonly Point B;

        public Line(Point a, Point b)
        {
            this.A = a;
            this.B = b;
        }

        public int Length => (int)Formulas.Manhattan(A, B);
    }
}
