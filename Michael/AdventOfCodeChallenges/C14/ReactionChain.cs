using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace AdventOfCodeChallenges.C14
{
    class ReactionChain
    {
        public (long totalOre, long totalFuel) Run(List<string> order, List<Reaction> allReactions, 
            Predicate<(ReactionNode fuel, ReactionNode ore)> runWhile)
        {
            var chain = GetReactionChain(order, allReactions);
            var pair = (chain.all.First(), chain.all.Last());

            while (runWhile(pair))
                chain.fuel.Request(1);

            return (pair.Item2.TotalProduced, pair.Item1.TotalProduced);
        }

        public (ReactionNode fuel, List<ReactionNode> all) GetReactionChain(List<string> order, List<Reaction> allReactions)
        {
            var asDic = allReactions.ToDictionary(x => x.Output.Ingredient, x => x);
            var chain = BuildChain(order, asDic);
            return chain;
        }

        private (ReactionNode fuel, List<ReactionNode> all) BuildChain(List<string> order, Dictionary<string, Reaction> allReactions)
        {
            allReactions["ORE"] = new Reaction(new IngredientCollection(null), ("ORE", 1));
            var asNodes = order.Select(chem => new ReactionNode(allReactions[chem])).ToList();
            asNodes.Last().Limit = output => output <= 1_000_000_000_000;

            foreach (var node in asNodes)
                foreach (var ingredient in allReactions[node.Output.Ingredient].Ingredients)
                {
                    var ingredientNode = asNodes.Find(n => n.Output.Ingredient == ingredient.ingredient);
                    node.LinkTo(ingredientNode);
                }

            return (asNodes.First(), asNodes);
        }

        public class ReactionNode
        {
            public Func<long, bool> Limit { get; set; }
            private readonly List<ReactionNode> _ingredientNodes = new List<ReactionNode>();
            private long _store, _lastStore;

            public long TotalProduced { get; private set; }

            public (string Ingredient, int Quantity) Output { get; }

            private readonly IngredientCollection _recipeIngredients;

            public ReactionNode(Reaction reaction)
            {
                Output = reaction.Output;
                _recipeIngredients = reaction.Ingredients;
            }

            public ReactionNode(Reaction reaction, Func<long, bool> runWhile) : this(reaction) =>
                Limit = runWhile;

            public void LinkTo(ReactionNode ingredientNode) => _ingredientNodes.Add(ingredientNode);

            private long[] _granteds;
            public long Request(long amount)
            {
                var orig = amount;
                long deduct = 0;
                if (_store > 0)
                {
                    deduct = Math.Min(amount, _store);
                    amount -= deduct;
                    _lastStore = _store;
                    _store -= deduct;
                }

                if (amount == 0) return orig;

                var batches = (long)Math.Ceiling(amount / (double)RunReaction());

                if (_granteds == null) _granteds = new long[_ingredientNodes.Count];

                for (int ing = 0; ing < _ingredientNodes.Count; ing++)
                {
                    var needed = _recipeIngredients.First(i => i.ingredient == _ingredientNodes[ing].Output.Ingredient).quantity;

                    var granted = _ingredientNodes[ing].Request(needed * batches);
                    _granteds[ing] = granted;
                    if (granted == 0) // someone is at the end of the line
                    {
                        // roll  back!
                        for (int j = 0; j < _granteds.Length; j++)
                        {
                            _ingredientNodes[j].Rollback(_granteds[j]);
                            _granteds[j] = 0;
                        }

                        return 0;
                    }
                }

                _granteds.Clear();

                var amountProduced = RunReaction() * batches;

                if (Limit?.Invoke(amountProduced + TotalProduced) == false)
                    return 0;

                TotalProduced += amountProduced;


                if (amountProduced > amount)
                {
                    _lastStore = _store;
                    _store += amountProduced - amount;
                }
                return amount;
            }

            private void Rollback(long amount)
            {
                for (int i = 0; i < _granteds.Length; i++)
                {
                    if (_granteds[i] > 0)
                        _ingredientNodes[i].Rollback(_granteds[i]);

                    _granteds[i] = 0;
                }

                TotalProduced -= amount;
                _store = _lastStore;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private int RunReaction() => Output.Quantity;

        }
    }

    static class ReactionNodeExt
    {
        public static void Clear(this long[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = 0;
            }
        }
    }
}
