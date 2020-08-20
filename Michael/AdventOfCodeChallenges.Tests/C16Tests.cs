using AdventOfCodeChallenges.C16;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Numerics;
using System.Linq;

namespace AdventOfCodeChallenges.Tests
{
    public class C16Tests
    {
        [Theory]
        [InlineData("12345678", "0,1,0,-1", 48226158)]
        [InlineData("48226158", "0,1,0,-1", 34040438)]
        [InlineData("34040438", "0,1,0,-1", 03415518)]
        [InlineData("03415518", "0,1,0,-1", 01029498)]
        public void BasicTest(string signal, string pattern, long expected)
        {
            var ftt = new FlawedFrequencyTransmission();
            var result = ftt.Execute(signal, pattern).Aggregate(0, (acc, curr) => acc * 10 + curr);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("12345678", 01029498, 4)]
        [InlineData("80871224585914546619083218645595", 24176176, 100)]
        public void FirstEightDigitsTest_After100Iterations(string input, int expected, int phases)
        {
            var ftt = new FlawedFrequencyTransmission();

            var res = ftt.ExecutePhases(input, "0,1,0,-1", phases);

            Assert.Equal(expected, res.Take(8).Aggregate(0, (acc, curr) => acc * 10 + curr));
        }
    }
}
