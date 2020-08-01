using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCodeChallenges.C14
{
    public class TheHellDoTheyWant
    {
        // I give up. I'm following some guy online. I get less, I get more. I've been doing
        // this for too long. It's after midnight and I skipped dinner b/c I'm a loser.
        // I can't even think. Why do I do this shit when I'm tired?
        public int Run(string input = C14.Challenge.Input)
        {
            var reactions = new ReactionParser().Parse(input);

            var fuel = reactions.Find(r => r.Output.Ingredient == "FUEL");
            //reactions.Remove(fuel);

            var topologicalOrder = TopologicalOrder(reactions);
            var (oreRequired, iterations) = OreRequired(topologicalOrder, reactions);

            return oreRequired;
        }

        private (int oreRequired, int iterations) OreRequired(List<string> topologicalOrder, List<Reaction> reactions)
        {
            int oreRequired = 0, iterations = 0;
            var needs = new Dictionary<string, int> { { "FUEL", 1 }};
            
            while (needs.Count > 0)
            {
                iterations++;
                var chemical = needs.Keys.OrderBy(k => topologicalOrder.IndexOf(k)).First();
                var qtyRequired = needs[chemical];
                needs.Remove(chemical);
                var reaction = reactions.Find(r => r.Output.Ingredient == chemical);

                var batches = (int)Math.Ceiling((double)qtyRequired / reaction.Output.Quantity);

                foreach (var (ingredient, qty) in reaction.Ingredients)
                {
                    if (ingredient == "ORE")
                        oreRequired += qty * batches;
                    else
                    {
                        if (needs.TryGetValue(ingredient, out var n))
                            needs[ingredient] = n + qty * batches;
                        else needs[ingredient] = qty * batches;
                    }
                }
            }
            return (oreRequired, iterations);
        }

        public static List<string> TopologicalOrder(List<Reaction> reactions)
        {
            var visitedNodes = new List<string>();
            var order = new List<string>();

            void dfs(List<Reaction> reactions, string node)
            {
                visitedNodes.Add(node);
                if (node == "ORE")
                    return;

                var reaction = reactions.First(r => r.Output.Ingredient == node);
                foreach (var ing in reaction.Ingredients)
                {
                    if (!visitedNodes.Contains(ing.ingredient))
                    {
                        dfs(reactions, ing.ingredient);
                    }
                }

                order.Add(node);
            }
            dfs(reactions, "FUEL");
            order.Reverse();
            order.Add("ORE");
            return order;
        }
    }
}
