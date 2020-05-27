using System.Linq;
using static System.Console;

namespace AdventOfCodeChallenges.Console
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var result = new C10.Challenge.Pt1().Run(C10.Challenge.Input);

            foreach (var kvp in result.OrderByDescending(r => r.Key).Take(2))
            {
                WriteLine("count: {0}", kvp.Key);
                foreach (var (asteroid, visibles) in kvp.Value)
                    WriteLine("\tasteroid: ({0}, {1}), count: {2}", asteroid.X, asteroid.Y, visibles.Count());
            }

            WriteLine(result);
            ReadLine();
        }
    }
}
