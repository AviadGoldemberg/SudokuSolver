using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
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
        }

        public override SolvingResult Solve()
        {
            bool isSolved = false;
            // Get the matrix for the Dancing Links algorithm.
            int[,] matrix = ConvertBoardToMatrix(_board);
            // Create Dancing Links solver.
            DancingLinks.DancingLinks DLX = new DancingLinks.DancingLinks(matrix);

            // Solve the board and also calculate the time.
            Stopwatch stopwatch = Stopwatch.StartNew();
            stopwatch.Start();
            isSolved = DLX.Solve();
            stopwatch.Stop();

            return new SolvingResult(stopwatch.ElapsedMilliseconds, isSolved);
        }

        /// <summary>
        /// Method which return the index of row, col and numbner in the matrix for the Dancing Links.
        /// </summary>
        /// <param name="row">Current row.</param>
        /// <param name="column">Current col.</param>
        /// <param name="num">Current num.</param>
        /// <returns></returns>
        private int IndexInCoverMatrix(int row, int column, int num)
        {
            return (row - 1) * _size * _size + (column - 1) * _size + (num - 1);
        }

        /// <summary>
        /// Initializes and creates a matrix used in the Dancing Links algorithm for solving Sudoku puzzles.
        /// The matrix includes constraints for each cell, row, column, and box in the Sudoku board.
        /// Helper method to <see cref="ConvertBoardToMatrix(ISudokuBoard)"/>
        /// </summary>
        /// <returns>A 2D integer array representing the matrix used in the Dancing Links algorithm.</returns>
        private int[,] CreateMatrix()
        {
            int[,] matrix = new int[_size * _size * _size, _size * _size * 4];

            int header = 0;
            CreateCellConstraints(matrix, ref header);
            CreateRowConstraints(matrix, ref header);
            CreateColumnConstraints(matrix, ref header);
            CreateBoxConstraints(matrix, ref header);
            return matrix;
        }

        /// <summary>
        /// Adds constraints for each box in the Sudoku board to the matrix used in the Dancing Links algorithm.
        /// These constraints ensure that each cell in a column is filled with a number that is unique within the box.
        /// Helper method to <see cref="CreateMatrix"/>
        /// </summary>
        /// <param name="matrix">The matrix used in the Dancing Links algorithm.</param>
        /// <param name="header">The current header index in the matrix, where the constraints will be added.</param>
        private void CreateBoxConstraints(int[,] matrix, ref int header)
        {
            for (int row = 1; row <= _size; row += _sectorSize)
            {
                for (int column = 1; column <= _size; column += _sectorSize)
                {
                    for (int num = 1; num <= _size; num++, header++)
                    {
                        for (int rowDelta = 0; rowDelta < _sectorSize; rowDelta++)
                        {
                            for (int columnDelta = 0; columnDelta < _sectorSize; columnDelta++)
                            {
                                int index = IndexInCoverMatrix(row + rowDelta, column + columnDelta, num);
                                matrix[index, header] = 1;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Adds constraints for each column in the Sudoku board to the matrix used in the Dancing Links algorithm.
        /// These constraints ensure that each cell in a column is filled with a number that is unique within the column.
        /// Helper method to <see cref="CreateMatrix"/>
        /// </summary>
        /// <param name="matrix">The matrix used in the Dancing Links algorithm.</param>
        /// <param name="header">The current header index in the matrix, where the constraints will be added.</param>
        private void CreateColumnConstraints(int[,] matrix, ref int header)
        {
            for (int column = 1; column <= _size; column++)
            {
                for (int num = 1; num <= _size; num++, header++)
                {
                    for (int row = 1; row <= _size; row++)
                    {
                        int index = IndexInCoverMatrix(row, column, num);
                        matrix[index,header] = 1;
                    }
                }
            }
        }

        /// <summary>
        /// Adds constraints for each row in the Sudoku board to the matrix used in the Dancing Links algorithm.
        /// These constraints ensure that each cell in a column is filled with a number that is unique within the row.
        /// Helper method to <see cref="CreateMatrix"/>
        /// </summary>
        /// <param name="matrix">The matrix used in the Dancing Links algorithm.</param>
        /// <param name="header">The current header index in the matrix, where the constraints will be added.</param>
        private void CreateRowConstraints(int[,] matrix, ref int header)
        {
            for (int row = 1; row <= _size; row++)
            {
                for (int num = 1; num <= _size; num++, header++)
                {
                    for (int column = 1; column <= _size; column++)
                    {
                        int index = IndexInCoverMatrix(row, column, num);
                        matrix[index, header] = 1;
                    }
                }
            }
        }

        /// <summary>
        /// Adds constraints for each cell in the Sudoku board to the matrix used in the Dancing Links algorithm.
        /// These constraints ensure that each cell in a column is filled with a number that is unique within the cell.
        /// Helper method to <see cref="CreateMatrix"/>
        /// </summary>
        /// <param name="matrix">The matrix used in the Dancing Links algorithm.</param>
        /// <param name="header">The current header index in the matrix, where the constraints will be added.</param>
        private void CreateCellConstraints(int[,] matrix, ref int header)
        {
            for (int row = 1; row <= _size; row++)
            {
                for (int column = 1; column <= _size; column++, header++)
                {
                    for (int num = 1; num <= _size; num++)
                    {
                        int index = IndexInCoverMatrix(row, column, num);
                        matrix[index, header] = 1;
                    }
                }
            }
        }

        /// <summary>
        /// Method which return Sudoku board as matrix for the use of the Dancing Links algorithm.
        /// The method create matrix which represent the Sudoku board as 1's and 0's for the Dancing Links algorithm.
        /// </summary>
        /// <param name="board">Sudoku board to create a matrix for it.</param>
        /// <returns>Matrix for the Dancing Links algorithm.</returns>
        private int[,] ConvertBoardToMatrix(ISudokuBoard board)
        {
            int[,] matrix = CreateMatrix();

            // Taking into account the values already entered in Sudoku's board instance
            for (int row = 1; row <= _size; row++)
            {
                for (int column = 1; column <= _size; column++)
                {
                    int currentNumber = board[row - 1, column - 1].Val;

                    if (currentNumber != 0)
                    {
                        for (int num = 1; num <= _size; num++)
                        {
                            if (num != currentNumber)
                            {
                                int fillRow = IndexInCoverMatrix(row, column, num);
                                for (int fillCol = 0; fillCol < matrix.GetLength(1); fillCol++)
                                {
                                    matrix[fillRow, fillCol] = 0;
                                }
                            }
                        }
                    }
                }
            }

            return matrix;
        }
    }
}
