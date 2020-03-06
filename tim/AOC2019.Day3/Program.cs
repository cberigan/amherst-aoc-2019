using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace AOC2019.Day3
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] inputcodes1 = File.ReadAllText("list1.txt").Split(',');
            string[] inputcodes2 = File.ReadAllText("list2.txt").Split(',');
            List<string> codes1 = inputcodes1.ToList();
            List<string> codes2 = inputcodes2.ToList();

            GetManhattanDistance(codes1, codes2);
            //Console.WriteLine(codes);
        }

        public static int GetDistance(string a, string b)
        {
            int aup = 0;
            int adown = 0;
            int aleft = 0;
            int aright = 0;
            int bup = 0;
            int bdown = 0;
            int bleft = 0;
            int bright = 0;

            Console.WriteLine($"list1 code: {a} | list2 code: {b}");
            if (a.StartsWith("U"))            
                aup = aup + Convert.ToInt32(a.Substring(1,a.Length));
            if (a.StartsWith("D"))
                adown = adown + Convert.ToInt32(a.Substring(1, a.Length)); 
            if (a.StartsWith("L"))
                aleft = aleft + Convert.ToInt32(a.Substring(1, a.Length)); 
            if (a.StartsWith("R"))
                aright = aright + Convert.ToInt32(a.Substring(1, a.Length)); 

            if (b.StartsWith("U"))
                bup = bup + Convert.ToInt32(b.Substring(1, b.Length));
            if (b.StartsWith("D"))
                bdown = bdown + Convert.ToInt32(b.Substring(1, b.Length));
            if (b.StartsWith("L"))
                bleft = bleft + Convert.ToInt32(b.Substring(1, b.Length));
            if (b.StartsWith("R"))
                bright = bright +Convert.ToInt32(b.Substring(1, b.Length));


            return 1;
        }

        public static int GetManhattanDistance(List<string> codes1, List<string> codes2)
        {
            int result = 0;
            for(int i = 0; i <= codes1.Count(); i++)
            {
                string code1 = codes1.ElementAt(i);
                string code2 = codes2.ElementAt(i);

            }

            return result;
        }
    }
}
