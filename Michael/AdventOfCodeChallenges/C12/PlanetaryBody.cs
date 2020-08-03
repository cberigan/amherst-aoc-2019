namespace AdventOfCodeChallenges.C12
{
    public class PlanetaryBody
    {
        // I was expecting this to come into play, but it hasn't
        public float Mass { get; set; } = 1.0f;
        public string Name { get; }

        public PlanetaryBody(string name) => Name = name;
        public PlanetaryBody()
        {

        }
    }
}
