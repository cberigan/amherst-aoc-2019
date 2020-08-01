using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCodeChallenges.C14
{
    public partial class Challenge
    {
        public class Pt1
        {
            //  what is the minimum amount of ORE required to produce exactly 1 FUEL
            public long Run(string input = null)
            {
                if (input == null) input = Input;
                //return new TheHellDoTheyWant().Run(input);

                var reactions = new ReactionParser().Parse(input);
                var topologicalOrder = TheHellDoTheyWant.TopologicalOrder(reactions);

                var res = new ReactionChain().Run(topologicalOrder, reactions, pair => pair.fuel.TotalProduced < 1);
                return res.totalOre;
            }
        }

        public class Pt2
        {
            // given an amount of ore, how much fuel can be produced
            const long TRILLION = 1000000000000;
            public long Run(string input = Input, long maxOre = TRILLION)
            {
                //return ByIterative(input, maxOre);
                return ByJumping(input, maxOre);
            }

            // This gives the right answer of 1,184,209
            private long ByIterative(string input, long maxOre)
            {
                var reactions = new ReactionParser().Parse(input);

                var topologicalOrder = TheHellDoTheyWant.TopologicalOrder(reactions);

                var chain = new ReactionChain().GetReactionChain(topologicalOrder, reactions);
                var ore = chain.all.Last();

                int i = 0;
                while (ore.TotalProduced < maxOre)
                {
                    i++;
                    if (chain.fuel.Request(1) == 0)
                        break;
                    if (i % 1000 == 0)
                        Console.WriteLine("{0}\t{1}\t{2}", i, chain.fuel.TotalProduced, ore.TotalProduced / 1_000_000_000);
                }

                return chain.fuel.TotalProduced;
            }

            // this gives a wrong answer of 925,382
            private static long ByJumping(string input, long maxOre)
            {
                var reactions = new ReactionParser().Parse(input);

                var topologicalOrder = TheHellDoTheyWant.TopologicalOrder(reactions);

                var oneReaction = new ReactionChain().Run(topologicalOrder, reactions, x => x.fuel.TotalProduced < 1);


                var startingPoint = (int)(maxOre / oneReaction.totalOre);
                var chain = new ReactionChain().GetReactionChain(topologicalOrder, reactions);
                var ore = chain.all.Last();

                chain.fuel.Request(startingPoint);

                int i = 0;
                var midpoint = (maxOre - ore.TotalProduced) / oneReaction.totalOre; // startingPoint / 10;
                while (ore.TotalProduced < maxOre)
                {
                    if (chain.fuel.Request(midpoint) == 0)
                    {
                        if (midpoint == 1) break;
                        midpoint = (maxOre - ore.TotalProduced) / oneReaction.totalOre;
                        midpoint = Math.Max(midpoint, 1);
                        Console.WriteLine("{0}\t{1}\t{2}", midpoint, chain.fuel.TotalProduced, ore.TotalProduced);
                    }
                    i++;

                    Console.WriteLine("{0}\t{1}\t{2}", i, chain.fuel.TotalProduced, ore.TotalProduced);
                }

                return chain.fuel.TotalProduced;
            }

        }


        class FailedAttempts
        {
            public long Run()
            {
                var parser = new ReactionParser();
                var input = Input;

                var allRecipes = parser.Parse(input);

                var fuel = allRecipes.Find(r => r.Output.Ingredient == "FUEL");

                var links = allRecipes.GroupBy(x => x.Output.Ingredient).ToDictionary(x => x.Key, x => x.ToArray());

                var ore = "ORE"; /* should have split the name and quantity */
                var allPaths = FindRecipes(fuel.Output, ore, links).ToList();

                //var totalOre = _nanoFactory.Run(allPaths);
                var totalOre = new QueueItUpFactory().Run(allPaths);
                return totalOre;

                //var minQuantity = allPaths.Select(FindQuantity).Min();
                //return minQuantity;

                static Reaction[] FindReactionsFor(string output, Dictionary<string, Reaction[]> allReactions)
                {
                    if (allReactions.TryGetValue(output, out var reactions))
                        return reactions;
                    return Array.Empty<Reaction>();
                }

                // alternative recipes not supported
                static IEnumerable<Reaction> FindRecipes(
                    (string ingredient, int quantity) output,
                    string startingIngredient,
                    Dictionary<string, Reaction[]> allRecipes)
                {
                    foreach (var reaction in FindReactionsFor(output.ingredient, allRecipes))
                    {
                        yield return reaction;

                        foreach (var ingredient in reaction.Ingredients)
                            foreach (var recipe in FindRecipes(ingredient, startingIngredient, allRecipes))
                                yield return recipe;
                    }
                }
            }
        }
    }
}
