using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCodeChallenges.C10
{
    public readonly struct Angle
    {
        public static Angle None => new Angle();
        public Angle(int xOffset, int yOffset)
        {
            XOffset = xOffset;
            YOffset = yOffset;
        }

        public readonly int XOffset;
        public readonly int YOffset;

        public override bool Equals(object obj)
        {
            return obj is Angle angle &&
                   XOffset == angle.XOffset &&
                   YOffset == angle.YOffset;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(XOffset, YOffset);
        }

        public static bool operator ==(Angle left, Angle right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Angle left, Angle right)
        {
            return !(left == right);
        }

        public override string ToString() => $"({XOffset}, {YOffset})";
    }
}
