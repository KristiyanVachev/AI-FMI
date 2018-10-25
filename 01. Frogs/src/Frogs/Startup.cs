using System;
using System.Collections.Generic;

namespace Frogs
{
    class Startup
    {
        static void Main(string[] args)
        {
            //Input
            int n = 2;

            //Creating field and tree
            var field = CreateField(n);

            var root = new Node(null, field, n);
            var tree = new Tree(root);

            //Algorithm

            var winner = FindWinner(root);

            if (winner != null)
            {
                var path = new List<string>();

                var currentNode = winner;

                while (currentNode != null)
                {
                    path.Add(currentNode.ToString());
                    currentNode = currentNode.Parent;
                }

                path.Reverse();

                foreach (var step in path)
                {
                    Console.WriteLine(step);
                }
            }
            else
            {
                Console.WriteLine("No solution.");
            }
        }

        private static Node FindWinner(Node node)
        {
            //Indexes of each frog that is close enough to make a jump
            var frogsToJumpIndexes = new int[4];
            frogsToJumpIndexes[0] = node.HoleIndex - 2;
            frogsToJumpIndexes[1] = node.HoleIndex - 1;
            frogsToJumpIndexes[2] = node.HoleIndex + 1;
            frogsToJumpIndexes[3] = node.HoleIndex + 2;

            for (int i = 0; i < 4; i++)
            {
                //Check if there is a frog at the possition or if the frog is facing the hole
                if (frogsToJumpIndexes[i] >= 0 && frogsToJumpIndexes[i] < node.Field.Length &&
                    FrogIsInTheRightDirection(node.HoleIndex, frogsToJumpIndexes[i], node.Field[frogsToJumpIndexes[i]]))
                {
                    //Create new field
                    var newField = new char[node.Field.Length];
                    Array.Copy(node.Field, newField, node.Field.Length);

                    //Make the jump (switch the places of the hole and the frog)
                    newField[node.HoleIndex] = node.Field[frogsToJumpIndexes[i]];
                    newField[frogsToJumpIndexes[i]] = '_';

                    //Create and add the new node
                    var newNode = new Node(node, newField, frogsToJumpIndexes[i]);
                    node.Children.Add(newNode);

                    //Check if field wins
                    if (FieldIsWinning(newNode.Field))
                    {
                        return newNode;
                    }
                    else
                    {
                        var winner = FindWinner(newNode);

                        if (winner != null)
                        {
                            return winner;
                        }
                    }
                }
            }

            return null;
        }

        private static bool FrogIsInTheRightDirection(int holeIndex, int frogIndex, char frog)
        {
            if (frog == '>' && holeIndex < frogIndex)
            {
                return false;
            }

            if (frog == '<' && holeIndex > frogIndex)
            {
                return false;
            }

            return true;
        }

        private static bool FieldIsWinning(char[] field)
        {
            for (int i = 0; i < field.Length / 2; i++)
            {
                if (field[i] == '>')
                {
                    return false;
                }
            }

            if (field[field.Length / 2] != '_')
            {
                return false;
            }

            for (int i = field.Length / 2 + 1; i < field.Length; i++)
            {
                if (field[i] == '<')
                {
                    return false;
                }
            }

            return true;
        }

        private static char[] CreateField(int n)
        {
            char[] field = new char[n + n + 1];

            for (int i = 0; i < n; i++)
            {
                field[i] = '>';
            }

            field[n] = '_';

            for (int i = n + 1; i < n + n + 1; i++)
            {
                field[i] = '<';
            }

            return field;
        }
    }
}
