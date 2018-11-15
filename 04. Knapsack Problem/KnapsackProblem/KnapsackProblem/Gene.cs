namespace KnapsackProblem
{
    public class Gene
    {
        public Gene(int weight, int value)
        {
            this.Weight = weight;
            this.Value = value;
        }

        public int Weight { get; }

        public int Value { get; }
    }
}
