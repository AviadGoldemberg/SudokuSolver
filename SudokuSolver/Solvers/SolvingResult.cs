using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Solvers
{
    /// <summary>
    /// Struct which represent solving result of solving sudoku board
    /// </summary>
    internal struct SolvingResult
    {
        public long SolvingTime { get; set; }
        public bool IsSolved { get; set; }

        public SolvingResult(long solvingTime, bool isSolved)
        {
            SolvingTime = solvingTime;
            IsSolved = isSolved;
        }
    }
}
