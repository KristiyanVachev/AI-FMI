using System;
using System.Collections.Generic;
using System.Linq;

namespace KnapsackProblem
{
    public class Startup
    {
        //Must be divisible by 4
        const int PopulationSize = 400;
        const int ImmortalsCount = PopulationSize / 50;
        const int MaxGenerations = 40;
        //Mutate 1 in ...
        const int MutationRate = 20;

        //Data
        const int MaxWeight = 5000;
        const int MaxObjects = 200;

        public static void Main()
        {
            var genes = GetGenes(3);

            var population = GenerateInitialPopulation(genes, MaxWeight, MaxObjects);

            for (int i = 0; i < MaxGenerations; i++)
            {
                population = Selection(population);

                population = CrossoverAndMutate(population, genes);

                Console.WriteLine("Step " + i + " : " + population.OrderByDescending(x => x.Fitness).FirstOrDefault().Genes.Sum(x => x.Value));
            }
        }

        private static Chromosome[] GenerateInitialPopulation(List<Gene> genes, int maxWeight, int maxObjects)
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
                        //If the gene makes the chromosome overweight or overStuffed, stop adding genes.
                        if (population[i].Genes.Select(x => x.Weight).Sum() + newGene.Weight >= maxWeight || population[i].Genes.Count() > maxObjects)
                        {
                            break;
                        }

                        population[i].Genes.Add(newGene);
                    }
                }

                ComputeFitness(population[i]);
            }

            return population;
        }

        /// <summary>
        /// Computing the fitness for every chromosome. Lower the number, better the fittness.
        /// </summary>
        /// <param name="chromosome"></param>
        /// <param name="maxWeight"></param>
        private static void ComputeFitness(Chromosome chromosome)
        {
            var totalWeight = chromosome.Genes.Select(x => x.Weight).Sum();

            //TODO - Tolerate overStuffness? Could make a bool and set fitness to maxValue if overstuffed.
            if (totalWeight > MaxWeight || chromosome.Genes.Count > MaxObjects)
            {
                chromosome.Fitness = -1;
            }
            else
            {
                var totalValue = chromosome.Genes.Select(x => x.Value).Sum();

                chromosome.Fitness = totalValue;
            }
        }

        /// <summary>
        /// Ordering by fitness and killing (setting to null) the unfittest half of the population.
        /// </summary>
        /// <param name="population"></param>
        /// <returns></returns>
        private static Chromosome[] Selection(Chromosome[] population)
        {
            population = population.OrderByDescending(x => x.Fitness).ToArray();

            //Spare the fittest half
            for (int i = population.Length / 2; i < population.Length; i++)
            {
                population[i] = null;
            }

            return population;
        }

        private static Chromosome[] CrossoverAndMutate(Chromosome[] population, IList<Gene> genes)
        {
            var randomGenerator = new Random();

            //Randomize the population left alive after the selection
            population = population.OrderByDescending(x => x != null).ToArray();
            var parents = population.Take(population.Length / 2).ToArray();
            var emptyPopulationSpaceIndex = population.Length / 2;

            //For each pair of parents
            for (int i = 0; i < population.Length - ImmortalsCount; i++)
            {
                while (true)
                {
                    var firstParent = parents[randomGenerator.Next(0, parents.Length - 1)];
                    var secondParent = parents[randomGenerator.Next(0, parents.Length - 1)];

                    var childGenes = new List<Gene>();
                    for (int j = 0; j < firstParent.Genes.Count; j++)
                    {
                        if (randomGenerator.Next(0, 1) == 0)
                        {
                            childGenes.Add(firstParent.Genes[j]);
                        }
                        else
                        {
                            if (secondParent.Genes.Count < j)
                            {
                                childGenes.Add(secondParent.Genes[j]);
                            }
                            else
                            {
                                //TODO: Maybe add first gene again
                                break;
                            }
                        }
                    }

                    var newChild = new Chromosome(childGenes);
                    newChild = Mutation(newChild, genes);

                    ComputeFitness(newChild);

                    if (newChild.Fitness >= 0)
                    {
                        population[i] = newChild;
                        break;
                    }
                }
            }

            for (int i = 0; i < ImmortalsCount; i++)
            {
                population[population.Length - ImmortalsCount + i] = parents[i];
            }

            if (population.Any(x => x == null))
            {
                Console.WriteLine();
            }

            return population;
        }

        private static Chromosome Mutation(Chromosome individual, IList<Gene> genes)
        {
            if (MutationRate != 0)
            {
                var randomGenerator = new Random();

                for (int j = 0; j < individual.Genes.Count; j += MutationRate)
                {
                    var randomGene = genes[randomGenerator.Next(0, genes.Count)];

                    //If gene is not already contained, do a mutation. Mutation strategy 2 yields the best results.
                    if (!individual.Genes.Contains(randomGene))
                    {
                        //Option 1 - Replace first gene
                        //individual.Genes[j] = randomGene;

                        //Option 2 - 50/50 add or replace
                        if (randomGenerator.Next(0, 1) == 0)
                        {
                            individual.Genes.Add(randomGene);
                        }
                        else
                        {
                            individual.Genes[j] = randomGene;
                        }

                        //Option 3 - Mutate only if replacing a gene makes the chromosome better.
                        //if (randomGene.Value >= individual.Genes[j].Value)
                        //{
                        //    individual.Genes[j] = randomGene;
                        //}
                    }
                }
            }

            return individual;
        }

        /// <summary>
        /// Chance for a swap. If less than half of the genes have been swapped, the chance icreases.
        /// </summary>
        /// <param name="genesIterated"></param>
        /// <param name="swaps"></param>
        /// <returns></returns>
        private static bool GetChanceForSwap(int genesIterated, int swaps)
        {
            if (swaps == 0)
            {
                return true;
            }

            var randomGenerator = new Random();

            //If the number is < 2, then we have made more than half the swaps.
            //4 / 3 = 1.3 || 2 / 2 = 1
            //If the number is >= 2, we have made less than half swaps.
            //4 / 1 = 4 || 4 / 2 = 2
            if (randomGenerator.Next(1, genesIterated / swaps) >= 2)
            {
                return true;
            }

            return false;
        }

        private static List<Gene> GetGenes(int data)
        {
            if (data == 1)
            {
                var genes = new List<Gene>
                {
                    new Gene(3, 2),
                    new Gene(1, 5),
                    new Gene(2, 3)
                };

                return genes;
            }
            else if (data == 2)
            {
                var genes2 = new List<Gene>
                {
                    new Gene(90, 150),
                    new Gene(130, 35),
                    new Gene(1530, 200),
                    new Gene(500, 160),
                    new Gene(150, 60),
                    new Gene(680, 45),
                    new Gene(270, 60),
                    new Gene(390, 40),
                    new Gene(230, 30),
                    new Gene(520, 10),
                    new Gene(110, 70),
                    new Gene(320, 30),
                    new Gene(240, 15),
                    new Gene(480, 10),
                    new Gene(730, 40),
                    new Gene(420, 70),
                    new Gene(430, 75),
                    new Gene(220, 80),
                    new Gene(70, 20),
                    new Gene(180, 12),
                    new Gene(40, 50),
                    new Gene(300, 10),
                    new Gene(900, 1),
                    new Gene(2000, 150)
                };

                return genes2;
            }
            else
            {
                var genes3 = new List<Gene>
                {
                    new Gene(1681, 447),
                    new Gene(1398, 178),
                    new Gene(4558, 233),
                    new Gene(829, 464),
                    new Gene(1249, 446),
                    new Gene(2154, 280),
                    new Gene(3742, 109),
                    new Gene(3445, 108),
                    new Gene(4305, 462),
                    new Gene(3274, 251),
                    new Gene(3194, 114),
                    new Gene(1250, 136),
                    new Gene(4873, 381),
                    new Gene(4222, 461),
                    new Gene(3030, 255),
                    new Gene(44, 252),
                    new Gene(752, 385),
                    new Gene(1699, 152),
                    new Gene(978, 330),
                    new Gene(3186, 212),
                    new Gene(4091, 159),
                    new Gene(3153, 215),
                    new Gene(469, 279),
                    new Gene(1301, 75),
                    new Gene(4661, 231),
                    new Gene(2847, 86),
                    new Gene(4124, 395),
                    new Gene(1351, 249),
                    new Gene(4434, 481),
                    new Gene(3934, 379),
                    new Gene(4383, 14),
                    new Gene(4849, 188),
                    new Gene(667, 485),
                    new Gene(2679, 146),
                    new Gene(1044, 372),
                    new Gene(4226, 440),
                    new Gene(4521, 458),
                    new Gene(1892, 75),
                    new Gene(2243, 143),
                    new Gene(1982, 205),
                    new Gene(4434, 312),
                    new Gene(3391, 195),
                    new Gene(3553, 255),
                    new Gene(2634, 217),
                    new Gene(1285, 160),
                    new Gene(4039, 267),
                    new Gene(259, 177),
                    new Gene(4782, 293),
                    new Gene(415, 10),
                    new Gene(484, 68),
                    new Gene(2940, 335),
                    new Gene(4022, 65),
                    new Gene(817, 150),
                    new Gene(426, 477),
                    new Gene(428, 130),
                    new Gene(1355, 342),
                    new Gene(1347, 310),
                    new Gene(3430, 417),
                    new Gene(1127, 440),
                    new Gene(4566, 129),
                    new Gene(1959, 200),
                    new Gene(3257, 98),
                    new Gene(230, 92),
                    new Gene(2622, 36),
                    new Gene(3166, 488),
                    new Gene(3401, 349),
                    new Gene(2624, 461),
                    new Gene(3454, 183),
                    new Gene(1217, 285),
                    new Gene(2322, 480),
                    new Gene(270, 245),
                    new Gene(3016, 165),
                    new Gene(3160, 472),
                    new Gene(632, 479),
                    new Gene(944, 46),
                    new Gene(3357, 276),
                    new Gene(564, 262),
                    new Gene(1864, 18),
                    new Gene(3029, 243),
                    new Gene(4416, 100),
                    new Gene(3915, 275),
                    new Gene(2695, 293),
                    new Gene(1315, 53),
                    new Gene(1809, 486),
                    new Gene(932, 417),
                    new Gene(2764, 340),
                    new Gene(3130, 455),
                    new Gene(2597, 18),
                    new Gene(729, 308),
                    new Gene(4516, 199),
                    new Gene(751, 285),
                    new Gene(3199, 116),
                    new Gene(4167, 428),
                    new Gene(328, 371),
                    new Gene(2348, 435),
                    new Gene(2475, 150),
                    new Gene(3264, 256),
                    new Gene(1012, 173),
                    new Gene(339, 386),
                    new Gene(4552, 121),
                    new Gene(3060, 350),
                    new Gene(3882, 495),
                    new Gene(1516, 276),
                    new Gene(2303, 442),
                    new Gene(1356, 26),
                    new Gene(1686, 327),
                    new Gene(3684, 431),
                    new Gene(498, 261),
                    new Gene(278, 92),
                    new Gene(609, 114),
                    new Gene(4714, 490),
                    new Gene(1412, 313),
                    new Gene(997, 18),
                    new Gene(3755, 105),
                    new Gene(4686, 363),
                    new Gene(1207, 134),
                    new Gene(4358, 191),
                    new Gene(2099, 487),
                    new Gene(344, 334),
                    new Gene(904, 73),
                    new Gene(899, 356),
                    new Gene(1420, 405),
                    new Gene(4483, 321),
                    new Gene(3541, 192),
                    new Gene(1385, 317),
                    new Gene(1982, 309),
                    new Gene(2692, 257),
                    new Gene(4058, 240),
                    new Gene(4099, 87),
                    new Gene(1561, 496),
                    new Gene(2151, 58),
                    new Gene(1870, 328),
                    new Gene(3249, 149),
                    new Gene(2501, 468),
                    new Gene(3280, 305),
                    new Gene(1464, 434),
                    new Gene(3832, 422),
                    new Gene(3022, 247),
                    new Gene(97, 91),
                    new Gene(2771, 168),
                    new Gene(85, 484),
                    new Gene(670, 304),
                    new Gene(3370, 382),
                    new Gene(351, 53),
                    new Gene(3903, 147),
                    new Gene(803, 250),
                    new Gene(1931, 476),
                    new Gene(2740, 154),
                    new Gene(855, 344),
                    new Gene(1264, 193),
                    new Gene(1516, 226),
                    new Gene(2718, 199),
                    new Gene(1323, 443),
                    new Gene(1582, 209),
                    new Gene(4357, 404),
                    new Gene(1362, 167),
                    new Gene(2541, 298),
                    new Gene(2217, 471),
                    new Gene(2093, 205),
                    new Gene(1917, 148),
                    new Gene(1238, 296),
                    new Gene(3740, 155),
                    new Gene(3682, 492),
                    new Gene(1362, 482),
                    new Gene(2589, 308),
                    new Gene(481, 404),
                    new Gene(2888, 496),
                    new Gene(887, 2),
                    new Gene(671, 419),
                    new Gene(2519, 219),
                    new Gene(1730, 104),
                    new Gene(1116, 1),
                    new Gene(2655, 48),
                    new Gene(4769, 51),
                    new Gene(3859, 465),
                    new Gene(2997, 126),
                    new Gene(143, 187),
                    new Gene(3030, 437),
                    new Gene(763, 137),
                    new Gene(874, 207),
                    new Gene(1294, 454),
                    new Gene(224, 215),
                    new Gene(4433, 322),
                    new Gene(2268, 344),
                    new Gene(3146, 482),
                    new Gene(2130, 281),
                    new Gene(4672, 329),
                    new Gene(1649, 392),
                    new Gene(3611, 345),
                    new Gene(4708, 70),
                    new Gene(4347, 253),
                    new Gene(1152, 7),
                    new Gene(3114, 499),
                    new Gene(373, 153),
                    new Gene(2348, 443),
                    new Gene(3769, 460),
                    new Gene(1192, 258),
                    new Gene(1055, 237),
                    new Gene(3028, 403),
                    new Gene(362, 430)
                };

                return genes3;
            }
        }
    }
}
