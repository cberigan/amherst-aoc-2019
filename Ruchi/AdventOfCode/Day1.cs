using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode
{
    public static class Day1
    {
        public static double CalculateFuel(double input)
        {
            return Math.Floor(input / 3) - 2;
        }
    }
}
