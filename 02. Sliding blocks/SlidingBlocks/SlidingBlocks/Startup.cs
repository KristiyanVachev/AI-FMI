using SlidingBlocks.DataStructure;
using System;
using System.Collections.Generic;

namespace SlidingBlocks
{
    class Startup
    {
        static void Main()
        {
            var blocks = new int[,]
            {
                { 1, 2, 3 },
                { 4, 5, 6 },
                { 0, 7, 8 }
            };

            var initialState = new BlocksState(blocks, CalculateManhattanDistance(blocks), "");

            if (initialState.ManhattanDistance == 0)
            {
                Console.WriteLine("Initial state is the solution.");
            }
            else
            {
                //"Priority stack" holding candidates for boards closest to the winning one. Ordered by lowest manhattan distance.
                var stack = new PriorityStack(initialState);
                
                //Keeping all visited blocks possitions in 1 dimensional form.
                var visited = new List<string>
                {
                    initialState.ToString()
                };

                //Finding the solution
                var solution = FindSolution(stack, visited);

                //Printing the result
                if (solution != null)
                {
                    Console.WriteLine(solution.Road.Length);

                    for (int i = 0; i < solution.Road.Length; i++)
                    {
                        switch (solution.Road[i])
                        {
                            case '1':
                                Console.WriteLine("left");
                                break;
                            case '2':
                                Console.WriteLine("right");
                                break;
                            case '3':
                                Console.WriteLine("up");
                                break;
                            case '4':
                                Console.WriteLine("down");
                                break;
                            default:
                                break;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No solution found");
                }
            }
        }

        private static BlocksState FindSolution(PriorityStack stack, List<string> visited)
        {
            //Starting by taking the initial state from the stack.
            var currentState = stack.Pop();

            while (currentState != null)
            {
                //Getting the coordinates of the blank block
                var zeroCoordinates = GetZeroCoordinates(currentState.Value.Blocks);
                var zeroX = zeroCoordinates.Item1;
                var zeroY = zeroCoordinates.Item2;

                //Coordinates for all the blocks that could move to the blank.
                var moves = new List<Tuple<int, int>>();
                moves.Add(Tuple.Create(zeroX, zeroY + 1)); //Left
                moves.Add(Tuple.Create(zeroX, zeroY - 1)); //Right
                moves.Add(Tuple.Create(zeroX + 1, zeroY)); //Up
                moves.Add(Tuple.Create(zeroX - 1, zeroY)); //Down

                //Direction the block is moving (0 - left, 1 - right...). Mapped to string values on output.
                int direction = 0;

                foreach (var move in moves)
                {
                    //The road the current node has taken plus the current direction.
                    direction++;
                    var road = currentState.Value.Road + direction;

                    //If the move is legal, we create a new state.
                    var newBlocksState = CreateNewBlocksState(currentState.Value.Blocks, zeroX, zeroY, move.Item1, move.Item2, road);

                    if (newBlocksState != null)
                    {
                        //A winning state is found.
                        if (newBlocksState.ManhattanDistance == 0)
                        {
                            return newBlocksState;
                        }

                        //If the state has already been visited, don't add it to the stack.
                        if (!visited.Contains(newBlocksState.ToString()))
                        {
                            stack.InsertBlocksState(newBlocksState);
                            visited.Add(newBlocksState.ToString());
                        }
                    }
                }

                //Checking the next node with the lowest manhattan distance.
                currentState = stack.Pop();
            }

            return null;
        }

        private static BlocksState CreateNewBlocksState(int[,] blocks, int zeroX, int zeroY, int newZeroX, int newZeroY, string road)
        {
            //If there is a tile inside the matrix.
            if (newZeroX < 0 || newZeroX >= blocks.GetLength(0) || newZeroY < 0 || newZeroY >= blocks.GetLength(1))
            {
                return null;
            }

            //Create a new array and make the move
            var newBlocks = blocks.Clone() as int[,];

            newBlocks[zeroX, zeroY] = newBlocks[newZeroX, newZeroY];
            newBlocks[newZeroX, newZeroY] = 0;

            //Calculating the manhattan distance.
            var newBlocksDistance = CalculateManhattanDistance(newBlocks);

            return new BlocksState(newBlocks, newBlocksDistance, road);
        }

        /// <summary>
        /// Finding the coordinates of the 0 block.
        /// </summary>
        /// <param name="blocks">2 dimensional array holding the blocks</param>
        /// <returns>Tuple holding the x and y coordinates for the 0 block.</returns>
        private static Tuple<int, int> GetZeroCoordinates(int[,] blocks)
        {
            for (int x = 0; x < blocks.GetLength(0); x++)
            {
                for (int y = 0; y < blocks.GetLength(1); y++)
                {
                    if (blocks[x, y] == 0)
                    {
                        return Tuple.Create(x, y);
                    }
                }
            }

            return Tuple.Create(-1, -1);
        }

        /// <summary>
        /// Calculating the Manhattan distance for the board by checking the the distance for each of the blocks
        /// from it's current possition to it's target possition.
        /// </summary>
        /// <param name="blocks">2 dimensional array holding the blocks</param>
        /// <returns>The sum of manhattan distance for each block.</returns>
        private static int CalculateManhattanDistance(int[,] blocks)
        {
            int manhattanDistance = 0;

            for (int x = 0; x < blocks.GetLength(0); x++)
            {
                for (int y = 0; y < blocks.GetLength(1); y++)
                {
                    int blockValue = blocks[x, y];

                    if (blockValue == 0)
                    {
                        continue;
                    }

                    int targetX = (blockValue - 1) / blocks.GetLength(0);
                    int targetY = (blockValue - 1) % blocks.GetLength(0);

                    int distanceToX = x - targetX;
                    int distanceToY = y - targetY;

                    manhattanDistance += (Math.Abs(distanceToX) + Math.Abs(distanceToY));
                }
            }

            return manhattanDistance;
        }
    }
}
