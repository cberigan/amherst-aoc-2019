using AdventOfCodeChallenges.Core;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JM.LinqFaster.SIMD;

namespace AdventOfCodeChallenges.C16
{
    public class FlawedFrequencyTransmissionVertical
    {
        const int patternLength = 4;
        const int distance = 2;
        const int one = 1;
        const int neg = 3;

        // try to traverse the diagonal of only the 1 and -1
        public sbyte[] Execute(sbyte[] input, int iters)
        {
            //Console.Write("starting run ");

            // pattern is 0,1,0,-1

            sbyte[] output = new sbyte[input.Length];
            input.CopyTo(output, 0);

            for (int i = 1; i < iters + 1; i++)
            {
                //Console.SetCursorPosition(13, 0);
                //Console.WriteLine(i);
                (input, output) = (output, input);

                input.Select((_, j) => (j, indexes: FindIndexes(j + 1, input)))
                    .AsParallel()
                    .ForAll(x =>
                    {
                        output[x.j] = (sbyte)new IndexableNumber(x.indexes.Sum(y =>
                            {
                                var n = -input[y.i];
                                if (!y.add)
                                    n = -n;
                                return n;
                            }))[0];
                    }
                );
                    

                //for (int j = 0; j < input.Length; j++)
                //{
                //    var indexes = FindIndexes(j + 1, input);

                //    //var sum = indexes.Sum(x => x.add ? input[x.i] : -input[x.i]);
                //    //var lastDigit = (sbyte)new IndexableNumber(sum)[0];
                //    //output[j] = lastDigit;
                //}


            }

            return output;
        }

        public sbyte[] Execute(string input, string pattern, int iters)
        {
            var inputBytes = Enumerable.Repeat(input, iters)
                    .SelectMany(s => s.Select(c => (sbyte)(c - 48)))
                    .ToArray();

            return Execute(inputBytes, iters);
        }

        IEnumerable<(int i, bool add)> FindIndexes(int iter, sbyte[] arr)
        {
            //var r = new List<(int, bool)>();
            var onePlace = one - 1;
            const int distance = 2;

            bool add = true;
            for (int i = iter; i < arr.Length + 1;)
            {
                // add one
                var jump = distance * iter;
                for (int j = 0; j < iter; j++)
                {
                    if (i + j - 1 < arr.Length)
                        //r.Add((i + j - 1, add));

                        yield return (i + j - 1, add);
                    else break;
                }
                add = !add;
                i += jump;
            }
            //return r;
        }
    }
}
