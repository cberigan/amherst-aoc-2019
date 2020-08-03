namespace AdventOfCodeChallenges.C14
{
    //public class NanoFactoryV2
    //{
    //    public int Run(Dictionary<string, Reaction[]> allReactions)
    //    {
    //        var orePool = new OrePool();
    //        var materialPool = new MaterialPool();
    //        var fuelDependencies = allReactions["FUEL"];

    //        foreach (var reaction in fuelDependencies.OrderBy(StepsToOre(0, allReactions)))
    //        {
    //            Process(reaction, orePool, materialPool, allReactions, reaction.Output.Quantity);
    //        }

    //        return -1;
    //    }

    //    private int Run(Dictionary<string, Reaction[]> allReactions)
    //    {
    //        var orePool = new OrePool();
    //        var materialPool = new MaterialPool();
    //        var factory = new Factory();
    //        var harvester = new Harvester(materialPool, orePool);

    //        var fuel = (ingredient: new Ingredient("FUEL"), 1);

    //        factory.RequestResource(fuel, harvester, allReactions);
    //    }

    //    class Factory
    //    {
    //        public void RequestResource((Ingredient ingredient, int quantity) request, 
    //            Harvester harvester,
    //            Dictionary<string, Reaction[]> allReactions)
    //        {
    //            //FUEL,1

    //            // Take 0, so 1
    //            var stillNeedToGet = harvester.Harvest(request.ingredient, request.quantity);


    //            // 44 XJWVT, 5 KHKGT, 1 QDVJ, 29 NZVS, 9 GPVTF, 48 HKGWZ
    //            var recipe = allReactions[request.ingredient.Name];
    //            foreach (var ingredient in recipe)
    //            {
    //                var subRequest = (
    //                    ingredient.Output.Ingredient, 
    //                    quantity: stillNeedToGet / ingredient.Output.Quantity  // 44 - 0 = 44, 44 XJWVT / 2 XJWVT from recipe = 11
    //                );
    //                var returnedResources = RequestResource(subRequest, harvester, allReactions);
    //            }
    //        }
    //    }

    //    class Harvester
    //    {
    //        private readonly MaterialPool materialPool;
    //        private readonly OrePool orePool;

    //        public Harvester(MaterialPool materialPool, OrePool orePool)
    //        {
    //            this.materialPool = materialPool;
    //            this.orePool = orePool;
    //        }

    //        /// <summary>
    //        ///  returns the remaining amount.
    //        /// </summary>
    //        internal int Harvest(Ingredient ingredient, int quantity)
    //        {
    //            var remaining = materialPool.Get(ingredient, quantity);

    //            if (ingredient.Name == "ORE")
    //            {
    //                orePool.HarvestOre(remaining);
    //                return 0;
    //            }
    //            return remaining;
    //        }
    //    }

    //    //private int Process(Reaction reaction, OrePool orePool, MaterialPool materialPool, Dictionary<string, Reaction[]> allReactions
    //    //    , Reaction parentReaction)
    //    //{
    //    //    foreach (var ingredient in reaction.Ingredients.OrderBy(x => IngredientStepsToOre(0, allReactions)(x.ingredient)))
    //    //    {
    //    //        // 44 XJWVT <- ingredient

    //    //        // desiredCount = 44 XJWVT -> 1 Fuel
    //    //        var desiredCount = ingredient.quantity * parentReaction.Output.Quantity;



    //    //        //if (ingredient.ingredient.Name == "ORE")
    //    //        //{
    //    //        //    var parentIngredient = parentReaction.Ingredients.First(x => x.ingredient == ingredient.ingredient);
    //    //        //    var desiredQuantity = parentIngredient.quantity;

    //    //        //    var remaining = materialPool.Get(ingredient.ingredient, desiredQuantity);
    //    //        //    var harvestedQuantity = orePool.HarvestOre(desiredQuantity - remaining);



    //    //        //    return desiredQuantity;
    //    //        //}
    //    //        //else
    //    //        //{
    //    //        //    int harvested = 0;
    //    //        //    foreach (var subReaction in allReactions[ingredient.ingredient.Name].OrderBy(StepsToOre(0, allReactions)))
    //    //        //    {
    //    //        //        harvested += Process(subReaction, orePool, materialPool, allReactions, parentCount + ingredient.quantity);
    //    //        //    }
    //    //        //    return harvested;
    //    //        //}
    //    //    }
    //    //    return 0;
    //    //}


    //    private Func<Reaction, int> StepsToOre(int stepCount, Dictionary<string, Reaction[]> allReactions)
    //    {
    //        return reaction =>
    //        {
    //            foreach (var ingredient in reaction.Ingredients)
    //            {
    //                if (ingredient.ingredient.Name == "ORE")
    //                    return stepCount;

    //                return IngredientStepsToOre(stepCount + 1, allReactions)(ingredient.ingredient);
    //            }

    //            return 0;
    //        };
    //    }

    //    private Func<Ingredient, int> IngredientStepsToOre(int stepCount, Dictionary<string, Reaction[]> allReactions)
    //    {
    //        return ingredient =>
    //        {
    //            if (ingredient.Name == "ORE")
    //                return stepCount;

    //            foreach (var subReaction in allReactions[ingredient.Name])
    //                return StepsToOre(stepCount + 1, allReactions)(subReaction);
    //            return 0;
    //        };
    //    }


    //    class OrePool
    //    {
    //        private int _harvestedOre = 0;
    //        public int HarvestedOre => _harvestedOre;
    //        public void HarvestOre(int quantity)
    //        {
    //            _harvestedOre += quantity;
    //        }
    //    }

    //    class MaterialPool
    //    {
    //        public Dictionary<Ingredient, int> LeftOvers { get; } = new Dictionary<Ingredient, int>();
    //        /// <summary>
    //        /// Returns the remaining amount.
    //        /// </summary>
    //        /// <param name="ingredient"></param>
    //        /// <param name="desiredQuantity"></param>
    //        /// <returns></returns>
    //        public int Get(Ingredient ingredient, int desiredQuantity)
    //        {
    //            if (LeftOvers.TryGetValue(ingredient, out var store))
    //            {
    //                var takeable = Math.Min(desiredQuantity, store);
    //                var remainder = store - takeable;
    //                LeftOvers[ingredient] = remainder;
    //                return remainder;
    //            }
    //            return 0;
    //        }
    //    }
    //}
}
