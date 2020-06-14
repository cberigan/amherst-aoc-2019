using System.Linq;
using static System.Console;

namespace AdventOfCodeChallenges.Console
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var result = new C12.Challenge.Pt2().RunUntilVelocitiesAreZeroAndDoubleIters();

            WriteLine(result);
            ReadLine();
        }
    }
}
