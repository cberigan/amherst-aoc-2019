using System;
namespace AdventOfCodeChallenges.Core.Input
{
    public interface ICommandInput
    {
        event EventHandler<int> InputReceived;

        void AwaitInput();
    }
}
