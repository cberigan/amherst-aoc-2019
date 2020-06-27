using AdventOfCodeChallenges.C5;
using System;

namespace AdventOfCodeChallenges.C7
{
    public class Amplifier
    {
        private Amplifier _linkedTo;
        protected readonly IntCodeStateMachine _cpu;

        public string Name { get; }
        public bool IsFinalStage { get; internal set; }
        public event EventHandler<int> OnOutput;

        public Amplifier(string name, int[] operatingMemory, Func<int> getPhaseSetting)
        {
            Name = name;
            _cpu = new IntCodeStateMachine( operatingMemory, getPhaseSetting());
        }

        public void Post(int input)
        {
            _cpu.SetInput(input);

            bool ran = false;
            while (!_cpu.IsHalted)
            {
                ran = true;
                _cpu.RunOnce();
                if (_cpu.Instruction.Op == OpCode.Read)
                {
                    OnOutput?.Invoke(this, _cpu.CurrentOutputValue);
                    break;
                }
            }

            if (ShouldPassMessageForward(ran)) // not a terminated E
                _linkedTo?.Post(_cpu.CurrentOutputValue);
            else if (_cpu.IsHalted)
                OnOutput?.Invoke(this, _cpu.CurrentOutputValue);
        }

        private bool ShouldPassMessageForward(bool ran) => (!ran && !IsFinalStage) || ran;

        public void Linkto(Amplifier target) => _linkedTo = target;
    }
}
