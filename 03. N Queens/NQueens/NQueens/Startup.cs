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
            //There is a queen on each row. The array holds the column possition for each row.
            var queens = new int[n];

            //Initial queen placement
            for (int row = 0; row < n; row++)
            {
                var x = GetLowestConflictingColumn(row, queens, row);
                queens[row] = x;
            }

            //Keep conflicts count for each queen.
            var queenConflicts = new int[n];
            for (int row = 0; row < queenConflicts.Length; row++)
            {
                queenConflicts[row] = GetConflictsForQueen(queens[row], row, queens, queens.Length);
            }

            var randomGenerator = new Random();

            //1. Choose random row. 
            //2. TODO Choose highest conflicting.
            //while (true)
            for (int i = 0; i < 1000000; i++)
            {
                //var randomColumn = randomGenerator.Next(0, n);
                //TODO chooese higest conflicting row.
                int maxValue = queenConflicts.Max();
                int maxValueIndex = queenConflicts.ToList().IndexOf(maxValue);
                
                //TODO: If column has 0 conflicts = break while.
                if (GetConflictsForQueen(queens[maxValueIndex], maxValueIndex, queens, queens.Length) == 0)
                {
                    continue;
                }

                //Find the least conflicted column
                var lowestConflicts = int.MaxValue;
                var xCandidates = new List<int>();

                for (int x = 0; x < n; x++)
                {
                    var currentXConflicts = GetConflictsForQueen(x, maxValueIndex, queens, queens.Length);

                    if (currentXConflicts < lowestConflicts)
                    {
                        lowestConflicts = currentXConflicts;
                        xCandidates.Clear();
                        xCandidates.Add(x);
                    }

                    if (currentXConflicts == lowestConflicts)
                    {
                        xCandidates.Add(x);
                    }
                }

                var newX = xCandidates[randomGenerator.Next(0, xCandidates.Count())];

                queens[maxValueIndex] = newX;
            }

            //Check all possible switches for conflicting, that gives lowest conflicts.

            Print(queens, n);
        }

        private static int GetLowestConflictingColumn(int row, int[] queens, int queensCount)
        {
            //Item1 = column, Item2 = conflicts for column
            var conflictsPerColummn = new List<Tuple<int, int>>();

            //Check the conflicts for each X on the row, and choose a X with lowest conflicts.
            for (int column = 0; column < queens.Length; column++)
            {
                var conflictsForX = GetConflictsForQueen(column, row, queens, queensCount);

                if (conflictsForX == 0)
                {
                    return column;
                }

                conflictsPerColummn.Add(Tuple.Create(column, conflictsForX));
            }

            //TODO choose random if more than one with lowest conflicts.
            return conflictsPerColummn.OrderBy(x => x.Item2).FirstOrDefault().Item1;
        }

        private static int GetConflictsForQueen(int colummn, int row, int[] queens, int queensCount)
        {
            //No need to check for column
            
            //TODO - Counting current queen.

            //Check for each row if it contains another queen on the column, the-queen is.
            var sameColumn = 0;
            for (int currentRow = 0; currentRow < queensCount; currentRow++)
            {
                if (queens[currentRow] == colummn)
                {
                    sameColumn++;
                }
            }

            //Check if the diagonal, running from bottom to top, contains queens.
            var downUpDiagonal = 0;
            for (int currentRow = 0; currentRow < queensCount; currentRow++)
            {
                if (currentRow + queens[currentRow] == colummn + row)
                {
                    downUpDiagonal++;
                }
            }

            //Check if the diagonal, running from top to bottom, contains queens.
            var upDownDiagonal = 0;
            for (int currentRow = 0; currentRow < queensCount; currentRow++)
            {
                if (queens[currentRow] - colummn == currentRow -row)
                {
                    upDownDiagonal++;
                }
            }

            return sameColumn + downUpDiagonal + upDownDiagonal;

            //RIP elegant solution
            //var sameRow = queens.Count(q => q.Item2 == y);
            //var downUpDiagonal = queens.Count(q => q.Item1 + q.Item2 == x + y);
            //var upDownDiagonal = queens.Count(q => q.Item1 - x == q.Item2 - y);
        }

        private static void Print(int[] queens, int n)
        {
            Console.Write(" |");
            for (int x = 0; x < n; x++)
            {
                Console.Write(x + "|");
            }
            Console.WriteLine();

            for (int y = 0; y < n; y++)
            {
                Console.Write(y + "|");
                for (int x = 0; x < n; x++)
                {
                    if (queens[y] == x)
                    {
                        Console.Write("Q|");
                    }
                    else
                    {
                        if ((y + x) % 2 == 0)
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
