using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SudokuSolver.Board;
using SudokuSolver.InputOutput;
using SudokuSolver.InputOutput.Files;
using SudokuSolver.Solvers;
using SudokuSolver.Solvers.DancingLinksSolver;

namespace SudokuTest
{
    [TestClass]
    public class SudokuSolveTest
    {
        [TestMethod]
        public void Solve_ValidBoards_true()
        {
            ISudokuSolver solver = null;
            // get boards input handler.
            IInput boardsInput = new FileInput(DancingLinksSolver.GetFilePath());
            // get all the boards from the input.
            string[] boards = boardsInput.GetString().Split('\n');
            // iterate each board and check if can solve it.
            foreach(string boardString in boards)
            {
                // create board and solve it.
                ISudokuBoard board = new ArraySudokuBoard(boardString);
                solver = new DancingLinksSolver(board);
                SolvingResult solvingResult = solver.Solve();
                // test the result.
                Assert.IsTrue(solvingResult.IsSolved && IsSolved(board));
            }
        }


        /// <summary>
        /// Method which check if sudoku board is solved.
        /// </summary>
        /// <param name="board">Sudoku board.</param>
        /// <returns>True if the board is solved, otherwise false.</returns>
        private static bool IsSolved(ISudokuBoard board)
        {
            int boardSize = board.GetBoardSize();
            // check each row
            for (int i = 0; i < boardSize; i++)
            {
                bool[] seen = new bool[boardSize];
                for (int j = 0; j < boardSize; j++)
                {
                    if (board[i, j].Val < 1 || board[i, j].Val > boardSize || seen[board[i, j].Val - 1])
                        return false;
                    seen[board[i, j].Val - 1] = true;
                }
            }

            // check each column
            for (int j = 0; j < boardSize; j++)
            {
                bool[] seen = new bool[boardSize];
                for (int i = 0; i < boardSize; i++)
                {
                    if (board[i, j].Val < 1 || board[i, j].Val > boardSize || seen[board[i, j].Val - 1])
                    {
                        return false;
                    }
                    seen[board[i, j].Val - 1] = true;
                }
            }

            // check each  subgrid
            int subgridSize = (int)Math.Sqrt(boardSize);
            for (int i = 0; i < boardSize; i += subgridSize)
            {
                for (int j = 0; j < boardSize; j += subgridSize)
                {
                    bool[] seen = new bool[boardSize];
                    for (int k = 0; k < boardSize; k++)
                    {
                        int row = i + k / subgridSize;
                        int col = j + k % subgridSize;
                        if (board[row, col].Val < 1 || board[row, col].Val > boardSize || seen[board[row, col].Val - 1])
                        {
                            return false;
                        }
                        seen[board[row, col].Val - 1] = true;
                    }
                }
            }

            // if all checks pass, the board is solved
            return true;
        }
    }
}
