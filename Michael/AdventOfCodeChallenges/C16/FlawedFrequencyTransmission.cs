using AdventOfCodeChallenges.Core;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Globalization.NumberStyles;

namespace AdventOfCodeChallenges.C16
{
    public class FlawedFrequencyTransmission
    {
        private const int AsciiNumberOffset = 48;

        public byte[] Execute(string signal, string pattern, int digitsToSkip = 1)
        {
            var inputSignal = signal.Select(c => (byte)(c - AsciiNumberOffset)).ToArray();
            return Execute(inputSignal, pattern, digitsToSkip);
        }

        public byte[] Execute(byte[] signal, string pattern, int digitsToSkip = 1)
        {
            var inputPattern = pattern.Split(',').Select(x => int.Parse(x, AllowLeadingSign | AllowTrailingWhite | AllowLeadingWhite)).ToArray();

            var res = Calculate(signal, inputPattern, digitsToSkip);

            return res;
        }

        public byte[] ExecutePhases(string signal, string pattern, int iterations, Func<byte[], int> digitsToSkip = null)
        {
            if (digitsToSkip == null) digitsToSkip = _ => 1;
            return Enumerable.Repeat(1, iterations)
                .Aggregate(signal.Select(c => (byte)( c - AsciiNumberOffset)).ToArray(), (acc, _) => 
                    Execute(acc, pattern, digitsToSkip(acc)));
        }

        public byte[] ExecutePhases(byte[] input, string pattern, int iterations, Func<byte[], int> digitsToSkip = null)
        {
            if (digitsToSkip == null) digitsToSkip = _ => 1;

            var inputPattern = pattern.Split(',').Select(x => int.Parse(x, AllowLeadingSign | AllowTrailingWhite | AllowLeadingWhite)).ToArray();

            byte[] output = new byte[input.Length];

            for (int i = 0; i < iterations; i++)
            {
                Calculate(input, inputPattern, digitsToSkip(output), output);
                (output, input) = (input, output);
            }

            return output;
        }

        private void Calculate(RepeatedString inputSignal, int[] pattern, int digitsToSkip, byte[] output)
        {
            output ??= new byte[inputSignal.Length];

            Parallel.For(0, inputSignal.Length, i =>
            {
                var iterationPattern = pattern.SelectMany(p =>
                    Enumerable.Repeat(p, i + 1))
                    .ToArray();

                var multiplicationPattern = MoreEnumerable.Generate(
                        iterationPattern.ToList(),
                        acc =>
                        {
                            acc.AddRange(iterationPattern);
                            return acc;
                        })
                    .SkipWhile(l => l.Count < inputSignal.Length + 1)
                    .First()
                    .Skip(digitsToSkip)
                    .Take(inputSignal.Length)
                    .ToArray();

                var sum =
                    Enumerable.Repeat(inputSignal.InternalString, inputSignal.Repeats)
                        .SelectMany(s => s.Zip(multiplicationPattern, (a, b) => (a, b))
                    )
                    .Sum(x => x.a * x.b);

                var onesDigit = (byte)new IndexableNumber(sum)[0]; // only keep the position in the 1s place

                output[i] = onesDigit;
            });


            //var result = inputSignal.Select((_, i) =>
            //{
            //    // the iteration pattern is the pattern argument's elements repeated
            //    // the by the iteration number we are on.
            //    // if i == 2, then each element is repeated in place 2 times.
            //    var iterationPattern = pattern.SelectMany(p => 
            //        Enumerable.Repeat(p, i + 1))
            //        .ToArray();

            //    var multiplicationPattern = MoreEnumerable.Generate(
            //            iterationPattern.ToList(), 
            //            acc =>
            //            {
            //                acc.AddRange(iterationPattern);
            //                return acc;
            //            })
            //        .SkipWhile(l => l.Count < inputSignal.Length + 1)
            //        .First()
            //        .Skip(digitsToSkip)
            //        .Take(inputSignal.Length)
            //        .ToArray();

            //    var output =
            //        inputSignal.Zip(multiplicationPattern, (a, b) => (a, b))
            //        .Sum(x => x.a * x.b);

            //    var onesDigit = new IndexableNumber(output)[0]; // only keep the position in the 1s place
                
            //    return onesDigit;
            //}).ToArray();

            //return result;
        }

        private byte[] Calculate(byte[] inputSignal, int[] pattern, int digitsToSkip, byte[] output = null)
        {
            output ??= new byte[inputSignal.Length];

            //for (int i = 0; i < inputSignal.Length; i++)
            Parallel.For(0, inputSignal.Length, i =>
            {
                var iterationPattern = new RepeatingDigits(pattern, i + 1);

                var enu = iterationPattern.GetEnumerator();
                for (int s = 0; s < digitsToSkip; s++) // skip the first element only the first time
                {
                    enu.MoveNext();
                }

                int sum = 0; int k = 0;
                while (k < inputSignal.Length)
                {
                    var a = inputSignal[k];
                    var b = enu.Current;
                    sum += a * b;
                    k++;

                    if (!enu.MoveNext())
                        enu = iterationPattern.GetEnumerator();
                }

                var onesDigit = (byte)new IndexableNumber(sum)[0]; // only keep the position in the 1s place

                output[i] = onesDigit;
            }
            );

            return output;
        }
    }
}
