using System;

namespace AdventOfCodeChallenges.C13.Inputs
{
    public interface IGameInput
    {
        event EventHandler<int> OnInput;
        void PollInput();
    }
}
