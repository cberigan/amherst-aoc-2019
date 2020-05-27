using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using AdventOfCodeChallenges.C1;
using AdventOfCodeChallenges.C2;
using AdventOfCodeChallenges.C6;
using AdventOfCodeChallenges.C7;
using AdventOfCodeChallenges.C10;
using AdventOfCodeChallenges.Core;
using Xunit;

namespace AdventOfCodeChallenges.Tests
{
    public class ChallengeTests
    {
        private readonly C1.Challenge _c1;
        //private readonly C2.ChallengePt2 _c2;

        public ChallengeTests()
        {
            _c1 = new C1.Challenge();
            //_c2 = new C2.ChallengePt2();
        }

        [Theory]
        [InlineData(12, 2)]
        [InlineData(14, 2)]
        [InlineData(1969, 654)]
        [InlineData(100756, 33583)]
        public void Challenge1Tests(int input, int expected)
        {
            var result = _c1.FuelRequired(input);
            Assert.Equal(result, expected);
        }

        //[Theory]
        //[InlineData(14, 2)]
        //[InlineData(1969,966)]
        //[InlineData(100756, 50346)]
        //public void Challenge2Tests(int input, int expected)
        //{
        //    var result = _c2.Run(new long[] { input });
        //    Assert.Equal(expected, result);
        //}

        [Fact]
        public void Challenge3Tests()
        {
            var path1 = @"R8,U5,L5,D3";
            var path2 = "U7,R6,D4,L4";

            var res = new C3.ChallengePt1().Run(path1, path2);

            Assert.Equal(6, (int)res);
        }

        [Theory]
        [InlineData("R8,U5,L5,D3", "U7,R6,D4,L4", 30)]
        //[InlineData("R75,D30,R83,U83,L12,D49,R71,U7,L72,U62,R66,U55,R34,D71,R55,D58,R83", "R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51,U98,R91,D20,R16,D67,R40,U7,R15,U6,R7", 1020)]
        public void Challenge3Pt2Tests(string inputa, string inputb, int expected)
        {
            //var inputa = "R75,D30,R83,U83,L12,D49,R71,U7,L72,U62,R66,U55,R34,D71,R55,D58,R83";
            //var inputb = "R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51,U98,R91,D20,R16,D67,R40,U7,R15,U6,R7";

            var fewestCombinedSteps = new C3.ChallengePt2().Run(inputa, inputb);

            Assert.Equal(expected, fewestCombinedSteps);
        }

        [Theory]
        [InlineData(111111, false)]
        [InlineData(223450, false)]
        [InlineData(123789, false)]
        [InlineData(123444, false)]
        [InlineData(111122, true)]
        public void Challenge4Tests(int i, bool expected)
        {
            var c4 = new C4.Challenge.Pt2().Run(i..(i + 1));
            Assert.Equal(expected, c4 == 1);
        }

        [Theory]
        [InlineData("1002,4,3,4,33", 4, 99)]
        [InlineData("1101,100,-1,4,0", 4, 99)]
        [InlineData("1001,5,100,5,99,-1", 4, 99)]
        [InlineData("3,12,6,12,15,1,13,14,13,4,13,99,-1,0,1,9", 0, 3)]
        //[InlineData("3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99",
        //    0, 999)]
        public void Challenge5Tests(string data, int position, int value)
        {
            var res = new C5.Challenge().Run(data.Split(",").Select(int.Parse).ToArray(),
                ((int val, int[] memory, int outputVal) x) => Assert.Equal(value, x.memory[position]));
        }

        [Fact]
        public void Challenge6_ParsesOrbits()
        {
            var parser = new OrbitParser();
            var orbits = parser.Parse(_orbits);
            var system = new OrbitalSystem(orbits);
            var totalOrbitCounts = system.DirectAndIndirectOrbitCount;
            Assert.Equal(42, totalOrbitCounts);
        }

        [Fact]
        public void Challenge6_Pt2()
        {
            var c = new C6.Challenge.Pt2();
            var res = c.Run(c6p2);
            Assert.Equal(4, res);
        }

        [Theory]
        [InlineData("3,15,3,16,1002,16,10,16,1,16,15,15,4,15,99,0,0", 4, 3, 2, 1, 0, 43210, false)]
        [InlineData("3,23,3,24,1002,24,10,24,1002,23,-1,23,101,5,23,23,1,24,23,23,4,23,99,0,0", 0, 1, 2, 3, 4, 54321, false)]
        [InlineData("3,31,3,32,1002,32,10,32,1001,31,-2,31,1007,31,0,33,1002,33,7,33,1,33,31,31,1,32,31,31,4,31,99,0,0,0", 1, 0, 4, 3, 2, 65210, false)]
        public void Challenge7Tests(string memory, int a, int b, int c, int d, int e, int expected, bool feedback)
        {
            var mem = memory.Split(",").Select(int.Parse).ToArray();
            var setting = new PhaseSettings(a, b, c, d, e);
            var con = new AmplifierController();

            var res = con.Run(mem, setting, feedback);
            Assert.Equal(expected, res);
        }

        [Fact]
        public void Challenge10CorrectlyParsesMap()
        {
            var expected = new bool[,]
            {
                {false, true, false, false, true },
                {false, false, false, false, false },
                {true, true, true,true, true },
                {false, false,false,false,true },
                {false,false,false,true,true }
            };

            var result = C10.Challenge.Parse(C10Test1Input).map;
            for (int y = 0; y <= expected.GetUpperBound(0); y++)
            {
                for (int x = 0; x <= expected.GetUpperBound(1); x++)
                {
                    Assert.Equal(expected[y, x], result[y, x]);
                }
            }
        }

        [Fact]
        public void Challenge10_Returns8AsteroidsSpottedAt_3_4()
        {
            var map = C10.Challenge.Parse(C10Test1Input);
            var coordinateMap = new CoordinateMap(map.map);
            var count = coordinateMap.VisibleAsteroids(new Coordinate(3, 4)).Count();

            Assert.Equal(8, count);
        }

        [Fact]
        public void Challenge10_ReturnsCorrectNumberOfVisibleAsteroids()
        {
            var map = C10.Challenge.Parse(C10Test1Input);
            var coordinateMap = new CoordinateMap(map.map);

            var asteroidLocations = coordinateMap.AsteroidLocations;
            var expected = new (Coordinate c, int count)[]
            {
                 ((1,0), 7),
                 ((4, 0), 7),
                 ((0,2), 6),
                 ((1,2), 7),
                 ((2,2), 7),
                 ((3,2), 7),
                 ((4,2), 5),
                 ((4,3), 7),
                 ((3,4), 8),
                 ((4,4), 7)
            };

            var result = asteroidLocations.Select(l =>
                    (l, coordinateMap.VisibleAsteroids(l).Count()))
                .ToDictionary(x => x.l, x => x.Item2);

            foreach (var exp in expected)
                Assert.Equal(exp.count, result[exp.c]);
        }

        [Fact]
        public void Challenge10Pt2_ReturnsDestroyedAsteroidsAtCorrectIndices()
        {
            (int x, int y, int order)[] expected = {
                (11,12,1), (12,1,2), (12,2,3),(12,8,10),(16,0,20),
                (16,9,50), (10,16,100), (9,6,199), (8,2, 200), (10,9, 201), 
                (11,1,299)};

            var res = new C10.Challenge.Pt2().RunImpl(C10Pt2Input, (11,13))
                        .Select((r,i) => (r.target.X, r.target.Y, i: i + 1));

            foreach(var x in from e in expected
                             join r in res on (e.x,e.y) equals (r.X, r.Y)
                             select (e, r.i))
            {
                Assert.Equal(x.e.order, x.i);
            }


        }

        [Theory]
        [InlineData("109,1,204,-1,1001,100,1,100,1008,100,16,101,1006,101,0,99", "109,1,204,-1,1001,100,1,100,1008,100,16,101,1006,101,0,99")]
        [InlineData("1102,34915192,34915192,7,4,7,99,0", "1219070632396864")]
        [InlineData("104,1125899906842624,99", "1125899906842624")]
        public void Challenge9Tests(string inputs, string expected)
        {
            var input = inputs.Split(',').Select(BigInteger.Parse).ToArray();
            var cpu = new C9.IntCodeStateMachine(input, 1);
            var outputs = new List<BigInteger>();
            cpu.OnOutput += (_, v) => outputs.Add(v);
            while (!cpu.IsHalted)
            {
                cpu.RunOnce();
            }


            var exp = expected.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(BigInteger.Parse).ToArray();
            Assert.Equal(exp, outputs);
        }



        private const string C10Test1Input = @".#..#
.....
#####
....#
...##";

        private const string C10Test2Input = @"......#.#.
#..#.#....
..#######.
.#.#.###..
.#..#.....
..#....#.#
#..#....#.
.##.#..###
##...#..#.
.#....####";

        private const string C10Test3Input = @"#.#...#.#.
.###....#.
.#....#...
##.#.#.#.#
....#.#.#.
.##..###.#
..#...##..
..##....##
......#...
.####.###.";

        private const string C10Test4Input = @".#..#..###
####.###.#
....###.#.
..###.##.#
##.##.#.#.
....###..#
..#.#..#.#
#..#.#.###
.##...##.#
.....#.#..";

        private const string C10Test5Input = @".#..##.###...#######
##.############..##.
.#.######.########.#
.###.#######.####.#.
#####.##.#.##.###.##
..#####..#.#########
####################
#.####....###.#.#.##
##.#################
#####.##.###..####..
..######..##.#######
####.##.####...##..#
.#####..#.######.###
##...#.##########...
#.##########.#######
.####.#.###.###.#.##
....##.##.###..#####
.#.#.###########.###
#.#.#.#####.####.###
###.##.####.##.#..##";

        private const string C10Pt2Input = @".#..##.###...#######
##.############..##.
.#.######.########.#
.###.#######.####.#.
#####.##.#.##.###.##
..#####..#.#########
####################
#.####....###.#.#.##
##.#################
#####.##.###..####..
..######..##.#######
####.##.####...##..#
.#####..#.######.###
##...#.##########...
#.##########.#######
.####.#.###.###.#.##
....##.##.###..#####
.#.#.###########.###
#.#.#.#####.####.###
###.##.####.##.#..##";

        private const string _orbits = @"COM)B
B)C
C)D
D)E
E)F
B)G
G)H
D)I
E)J
J)K
K)L";
        private const string c6p2 = @"COM)B
B)C
C)D
D)E
E)F
B)G
G)H
D)I
E)J
J)K
K)L
K)YOU
I)SAN";
    }
}