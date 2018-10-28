namespace SlidingBlocks.DataStructure
{
    public class PriorityStack
    {
        public Node Head { get; set; }

        public PriorityStack(BlocksState initialState)
        {
            var newNode = new Node(initialState);
            this.Head = newNode;
        }

        public Node Pop()
        {
            if (this.Head == null)
            {
                return null;
            }

            var head = this.Head;

            this.Head = head.Next;

            return head;
        }

        public void InsertBlocksState(BlocksState newBlocksState)
        {
            var newNode = new Node(newBlocksState);

            //If the stack is empty, put the new node as head.
            if (this.Head == null)
            {
                this.Head = newNode;
            }
            else
            {
                //In the case when the newNode has better distance than the head, switch them right away.
                if (this.Head.Value.ManhattanDistance > newNode.Value.ManhattanDistance)
                {
                    newNode.Next = this.Head;
                    this.Head = newNode;
                }
                else
                {
                    //If the head has better distance than the new node, traverse tha stack while 
                    //a node with worse distance or the end of the stack is found.
                    var previousNode = this.Head;
                    var currentNode = this.Head.Next;

                    while (true)
                    {
                        if (currentNode == null)
                        {
                            previousNode.Next = newNode;
                            break;
                        }

                        if (currentNode.Value.ManhattanDistance > newNode.Value.ManhattanDistance)
                        {
                            previousNode.Next = newNode;
                            newNode.Next = currentNode;
                            break;
                        }

                        currentNode = currentNode.Next;
                    }
                }

            }
        }
    }
}
