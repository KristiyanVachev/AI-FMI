using System;
using System.Collections.Generic;
using System.Linq;

namespace NQueens
{
    class Startup
    {
        public static void Main()
        {
            int n = 4;

            var queens = new List<Tuple<int, int>>();
            //var queens = new int[n];

            //Initial queen placement
            for (int y = 0; y < n; y++)
            {
                var x = GetLowestConflictingX(y, queens, n);
                //queens[y] = x;
                queens.Add(Tuple.Create(x, y));
            }

            var randomGenerator = new Random();
            //Choose random row. Then choose highest conflicting.
            //while (true)
            //{
            //    var randomColumn = randomGenerator.Next(0, n);

            //    //Find the least conflicted X
            //    var lowestConflicts = int.MaxValue;
            //    var xCandidates = new List<int>();

            //    for (int x = 0; x < n; x++)
            //    {
            //        var currentXConflicts = ConflictsForQueen(x, randomColumn, queens);

            //        if (currentXConflicts < lowestConflicts)
            //        {
            //            lowestConflicts = currentXConflicts;
            //            xCandidates.Clear();
            //            xCandidates.Add(x);
            //        }

            //        if (currentXConflicts == lowestConflicts)
            //        {
            //            xCandidates.Add(x);
            //        }
            //    }

            //    var newX = xCandidates[randomGenerator.Next(0, xCandidates.Count())];

            //    //queens[randomColumn].Item1 = newX;

            //}


            //Check all possible switches for conflicting, that gives lowest conflicts.

            Print(queens, n);
        }

        private static int GetLowestConflictingX(int y, List<Tuple<int, int>> queens, int n)
        {
            //Item1 = x, Item2 = conflicts for x
            var conflictsPerX = new List<Tuple<int, int>>();

            for (int x = 0; x < n; x++)
            {
                var conflictsForX = ConflictsForQueen(x, y, queens);

                if (conflictsForX == 0)
                {
                    return x;
                }

                conflictsPerX.Add(Tuple.Create(x, conflictsForX));
            }

            return conflictsPerX.OrderBy(x => x.Item2).FirstOrDefault().Item1;
        }

        //TODO test
        private static int ConflictsForQueen(int x, int y, List<Tuple<int, int>> queens)
        {
            var sameRow = queens.Count(q => q.Item2 == y);
            var sameColumn = queens.Count(q => q.Item1 == x);
            var downUpDiagonal = queens.Count(q => q.Item1 + q.Item2 == x + y);
            var upDownDiagonal = queens.Count(q => q.Item1 - x == q.Item2 - y);

            return sameRow + sameColumn + downUpDiagonal + upDownDiagonal;
        }

        private static void Print(List<Tuple<int, int>> queens, int n)
        {
            var orderedQueens = queens.OrderBy(x => x.Item1).ThenBy(x => x.Item2).ToList();
            var currentQueen = 0;

            //X indeces
            Console.Write(" |");
            for (int x = 0; x < n; x++)
            {
                Console.Write(x + "|");
            }
            Console.WriteLine();

            for (int x = 0; x < n; x++)
            {
                Console.Write(x + "|");
                for (int y = 0; y < n; y++)
                {
                    if (currentQueen < orderedQueens.Count() && orderedQueens[currentQueen].Item1 == x && orderedQueens[currentQueen].Item2 == y)
                    {
                        Console.Write("Q|");
                        currentQueen++;
                    }
                    else
                    {
                        if ((x + y) % 2 == 0)
                        {
                            Console.Write("▓|");
                        }
                        else
                        {
                            Console.Write("░|");
                        }
                    }
                }
                Console.WriteLine();
            }
        }
    }
}
