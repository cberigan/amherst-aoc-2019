using AdventOfCodeChallenges.C6;
using System;
using System.Collections.Generic;

namespace AdventOfCodeChallenges.Core
{
    public interface IParser<T> { List<T> Parse(string input); }
    public sealed class OrbitParser : IParser<(CelestialBody inner, CelestialBody outer)>
    {
        public List<(CelestialBody inner, CelestialBody outer)> Parse(string input)
        {
            var l = new List<(CelestialBody, CelestialBody)>();
            var s = input.AsSpan();
            int lf = 0;
            while ((lf = s.IndexOf('\n')) > 0)
            {
                var line = s.Slice(0, lf - 1);
                var paren = line.IndexOf(')');
                var inner = new CelestialBody(line.Slice(0, paren).ToString());
                var outer = new CelestialBody(line.Slice(paren + 1).ToString());

                s = s.Slice(lf + 1);
                l.Add((inner, outer));
            }

            var innerLast = new CelestialBody(s.Slice(0, s.IndexOf(')')).ToString());
            var outerLast = new CelestialBody(s.Slice(s.IndexOf(')') + 1).ToString());

            l.Add((innerLast, outerLast));

            return l;
        }
    }
}
