using System;
using System.Threading;

namespace AdventOfCodeChallenges.C13.Inputs
{
    public class KeypadJoystickInput : IGameInput
    {
        public event EventHandler<int> OnInput;

        //public void PollInput() // run without waiting on the user. Resulted in sometimes inputs not being received.
        //{
        //    Console.SetCursorPosition(20, 0);
        //    Console.Write("Input:  ");

        //    if (!Console.KeyAvailable)
        //    {
        //        OnInput?.Invoke(this, 0);
        //        return;
        //    }

        //    var input = Console.ReadKey();
        //    var ret = input.Key switch
        //    {
        //        ConsoleKey.LeftArrow => -1,
        //        ConsoleKey.RightArrow => 1,
        //        _ => 0
        //    };

        //    Console.Write(ret);

        //    OnInput.Invoke(this, ret);

        //}

        public void PollInput() // wait for input
        {
            Thread.Sleep(300);
            Console.SetCursorPosition(20, 0);
            Console.Write("awaiting input");

            var input = Console.ReadKey();
            var ret = input.Key switch
            {
                ConsoleKey.LeftArrow => -1,
                ConsoleKey.RightArrow => 1,
                _ => 0
            };
            Console.SetCursorPosition(20, 0);
            Console.Write("                    ");
            Console.SetCursorPosition(20, 0);
            Console.Write(ret);

            OnInput.Invoke(this, ret);
        }
    }
}
