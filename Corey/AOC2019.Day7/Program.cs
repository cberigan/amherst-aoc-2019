using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2019.Day7
{
    class Program
    {
        static void Main(string[] args)
        {
            var width = 25;
            var height = 6;
            var pixels = File.ReadAllText("input.txt");
            var layerLength = 150;
            List<int> layer = new List<int>();
            List<int> finalImage = new List<int>();
            List<List<int>> layers = new List<List<int>>();
            var minZeros = int.MaxValue;
            var minSum = 0;
            for(var i = 0; i < pixels.Length; i++)
            {
                
                layer.Add((int)char.GetNumericValue(pixels[i]));
                if (layer.Count % layerLength == 0)
                {
                    var numZeros = 0;
                    var numOnes = 0;
                    var numTwos = 0;
                    for (int j = 0; j < layer.Count; j++)
                    {
                        if (layer[j] == 0) numZeros++;
                        if (layer[j] == 1) numOnes++;
                        if (layer[j] == 2) numTwos++;
                    }
                    if (numZeros < minZeros)
                    {
                        minZeros = numZeros;
                        minSum = numOnes * numTwos;
                    }
                    layers.Add(layer);

                    layer = new List<int>();
                }
            }

            //we have all our layers
            for (int i = 0; i < layers[0].Count; i++)
            {
                foreach (var l in layers)
                {
                    if (l[i] != 2) 
                    {
                        finalImage.Add(l[i]);
                        break;
                    }
                }
            }

            

            Console.WriteLine("Min Zeros: " + minZeros);
            Console.WriteLine("Min Sum: " + minSum);
            for (int i = 0; i < finalImage.Count; i++)
            {
                if (i % 25 == 0) Console.Write(Environment.NewLine);
                Console.Write(finalImage[i] == 1 ? 'O' : ' ');
            }
            Console.ReadLine();

        }
    }
}
