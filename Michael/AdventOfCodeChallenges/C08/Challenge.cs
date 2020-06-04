using AdventOfCodeChallenges.Core;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCodeChallenges.C8
{
    public partial class Challenge
    {
        public class Pt1
        {
            public int Run(string input = null, int width = 25, int height = 6)
            {
                if (input == null) input = Input;
               
                var layers = input.Split(width * height);
                var layerWithFewestZeroDigits =
                    layers.MinBy(text => text.Count(c => c == '0')).First();

                var (ones, twos) = layerWithFewestZeroDigits
                    .Aggregate((ones: 0, twos: 0), (acc, c) => c switch
                    {
                        '1' => (acc.ones + 1, acc.twos),
                        '2' => (acc.ones, acc.twos + 1),
                        _ => acc
                    });
                return ones * twos;
            }
        }

        public class Pt2
        {
            public string Run(string input=  null, int width=25, int height = 6)
            {
                if (input == null) input = Input;
                var layers = input.Split(width * height);

                var finalLayer = layers
                    .Transpose()
                    .Select(chars =>
                        chars.Where(c => c != Color.Transparent).Take(1).DefaultIfEmpty(Color.Transparent))
                    .Transpose()
                    .First();

                var seed = (sb: new StringBuilder(width * height + height), i:1);
                var squaredImage = finalLayer.Aggregate(seed, 
                    (acc,c) =>
                    {
                        var (sb, i) = acc;
                        sb.Append(c == Color.Black ? '_' : c);
                        if (i == width) {
                            sb.AppendLine();
                            i = 0;
                        }
                        return (sb, i + 1);
                    })
                    .sb.ToString();

                return squaredImage;
            }
        }

        private static class Color
        {
            public const char Black = '0';
            public const char White = '1';
            public const char Transparent = '2';
        }
    }
}
