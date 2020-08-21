using AdventOfCodeChallenges.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Globalization.NumberStyles;


namespace AdventOfCodeChallenges.C16
{
    public class FlawedFrequencyTransmission
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
            
            Console.Write("starting run ");

            for (int i = 0; i < iterations; i++)
            {
                
                Console.SetCursorPosition(13, 0);
                Console.Write(i);
                
                Calculate(input, inputPattern, digitsToSkip(output), output);
                (output, input) = (input, output);
            }

            return output;
        }

        private sbyte[] Calculate(sbyte[] inputSignal, sbyte[] pattern, int digitsToSkip, sbyte[] output = null)
        {
            output ??= new sbyte[inputSignal.Length];

            //for (int i = 0; i < inputSignal.Length; i++)
            Parallel.For(0, inputSignal.Length, i =>
            {
                var iterationPattern = new RepeatingDigits(pattern, i + 1);

                int sum = 0;
                for (int j = 0; j < inputSignal.Length; j++)
                {
                    var a = inputSignal[j];
                    var b = iterationPattern[j + digitsToSkip];
                    sum += a * b;
                }
                var onesDigit = (sbyte)new IndexableNumber(sum)[0]; // only keep the position in the 1s place
                output[i] = onesDigit;
            }
            );

            //Console.WriteLine();
            //Console.WriteLine(string.Join("", output));

            return output;
        }
    }
}
