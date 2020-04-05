using AdventOfCodeChallenges.C4;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;

namespace AdventOfCodeChallenges.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            BenchmarkRunner.Run<Challenge4Benchmark>();
            Console.ReadLine();
        }
    }

    [KeepBenchmarkFiles]
    public class Challenge4Benchmark
    {
        private readonly Challenge.Pt2 _original;
        private readonly Challenge.Iter2 _updated;

        public Challenge4Benchmark()
        {
            _original = new C4.Challenge.Pt2();
            _updated = new C4.Challenge.Iter2();
        }

        [Benchmark]
        public void Original()
        {
            _original.Run();
        }

        [Benchmark]
        public void Updated()
        {
            _updated.Run();
        }
    }
}
