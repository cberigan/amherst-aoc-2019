using System;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace AdventOfCodeChallenges.Core
{
    public static class Formulas
    {
        public static double Manhattan(Point a, Point b)
        {
            var xd = a.X - b.X;
            var yd = a.Y - b.Y;
            return Math.Abs(xd) + Math.Abs(yd);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Gcf(long a, long b)
        {
            while (b != 0)
            {
                long temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Lcm(long a, long b)
        {
            return (a / Gcf(a, b)) * b;
        }
    }
}
