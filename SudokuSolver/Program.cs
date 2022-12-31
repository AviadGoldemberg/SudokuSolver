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
            string boardStr = "0000000000000000";
            ISudokuBoard board = new ArraySudokuBoard(boardStr);
            ISudokuSolver solver = new DancingLinksSolver(board);
            SolvingResult solvingResult = solver.Solve();
            if (solvingResult.IsSolved)
            {
                Console.WriteLine($"Solved in {solvingResult.SolvingTime}ms");
                Console.WriteLine(board.BoardOutput());
            }
            else
            {
                Console.WriteLine($"Can't solve the board! took {solvingResult.SolvingTime}ms");
            }
        }
    }
}
