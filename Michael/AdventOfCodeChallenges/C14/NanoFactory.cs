using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCodeChallenges.C14
{
    public sealed class NanoFactory
    {
        /// <summary>
        /// Assumes fuel is the first item
        /// </summary>
        /// <param name="chain"></param>
        public int Run(IReadOnlyList<Reaction> chain)
        {
            // 44 XJWVT, 5 KHKGT, 1 QDVJ, 29 NZVS, 9 GPVTF, 48 HKGWZ => 1 FUEL
            var target = chain[0];

            var storedOre = new StoredOre();
            var total = 0;
            foreach (var ingredient in target.Ingredients)
            {
                // 44 XJWVT 
                total += GetChildIngredientCount(ingredient, chain, target.Output.Quantity, storedOre);
            }
            return total;
        }

        class StoredOre
        {
            public int Count = 0;

            internal int Get(int subMaterials)
            {
                if (subMaterials > Count) return 0;
                Count -= subMaterials;
                return subMaterials;
            }
        }

        private int GetChildIngredientCount((string i, int quantity) ingredient, IReadOnlyList<Reaction> all, int howMuchOfParent, StoredOre storedOre)
        {
            var (i, quantity) = ingredient;
            var reaction = all.FirstOrDefault(x => x.Output.Ingredient == i);
            if (reaction == null)
            {
                reaction = new Reaction(new IngredientCollection(new List<(string, int)>()), ("ORE", 1));
            }

            // 44 XJWVT is 44
            var ingredientInput = quantity * howMuchOfParent;
            // 7 DCFZ, 7 PSHF => 2 XJWVT is 2
            var reactionOutput = reaction.Output.Quantity;

            // if there's any amount over a whole number, add 1
            int timesToExecuteReaction = (int)Math.Ceiling((double)ingredientInput / reactionOutput);

            var subMaterials =
                reaction.Ingredients.Count == 0 ?
                    reactionOutput * timesToExecuteReaction :
                    reaction.Ingredients.Sum(subIngredient => GetChildIngredientCount(subIngredient, all, timesToExecuteReaction, storedOre));

            
            var remainder = subMaterials - ingredientInput;
            if (remainder < 0) return 0;
            storedOre.Count += remainder;

            var totalMaterial = subMaterials - storedOre.Get(ingredientInput);
            return totalMaterial;
        }

        private int CalculateMaterial(int a, int b)
        {
            var (high, low) = (Math.Max(a, b), Math.Min(a, b));
            var res = (double)low / high - low / high > 0 ?
                low / high + 1 :
                low / high;
            return res;
        }

        // trying an approach where a manufacture step is encapsulated and retains how much extra it produced

        //public int Run(IReadOnlyList<Reaction> chain)
        //{
        //    var target = chain[0];

        //    var allSteps = chain.Skip(1).Select(reaction =>
        //        (
        //            reaction,
        //            manufactureSteps: reaction.Ingredients.Select(i => new ManufactureSubStep { Input = i }).ToList()
        //        )
        //    ).ToList();

        //    var steps = chain.ToDictionary(c => c.Output.Ingredient, c => (c.Output.Quantity, c.Ingredients));


        //}

       
        class ManufactureSubStep
        {
            public (string Ingredient, int quantity) Input { get; set; }
            public int InputLeftover { get; set; }
        }
    }
}
