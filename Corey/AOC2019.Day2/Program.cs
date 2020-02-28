using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2019.Day2
{
    class Program
    {
        static void Main(string[] args)
        {
            var codes = File.ReadAllText("input.txt").Split(',');


            for (int noun = 0; noun < 100; noun++)
            {
                for (int verb = 0; verb < 100; verb++)
                {
                    if (Output(codes.Select(x => int.Parse(x)).ToArray(), noun, verb) == 19690720)
                    {
                        Console.WriteLine(noun);
                        Console.WriteLine(verb);
                        Console.WriteLine(100 * noun + verb);
                        break;
                    }
                }
            }
          
                Console.ReadLine();
        }


        static int Output(int[] codes, int noun, int verb)
        {

            codes[1] = noun;
            codes[2] = verb;
            for (int i = 0; i < codes.Length; i += 4)
            {
                var op = codes[i];
                if (op == 99) break;

                var first = codes[codes[i + 1]];
                var second = codes[codes[i + 2]];
                var output = codes[i + 3];

                if (op == 1)
                {
                    codes[output] = first + second;
                }
                else if (op == 2)
                {
                    codes[output] = first * second;
                }
            }

            return codes[0];
        }
    }
}
