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
            public string Run(string input = Input, int iters = 100)
            {
                var inputBytes = Enumerable.Repeat(input, iters)
                    .SelectMany(s => s.Select(c => (sbyte)(c - 48)))
                    .ToArray();

                var ftt = new FftStateMachine();

                var res = ftt.Execute(inputBytes, iters);

                Console.WriteLine();
                Console.WriteLine(string.Join("", res));

                var messageOffset = res.Take(7).Aggregate(0, (acc, curr) => acc * 10 + curr);
                Console.WriteLine(messageOffset);
                try
                {
                    var message = string.Join("", Enumerable.Range(messageOffset, 7).Select(i => res[i]));
                    return message;
                }
                catch
                {
                    Console.WriteLine("failed grabbing the message by index");
                    Console.ReadLine();
                    return "-1";
                }
            }

            //public string Run(string input = Input, string pattern = Pattern, int iters = 100)
            //{
            //    var inputBytes = Enumerable.Repeat(input, iters)
            //        .SelectMany(s => s.Select(c => (sbyte)(c - 48)))
            //        .ToArray();

            //    var ftt = new FlawedFrequencyTransmission();

            //    var res = ftt.ExecutePhases(inputBytes, pattern, iters,
            //        digitsToSkip: r => r.Take(7).Aggregate(0, (acc, curr) => acc * 10 + curr));

            //    Console.WriteLine();
            //    Console.WriteLine(string.Join("", res));

            //    var messageOffset = res.Take(7).Aggregate(0, (acc, curr) => acc * 10 + curr);
            //    var message = string.Join("", Enumerable.Range(messageOffset, 7).Select(i => res[i]));
            //    return message;
            //}

            //public string Run(string input = Input, int iters = 100, int repeat = 10_000)
            //{
            //    var inputBytes = Enumerable.Repeat(input, repeat)
            //        .SelectMany(s => s.Select(c => (sbyte)(c - 48)))
            //        .ToArray();

            //    var ftt = new FlawedFrequencyTransmissionVertical();
            //    var res = ftt.Execute(inputBytes, iters, digitsToSkip: r => r.Take(7).Aggregate(0, (acc, curr) => acc * 10 + curr));

            //    var messageOffset = res.Take(7).Aggregate(0, (acc, curr) => acc * 10 + curr);
            //    var message = string.Join("", Enumerable.Range(messageOffset, 7).Select(i => res[i]));
            //    return message;
            //}
        }
    }
}
