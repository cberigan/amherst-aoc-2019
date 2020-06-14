using AdventOfCodeChallenges.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using static System.Globalization.NumberStyles;

namespace AdventOfCodeChallenges.C12
{
    public static class Challenge
    {
        public const string Input = @"<x=-15, y=1, z=4>
<x=1, y=-10, z=-8>
<x=-5, y=4, z=9>
<x=4, y=6, z=-2>";

        public const string Example = @"<x=-1, y=0, z=2>
<x=2, y=-10, z=-7>
<x=4, y=-8, z=8>
<x=3, y=5, z=-1>";

        public const string Pt2TestInput = @"<x=-8, y=-10, z=0>
<x=5, y=5, z=10>
<x=2, y=-7, z=3>
<x=9, y=-8, z=-3>";

        public class Pt1
        {
            // total energy after 1000 iters
            public int Run(string input = Input)
            {
                var planetPositions = input.Split('\n').Select(ParseLine);
                var inputs = planetPositions.Select(pp => (
                                pp, Vector3.Zero)).ToArray();

                const int iterations = 1000;
                for (int i = 0; i < iterations; i++)
                {
                    PlanetarySystem.Step(inputs, inputs);
                }

                var totalSystemEnergy = PlanetarySystem.TotalEnergy(inputs);
                return (int)totalSystemEnergy;
            }
        }

        public class Pt2
        {
            public int Attempt1(string input = Example)
            {
                // assumes that any state could be repeated
                // how many steps until all the moons are in the same points and velocities.
                // Didn't wait long enough to find out if it worked

                var planetPositions = input.Split('\n').Select(ParseLine);
                var inputs = planetPositions.Select(pp => (pp, Vector3.Zero)).ToArray();

                var history = new HashSet<long>();
                var comparer = new Comparer();
                history.Add(comparer.GetHashCode(inputs));
                int i = 1;
                (Vector3 position, Vector3 velocity)[] state = inputs;
                while (true)
                {
                    PlanetarySystem.Step(state, state);
                    if (!history.Add(comparer.GetHashCode(state)))
                        return i;
                    i++;
                    if (i % 1000 == 0) Console.WriteLine(i);
                }
            }

            // attempt 2
            public int RunUntilVelocitiesAreZeroAndDoubleIters(string input = Pt2TestInput)
            {
                // this version (with changes from the Rust version put into the PlanetarySystem code
                // and allocations removed) is a lot faster than it used to be, but will take forever
                // to find the answer.
                var planetPositions = input.Split('\n').Select(ParseLine);
                var inputs = planetPositions.Select((pp, i) => (pp, Vector3.Zero)).ToArray();

                ( Vector3 position, Vector3 velocity)[] state = inputs.ToArray();
                int i = 0;
                bool notAllZeros = true;
                while (notAllZeros)
                {
                    i++;
                    PlanetarySystem.Step(state, state);
                    notAllZeros = !state.All(s => s.velocity == Vector3.Zero);
                    if (i % 1_000_000 == 0) Console.WriteLine(i);
                }

                // if all of the velocities are zero
                return i * 2;
            }

            // solution
            public long Run() => RunUsingLeastCommonMultiples();
            // Find the number of cycles for each XYZ axis of each moon to return to 0, then use the Least Common Multiple of all axes.
            // I still don't know why you have to separate them to get the right answer.
            public long RunUsingLeastCommonMultiples(string input = Input)
            {
                // totally stole this from this guy's rust code. https://github.com/prscoelho/aoc2019/blob/master/src/aoc12/mod.rs
                // It looks like the difference between what I and he did was that
                // he calculated the velocities of each axis (x,y,z) separately
                // and found how many cycles it took for all of the moons to return to origin on that axis only.
                // Then he gets the least common multiple of the number of steps for each axis.

                // What I did was keep all the axes together, and I have the number of cycles for each moon.
                // I don't know why, but splitting it into the number of cycles for each axis rather than moon
                // seems to produce the correct answer.
                // I don't really understand how this is better than finding the orbital period of each moon,
                // but if I find the least common multiple of those values, I get a wildly off answer.


                var planetPositions = input.Split('\n').Select(ParseLine);
                var inputs = planetPositions.Select((pp, i) => (new PlanetaryBody(i.ToString()), pp, Vector3.Zero)).ToArray();

                (PlanetaryBody, Vector3 position, Vector3 velocity)[] state = inputs.ToArray();

                var x = StepCountToReturnToOriginForAllMoonsByAxis(inputs.Select(i => (int)i.pp.X).ToArray());
                var y = StepCountToReturnToOriginForAllMoonsByAxis(inputs.Select(i => (int)i.pp.Y).ToArray());
                var z = StepCountToReturnToOriginForAllMoonsByAxis(inputs.Select(i => (int)i.pp.Z).ToArray());

                return Formulas.Lcm(x, Formulas.Lcm(y, z));
                //                pub fn solve_second(input: &str)->u64 {
                //                    let moons = read_moons(input);
                //                    let x = find_steps_axis(moons.iter().map(| m | m.position.x).collect());
                //                    let y = find_steps_axis(moons.iter().map(| m | m.position.y).collect());
                //                    let z = find_steps_axis(moons.iter().map(| m | m.position.z).collect());
                //                    lcm3(x, y, z)
                //}


                static long StepCountToReturnToOriginForAllMoonsByAxis(int[] inPositions)
                {
                    var positionsClone = (int[])inPositions.Clone();
                    var velocities = new int[inPositions.Length];
                    var velocitiesEnd = new int[inPositions.Length];

                    var steps = 0L;

                    while (true)
                    {
                        var vDelta = VelocityDiff(positionsClone);
                        for (int i = 0; i < velocities.Length; i++)
                            velocities[i] += vDelta[i];
                        for (int p = 0; p < positionsClone.Length; p++)
                            positionsClone[p] += velocities[p];
                        
                        steps++;

                        if (velocities.SequenceEqual(velocitiesEnd))
                            break;
                    }

                    return steps * 2;

                    //                fn find_steps_axis(mut positions: Vec<i32>) -> u64 {
                    //                    let mut velocities = vec![0; positions.len()];
                    //                    let velocities_end = velocities.clone();

                    //                    let mut steps = 0;
                    //                    loop {
                    //                        let velocity_change = velocity_diff(&positions);
                    //                        for (v, change) in velocities.iter_mut().zip(velocity_change) {
                    //                            *v += change;
                    //                        }
                    //                        for (p, v) in positions.iter_mut().zip(velocities.iter()) {
                    //                            *p += v;
                    //                        }

                    //                        steps += 1;

                    //                        if velocities == velocities_end {
                    //                            break;
                    //                        }
                    //                    }
                    //                    steps * 2
                    //}

                    static int[] VelocityDiff(int[] positions)
                    {
                        var res = new int[positions.Length];
                        for (int i = 0; i < positions.Length; i++)
                        {
                            for (int j = i + 1; j < positions.Length; j++)
                            {
                                res[i] += positions[j].CompareTo(positions[i]);
                                res[j] += positions[i].CompareTo(positions[j]);
                            }
                        }
                        return res;

                    }

                        //fn velocity_diff(positions: &[i32]) -> Vec<i32> {
//                    let mut result = vec![0; positions.len()];
//                    for (idx1, pos1) in positions.iter().enumerate() {
//                        for (idx2, pos2) in positions.iter().enumerate().skip(idx1 + 1) {
//                            result[idx1] += compare_axis(*pos1, *pos2);
//                            result[idx2] += compare_axis(*pos2, *pos1);
//                        }
//                    }
//                    result
//}
            }

            }


            protected int[] FindOrbitalPeriods((Vector3 position, Vector3 velocity)[] stateInput)
            {
                
                (Vector3 position, Vector3 velocity)[] state = stateInput.ToArray();
                PlanetarySystem.Step(state, state);

                var foundOrbitalPeriods = new int[4];
                int i = 1;
                while (!foundOrbitalPeriods.All(p => p > 0))
                {
                    PlanetarySystem.Step(state, state);
                    int p = 0;
                    foreach (var s in state)
                    {
                        if (foundOrbitalPeriods[p] == 0 && s.velocity == Vector3.Zero)
                        {
                            foundOrbitalPeriods[p] =  i;
                            //Console.WriteLine("Found orbital period for {0} of {1} iters", s.Item1.Name, i);
                        }
                        p++;
                    }
                    i++;
                }

                return foundOrbitalPeriods;
            }
        }

        class Comparer //: IEqualityComparer<(Vector3 pos, Vector3 vel)[]>
        {
            //public bool Equals([AllowNull] (Vector3 pos, Vector3 vel)[] x, [AllowNull] (Vector3 pos, Vector3 vel)[] y)
            //{
            //    for (int i = 0; i < x.Length; i++)
            //    {
            //        if (x[i] != y[i]) return false;
            //    }
            //    return true;
            //}

            public long GetHashCode([DisallowNull] (Vector3 pos, Vector3 vel)[] obj)
            {

                long h = -1923861349L;
                foreach (var x in obj)
                {
                    h = h * -1521134295L + x.pos.GetHashCode();
                    h = h * -1521134295L + x.vel.GetHashCode();
                }
                return h;
            }
        }

        static Vector3 ParseLine(string line)
        {
            //pos=<x= 2, y=-1, z= 1>, vel=<x= 3, y=-1, z=-1>

            var firstPart = readPart(line.AsSpan());

            var pos = parsePart(firstPart);

            return pos;

            ReadOnlySpan<char> readPart(ReadOnlySpan<char> slice)
            {
                var posEnd = slice.IndexOf('>');
                var posStart = slice.IndexOf('<') + 1;
                var posPart = slice.Slice(posStart, posEnd - posStart);
                return posPart;
            }

            Vector3 parsePart(ReadOnlySpan<char> part)
            {
                int parseNext(ReadOnlySpan<char> slice, int length)
                {
                    return int.Parse(slice.Slice(2, length - 2), AllowLeadingSign | AllowLeadingWhite);
                }
                var x = parseNext(part, part.IndexOf(','));
                var y = parseNext(part.Slice(part.IndexOf('y')), part.Slice(part.IndexOf('y')).IndexOf(','));
                var z = parseNext(part.Slice(part.IndexOf('z')), part.Slice(part.IndexOf('z')).Length);

                return new Vector3(x, y, z);
            }
        }
    }
}
