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

            //When AI is in a disadvantage and has no winning move (if human plays best) 
            //it still plays to prolong it's defeat.

            //bool isAiTurn = true;
            //var board = new char[] {
            //    HumanSign, '1', '2',
            //    HumanSign, '4', '5',
            //    '6', '7', '8'
            //};

            int move;

            for (int i = 0; i <= 9; i++)
            {
                Console.Clear();
                Console.WriteLine("Human: {0} AI: {1}", HumanSign, AISign);
                PrintBoard(board);

                //Check for winner
                var boardResult = CheckFinalBoard(board);
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
                    move = GetMove(board, isAiTurn, Tuple.Create(-1, int.MinValue), Tuple.Create(-1, int.MaxValue)).Item1;
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
        }

        /// <summary>
        /// Returns the best possible move the player can make. Implements alpha-beta pruning.
        /// </summary>
        /// <param name="board">The current board filled with player signs.</param>
        /// <param name="maximize">Whether the player is maximizer or minimizer.</param>
        /// <param name="alpha">Score of the best move the maximizer can make.</param>
        /// <param name="beta">Score of the best move the minimizer can make.</param>
        /// <returns>Tuple with Item1 being the board tile index, and Item2 the score of the move.</int></returns>
        private static Tuple<int, int> GetMove(char[] board, bool maximize, Tuple<int, int> alpha, Tuple<int, int> beta)
        {
            char currentPlayerSign = maximize ? AISign : HumanSign;
            var currentBestScore = maximize ? beta : alpha;

            //If multiple moves are available, explore each of them
            var movesResults = new List<Tuple<int, int>>();

            //For each board tile
            for (int i = 0; i < board.Length; i++)
            {
                //If it's not filled
                if (board[i] != AISign && board[i] != HumanSign)
                {
                    //Make the move
                    char[] newBoard = new char[board.Length];
                    Array.Copy(board, newBoard, board.Length);
                    newBoard[i] = currentPlayerSign;
                    
                    //Get the best score for the current move
                    var boardScore = CheckFinalBoard(newBoard);
                    bool boardIsFull = newBoard.Count(x => x == AISign) + newBoard.Count(x => x == HumanSign) == newBoard.Length;

                    Tuple<int, int> bestScore = boardScore != 0 || boardIsFull
                        ? Tuple.Create(i, boardScore)
                        : Tuple.Create(i, GetMove(newBoard, !maximize, maximize ? alpha : currentBestScore, !maximize ? beta : currentBestScore).Item2);

                    //Add the new move as a possible move
                    movesResults.Add(bestScore);

                    //Alpha-beta pruning
                    if (maximize)
                    {
                        if (bestScore.Item2 > beta.Item2)
                        {
                            break;
                        }
                        else
                        {
                            currentBestScore = bestScore;
                        }
                    }
                    else
                    {
                        if (bestScore.Item2 < alpha.Item2)
                        {
                            break;
                        }
                        else
                        {
                            currentBestScore = bestScore;
                        }
                    }

                }
            }

            //Choose the best result if maximize or the worst if minimize
            return maximize ? movesResults.OrderBy(x => x.Item2).ThenBy(x => x.Item1).LastOrDefault() : movesResults.OrderBy(x => x.Item2).ThenBy(x => x.Item1).FirstOrDefault();
        }

        /// <summary>
        /// Pretty print of the board.
        /// </summary>
        /// <param name="board"></param>
        private static void PrintBoard(char[] board)
        {
            Console.WriteLine("|" + board[0] + "|" + board[1] + "|" + board[2] + "|");
            Console.WriteLine("|" + board[3] + "|" + board[4] + "|" + board[5] + "|");
            Console.WriteLine("|" + board[6] + "|" + board[7] + "|" + board[8] + "|");
        }

        /// <summary>
        /// Checks if the board is in a final state.
        /// </summary>
        /// <param name="board"></param>
        /// <returns>10 if the AI wins, -10 if the human wins and 0 in any other case.</returns>
        private static int CheckFinalBoard(char[] board)
        {
            int movesLeft = board.Length - board.Count(x => x == AISign || x == HumanSign);
            int score = 1 + movesLeft;

            //Check horizontally
            for (int i = 0; i < 9; i += 3)
            {
                if (board[i] == board[i + 1] && board[i + 1] == board[i + 2])
                {
                    return board[i] == AISign ? score : -score;
                }
            }

            //Check vertically
            for (int i = 0; i < 3; i++)
            {
                if (board[i] == board[i + 3] && board[i + 3] == board[i + 6])
                {
                    return board[i] == AISign ? score : -score;
                }
            }

            //Check diagonals
            if (board[0] == board[4] && board[4] == board[8])
            {
                return board[0] == AISign ? score : -score;
            }

            if (board[2] == board[4] && board[4] == board[6])
            {
                return board[2] == AISign ? score : -score;
            }

            //Draw
            return 0;
        }

        /// <summary>
        /// Prompts the human to enter who's turn it is.
        /// </summary>
        /// <returns></returns>
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
