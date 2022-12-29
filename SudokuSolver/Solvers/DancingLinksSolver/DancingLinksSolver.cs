using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuSolver.Board;

namespace SudokuSolver.Solvers.DancingLinksSolver
{
    internal class DancingLinksSolver : ISudokuSolver
    {
        private readonly int _size;
        private readonly int _sectorSize;
        public DancingLinksSolver(ISudokuBoard board) : base(board)
        {
            _size = board.GetBoardSize();
            _sectorSize = (int)Math.Sqrt(_size);
            ConvertBoardToMatrix();
        }

        public override SolvingResult Solve()
        {
            SolvingResult solvingResult = new SolvingResult();
            return solvingResult;
        }

        /// <summary>
        /// Converts a sudoku board into a matrix of 1's and 0's for the Dancing Links algorithm
        /// </summary>
        /// <returns>Matrix representing the converted sudoku board.</returns>
        private int[,] ConvertBoardToMatrix()
        {

            // init matrix with 0.
            int[,] matrix = new int[_size * _size * _size, _size * _size * 4];
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    matrix[i, j] = 0;
                }
            }

            // create the matrix.
            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    int value = _board[i, j].Val;
                    // if there is need to append 1's to the matrix
                    if (value != 0)
                    {
                        SetMatrixValue(matrix, i, j, value - 1,  1);
                    }
                }
            }
            return matrix;
        }

        /// <summary>
        /// Sets the specified value in the given matrix for the cell at the specified row and column, as well as for the corresponding row, column, and subgrid.
        /// </summary>
        /// <param name="matrix">Matrix which represent the Dancing Links matrix.</param>
        /// <param name="row">Row index</param>
        /// <param name="column">Column index</param>
        /// <param name="value">The value to set in the matrix.</param>
        /// <param name="setValue">The value to set in the matrix (1 or 0).</param>
        void SetMatrixValue(int[,] matrix, int row, int column, int value, int setValue)
        {
            // calculate the indices for the corresponding row, column, and subgrid
            int rowIndex = row * _size + value;
            int columnIndex = column * _size + value;
            int subgridRow = row / _sectorSize;
            int subgridColumn = column / _sectorSize;
            int subgridIndex = subgridRow * _sectorSize + subgridColumn;

            // set the value in the matrix.
            matrix[row * _size + column, rowIndex] = setValue;
            matrix[row * _size + column, _size * _size + columnIndex] = setValue;
            matrix[row * _size + column, _size * _size * 2 + subgridIndex] = setValue;
            matrix[row * _size + column, _size * _size * 3 + value] = setValue;
        }
    }
}
