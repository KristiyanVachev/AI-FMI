using System;
using System.Collections.Generic;
using System.Linq;

namespace KnapsackProblem
{
    public class Startup
    {
        const int PopulationSize = 8;

        public static void Main()
        {
            //Input
            int maxWeight = 5;
            int maxObjects = 3;

            var genes = new List<Gene>
            {
                new Gene(3, 2),
                new Gene(1, 5),
                new Gene(2, 3)
            };

            //Generata initial population - should be divisible by 4
            var population = GenerateInitialPopulation(genes, maxWeight, maxObjects);

            //Compute fitness
            foreach (var chromesome in population)
            {
                ComputeFitness(chromesome, maxWeight);
            }

            //Selection - Kill 50%
            population.OrderBy(x => x.Fitness);
            //Kill the unfitter half.

            //Crossover each of the parents at random and fill the place of the dead.

            //Mutate everybody

            //Judge for convergence

            //Repeat
        }

        private static ICollection<Chromosome> GenerateInitialPopulation(List<Gene> genes, int maxWeight, int maxObjects)
        {
            var population = new Chromosome[PopulationSize];
            var randomGenerator = new Random();

            for (int i = 0; i < PopulationSize; i++)
            {
                population[i] = new Chromosome();

                while (true)
                {
                    //Select a gene at random
                    var newGene = genes[randomGenerator.Next(genes.Count)];

                    //If the gene has already been selected, try again.
                    if (!population[i].Genes.Contains(newGene))
                    {
                        population[i].Genes.Add(newGene);

                        //If the gene makes the chromosome overweight or overStuffed, stop adding genes.
                        if (population[i].Genes.Select(x => x.Weight).Sum() >= maxWeight || population[i].Genes.Count() > maxObjects)
                        {
                            break;
                        }
                    }
                }
            }

            return population;
        }

        private static void ComputeFitness(Chromosome chromosome, int maxWeight)
        {
            var totalWeight = chromosome.Genes.Select(x => x.Weight).Sum();

            //TODO - Tolerate overStuffness? Could make a bool and set fitness to maxValue if overstuffed.
            
            chromosome.Fitness = Math.Abs(maxWeight - totalWeight);
        }
    }
}
