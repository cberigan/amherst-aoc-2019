using System;

namespace AdventOfCodeChallenges.C5
{

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

}
