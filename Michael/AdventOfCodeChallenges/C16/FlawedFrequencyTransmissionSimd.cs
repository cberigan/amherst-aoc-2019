using AdventOfCodeChallenges.Core;
using JM.LinqFaster.SIMD;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Globalization.NumberStyles;

namespace AdventOfCodeChallenges.C16
{
    // ran benchmark.net to see how much faster the SIMD version is!!!!!
    // |  Method |      Mean |    Error |   StdDev | Ratio | RatioSD |
    // |-------- |----------:|---------:|---------:|------:|--------:|
    // | Regular |  81.20 ms | 1.608 ms | 2.254 ms |  1.00 |    0.00 |
    // |    Simd | 111.39 ms | 2.195 ms | 5.258 ms |  1.32 |    0.08 |
    //
    // ... well shit.
    /// <summary>
    /// Don't use this...it's slower than non-simd. Leaving it for my shame!
    /// </summary>
    public class FlawedFrequencyTransmissionSimd
    {
        private const int AsciiNumberOffset = 48;

        public sbyte[] Execute(string signal, string pattern, int digitsToSkip = 1)
        {
            var inputSignal = signal.Select(c => (sbyte)(c - AsciiNumberOffset)).ToArray();
            return Execute(inputSignal, pattern, digitsToSkip);
        }

        public sbyte[] Execute(sbyte[] signal, string pattern, int digitsToSkip = 1)
        {
            var inputPattern = pattern.Split(',').Select(x => sbyte.Parse(x, AllowLeadingSign | AllowTrailingWhite | AllowLeadingWhite)).ToArray();

            var res = Calculate(signal, inputPattern, digitsToSkip);

            return res;
        }

        public sbyte[] ExecutePhases(string signal, string pattern, int iterations, Func<sbyte[], int> digitsToSkip = null)
        {
            if (digitsToSkip == null) digitsToSkip = _ => 1;
            return Enumerable.Repeat(1, iterations)
                .Aggregate(signal.Select(c => (sbyte)(c - AsciiNumberOffset)).ToArray(), (acc, _) =>
                   Execute(acc, pattern, digitsToSkip(acc)));
        }

        public sbyte[] ExecutePhases(sbyte[] input, string pattern, int iterations, Func<sbyte[], int> digitsToSkip = null)
        {
            if (digitsToSkip == null) digitsToSkip = _ => 1;

            var inputPattern = pattern.Split(',').Select(x => sbyte.Parse(x, AllowLeadingSign | AllowTrailingWhite | AllowLeadingWhite)).ToArray();

            sbyte[] output = new sbyte[input.Length];

            for (int i = 0; i < iterations; i++)
            {
                Console.Write("starting run ");
                Console.WriteLine(i);
                Calculate(input, inputPattern, digitsToSkip(output), output);
                (output, input) = (input, output);

            }

            return output;
        }


        // same function as the old one above, but uses iterators, no arrays, and no linq
        private sbyte[] Calculate(sbyte[] inputSignal, sbyte[] pattern, int digitsToSkip, sbyte[] output = null)
        {
            output ??= new sbyte[inputSignal.Length];

            //for (int i = 0; i < inputSignal.Length; i++)
            Parallel.For(0, inputSignal.Length, i =>
            {
                var iterationPattern = new RepeatingDigits(pattern, i + 1);

                //int sum = 0;
                //for (int j = 0; j < inputSignal.Length; j++)
                //{
                //    var a = inputSignal[j];
                //    var b = iterationPattern[j + digitsToSkip];
                //    sum += a * b;
                //}
                //var onesDigita = (sbyte)new IndexableNumber(sum)[0]; // only keep the position in the 1s place



                // simd

                var offset = Vector<sbyte>.Count;
                var muls = new sbyte[offset];
                var sumd = Vector<sbyte>.Zero;
                int k = 0;
                if (offset < inputSignal.Length)
                {
                    for (; k < inputSignal.Length - offset; k += offset)
                    {
                        // populate the muls
                        for (int m = 0; m < offset; m++)
                        {
                            muls[m] = iterationPattern[k + m + digitsToSkip];
                        }

                        var v1 = new Vector<sbyte>(inputSignal, k);
                        var v2 = new Vector<sbyte>(muls);

                        sumd += v1 * v2;
                    }
                }

                int total = 0;
                for (int t = 0; t < offset; t++)
                {
                    total += sumd[t];
                }

                for (; k < inputSignal.Length; k++)
                {
                    total += (inputSignal[k] * iterationPattern[k + digitsToSkip]);
                }

                var onesDigit = (sbyte)new IndexableNumber(total)[0]; // only keep the position in the 1s place

                output[i] = onesDigit;

                //Debug.Assert(onesDigita == onesDigit);
            }
            );

            //Console.WriteLine();
            //Console.WriteLine(string.Join("", output));

            return output;
        }



        //public byte[] ExecutePhases_Old(byte[] input, string pattern, int iterations, Func<byte[], int> digitsToSkip = null)
        //{
        //    if (digitsToSkip == null) digitsToSkip = _ => 1;

        //    var inputPattern = pattern.Split(',').Select(x => int.Parse(x, AllowLeadingSign | AllowTrailingWhite | AllowLeadingWhite)).ToArray();

        //    byte[] output = new byte[input.Length];

        //    for (int i = 0; i < iterations; i++)
        //    {
        //        Console.Write("starting run ");
        //        Console.WriteLine(i);
        //        Calculate_Old(input, inputPattern, digitsToSkip(output), output);
        //        (output, input) = (input, output);

        //    }

        //    return output;
        //}

        // old version left for posterity. Was replaced b/c challenge 2 uses WAY too much memory.
        //private void Calculate(RepeatedString inputSignal, int[] pattern, int digitsToSkip, byte[] output)
        //{
        //    output ??= new byte[inputSignal.Length];

        //    Parallel.For(0, inputSignal.Length, i =>
        //    {
        //        var iterationPattern = pattern.SelectMany(p =>
        //            Enumerable.Repeat(p, i + 1))
        //            .ToArray();

        //        var multiplicationPattern = MoreEnumerable.Generate(
        //                iterationPattern.ToList(),
        //                acc =>
        //                {
        //                    acc.AddRange(iterationPattern);
        //                    return acc;
        //                })
        //            .SkipWhile(l => l.Count < inputSignal.Length + 1)
        //            .First()
        //            .Skip(digitsToSkip)
        //            .Take(inputSignal.Length)
        //            .ToArray();

        //        var sum =
        //            Enumerable.Repeat(inputSignal.InternalString, inputSignal.Repeats)
        //                .SelectMany(s => s.Zip(multiplicationPattern, (a, b) => (a, b))
        //            )
        //            .Sum(x => x.a * x.b);

        //        var onesDigit = (byte)new IndexableNumber(sum)[0]; // only keep the position in the 1s place

        //        output[i] = onesDigit;
        //    });


        //    //var result = inputSignal.Select((_, i) =>
        //    //{
        //    //    // the iteration pattern is the pattern argument's elements repeated
        //    //    // the by the iteration number we are on.
        //    //    // if i == 2, then each element is repeated in place 2 times.
        //    //    var iterationPattern = pattern.SelectMany(p => 
        //    //        Enumerable.Repeat(p, i + 1))
        //    //        .ToArray();

        //    //    var multiplicationPattern = MoreEnumerable.Generate(
        //    //            iterationPattern.ToList(), 
        //    //            acc =>
        //    //            {
        //    //                acc.AddRange(iterationPattern);
        //    //                return acc;
        //    //            })
        //    //        .SkipWhile(l => l.Count < inputSignal.Length + 1)
        //    //        .First()
        //    //        .Skip(digitsToSkip)
        //    //        .Take(inputSignal.Length)
        //    //        .ToArray();

        //    //    var output =
        //    //        inputSignal.Zip(multiplicationPattern, (a, b) => (a, b))
        //    //        .Sum(x => x.a * x.b);

        //    //    var onesDigit = new IndexableNumber(output)[0]; // only keep the position in the 1s place

        //    //    return onesDigit;
        //    //}).ToArray();

        //    //return result;
        //}


        //public byte[] Calculate_Old(byte[] inputSignal, int[] pattern, int digitsToSkip, byte[] output = null)
        //{
        //    output ??= new byte[inputSignal.Length];

        //    //for (int i = 0; i < inputSignal.Length; i++)
        //    Parallel.For(0, inputSignal.Length, i =>
        //    {
        //        var iterationPattern = new RepeatingDigits(pattern, i + 1);

        //        var tmpList = new List<int>(iterationPattern.Length);
        //        for (int j = 0; j < iterationPattern.Length; j++)
        //        {
        //            var v = iterationPattern[j];
        //            tmpList.Add(v);
        //        }

        //        var multiplicationPattern = MoreEnumerable.Generate(
        //                tmpList,
        //                acc =>
        //                {
        //                    for (int i = 0; i < iterationPattern.Length; i++)
        //                    {
        //                        acc.Add(iterationPattern[i]);
        //                    }
        //                    return acc;
        //                })
        //            .SkipWhile(l => l.Count < inputSignal.Length + 1)
        //            .First()
        //            .Skip(digitsToSkip)
        //            .Take(inputSignal.Length);

        //        int sum = 0; int k = 0;
        //        foreach (var b in multiplicationPattern)
        //        {
        //            var a = inputSignal[k];
        //            sum += a * b;
        //            k++;
        //        }

        //        var onesDigit = (byte)new IndexableNumber(sum)[0]; // only keep the position in the 1s place

        //        output[i] = onesDigit;
        //    }
        //    );

        //    return output;
        //}
    }
}
