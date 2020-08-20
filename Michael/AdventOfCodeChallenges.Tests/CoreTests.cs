using AdventOfCodeChallenges.Core;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Xunit;

namespace AdventOfCodeChallenges.Tests
{
    public class CoreTests
    {
        [Theory]
        [InlineData(0123456789, 0, 9)]
        [InlineData(0123456789, 1, 8)]
        [InlineData(0123456789, 2, 7)]
        [InlineData(0123456789, 3, 6)]
        [InlineData(0123456789, 4, 5)]
        [InlineData(0123456789, 5, 4)]
        [InlineData(0123456789, 6, 3)]
        [InlineData(0123456789, 7, 2)]
        [InlineData(0123456789, 8, 1)]
        [InlineData(0123456789, 9, 0)]
        public void IndexableNumberTests(int input, int i, int expected)
        {
            var res = new IndexableNumber(input)[i];
            Assert.Equal(expected, res);
        }
    }
}
