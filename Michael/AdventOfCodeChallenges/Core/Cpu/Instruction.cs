namespace AdventOfCodeChallenges.Core.Cpu
{
    public readonly struct Instruction
    {
        private readonly long v;

        public Instruction(long v) => this.v = v;

        public OpCode Op => (OpCode)(int)(v % 100);
        public ParameterModeEnum Arg1Mode => (ParameterModeEnum)(int)(v / 100 % 10);
        public ParameterModeEnum Arg2Mode => (ParameterModeEnum)(int)(v / 1000 % 10);
        public ParameterModeEnum Arg3Mode => (ParameterModeEnum)(int)(v / 10000 % 10);
    }
}
