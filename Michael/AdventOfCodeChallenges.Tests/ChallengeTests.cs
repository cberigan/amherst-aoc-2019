using System;
using AdventOfCodeChallenges.C1;
using AdventOfCodeChallenges.C2;
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
    }
}
