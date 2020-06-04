using System.Linq;
using static System.Console;

namespace AdventOfCodeChallenges.Console
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var result = new C10.Challenge.Pt2().Run(C10.Challenge.Input);

            WriteLine(result);
            ReadLine();
        }
    }
}
