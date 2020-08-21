using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
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
            public string Run(string input = Input, int iters = 100, int repeat = 10_000)
            {
                var messageOffset = input.Take(7).Aggregate(0, (acc, curr) => acc * 10 + (curr-48));

                var inputBytes = Enumerable.Repeat(input, repeat)
                    .SelectMany(s => s.Select(c => (sbyte)(c - 48)))
                    .Skip(messageOffset)
                    .ToArray();


                // internet people say the answer depends on what is on the right hand side and the left hand side doesn't matter
                // so they truncate it


                var ftt = new FftStateMachine();

                var res = ftt.Execute(inputBytes, iters);

                var msg = string.Join("", res.Take(8));
                return msg;
            }

            // once again, I have no idea why this works
            // ppl figured out that nothing before the message offset matters
            // the message offset is from the initial input NOT from the final calculated version,
            // which I misunderstood.
            // Also, there seems to be a pattern with the orders, which is why,
            // I imagine, doing sums.Last() - sums[i] %10 gives a correct single
            // digit sum. Again, I'd have never figured this out.
            // this is probably the 3rd challenge that I had to steal from online
            // but don't understand why it works.
            public int Stolen()
            {
                // changing lists to arrays cut the runtime in about half.
                var numbers = Input.Select(c => (sbyte)c - 48).ToArray();
                var start = Fold(numbers, 7);
                var end = numbers.Length * 10_000;

                var current = new sbyte[end - start];
                for (int i = start; i < end; i++)
                {
                    current[i - start] = (sbyte)numbers[i % numbers.Length];
                }

                var sums = new int[current.Length + 1]; 
                // lifted this array up since all values get overridden and to not create
                // more allocations. dropped runtime from 160ms to 126ms.
                for (int _ = 0; _ < 100; _++)
                {
                    var total = 0;
                    sums[0] = 0;

                    for (int i = 0; i < current.Length; i++)
                    {
                        total += current[i];
                        sums[i + 1] = total;
                    }

                    for (int i = 0; i < current.Length; i++)
                    {
                        var value = sums[sums.Length - 1] - sums[i];
                        current[i] = (sbyte)(value % 10);
                    }
                }

                var res = Fold(current, 8);
                return res;
            }

            // moving these functions from .Aggregate dropped the runtime from 150ish ms to 127ish ms
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            private static int Fold(int[] arr, int take)
            {
                int total = 0;
                for (int i = 0; i < take; i++)
                {
                    total *= 10;
                    total += arr[i];
                }
                return total;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            private static int Fold(sbyte[] arr, int take)
            {
                int total = 0;
                for (int i = 0; i < take; i++)
                {
                    total *= 10;
                    total += arr[i];
                }
                return total;
            }
        }
    }
}
