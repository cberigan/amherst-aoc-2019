using AdventOfCodeChallenges.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Globalization.NumberStyles;

namespace AdventOfCodeChallenges.C16
{
    public class FftStateMachine
    {
        public sbyte[] Execute(string signal, int iterations)
        {
            var inputSignal = signal.Select(c => (sbyte)(c - 48)).ToArray();
            return Execute(inputSignal, iterations);
        }

        public sbyte[] Execute(sbyte[] signal, int iterations)
        {
            sbyte[] output = new sbyte[signal.Length];
            sbyte[] last = null;
            Console.Write("starting run ");

            for (int i = 0; i < iterations; i++)
            {
                Console.SetCursorPosition(13, 0);
                Console.Write(i);

                for (int iter = 0; iter < signal.Length; iter++)
                //Parallel.For(0, signal.Length, iter =>
                {
                    last = Calculate(signal, iter, output);

                }
                //);
                (signal, output) = (output, signal);
            }
            

            return last;
        }

        // benchmark .net says 53ms, compared to 76ms for default
        // changing the state machine to jump to the next non-zero state (skipping ahead by the repeat count)
        // brings the time down to 39ms
        private sbyte[] Calculate(sbyte[] inputSignal, int iteration, sbyte[] output)
        {
            var state = new IteratorStateMachine(inputSignal, iteration);
            int x = 0;
            state.Next(ref x); // skip the first one
            int sum = 0;
            for (int i = 0; i < inputSignal.Length; i++)
            {
                sum += state.Next(ref i);
            }
            output[iteration] = (sbyte)new IndexableNumber(sum)[0];
            return output;
        }

        struct IteratorStateMachine
        {
            private readonly sbyte[] _arr;
            private readonly int _repeatCount;
            private int _count;
            private int _state;// 0,2 = 0, 1 = add, 3 = sub

            public IteratorStateMachine(sbyte[] arr, int repeatCount)
            {
                _arr = arr;
                _count = _repeatCount = repeatCount;
                _state = 0;
            }

            public sbyte Next(ref int index)
            {
                
                sbyte res;

                switch (_state)
                {
                    case 0:
                    case 2:
                        res = 0;
                        break;
                    case 1:
                        res = _arr[index];
                        break;
                    case 3:
                        res = (sbyte)-_arr[index];
                        break;
                    default:
                        res = 0;
                        break;
                }

                switch (_count)
                {
                    case 0:
                        _count = _repeatCount;
                        switch (_state)
                        {
                            case 1:
                            case 3:
                                _state = (_state + 2) % 4; // skip ahead to the next valid state, assuming that non-zero has been done correctly.
                                index += _repeatCount + 1;
                                break;
                            default: 
                                _state = (_state + 1) % 4; // skip ahead to the next valid state, assuming that non-zero has been done correctly.

                                break;
                        }
                        break;
                    default:
                        _count--;
                        break;
                }

                return res;
            }
        }
    }
}
