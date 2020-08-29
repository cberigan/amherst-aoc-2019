using AdventOfCodeChallenges.Core.Collections;
using System;
using System.Collections.Generic;

namespace AdventOfCodeChallenges.Core.Cpu
{

    readonly struct Arguments
    {
        public delegate int ReadValueDelegate(long index, List<long> memory, ParameterModeEnum mode);

        public int Arg1 { get; }
        public int Arg2 { get; }
        public int Arg3 { get; }
               
        public int Arg1Index { get; }
        public int Arg2Index => Arg1Index + 1;
        public int Arg3Index => Arg1Index + 2;
               
        public int TargetIndex { get; }

        public Arguments(BigList<int> memory, Instruction ins, int opCodeArgLength, int argStartIndex, int relativeBaseOffset)
        {
            this.Arg1Index = argStartIndex;
            if (ins.Op == OpCode.Halt)
            {
                Arg1 = Arg2 = Arg3 = TargetIndex = 0;
                return;
            }

            Arg1 = ReadValue(argStartIndex, relativeBaseOffset, memory, ins.Arg1Mode);
            Arg2 = opCodeArgLength > 1 ? ReadValue(argStartIndex + 1, relativeBaseOffset, memory, ins.Arg2Mode) : int.MinValue;
            Arg3 = opCodeArgLength > 2 ? ReadValue(argStartIndex + 2, relativeBaseOffset, memory, ins.Arg3Mode) : int.MinValue;

            var targetArgMode = opCodeArgLength switch
            {
                0 => ins.Arg1Mode,
                1 => ins.Arg1Mode,
                2 => ins.Arg2Mode,
                3 => ins.Arg3Mode,
                _ => throw new ArgumentOutOfRangeException()
            };

            TargetIndex = targetArgMode switch
            {
                ParameterModeEnum.ImmediateMode => argStartIndex + opCodeArgLength - 1,
                ParameterModeEnum.PositionMode => memory[argStartIndex + opCodeArgLength - 1],
                ParameterModeEnum.RelativeMode => relativeBaseOffset + memory[argStartIndex + Math.Max(opCodeArgLength, 1) - 1],
                _ => throw new NotImplementedException()
            };
        }

        private static int ReadValue(int index, int relativeBaseOffset, BigList<int> memory, ParameterModeEnum mode)
        {
            return mode switch
            {
                ParameterModeEnum.ImmediateMode => memory[index],
                ParameterModeEnum.PositionMode => memory[memory[index]],
                ParameterModeEnum.RelativeMode => memory[relativeBaseOffset + memory[index]],
                _ => throw new NotImplementedException()
            };
        }
    }
}
