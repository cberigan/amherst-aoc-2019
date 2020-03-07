using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

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
    }
}
