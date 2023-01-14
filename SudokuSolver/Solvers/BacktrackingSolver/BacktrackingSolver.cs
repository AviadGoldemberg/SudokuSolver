using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using SudokuSolver.Board;

namespace SudokuSolver.Solvers.BacktrackingSolver
{
    internal class BacktrackingSolver : ISudokuSolver
    {
        private readonly int _size;

        /// <summary>
        /// Represents Position in sudoku board.
        /// </summary>
        private class Position
        {
            public int Row { get; set; }
            public int Column { get; set; }
            public Position(int row, int col)
            {
                Row = row;
                Column = col;
            }
        }

        public BacktrackingSolver(ISudokuBoard board) : base(board)
        {
            _size = board.GetBoardSize();
        }

        /// <summary>
        /// Method which solve a sudoku board using Backtracking.
        /// </summary>
        /// <returns>Solving result</returns>
        public override SolvingResult Solve()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            stopwatch.Start();

            bool isSolved = SolveBoard();

            stopwatch.Stop();

            return new SolvingResult(stopwatch.ElapsedMilliseconds, isSolved);
        }

        /// <summary>
        /// Method which solve sudoku board using Backtracking.
        /// </summary>
        /// <returns>If the board was solved or not.</returns>
        private bool SolveBoard()
        {
            // find next empty cell.
            Position emptyCellPos = FindEmptyCell();
            // if there is no more empty cell, solution was found.
            if (emptyCellPos == null)
                return true;

            // getting the empty cell row and col.
            int row = emptyCellPos.Row;
            int col = emptyCellPos.Column;

            // iterate each possible number
            for (int number = 1; number <= _size; number++)
            {
                // if can insert the number update the board and continue recursion.
                if (_board.IsValid(number, row, col))
                {
                    _board[row, col].Val = number;
                    if (SolveBoard())
                        return true;
                    
                    // if we get here, the number that was gussed is invalid. so reset the number and continue searching.
                    _board[row, col].Val = 0;
                }
            }

            return false;
        }

        /// <summary>
        /// Method which find the next empty cell in the sudoku board.
        /// </summary>
        /// <returns>The next empty cell at the Sudoku board. If there is not empty cell return null.</returns>
        private Position FindEmptyCell()
        {
            for (int row = 0; row < _size; row++)
                for (int col = 0; col < _size; col++)
                    if (_board[row, col].Val == 0)
                        return new Position(row, col);
            return null;
        }
    }
}
