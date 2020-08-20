using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;

namespace AdventOfCodeChallenges.C16
{
    public partial class Challenge
    {
        public class Pt1
        {
            public string Run()
            {
                var input = Input;
                var ftt = new FlawedFrequencyTransmission();

                var res = ftt.ExecutePhases(input, "0,1,0,-1", 100);
                var first8Digits = string.Join("", res.Take(8));

                return first8Digits;
            }
        }

        // same as before, but puzzle input is repeated 10_000 times
        // first 7 digits are the message offset
        public class Pt2
        {
            public string Run()
            {
                var inputBytes = Enumerable.Repeat(Input, 10_000)
                    .SelectMany(s => s.Select(c => (byte)(c - 48)))
                    .ToArray();

                var ftt = new FlawedFrequencyTransmission();

                var res = ftt.ExecutePhases(inputBytes, "0,1,0,-1", 100, 
                    digitsToSkip: r => r.Take(7).Aggregate(0, (acc, curr) => acc * 10 + curr));

                var messageOffset = res.Take(7).Aggregate(0, (acc, curr) => acc * 10 + curr);
                var message = string.Join("", Enumerable.Range(messageOffset, 7).Select(i => res[i]));
                return message;
            }
        }
    }
}
