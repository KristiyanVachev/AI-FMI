using System;
using System.Collections.Generic;
using System.Linq;

namespace NQueens
{
    class Startup
    {
        public static void Main()
        {
            int n = 2000;
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
                queenConflicts[row] = GetConflictsForBlock(queens[row], row, queens, queens.Length);
            }

            //Algorithm
            int highestConflictingRow = GetHighestConflictingQueen(queenConflicts);

            while (highestConflictingRow >= 0)
            {
                var bestNewColumn = GetLowestConflictingColumn(highestConflictingRow, queens, queens.Length);

                queenConflicts = UpdateQueenConflicts(queens, queenConflicts, highestConflictingRow, queens[highestConflictingRow], bestNewColumn);

                queens[highestConflictingRow] = bestNewColumn;

                highestConflictingRow = GetHighestConflictingQueen(queenConflicts);
            }

            Console.WriteLine("Total conflicts: " + queenConflicts.Sum());

            if (queens.Length <= 10)
            {
                Print(queens);
            }
        }

        /// <summary>
        /// Updating the conflicts for each queen. C
        /// Increasing or decreasing the conflict count if the queen has been in the lines of the previous or new
        /// position of the queen being swapped.
        /// </summary>
        /// <param name="queens">Array holding each queen's coordinates. Row/y as the array index, and column/x as the array's value.</param>
        /// <param name="queenConflicts">Array holding the conflicts count for each queen's row.</param>
        /// <param name="row">Queen's row.</param>
        /// <param name="oldColumn">Queen's old column</param>
        /// <param name="newColumn">Queen's new column.</param>
        /// <returns>The array holding the conflicts count for all queens.</returns>
        private static int[] UpdateQueenConflicts(int[] queens, int[] queenConflicts, int row, int oldColumn, int newColumn)
        {
            for (int currentRow = 0; currentRow < queens.Length; currentRow++)
            {
                if (currentRow != row)
                {
                    if (queens[currentRow] == oldColumn)
                    {
                        queenConflicts[currentRow]--;
                    }

                    if (queens[currentRow] == newColumn)
                    {
                        queenConflicts[currentRow]++;
                    }

                    if (currentRow + queens[currentRow] == oldColumn + row)
                    {
                        queenConflicts[currentRow]--;
                    }

                    if (currentRow + queens[currentRow] == newColumn + row)
                    {
                        queenConflicts[currentRow]++;
                    }

                    if (queens[currentRow] - oldColumn == currentRow - row)
                    {
                        queenConflicts[currentRow]--;
                    }

                    if (queens[currentRow] - newColumn == currentRow - row)
                    {
                        queenConflicts[currentRow]++;
                    }
                }
                else
                {
                    queenConflicts[currentRow] = GetConflictsForBlock(queens[row], row, queens, queens.Length);
                }
            }

            return queenConflicts;
        }

        /// <summary>
        /// Find the row of the queen that has the most conflicts. If more than one, return randomly.
        /// </summary>
        /// <param name="queenConflicts">Array holding the conflicts count for each queen's row.</param>
        /// <returns>The row of the queen with the most conflicts.</returns>
        private static int GetHighestConflictingQueen(int[] queenConflicts)
        {
            int maxConflicts = 1;
            var candidates = new List<int>();

            for (int currentRow = 0; currentRow < queenConflicts.Length; currentRow++)
            {
                if (queenConflicts[currentRow] > maxConflicts)
                {
                    maxConflicts = queenConflicts[currentRow];
                    candidates.Clear();
                }

                if (maxConflicts == queenConflicts[currentRow])
                {
                    candidates.Add(currentRow);
                }
            }

            if (candidates.Count == 0)
            {
                return -1;
            }

            var randomGenerator = new Random();
            var randomColumnIndex = randomGenerator.Next(0, candidates.Count());

            return candidates[randomColumnIndex];
        }

        /// <summary>
        /// Check the conflicts for each column of the queen's row and return the one with the lowest conflicts.
        /// If more than one, choose randomly.
        /// </summary>
        /// <param name="row">Queen's row.</param>
        /// <param name="queens">Array holding each queen's coordinates. Row/y as the array index, and column/x as the array's value.</param>
        /// <param name="queensCount">Count of how many queens are initialized in the array so far.</param>
        /// <returns>Index of the column with lowest conflicts.</returns>
        private static int GetLowestConflictingColumn(int row, int[] queens, int queensCount)
        {
            int lowestConflictCound = int.MaxValue;
            var candidates = new List<int>();

            for (int column = 0; column < queens.Length; column++)
            {
                var conflictsForBlock = GetConflictsForBlock(column, row, queens, queensCount);

                if (conflictsForBlock < lowestConflictCound)
                {
                    lowestConflictCound = conflictsForBlock;
                    candidates.Clear();
                }

                if (conflictsForBlock == lowestConflictCound)
                {
                    candidates.Add(column);
                }
            }

            var randomGenerator = new Random();
            var randomColumnIndex = randomGenerator.Next(0, candidates.Count());

            return candidates[randomColumnIndex];
        }

        /// <summary>
        /// Calculating how many conflicts the block has.
        /// </summary>
        /// <param name="colummn">Queen's column</param>
        /// <param name="row">Queen's row.</param>
        /// <param name="queens">Array holding each queen's coordinates. Row/y as the array index, and column/x as the array's value.</param>
        /// <param name="queensCount">Count of how many queens are initialized in the array so far.</param>
        /// <returns>Conflicts count for block.</returns>
        private static int GetConflictsForBlock(int colummn, int row, int[] queens, int queensCount)
        {
            var totalConflicts = 0;

            for (int currentRow = 0; currentRow < queensCount; currentRow++)
            {
                if (currentRow != row)
                {
                    //Another queen on the sam column
                    if (queens[currentRow] == colummn)
                    {
                        totalConflicts++;
                    }

                    //Another queen on the diagonal going from down to up
                    if (currentRow + queens[currentRow] == colummn + row)
                    {
                        totalConflicts++;
                    }

                    //Another queen on the diagonal going from up to down
                    if (queens[currentRow] - colummn == currentRow - row)
                    {
                        totalConflicts++;
                    }

                    //No need to check for queens on the same column.
                }
            }

            return totalConflicts;

            //RIP elegant solution
            //var sameRow = queens.Count(q => q.Item2 == y);
            //var downUpDiagonal = queens.Count(q => q.Item1 + q.Item2 == x + y);
            //var upDownDiagonal = queens.Count(q => q.Item1 - x == q.Item2 - y);
        }

        /// <summary>
        /// Fancy print of the board with queens in place.
        /// </summary>
        /// <param name="queens">Array holding each queen's coordinates. Row/y as the array index, and column/x as the array's value.</param>
        private static void Print(int[] queens)
        {
            Console.Write(" |");
            for (int x = 0; x < queens.Length; x++)
            {
                Console.Write(x + "|");
            }
            Console.WriteLine();

            for (int y = 0; y < queens.Length; y++)
            {
                Console.Write(y + "|");
                for (int x = 0; x < queens.Length; x++)
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
