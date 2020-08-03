using AdventOfCodeChallenges.Core.Input;
using System;

namespace AdventOfCodeChallenges.C15
{
    class ConsoleInput : ICommandInput
    {
        public event EventHandler<int> InputReceived;

        public void AwaitInput()
        {
            var input = Console.ReadKey(true).KeyChar;
            var val = input - 48;
            InputReceived?.Invoke(null, val);
        }
    }
}
