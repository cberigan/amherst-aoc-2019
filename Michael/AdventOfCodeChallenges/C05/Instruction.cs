namespace AdventOfCodeChallenges.C5
{

    public readonly struct Instruction
    {
        private readonly int v;

        public Instruction(int v) => this.v = v;

        public OpCode Op => (OpCode)(v % 100);
        public ParameterModeEnum Arg1Mode => (ParameterModeEnum)(v / 100 % 10);
        public ParameterModeEnum Arg2Mode => (ParameterModeEnum)(v / 1000 % 10);
        public ParameterModeEnum Arg3Mode => (ParameterModeEnum)(v / 10000 % 10);
    }

}
