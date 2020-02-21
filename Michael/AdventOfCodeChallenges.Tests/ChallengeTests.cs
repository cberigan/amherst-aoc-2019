using System;
using AdventOfCodeChallenges.C1;
using AdventOfCodeChallenges.C2;
using Xunit;

namespace AdventOfCodeChallenges.Tests
{
    public class ChallengeTests
    {
        private readonly C1.Challenge _c1;
        private readonly C2.Challenge _c2;

        public ChallengeTests()
        {
            _c1 = new C1.Challenge();
            _c2 = new C2.Challenge();
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

        [Theory]
        [InlineData(14, 2)]
        [InlineData(1969,966)]
        [InlineData(100756, 50346)]
        public void Challenge2Tests(int input, int expected)
        {
            var result = _c2.Run(new long[] { input });
            Assert.Equal(expected, result);
        }
    }
}
