using AdventOfCodeChallenges.Core;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCodeChallenges.C14
{
    public class ReactionParser : IParser<Reaction>
    {
        private enum LineState { Input, Output }
        public List<Reaction> Parse(string input)
        {
            // "12 VJWQR, 1 QTBC => 6 BGXJV"
            var result = input
                .Split(Environment.NewLine)
                .Select(line =>
                    {
                        var ingredients =
                            line.Split(new string[] { ",", "=>" }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(part => ParseIngredient(part))
                            .ToList();

                        var inputs = ingredients;
                        var output = ingredients.Last();
                        inputs.RemoveAt(ingredients.Count - 1);

                        return new Reaction(new IngredientCollection(inputs), output);
                    })
                .ToList();

            return result;
        }



        public (string i, int quantity) ParseIngredient(string part)
        {
            var trimmed = part.AsSpan().Trim();
            var quantityLength = trimmed.IndexOf(' ');
            var quantity = int.Parse(trimmed.Slice(0, quantityLength));
            var name = new string(trimmed.Slice(quantityLength + 1));

            var ing = name;
            return (ing, quantity);
        }
    }
}
