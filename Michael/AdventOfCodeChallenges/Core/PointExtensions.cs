using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace AdventOfCodeChallenges.Core
{
    public static class PointExtensions
    {
        public static (bool intersects, Point point) Intersects(this Line a, Line b)
        {
            float p0_x, p0_y,  p1_x,  p1_y, p2_x,  p2_y,  p3_x,  p3_y;
            (p0_x, p0_y) = a.A;
            (p1_x, p1_y) = a.B;
            (p2_x, p2_y) = b.A;
            (p3_x, p3_y) = b.B;
            float s1_x, s1_y, s2_x, s2_y;
            s1_x = p1_x - p0_x; s1_y = p1_y - p0_y;
            s2_x = p3_x - p2_x; s2_y = p3_y - p2_y;

            float s, t;
            s = (-s1_y * (p0_x - p2_x) + s1_x * (p0_y - p2_y)) / (-s2_x * s1_y + s1_x * s2_y);
            t = (s2_x * (p0_y - p2_y) - s2_y * (p0_x - p2_x)) / (-s2_x * s1_y + s1_x * s2_y);

            float i_x, i_y;
            if (s >= 0 && s <= 1 && t >= 0 && t <= 1)
            {
                // Collision detected
                i_x = p0_x + (t * s1_x);
                i_y = p0_y + (t * s1_y);
                return (true, new Point((int)i_x, (int)i_y));
            }

            return (false, new Point(0,0)); // No collision
        }

        public static int Steps(this Line line) => (int)Formulas.Manhattan(line.A, line.B);

        public static (Line line, int distance) ClipAt(this Line l, Point p)
        {
            if ((p.X == l.A.X || p.X == l.B.X))
            {
                var yOffset = p.Y - l.B.Y;
                var newEnd = l.B;
                newEnd.Offset(0, yOffset);

                return (new Line(l.A, newEnd), Math.Abs(yOffset));

            }
            else if(p.Y == l.A.Y || p.Y == l.B.Y)
            {
                var xOffset = p.X - l.B.X;
                var newEnd = l.B;
                newEnd.Offset(xOffset, 0);

                return (new Line(l.A, newEnd), Math.Abs(xOffset));
            }
            throw new ArgumentOutOfRangeException("Point does not intersect line");
        }

        public static void Deconstruct(this Point p, out float x, out float y)
        {
            x = p.X;
            y = p.Y;
        }
    }
}
