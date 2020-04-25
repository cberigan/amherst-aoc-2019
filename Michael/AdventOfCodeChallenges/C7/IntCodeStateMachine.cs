using AdventOfCodeChallenges.C5;
using static AdventOfCodeChallenges.C5.OpCode;

namespace AdventOfCodeChallenges.C7
{
    public class IntCodeStateMachine
    {
        private int nextInput;
        private bool _halted;

        public Instruction Instruction { get; private set; }
        public bool IsHalted => Instruction.Op == OpCode.Halt || _halted;
        public int? Phase { get; set; }
        public int NextWriteInstruction { get; set; }
        public int[] Memory { get; set; }
        public int CurrentOffset { get; private set; }
        public int CurrentOutputValue { get; private set; }
        public bool IsAtEnd => CurrentOffset >= Memory.Length;

        public IntCodeStateMachine(int[] memory, int phase)
        {
            Memory = memory;
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

        public void Halt() => CurrentOffset = Memory.Length;

        private Instruction Read() => new Instruction(Memory[CurrentOffset]);

        private Arguments Decode()
        {
            var len = OpCodeArgLength(Instruction.Op);
            return new Arguments(Memory, Instruction, len, CurrentOffset + 1, ReadValue);

            static int ReadValue(int index, int[] memory, ParameterModeEnum mode)
            {
                return mode == ParameterModeEnum.ImmediateMode
                    ? memory[index]
                    : memory[memory[index]];
            }
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
                    CurrentOffset += OpCodeArgLength(ins.Op) + 1;
                    break;
                case OpCode.Read:
                    CurrentOutputValue = args.Arg1;
                    CurrentOffset += OpCodeArgLength(ins.Op) + 1;
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
                _ => 1 // halt. since I'm no longer jumping to the end of the memory, this might be a bug now
            };
        }
    }
}
