using System;
using System.Collections.Generic;

namespace AdventOfCodeChallenges.C5
{
    public class IntCode
    {
        private int? _phase;
        private int nextInput;

        private int _currentOutputVal;

        public bool IsHalted { get; private set; }
        public event EventHandler<int> OnOutput;

        public IntCode(int phase)
        {
            _phase = phase;
            _currentOutputVal = 0;
        }


        public void SetInput(int i) {
            nextInput = i;
        }

        public (int val, int[] memory, int outputVal, bool isHalted) Run(int[] arg)
        {
            var memory = new int[arg.Length];
            arg.CopyTo(memory, 0);

            bool isHalted = false;
            for (int i = 0; i < memory.Length;)
            {
                Instruction ins = Read(memory, i);
                Arguments args = Decode(ins, memory, i);
                Execute(ins, args, memory, ref i);
                isHalted = IsHalted = ins.Op == OpCode.Halt;
            }

            return (memory[0], memory, _currentOutputVal, isHalted);
        }

        private int i = 0;
        private int[] _runToReadMemory;
        public (int val, int[] memory, int outputVal, bool isHalted) RunToRead(int[] arg)
        {
            // I really messed this up
            if (_runToReadMemory == null)
            {
                _runToReadMemory = new int[arg.Length];
                arg.CopyTo(_runToReadMemory, 0);
            }
            var memory = _runToReadMemory;
            bool isHalted = false;
            for (; i < memory.Length;)
            {
                Instruction ins = Read(memory, i);
                Arguments args = Decode(ins, memory, i);
                Execute(ins, args, memory, ref i);
                isHalted = IsHalted = ins.Op == OpCode.Halt;
                if (ins.Op == OpCode.Read) break;
            }

            if (IsHalted) i = 0;//nice way to make a jank ass state machine where you didn't want one
            return (memory[0], memory, _currentOutputVal, isHalted);
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

        public static IEnumerable<(OpCode op, int offset)> InstructionIndexes(int[] memory)
        {
            int offset = 0;
            for (int i = 0; i < memory.Length; i++)
            {
                var currentOp = new Instruction(memory[i]).Op;
                yield return (currentOp, offset);
                offset += OpCodeArgLength(currentOp) + 1;
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
                    memory[args.TargetIndex] = _phase.GetValueOrDefault(nextInput);
                    _phase = null;
                    i += OpCodeArgLength(ins.Op) + 1;
                    break;
                case OpCode.Read:
                    _currentOutputVal = args.Arg1;
                    OnOutput?.Invoke(this, _currentOutputVal);
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
                //default:
                //    i = memory.Length;
                //    break;
            }
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
                OpCode.JumpIfFalse => 2,
                OpCode.LT => 3,
                OpCode.E => 3,
                _ => 1 // halt
            };
        }
    }

}
