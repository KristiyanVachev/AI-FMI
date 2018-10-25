using System;
using System.Collections.Generic;

namespace Frogs
{
    public class Node
    {
        public char[] Field { get; set; }

        public int HoleIndex { get; set; }

        public Node Parent { get; set; }

        public List<Node> Children{ get; set; }

        public Node(Node parent, char[] field, int holeIndex)
        {
            this.Parent = parent;
            this.Field = field;
            this.HoleIndex = holeIndex;
            this.Children = new List<Node>();
        }

        public void PrintField()
        {
            foreach (var item in Field)
            {
                Console.Write(item);
            }

            Console.WriteLine();
        }

        public override string ToString()
        {
            var step = "";

            foreach (var item in Field)
            {
                step += item;
            }

            return step;
        }
    }
}
