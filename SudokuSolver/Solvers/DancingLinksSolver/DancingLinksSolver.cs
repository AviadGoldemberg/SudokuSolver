using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using SudokuSolver.Board;
using SudokuSolver.Solvers.DancingLinksSolver.DancingLinks;
using SudokuSolver.InputOutput;
using SudokuSolver.InputOutput.Files;

namespace SudokuSolver.Solvers.DancingLinksSolver
{
    /// <summary>
    /// Implementation of solving Sudoku board using Dancing Links algorithm. 
    /// </summary>
    internal class DancingLinksSolver : ISudokuSolver
    {
        private int _size;
        private int _subgridSize;
        private const int NUMBER_OF_SOLUTIONS = 1;

        public DancingLinksSolver(ISudokuBoard board) : base(board)
        {
            _size = board.GetBoardSize();
            _subgridSize = (int)Math.Sqrt(_size);
        }

        /// <summary>
        /// Method which try to solve the current sudoku board.
        /// </summary>
        /// <returns>Solving result.</returns>
        public override SolvingResult Solve()
        {
            // start timer.
            Stopwatch stopwatch = Stopwatch.StartNew();
            stopwatch.Start();

            // get the SparseMatrix for the Dancing Links algorithm.
            SparseMatrix matrix = ConvertBoardToMatrix();
            // create Dancing Links solver.
            DancingLinks.DancingLinks DLX = new DancingLinks.DancingLinks(matrix, NUMBER_OF_SOLUTIONS);
            // solve the exact cover problem which the matrix represent.
            DancingLinksResult DLXResult = DLX.Solve();

            // stop timer.
            stopwatch.Stop();

            // add the DLX solution to the board.
            if (DLXResult.IsSolved)
                AddSolutionToBoard(DLXResult.AllSolutions[0]);

            return new SolvingResult(stopwatch.ElapsedMilliseconds, DLXResult.IsSolved);
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
        /// Initializes and creates a SparseMatrix used in the Dancing Links algorithm for solving Sudoku puzzles.
        /// The matrix includes constraints for each cell, row, column, and box in the Sudoku board.
        /// Helper method to <see cref="ConvertBoardToMatrix(ISudokuBoard)"/>
        /// </summary>
        /// <returns>SparseMatrix which represent exact cover problem.</returns>
        private SparseMatrix CreateMatrix()
        {
            SparseMatrix matrix = new SparseMatrix(_size * _size * _size, _size * _size * 4);

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
        private void CreateBoxConstraints(SparseMatrix matrix, ref int header)
        {
            // create box constraints in the sparse matrix.
            for (int row = 1; row <= _size; row += _subgridSize)
                for (int column = 1; column <= _size; column += _subgridSize)
                    for (int num = 1; num <= _size; num++, header++)
                        for (int rowDelta = 0; rowDelta < _subgridSize; rowDelta++)
                            for (int columnDelta = 0; columnDelta < _subgridSize; columnDelta++)
                                matrix.Add(new SparseMatrixIndex(IndexInCoverMatrix(row + rowDelta, column + columnDelta, num), header));
        }

        /// <summary>
        /// Adds constraints for each column in the Sudoku board to the matrix used in the Dancing Links algorithm.
        /// These constraints ensure that each cell in a column is filled with a number that is unique within the column.
        /// Helper method to <see cref="CreateMatrix"/>
        /// </summary>
        /// <param name="matrix">The matrix used in the Dancing Links algorithm.</param>
        /// <param name="header">The current header index in the matrix, where the constraints will be added.</param>
        private void CreateColumnConstraints(SparseMatrix matrix, ref int header)
        {
            // create column constraints in the sparse matrix.
            for (int column = 1; column <= _size; column++)
                for (int num = 1; num <= _size; num++, header++)
                    for (int row = 1; row <= _size; row++)
                        matrix.Add(new SparseMatrixIndex(IndexInCoverMatrix(row, column, num), header));
        }

        /// <summary>
        /// Adds constraints for each row in the Sudoku board to the matrix used in the Dancing Links algorithm.
        /// These constraints ensure that each cell in a column is filled with a number that is unique within the row.
        /// Helper method to <see cref="CreateMatrix"/>
        /// </summary>
        /// <param name="matrix">The matrix used in the Dancing Links algorithm.</param>
        /// <param name="header">The current header index in the matrix, where the constraints will be added.</param>
        private void CreateRowConstraints(SparseMatrix matrix, ref int header)
        {
            // create row constraints in the sparse matrix.
            for (int row = 1; row <= _size; row++)
                for (int num = 1; num <= _size; num++, header++)
                    for (int column = 1; column <= _size; column++)
                        matrix.Add(new SparseMatrixIndex(IndexInCoverMatrix(row, column, num), header));
        }

        /// <summary>
        /// Adds constraints for each cell in the Sudoku board to the matrix used in the Dancing Links algorithm.
        /// These constraints ensure that each cell in a column is filled with a number that is unique within the cell.
        /// Helper method to <see cref="CreateMatrix"/>
        /// </summary>
        /// <param name="matrix">The matrix used in the Dancing Links algorithm.</param>
        /// <param name="header">The current header index in the matrix, where the constraints will be added.</param>
        private void CreateCellConstraints(SparseMatrix matrix, ref int header)
        {
            // create cell constraints in the sparse matrix.
            for (int row = 1; row <= _size; row++)
                for (int column = 1; column <= _size; column++, header++)
                    for (int num = 1; num <= _size; num++)
                        matrix.Add(new SparseMatrixIndex(IndexInCoverMatrix(row, column, num), header));
        }

        /// <summary>
        /// Method which return Sudoku board as matrix for the use of the Dancing Links algorithm.
        /// The method create matrix which represent the Sudoku board as 1's and 0's for the Dancing Links algorithm.
        /// </summary>
        /// <param name="board">Sudoku board to create a matrix for it.</param>
        /// <returns>Matrix for the Dancing Links algorithm.</returns>
        private SparseMatrix ConvertBoardToMatrix()
        {
            // create the constraints in the matrix.
            SparseMatrix matrix = CreateMatrix();

            // remove unnecessary rows from the matrix to reduce iterations in Dancing Links Algorithm.
            for (int row = 1; row <= _size; row++)
                for (int column = 1; column <= _size; column++)
                    RemoveUnnecessaryRowsFromMatrix(matrix, row, column);

            return matrix;
        }

        /// <summary>
        /// Method which remove unnecessary row from the sparse matrix in order to reduce search time in the Dancing Links algorithm.
        /// </summary>
        /// <param name="matrix">Sparse matrix to remove from it the rows.</param>
        /// <param name="row">Current row in the Sudoku board.</param>
        /// <param name="column">Current Column in the Sudoku board.</param>
        private void RemoveUnnecessaryRowsFromMatrix(SparseMatrix matrix, int row, int column)
        {
            // getting the number in the row and col
            int currentNumber = _board[row - 1, column - 1].Val;

            // if the number is const, I need to remove every other number from the matrix which represent the same row and col.
            if (currentNumber != 0)
            {
                for (int num = 1; num <= _size; num++)
                    if (num != currentNumber)
                        matrix.Remove(IndexInCoverMatrix(row, column, num));
            }
            // if the number is valid, to get more efficient search method, I remove every number which is not valid in the board.
            else
            {
                for (int num = 1; num <= _size; num++)
                    if (!_board[row - 1, column - 1].PossibleValues.Contains(num))
                        matrix.Remove(IndexInCoverMatrix(row, column, num));
            }
        }

        /// <summary>
        /// Method which add the solution from the Dancing Links algorithm to the board.
        /// </summary>
        /// <param name="solution">Solution that Dancing Links return.</param>
        private void AddSolutionToBoard(Stack<DancingLinksNode> solution)
        {
            foreach (DancingLinksNode node in solution)
            {
                // find the DLX node in same row as 'node' with smallest column name, which represents
                // the row and column of a cell on the Sudoku board
                DancingLinksNode rowColNode = node;
                int min = int.Parse(rowColNode.Column.Name);

                for (DancingLinksNode tmp = node.Right; tmp != node; tmp = tmp.Right)
                {
                    int val = int.Parse(tmp.Column.Name);

                    if (val < min)
                    {
                        min = val;
                        rowColNode = tmp;
                    }
                }
                // find the row and column to put the value in.
                int ans = int.Parse(rowColNode.Column.Name);
                int row = ans / _size;
                int col = ans % _size;

                // find the value to put in the board.
                ans = int.Parse(rowColNode.Right.Column.Name);
                int num = (ans % _size) + 1;

                // add the value to the board
                _board[row, col].Val = num;
            }
        }
    }
}