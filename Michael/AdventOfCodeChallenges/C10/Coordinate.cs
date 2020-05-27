using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;

namespace AdventOfCodeChallenges.C10
{
    public readonly struct Coordinate
    {
        public static Coordinate Origin => new Coordinate();

        public Coordinate(int x, int y)
        {
            X = x;
            Y = y;
        }

        public readonly int X;
        public readonly int Y;

        public Angle GetAngle(Coordinate b) =>
            new Angle(
                X > b.X ? X - b.X : b.X - X,
                Y > b.Y ? Y - b.Y : b.Y - Y);

        public override bool Equals(object obj) => obj is Coordinate coordinate &&
                   X == coordinate.X &&
                   Y == coordinate.Y;

        public Coordinate Offset(int xOffset, int yOffset) =>
            new Coordinate(X + xOffset, Y + yOffset);

        public override int GetHashCode() => HashCode.Combine(X, Y);

        public static bool operator ==(Coordinate left, Coordinate right) =>
            left.Equals(right);

        public static bool operator !=(Coordinate left, Coordinate right) =>
            !(left == right);

        public override string ToString() => $"({X},{Y})";

        public static implicit operator Coordinate  ((int x, int y) t) => new Coordinate(t.x, t.y);

        public static Coordinate operator +(Coordinate a, Coordinate b) => new Coordinate(a.X + b.X, a.Y + b.Y);
        public static Coordinate operator -(Coordinate a, Coordinate b) => new Coordinate(a.X - b.X, a.Y - b.Y);
    }
}
