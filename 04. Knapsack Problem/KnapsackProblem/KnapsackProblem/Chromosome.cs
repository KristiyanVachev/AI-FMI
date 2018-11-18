using System.Collections.Generic;

namespace KnapsackProblem
{
    public class Chromosome
    {
        public Chromosome()
        {
            this.Genes = new List<Gene>();
        }

        public Chromosome(IList<Gene> genes)
        {
            this.Genes = genes;
        }


        public IList<Gene> Genes { get; set; }

        public int Fitness { get; set; }
    }
}
