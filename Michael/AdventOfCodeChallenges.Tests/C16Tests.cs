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

        [Fact]
        public void BasicItersTest()
        {
            var ftt = new FlawedFrequencyTransmissionSimd();
            var res = ftt.ExecutePhases("12345678", "0,1,0,-1", 4)
                .Aggregate(0, (acc, curr) => acc * 10 + curr);
            Assert.Equal(01029498, res);
        }

        [Theory]
        [InlineData("12345678", 48226158)]
        [InlineData("48226158", 34040438)]
        [InlineData("34040438", 03415518)]
        [InlineData("03415518", 01029498)]
        public void StateMachineTest(string signal, long expected)
        {
            var ftt = new FlawedFrequencyTransmissionVertical();
            var result = ftt.Execute(signal.Select(c => (sbyte)(c - 48)).ToArray(), 1).Aggregate(0, (acc, curr) => acc * 10 + curr);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void StateMachine4Iters()
        {
            var fft = new FftStateMachine();
            var res = fft.Execute("12345678", 4)
                .Aggregate(0, (acc, curr) => acc * 10 + curr);
            Assert.Equal(01029498, res);
        }

        [Theory]
        [InlineData("12345678", 48226158)]
        [InlineData("48226158", 34040438)]
        [InlineData("34040438", 03415518)]
        [InlineData("03415518", 01029498)]
        public void VerticalTest(string signal, long expected)
        {
            var ftt = new FftStateMachine();
            var result = ftt.Execute(signal.Select(c => (sbyte)(c - 48)).ToArray(), 1).Aggregate(0, (acc, curr) => acc * 10 + curr);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("12345678", 01029498, 4)]
        //[InlineData("03036732577212944063491565474664", 84462026, 100)]
        //[InlineData("02935109699940807407585447034323", 78725270, 100)]
        //[InlineData("03081770884921959731165446850517", 53553731, 100)]
        public void FirstEightDigitsTest_After100Iterations(string input, int expected, int phases)
        {
            var ftt = new FlawedFrequencyTransmissionSimd();

            var inputBytes = Enumerable.Repeat(input, 10_000)
                    .SelectMany(s => s.Select(c => (sbyte)(c - 48)))
                    .ToArray();
            var res = ftt.ExecutePhases(inputBytes, "0,1,0,-1", phases, d =>
            {
                var offset = d.Take(7).Aggregate(0, (acc, curr) => acc * 10 + curr);
                var res = Enumerable.Range(offset, 8).Select(x => d[x]).Aggregate(0, (acc, curr) => acc * 10 + curr);
                return res;
            });

            Assert.Equal(expected, res.Take(8).Aggregate(0, (acc, curr) => acc * 10 + curr));
        }
    }
}
