using System;
using System.Linq;

namespace AdventOfCodeChallenges.C7
{
    public class AmplifierController
    {
        public int Run(int[] memory, PhaseSettings phaseSettings, bool feedback = false)
        {
            int final = -1;
            
            var a = new Amplifier("A", memory, () => phaseSettings.A);
            var b = new Amplifier("B", memory, () => phaseSettings.B);
            a.Linkto(b);                                             
            var c = new Amplifier("C", memory, () => phaseSettings.C);
            b.Linkto(c);                                             
            var d = new Amplifier("D", memory, () => phaseSettings.D);
            c.Linkto(d);
            var e = new Amplifier("E", memory, () => phaseSettings.E) { IsFinalStage = true };
            d.Linkto(e);
            if (feedback)
                e.Linkto(a);
            e.OnOutput += (_, i) => final = i;


            a.Post(0);

            return final;
        }
    }

    public struct PhaseSettings
    {
        public PhaseSettings(int a, int b, int c, int d, int e)
        {
            A = a;
            B = b;
            C = c;
            D = d;
            E = e;
        }

        public int A { get; set; }
        public int B { get; set; }
        public int C { get; set; }
        public int D { get; set; }
        public int E { get; set; }
    }
}
