using AdventOfCodeChallenges.C12;
using AdventOfCodeChallenges.Core;
using MoreLinq;
using System;
using System.Linq;
using System.Numerics;
using Xunit;
using static System.Globalization.NumberStyles;

namespace AdventOfCodeChallenges.Tests
{
    public class C12Tests
    {
        [Fact]
        public void TwoBodyGravityTest()
        {
            var Ganymede = new PlanetaryBody("Ganymede");
            var gpos = new Vector3(3, 0, 0);
            var Callisto = new PlanetaryBody("Callisto");
            var cpos = new Vector3(5, 0, 0);

            var res = PlanetarySystem.Step(new[] { (gpos, Vector3.Zero), (cpos, Vector3.Zero) });

            var gres = res[0];
            var cres = res[1];

            Assert.Equal(1, gres.velocity.X);
            Assert.Equal(-1, cres.velocity.X);
        }

        [Fact]
        public void GravityTest()
        {
            var input = new (Vector3 position, Vector3 velocity)[]
            {
               (new Vector3(-1, 0, 2), Vector3.Zero),
               (new Vector3(2, -10, -7), Vector3.Zero),
               (new Vector3(4, -8, 8), Vector3.Zero),
               (new Vector3(3, 5, -1), Vector3.Zero)
            };

            var expectedResults = Results.Split('\n').Where(line => !string.IsNullOrWhiteSpace(line))
                    .Split(5)
                    .Select(section =>
                    {
                        var s = section[0].AsSpan().Slice(6, 2);
                        var ss = section[0].Substring(6, 2);
                        return (step: int.Parse(s, AllowLeadingWhite | AllowTrailingWhite),
                         bodies: section.Skip(1).Select(Parse));
                    });

            int currentStep = 0;
            var system = input.ToArray();
            foreach (var expectedResult in expectedResults)
            {
                while (currentStep < expectedResult.step)
                {
                    system = PlanetarySystem.Step(system);
                    currentStep++;
                }

                foreach (var set in system.Zip(expectedResult.bodies, (a,b) => (body: a, expected :b)))
                {
                    Assert.Equal(set.body.position, set.expected.pos);
                    Assert.Equal(set.body.velocity, set.expected.vel);
                }
            }
        }

        (Vector3 pos, Vector3 vel) Parse(string line)
        {
            //pos=<x= 2, y=-1, z= 1>, vel=<x= 3, y=-1, z=-1>

            var firstPart = readPart(line.AsSpan());
            var secondPart = readPart(line.AsSpan().Slice(line.IndexOf(',', firstPart.Length + 5)));

            var pos = parsePart(firstPart);
            var vel = parsePart(secondPart);

            return (pos, vel);

            ReadOnlySpan<char> readPart(ReadOnlySpan<char> slice)
            {
                var posEnd = slice.IndexOf('>');
                var posStart = slice.IndexOf('<') + 1;
                var posPart = slice.Slice(posStart, posEnd - posStart);
                return posPart;
            }

            Vector3 parsePart(ReadOnlySpan<char> part)
            {
                int parseNext(ReadOnlySpan<char> slice)
                {
                    return int.Parse(slice.Slice(2, 2), AllowLeadingSign | AllowLeadingWhite);
                }
                var x = parseNext(part);
                var y = parseNext(part.Slice(6));
                var z = parseNext(part.Slice(12));

                return new Vector3(x, y, z);
            }
        }

        private const string Results = @"After 1 step:
pos=<x= 2, y=-1, z= 1>, vel=<x= 3, y=-1, z=-1>
pos=<x= 3, y=-7, z=-4>, vel=<x= 1, y= 3, z= 3>
pos=<x= 1, y=-7, z= 5>, vel=<x=-3, y= 1, z=-3>
pos=<x= 2, y= 2, z= 0>, vel=<x=-1, y=-3, z= 1>

After 2 steps:
pos=<x= 5, y=-3, z=-1>, vel=<x= 3, y=-2, z=-2>
pos=<x= 1, y=-2, z= 2>, vel=<x=-2, y= 5, z= 6>
pos=<x= 1, y=-4, z=-1>, vel=<x= 0, y= 3, z=-6>
pos=<x= 1, y=-4, z= 2>, vel=<x=-1, y=-6, z= 2>

After 3 steps:
pos=<x= 5, y=-6, z=-1>, vel=<x= 0, y=-3, z= 0>
pos=<x= 0, y= 0, z= 6>, vel=<x=-1, y= 2, z= 4>
pos=<x= 2, y= 1, z=-5>, vel=<x= 1, y= 5, z=-4>
pos=<x= 1, y=-8, z= 2>, vel=<x= 0, y=-4, z= 0>

After 4 steps:
pos=<x= 2, y=-8, z= 0>, vel=<x=-3, y=-2, z= 1>
pos=<x= 2, y= 1, z= 7>, vel=<x= 2, y= 1, z= 1>
pos=<x= 2, y= 3, z=-6>, vel=<x= 0, y= 2, z=-1>
pos=<x= 2, y=-9, z= 1>, vel=<x= 1, y=-1, z=-1>

After 5 steps:
pos=<x=-1, y=-9, z= 2>, vel=<x=-3, y=-1, z= 2>
pos=<x= 4, y= 1, z= 5>, vel=<x= 2, y= 0, z=-2>
pos=<x= 2, y= 2, z=-4>, vel=<x= 0, y=-1, z= 2>
pos=<x= 3, y=-7, z=-1>, vel=<x= 1, y= 2, z=-2>

After 6 steps:
pos=<x=-1, y=-7, z= 3>, vel=<x= 0, y= 2, z= 1>
pos=<x= 3, y= 0, z= 0>, vel=<x=-1, y=-1, z=-5>
pos=<x= 3, y=-2, z= 1>, vel=<x= 1, y=-4, z= 5>
pos=<x= 3, y=-4, z=-2>, vel=<x= 0, y= 3, z=-1>

After 7 steps:
pos=<x= 2, y=-2, z= 1>, vel=<x= 3, y= 5, z=-2>
pos=<x= 1, y=-4, z=-4>, vel=<x=-2, y=-4, z=-4>
pos=<x= 3, y=-7, z= 5>, vel=<x= 0, y=-5, z= 4>
pos=<x= 2, y= 0, z= 0>, vel=<x=-1, y= 4, z= 2>

After 8 steps:
pos=<x= 5, y= 2, z=-2>, vel=<x= 3, y= 4, z=-3>
pos=<x= 2, y=-7, z=-5>, vel=<x= 1, y=-3, z=-1>
pos=<x= 0, y=-9, z= 6>, vel=<x=-3, y=-2, z= 1>
pos=<x= 1, y= 1, z= 3>, vel=<x=-1, y= 1, z= 3>

After 9 steps:
pos=<x= 5, y= 3, z=-4>, vel=<x= 0, y= 1, z=-2>
pos=<x= 2, y=-9, z=-3>, vel=<x= 0, y=-2, z= 2>
pos=<x= 0, y=-8, z= 4>, vel=<x= 0, y= 1, z=-2>
pos=<x= 1, y= 1, z= 5>, vel=<x= 0, y= 0, z= 2>

After 10 steps:
pos=<x= 2, y= 1, z=-3>, vel=<x=-3, y=-2, z= 1>
pos=<x= 1, y=-8, z= 0>, vel=<x=-1, y= 1, z= 3>
pos=<x= 3, y=-6, z= 1>, vel=<x= 3, y= 2, z=-3>
pos=<x= 2, y= 0, z= 4>, vel=<x= 1, y=-1, z=-1>";
    }
}
