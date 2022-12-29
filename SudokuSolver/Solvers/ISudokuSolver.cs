using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuSolver.Board;

namespace SudokuSolver.Solvers
{
    /// <summary>
    /// Abstract class which represent a sudoku solver.
    /// </summary>
    internal abstract class ISudokuSolver
    {
        protected ISudokuBoard _board;
        public ISudokuSolver(ISudokuBoard board)
        {
            _board = board;
        }
        public abstract SolvingResult Solve();

    }
}
