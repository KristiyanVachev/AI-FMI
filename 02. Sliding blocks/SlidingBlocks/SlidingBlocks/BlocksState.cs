namespace SlidingBlocks
{
    public class BlocksState
    {
        public int[,] Blocks { get; set; }

        public int ManhattanDistance { get; set; }

        public string Road { get; set; }

        public BlocksState(int[,] blocks, int manhattanDistance, string road)
        {
            this.Blocks = blocks;
            this.ManhattanDistance = manhattanDistance;
            this.Road = road;
        }

        /// <summary>
        /// Transforming the 2 dimensional blocks array into a 1 dimensional string representation
        /// </summary>
        /// <returns>1 dimensional string representation of the blocks matrix</returns>
        public override string ToString()
        {
            var result = "";

            for (int x = 0; x < Blocks.GetLength(0); x++)
            {
                for (int y = 0; y < Blocks.GetLength(1); y++)
                {
                    result += Blocks[x, y];
                }
            }

            return result;
        }
    }
}
