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
        private readonly int _sectorSize;

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
            _sectorSize = (int)Math.Sqrt(_size);
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
            for (int i = 1; i <= _size; i++)
            {
                // if can insert the number update the board and continue recursion.
                if (isValid(i, emptyCellPos))
                {
                    _board[row, col].Val = i;
                    if (SolveBoard())
                        return true;
                    
                    // if we get here, the number that was gussed is invalid. so reset the number and continue searching.
                    _board[row, col].Val = 0;
                }
            }

            return false;
        }

        /// <summary>
        /// Method which check if number is valid in some position at the sudoku board.
        /// </summary>
        /// <param name="num">Number to check.</param>
        /// <param name="pos">Position of the number to check.</param>
        /// <returns></returns>
        private bool isValid(int num, Position pos)
        {
            // check row and col.
            for (int index = 0; index < _size; index++)
            {
                if (_board[pos.Row, index].Val == num) { return false; }
                if (_board[index, pos.Column].Val == num) { return false; }
            }

            // check box.
            int rowBoxStart = _sectorSize * (pos.Row / _sectorSize);
            int colBoxStart = _sectorSize * (pos.Column / _sectorSize);
            int rowBoxEnd = rowBoxStart + _sectorSize;
            int colBoxEnd = colBoxStart + _sectorSize;
            for (int row = rowBoxStart; row < rowBoxEnd; row++)
                for (int col = colBoxStart; col < colBoxEnd; col++)
                    if (_board[row, col].Val == num)
                        return false;
            
            // if there is no number in the row, column and box, num is valid.
            return true;
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
