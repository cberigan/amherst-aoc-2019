using AdventOfCodeChallenges.C5;
using static AdventOfCodeChallenges.C5.OpCode;
using System;
using AdventOfCodeChallenges.Core.Collections;
using System.Numerics;
using System.Collections.Generic;

namespace AdventOfCodeChallenges.C9
{
    public class IntCodeStateMachine
    {
        private int nextInput;
        private bool _halted;

        public Instruction Instruction { get; private set; }
        public bool IsHalted => Instruction.Op == OpCode.Halt || _halted;
        public BigInteger? Phase { get; set; }
        public BigInteger NextWriteInstruction { get; set; }
        public BigList<BigInteger> Memory { get; set; }
        public BigInteger CurrentOffset { get; private set; }
        public BigInteger CurrentOutputValue { get; private set; }
        public bool IsAtEnd => CurrentOffset >= Memory.Count;
        public BigInteger RelativeBaseOffset = 0;

        public event EventHandler<BigInteger> OnOutput;

        public IntCodeStateMachine(IEnumerable<BigInteger> memory, int? phase = null)
        {
            Memory = new BigList<BigInteger>(memory);
            Phase = phase;
        }

        public void SetInput(int i) => nextInput = i;

        public void Reset()
        {
            CurrentOffset = 0;
            _halted = true;
        }

        public void RunOnce()
        {
            if (IsHalted) return;
            Instruction = Read();
            Arguments args = Decode();
            Execute(args);
        }

        public void RunAll()
        {
            while (!IsHalted)
                RunOnce();
        }

        public void Halt() => CurrentOffset = Memory.Count;

        private Instruction Read() => new Instruction(Memory[CurrentOffset]);

        private Arguments Decode()
        {
            var len = OpCodeArgLength(Instruction.Op);
            return new Arguments(Memory, Instruction, len, CurrentOffset + 1, RelativeBaseOffset);
        }

        private void Execute(Arguments args)
        {
            var memory = Memory;
            var ins = Instruction;

            switch (Instruction.Op)
            {
                case Add:
                    memory[args.TargetIndex] = args.Arg1 + args.Arg2;
                    CurrentOffset += OpCodeArgLength(ins.Op) + 1;
                    break;
                case Mul:
                    memory[args.TargetIndex] = args.Arg1 * args.Arg2;
                    CurrentOffset += OpCodeArgLength(ins.Op) + 1;
                    break;
                case Write:
                    memory[args.TargetIndex] = Phase.GetValueOrDefault(nextInput);
                    Phase = null;
                    CurrentOffset += 2;
                    break;
                case OpCode.Read:
                    CurrentOutputValue = args.Arg1;
                    CurrentOffset += OpCodeArgLength(ins.Op) + 1;
                    OnOutput?.Invoke(this, CurrentOutputValue);
                    break;
                case JumpIfTrue:
                    if (args.Arg1 != 0)
                        CurrentOffset = args.Arg2;
                    else
                        CurrentOffset += OpCodeArgLength(ins.Op) + 1;
                    break;
                case JumpIfFalse:
                    if (args.Arg1 == 0)
                        CurrentOffset = args.Arg2;
                    else CurrentOffset += OpCodeArgLength(ins.Op) + 1;
                    break;
                case LT:
                    memory[args.TargetIndex] = args.Arg1 < args.Arg2 ? 1 : 0;
                    CurrentOffset += OpCodeArgLength(ins.Op) + 1;
                    break;
                case E:
                    memory[args.TargetIndex] = args.Arg1 == args.Arg2 ? 1 : 0;
                    CurrentOffset += OpCodeArgLength(ins.Op) + 1;
                    break;
                case AdjustRelativeBase:
                    RelativeBaseOffset += args.Arg1;
                    CurrentOffset += OpCodeArgLength(ins.Op) + 1;
                    break;
                case OpCode.Halt:
                    break;
            }
        }


        static int OpCodeArgLength(OpCode op)
        {
            return op switch
            {
                Add => 3,
                Mul => 3,
                Write => 0,
                OpCode.Read => 1,
                OpCode.Halt => 1,
                JumpIfTrue => 2,
                JumpIfFalse => 2,
                LT => 3,
                E => 3,
                AdjustRelativeBase => 1,
                _ => 1 // halt. since I'm no longer jumping to the end of the memory, this might be a bug now
            };
        }
    }
}
