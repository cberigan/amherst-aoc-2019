using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCodeChallenges.C14
{
    public class QueueItUpFactory
    {
        public long Run(IReadOnlyList<Reaction> chain)
        {
            var fuel = chain.First(r => r.Output.Ingredient == "FUEL");

            var oreRequests = new List<(string from, string target, int batchSize, int batchOutput, int totalCountRequested)>();

            FindSum(fuel, chain, oreRequests, fuel.Output.Quantity);

            var oreCountAfterConsolidation = oreRequests
                .GroupBy(r => new { r.target, r.batchSize, r.batchOutput })
                .Sum(g =>
                {
                    //var totalOre = g.Sum(x => (long)Math.Ceiling((double)x.totalCountRequested / x.batchOutput * x.batchSize));
                    //return totalOre;

                    var totalRequestedCount = g.Sum(x => (long)x.totalCountRequested);
                    var totalBatches = (long)Math.Ceiling((double)totalRequestedCount / g.Key.batchOutput);
                    var totalOre = totalBatches * g.Key.batchSize;
                    return totalOre;
                });

            //var part = oreRequests
            //    .GroupBy(r => new { r.target, r.batchSize, r.batchOutput })
            //    .Select(g =>
            //    {
            //        var totalRequestedCount = g.Sum(x => (long)x.totalCountRequested);
            //        var totalBatches = (long)Math.Ceiling((double)totalRequestedCount / g.Key.batchOutput);
            //        var totalOre = totalBatches * g.Key.batchSize;
            //        return new { g.Key.target, totalOre };
            //    })
            //    .ToList();

            return oreCountAfterConsolidation;
        }


        private void FindSum(Reaction product, IReadOnlyList<Reaction> chain, 
            List<(string from, string target, int batchSize, int batchOutput, int totalCountRequested)> oreRequests, 
            int productBatches)
        {
            foreach (var input in product.Ingredients)
            {
                var recipe = chain.FirstOrDefault(r => r.Output.Ingredient == input.ingredient);
                if (recipe != null)
                {
                    FindSum(recipe, chain, oreRequests, productBatches * input.quantity);
                    //oreRequests.Add((input.ingredient, product.Output.Ingredient, input.quantity, product.Output.Quantity, productBatches));
                }
                else // it's ore!
                    oreRequests.Add((input.ingredient, product.Output.Ingredient, input.quantity, product.Output.Quantity, productBatches));
            }
        }
    }
}
