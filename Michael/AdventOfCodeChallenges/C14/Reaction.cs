namespace AdventOfCodeChallenges.C14
{
    [System.Diagnostics.DebuggerDisplay("Target = {Output.Ingredient}")]
    public class Reaction
    {
        public Reaction(IngredientCollection inputs, (string ingredient, int quantity) output)
        {
            Ingredients = inputs;
            Output = output;
        }

        public IngredientCollection Ingredients { get; }
        public (string Ingredient, int Quantity) Output { get; }
    }
}