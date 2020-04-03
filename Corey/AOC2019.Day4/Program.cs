using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2019.Day4
{
    class Program
    {
        static void Main(string[] args)
        {
            int min = 372037;
            int max = 905157;

            Console.WriteLine(MeetsCriteria2(112233));
            Console.WriteLine(MeetsCriteria2(123444));
            Console.WriteLine(MeetsCriteria2(111122));
            Console.WriteLine(MeetsCriteria2(124444));

            int count = 0;
            int count2 = 0;
            for (int i = min; i <= max; i++)
            {
                if (MeetsCriteria(i)) count++;
                if (MeetsCriteria2(i)) count2++;
            }
            Console.WriteLine(count);
            Console.WriteLine(count2);
            Console.ReadLine();
        }


        static bool MeetsCriteria(int num)
        {
            var chars = num.ToString().ToCharArray();
            bool hasDouble = false;
            bool isIncreasing = true;
            for (int j = 0; j < chars.Length - 1; j++)
            {
                hasDouble |= chars[j] == chars[j + 1];
                isIncreasing &= ((byte)chars[j]) <= ((byte)chars[j + 1]);
                if (!isIncreasing) return false;
            }

            return hasDouble & isIncreasing;
        }


        static bool MeetsCriteria2(int num)
        {
            var chars = num.ToString().ToCharArray();
            bool isIncreasing = true;
            
            bool hasDouble = false;
            for (int j = 0; j < chars.Length - 1; j++)
            {
                if(chars[j] == chars[j + 1])
                {
                    int matchStart = j;
                    int matchEnd = j+1;
                    while(chars[matchStart] == chars[matchEnd])
                    {
                        matchEnd++;
                        if (matchEnd == chars.Length) break;
                    }
                    if (matchEnd - matchStart == 2) hasDouble = true;
                    j = matchEnd - 2;
                } 
                isIncreasing &= ((byte)chars[j]) <= ((byte)chars[j + 1]);
                if (!isIncreasing) return false;
            }

            return isIncreasing & hasDouble;
        }
    }
}
