using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace TicTacToe
{
    public class Startup
    {
        const char AISign = '█';
        const char HumanSign = '▒';

        public static void Main()
        {
            //Choose turn
            bool isAiTurn = ReadTurn();
            var board = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8' };

            //bool isAiTurn = true;
            //var board = new char[] {
            //    HumanSign, '1', AISign,
            //    AISign, '4', AISign,
            //    '6', HumanSign, HumanSign
            //};

            int move;

            for (int i = 0; i <= 9; i++)
            {
                Console.Clear();
                Console.WriteLine("Human: {0} AI: {1}", HumanSign, AISign);
                PrintBoard(board);

                //Check for winner
                var boardResult = CheckFinalBoard(board, AISign);
                if (boardResult != 0)
                {
                    if (boardResult > 0)
                    {
                        Console.WriteLine("AI Wins");
                    }
                    else
                    {
                        Console.WriteLine("Humman wins");
                    }

                    break;
                }
                else
                {
                    if (i == 9)
                    {
                        Console.WriteLine("Draw");
                        break;
                    }
                }

                if (isAiTurn)
                {
                    Console.WriteLine("AI's move:");
                    move = GetMove(board, isAiTurn).Item1;
                    board[move] = AISign;
                }
                else
                {
                    Console.WriteLine("Choose available block number:");

                    while (true)
                    {
                        var inputMove = Console.ReadLine();

                        if (inputMove.Length == 1 && Regex.IsMatch(inputMove, @"^[0-9]+$") && board.Contains(char.Parse(inputMove)))
                        {
                            move = int.Parse(inputMove);
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid number!");
                        }
                    }

                    board[move] = HumanSign;
                }

                isAiTurn = !isAiTurn;
            }



            //var move = GetMove(initialBoard, 0, isAiTurn);


            //PrintBoard(initialBoard);

            //While end
            //First player move
            //If player, read input
            //If AI, GetAIMove - returns state. Check if winning. Else next move.

        }

        private static Tuple<int, int> GetMove(char[] board, bool maximize)
        {
            char currentPlayerSign = maximize ? AISign : HumanSign;

            //If multiple moves are available, explore each of them
            var movesResults = new List<Tuple<int, int>>();

            for (int i = 0; i < board.Length; i++)
            {
                if (board[i] != AISign && board[i] != HumanSign)
                {
                    //Make the move
                    char[] newBoard = new char[board.Length];
                    Array.Copy(board, newBoard, board.Length);

                    newBoard[i] = currentPlayerSign;

                    //Check if board is full. Return if it is. Else, recurse.
                    //Check if move is winning or full
                    var boardScore = CheckFinalBoard(newBoard, AISign);
                    bool boardIsFull = newBoard.Count(x => x == AISign) + newBoard.Count(x => x == HumanSign) == newBoard.Length;

                    if (boardScore != 0 || boardIsFull)
                    {
                        movesResults.Add(Tuple.Create(i, boardScore));
                    }
                    else
                    {
                        movesResults.Add(Tuple.Create(i, GetMove(newBoard, !maximize).Item2));
                    }
                }
            }

            //Choose the best result if maximize or the worst if minimize
            return maximize ? movesResults.OrderBy(x => x.Item2).LastOrDefault() : movesResults.OrderBy(x => x.Item2).FirstOrDefault();
        }

        private static void PrintBoard(char[] board)
        {
            Console.WriteLine("|" + board[0] + "|" + board[1] + "|" + board[2] + "|");
            Console.WriteLine("|" + board[3] + "|" + board[4] + "|" + board[5] + "|");
            Console.WriteLine("|" + board[6] + "|" + board[7] + "|" + board[8] + "|");
        }

        private static int CheckFinalBoard(char[] board, char playerSign)
        {
            //Check horizontally
            for (int i = 0; i < 9; i += 3)
            {
                if (board[i] == board[i + 1] && board[i + 1] == board[i + 2])
                {
                    return board[i] == playerSign ? 10 : -10;
                }
            }

            //Check vertically
            for (int i = 0; i < 3; i++)
            {
                if (board[i] == board[i + 3] && board[i + 3] == board[i + 6])
                {
                    return board[i] == playerSign ? 10 : -10;
                }
            }

            //Check diagonals
            if (board[0] == board[4] && board[4] == board[8])
            {
                return board[0] == playerSign ? 10 : -10;
            }

            if (board[2] == board[4] && board[4] == board[6])
            {
                return board[2] == playerSign ? 10 : -10;
            }

            //Draw
            return 0;
        }

        private static bool ReadTurn()
        {
            Console.WriteLine("Choose who goes first. 0 for human, 1 for AI.");
            var input = Console.ReadLine();

            bool isAiTurn;

            switch (input)
            {
                case "0":
                    isAiTurn = false;
                    break;
                case "1":
                    isAiTurn = true;
                    break;
                default:
                    Console.WriteLine("Seems like you are too dumb to follow a simple request! You'll need all the help you can get. It's the your turn.");
                    Console.ReadLine();
                    isAiTurn = false;
                    break;
            }

            return isAiTurn;
        }
    }
}
