using AdventOfCodeChallenges.Core.Cpu;
using AdventOfCodeChallenges.Core.Input;
using System;

namespace AdventOfCodeChallenges.C15
{
    public class RepairDroid
    {
        private IntCodeStateMachine _cpu;
        private ICommandInput _commandInput;
        private RepairDroidStatusCode _lastOutput;

        public event EventHandler<RepairDroidStatusCode> OnReply;

        public RepairDroid(IntCodeStateMachine cpu, ICommandInput commandInput)
        {
            _cpu = cpu;
            _commandInput = commandInput;
            _commandInput.InputReceived += (_, v) =>
            {
                var cmd = (MovementCommand)v;
                if (v < 1 || v > 4)
                {
                    throw new Exception("Wrong input value");
                }
                _cpu.SetInput((int)cmd);
            };

            _cpu.OnOutput += (_, o) =>
            {
                _lastOutput = (RepairDroidStatusCode)o;
                OnReply?.Invoke(this, _lastOutput);
                commandInput.AwaitInput();
            };
        }

        public void ExecuteInstruction() => _cpu.RunAll();

        public void Stop() => _cpu.Halt();
    }
}
