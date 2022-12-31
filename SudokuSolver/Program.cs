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
            string boardStr = "10023400<06000700080007003009:6;0<00:0010=0;00>0300?200>000900<0=000800:0<201?000;76000@000?005=000:05?0040800;0@0059<00100000800200000=00<580030=00?0300>80@000580010002000=9?000<406@0=00700050300<0006004;00@0700@050>0010020;1?900=002000>000>000;0200=3500<";
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
