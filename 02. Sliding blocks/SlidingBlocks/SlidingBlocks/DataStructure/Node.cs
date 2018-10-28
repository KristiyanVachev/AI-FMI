namespace SlidingBlocks.DataStructure
{
    public class Node
    {
        public BlocksState Value { get; set; }

        public Node Next { get; set; }

        public Node(BlocksState value)
        {
            this.Value = value;
        }
    }
}
