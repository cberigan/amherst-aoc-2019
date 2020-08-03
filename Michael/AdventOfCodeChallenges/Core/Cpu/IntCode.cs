using System;
using System.Collections.Generic;
using static AdventOfCodeChallenges.Core.Cpu.OpCode;

namespace AdventOfCodeChallenges.Core.Cpu
{
    public class IntCodeStateMachine
    {
        private int nextInput;
        private bool _halted;

        public Instruction Instruction { get; private set; }
        public bool IsHalted => Instruction.Op == OpCode.Halt || _halted;
        public int? Phase { get; set; }
        public int NextWriteInstruction { get; set; }
        public List<int> Memory { get; set; }
        public int CurrentOffset { get; private set; }
        public int CurrentOutputValue { get; private set; }
        public bool IsAtEnd => CurrentOffset >= Memory.Count;
        public int RelativeBaseOffset = 0;

        public event EventHandler<long> OnOutput;

        public IntCodeStateMachine(IEnumerable<int> memory, int? phase = null)
        {
            Memory = new List<int>(memory);
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
                Write => 1,
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
