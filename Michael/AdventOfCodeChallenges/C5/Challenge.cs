using AdventOfCodeChallenges.Core;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace AdventOfCodeChallenges.C5
{
    public class Challenge
    {
        private static readonly int[] RawInputs = "3,225,1,225,6,6,1100,1,238,225,104,0,1001,92,74,224,1001,224,-85,224,4,224,1002,223,8,223,101,1,224,224,1,223,224,223,1101,14,63,225,102,19,83,224,101,-760,224,224,4,224,102,8,223,223,101,2,224,224,1,224,223,223,1101,21,23,224,1001,224,-44,224,4,224,102,8,223,223,101,6,224,224,1,223,224,223,1102,40,16,225,1102,6,15,225,1101,84,11,225,1102,22,25,225,2,35,96,224,1001,224,-350,224,4,224,102,8,223,223,101,6,224,224,1,223,224,223,1101,56,43,225,101,11,192,224,1001,224,-37,224,4,224,102,8,223,223,1001,224,4,224,1,223,224,223,1002,122,61,224,1001,224,-2623,224,4,224,1002,223,8,223,101,7,224,224,1,223,224,223,1,195,87,224,1001,224,-12,224,4,224,1002,223,8,223,101,5,224,224,1,223,224,223,1101,75,26,225,1101,6,20,225,1102,26,60,224,101,-1560,224,224,4,224,102,8,223,223,101,3,224,224,1,223,224,223,4,223,99,0,0,0,677,0,0,0,0,0,0,0,0,0,0,0,1105,0,99999,1105,227,247,1105,1,99999,1005,227,99999,1005,0,256,1105,1,99999,1106,227,99999,1106,0,265,1105,1,99999,1006,0,99999,1006,227,274,1105,1,99999,1105,1,280,1105,1,99999,1,225,225,225,1101,294,0,0,105,1,0,1105,1,99999,1106,0,300,1105,1,99999,1,225,225,225,1101,314,0,0,106,0,0,1105,1,99999,108,677,226,224,102,2,223,223,1006,224,329,1001,223,1,223,1108,226,677,224,1002,223,2,223,1006,224,344,101,1,223,223,7,226,677,224,102,2,223,223,1006,224,359,1001,223,1,223,1007,226,677,224,1002,223,2,223,1006,224,374,1001,223,1,223,1108,677,226,224,102,2,223,223,1005,224,389,1001,223,1,223,107,226,226,224,102,2,223,223,1006,224,404,101,1,223,223,1107,226,226,224,1002,223,2,223,1005,224,419,1001,223,1,223,1007,677,677,224,102,2,223,223,1006,224,434,101,1,223,223,1107,226,677,224,1002,223,2,223,1006,224,449,101,1,223,223,107,677,677,224,102,2,223,223,1005,224,464,1001,223,1,223,1008,226,226,224,1002,223,2,223,1005,224,479,101,1,223,223,1007,226,226,224,102,2,223,223,1005,224,494,1001,223,1,223,8,677,226,224,1002,223,2,223,1005,224,509,1001,223,1,223,108,677,677,224,1002,223,2,223,1005,224,524,1001,223,1,223,1008,677,677,224,102,2,223,223,1006,224,539,1001,223,1,223,7,677,226,224,1002,223,2,223,1005,224,554,101,1,223,223,1108,226,226,224,1002,223,2,223,1005,224,569,101,1,223,223,107,677,226,224,102,2,223,223,1005,224,584,101,1,223,223,8,226,226,224,1002,223,2,223,1005,224,599,101,1,223,223,108,226,226,224,1002,223,2,223,1006,224,614,1001,223,1,223,7,226,226,224,102,2,223,223,1006,224,629,1001,223,1,223,1107,677,226,224,102,2,223,223,1005,224,644,101,1,223,223,8,226,677,224,102,2,223,223,1006,224,659,1001,223,1,223,1008,226,677,224,1002,223,2,223,1006,224,674,1001,223,1,223,4,223,99,226"
            .Split(",")
            .Select(int.Parse)
            .ToArray();

        public int Run(int[] inputs = null, Action<(int val, int[] memory, int outputVal)> test = null)
        {
            var arg = inputs ?? RawInputs;

            var r = new IntCode2(5).Run(arg);
            Console.WriteLine("output write val: {0}", r.outputVal);
            test?.Invoke(r);
            return r.val;
        }

        struct IntCode2
        {
            private readonly int _currentInputVal;
            private int _currentOutputVal;

            public IntCode2(int currentInputVal)
            {
                _currentInputVal = currentInputVal;
                _currentOutputVal = 0;
            }

            public (int val, int[] memory, int outputVal) Run(int[] arg)
            {
                var memory = new int[arg.Length];
                Array.Copy(arg, memory, arg.Length);
                for (int i = 0; i < memory.Length;)
                {
                    Instruction ins = Read(memory, i);
                    Arguments args = Decode(ins, memory, i);
                    Execute(ins, args, memory, ref i);
                }

                return (memory[0], memory, _currentOutputVal);
            }

            private Instruction Read(int[] memory, int i) => new Instruction(memory[i]);

            private Arguments Decode(Instruction ins, int[] memory, int i)
            {
                var len = OpCodeArgLength(ins.Op);
                return new Arguments(memory, ins, len, i + 1, ReadValue);

                static int ReadValue(int index, int[] memory, ParameterModeEnum mode)
                {
                    return mode == ParameterModeEnum.ImmediateMode
                        ? memory[index]
                        : memory[memory[index]];
                }
            }

            private void Execute(Instruction ins, Arguments args, int[] memory, ref int i)
            {
                switch (ins.Op)
                {
                    case OpCode.Add:
                        memory[args.TargetIndex] = args.Arg1 + args.Arg2;
                        i += OpCodeArgLength(ins.Op) + 1;
                        break;
                    case OpCode.Mul:
                        memory[args.TargetIndex] = args.Arg1 * args.Arg2;
                        i += OpCodeArgLength(ins.Op) + 1;
                        break;
                    case OpCode.Write:
                        memory[args.TargetIndex] = _currentInputVal;
                        i += OpCodeArgLength(ins.Op) + 1;
                        break;
                    case OpCode.Read:
                        _currentOutputVal = args.Arg1;
                        //if (_currentOutputVal != 0)
                        //    Debugger.Break();
                        i += OpCodeArgLength(ins.Op) + 1;
                        break;
                    case OpCode.JumpIfTrue:
                        if (args.Arg1 != 0)
                            i = args.Arg2;
                        else
                            i += OpCodeArgLength(ins.Op) + 1;
                        break;
                    case OpCode.JumpIfFalse:
                        if (args.Arg1 == 0)
                            i = args.Arg2;
                        else i += OpCodeArgLength(ins.Op) + 1;
                        break;
                    case OpCode.LT:
                        memory[args.TargetIndex] = args.Arg1 < args.Arg2 ? 1 : 0;
                        i += OpCodeArgLength(ins.Op) + 1;
                        break;
                    case OpCode.E:
                        memory[args.TargetIndex] = args.Arg1 == args.Arg2 ? 1 : 0;
                        i += OpCodeArgLength(ins.Op) + 1;
                        break;
                    case OpCode.Halt:
                        i = memory.Length;
                        break;

                }
            }
        }

        readonly struct Instruction
        {
            private readonly int v;

            public Instruction(int v) => this.v = v;

            public OpCode Op => (OpCode)(v % 100);
            public ParameterModeEnum Arg1Mode => (ParameterModeEnum)(v / 100 % 10);
            public ParameterModeEnum Arg2Mode => (ParameterModeEnum)(v / 1000 % 10);
            public ParameterModeEnum Arg3Mode => (ParameterModeEnum)(v / 10000 % 10);
        }

        readonly struct Arguments
        {
            private readonly int argStartIndex;

            public int Arg1 { get; }
            public int Arg2 { get; }
            public int Arg3 { get; }

            public int Arg1Index => argStartIndex;
            public int Arg2Index => argStartIndex + 1;
            public int Arg3Index => argStartIndex + 2;

            public int TargetIndex { get; }

            public Arguments(int[] memory, Instruction ins, int opCodeArgLength, int argStartIndex, 
                Func<int, int[], ParameterModeEnum, int> readValue)
            {
                this.argStartIndex = argStartIndex;
                if (ins.Op == OpCode.Halt)
                {
                    Arg1 = Arg2 = Arg3 = TargetIndex = 0;
                    return;
                }

                Arg1 = readValue(argStartIndex, memory, ins.Arg1Mode);
                Arg2 = opCodeArgLength > 1 ? readValue(argStartIndex + 1, memory, ins.Arg2Mode) : int.MinValue;
                Arg3 = opCodeArgLength > 2 ? readValue(argStartIndex + 2, memory, ins.Arg3Mode) : int.MinValue;
                TargetIndex = ins.Arg3Mode == ParameterModeEnum.ImmediateMode
                    ? argStartIndex + opCodeArgLength - 1
                    : memory[argStartIndex + opCodeArgLength - 1];
            }
        }

        internal enum ParameterModeEnum { 
            PositionMode = 0,
            ImmediateMode = 1
        }

        enum OpCode
        {
            Add = 1,
            Mul = 2,
            Write = 3,
            Read = 4,
            JumpIfTrue = 5,
            JumpIfFalse = 6,
            LT = 7,
            E = 8,
            Halt = 99
        }

        static int OpCodeArgLength(OpCode op)
        {
            return op switch
            {
                OpCode.Add => 3,
                OpCode.Mul => 3,
                OpCode.Write => 1,
                OpCode.Read => 1,
                OpCode.Halt => 1,
                OpCode.JumpIfTrue => 2,
                OpCode.JumpIfFalse =>2,
                OpCode.LT => 3,
                OpCode.E => 3
            };
        }
    }
}
