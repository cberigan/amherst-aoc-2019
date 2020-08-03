using MoreLinq;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCodeChallenges.C14
{
    public sealed class IngredientCollection : IEnumerable<(string ingredient, int quantity)>
    {
        private readonly IReadOnlyCollection<(string ingredient, int quantity)> _ingredients;

        public IngredientCollection(IReadOnlyCollection<(string ingredient, int quantity)> ingredients)
        {
            _ingredients = ingredients ?? new (string ingredient, int quantity)[] { };
            HashCode = _ingredients.Select(x => System.HashCode.Combine(x.GetHashCode()))
                .DefaultIfEmpty(0)
                .Aggregate((acc, next) => System.HashCode.Combine(acc, next));
        }

        public IEnumerator<(string ingredient, int quantity)> GetEnumerator() => _ingredients.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _ingredients.GetEnumerator();

        public int Count => _ingredients.Count;

        public int HashCode { get; }
    }
}
