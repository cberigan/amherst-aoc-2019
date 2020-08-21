using System.Diagnostics;
using static System.Console;

namespace AdventOfCodeChallenges.Console
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var sw = Stopwatch.StartNew();
            var result = new C16.Challenge.Pt2().Stolen();
            sw.Stop();

            WriteLine(result);
            WriteLine("finished in {0}", sw.ElapsedMilliseconds);
            ReadLine();
        }
    }
}
