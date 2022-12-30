using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuSolver.Solvers.DancingLinksSolver;
using SudokuSolver.Solvers;
using SudokuSolver.Board;

namespace SudokuSolver
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string boardStr = "800000070006010053040600000000080400003000700020005038000000800004050061900002000";
            ISudokuBoard board = new ArraySudokuBoard(9, boardStr);
            ISudokuSolver solver = new DancingLinksSolver(board);
            SolvingResult solvingResult = solver.Solve();
            if (solvingResult.IsSolved)
            {
                Console.WriteLine($"Solved in {solvingResult.SolvingTime}ms");
                for (int i = 0; i < board.GetBoardSize(); i++)
                {
                    for (int j = 0; j < board.GetBoardSize(); j++)
                    {
                        Console.Write(board[i, j].Val.ToString() + " ");
                    }
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine($"Can't solve the board! took {solvingResult.SolvingTime}ms");
            }
        }
    }
}
