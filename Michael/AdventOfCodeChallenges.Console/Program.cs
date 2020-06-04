using System.Linq;
using static System.Console;

namespace AdventOfCodeChallenges.Console
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var result = new C11.Challenge.Pt2().Run();

            WriteLine(result);
            ReadLine();
        }
    }
}
