using System.Collections.Generic;

namespace KnapsackProblem
{
    public class Chromosome
    {
        public Chromosome()
        {
            this.Genes = new List<Gene>();
        }

        public ICollection<Gene> Genes { get; set; }

        public int Fitness { get; set; }
    }
}
