using AdventOfCodeChallenges.C16;
using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCodeChallenges.Benchmark
{
    public class C16Benchmark
    {
        private readonly FlawedFrequencyTransmission _regular;

        private readonly FlawedFrequencyTransmissionSimd _simd;
        private readonly FlawedFrequencyTransmissionVertical _vertical;
        private readonly FftStateMachine _sm;

        public C16Benchmark()
        {
            _regular = new FlawedFrequencyTransmission();
            _simd = new FlawedFrequencyTransmissionSimd();
            _vertical = new FlawedFrequencyTransmissionVertical();
            _sm = new FftStateMachine();
        }

        [Benchmark(Baseline =true)]
        public void Regular()
        {
            _regular.ExecutePhases(C16.Challenge.Input, C16.Challenge.Pattern, 100);
        }

        [Benchmark]
        public void Simd()
        {
            _simd.ExecutePhases(C16.Challenge.Input, C16.Challenge.Pattern, 100);
        }

        //[Benchmark]
        //public void Vertical()
        //{

        //    _vertical.Execute(C16.Challenge.Input, C16.Challenge.Pattern, 100);
        //}
        [Benchmark]
        public void Statemachine()
        {
            _sm.Execute(Challenge.Input, 100);
        }
    }
}
