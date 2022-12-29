using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuSolver.Solvers.DancingLinksSolver;
using SudokuSolver.Board;

namespace SudokuSolver
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string boardStr = "800000070006010053040600000000080400003000700020005038000000800004050061900002000";
            ISudokuBoard board = new ArraySudokuBoard(9, boardStr);
            DancingLinksSolver solver = new DancingLinksSolver(board);
        }
    }
}
