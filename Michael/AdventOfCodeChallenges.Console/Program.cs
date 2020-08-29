using System.Diagnostics;
using static System.Console;

namespace AdventOfCodeChallenges.Console
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var sw = Stopwatch.StartNew();
            var result = new C17.Challenge.Pt1().Run();
            sw.Stop();

            WriteLine(result);
            WriteLine("finished in {0}", sw.ElapsedMilliseconds);
            ReadLine();
        }
    }
}
