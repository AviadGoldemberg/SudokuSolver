using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SudokuSolver.Board;
using SudokuSolver.InputOutput;
using SudokuSolver.InputOutput.Files;
using SudokuSolver.Solvers;
using SudokuSolver.Solvers.DancingLinksSolver;
using SudokuSolver.Exceptions;

namespace SudokuTest
{
    [TestClass]
    public class SudokuSolveTest
    {
        [TestMethod]
        public void Solve_ValidBoards_True()
        {;
            // Arrange
            string[] boardsArray = GetBoardsFromFile("BoardsFiles\\ValidUnsolvedBoards.txt");

            // Act
            foreach (string boardStr in boardsArray)
            {
                ISudokuBoard board = new ArraySudokuBoard(boardStr);
                ISudokuSolver solver = new DancingLinksSolver(board);
                SolvingResult solvingResult = solver.Solve();

                // Assert
                Assert.IsTrue(solvingResult.IsSolved && IsSolved(board));
            }
        }

        [TestMethod]
        public void Solve_InvalidBoards_False()
        {
            // Arrange
            string[] boardsArray = GetBoardsFromFile("BoardsFiles\\InvalidUnsolvedBoards.txt");

            // Act
            foreach (string boardStr in boardsArray)
            {
                ISudokuBoard board = new ArraySudokuBoard(boardStr);
                ISudokuSolver solver = new DancingLinksSolver(board);
                SolvingResult solvingResult = solver.Solve();

                // Assert
                Assert.IsFalse(solvingResult.IsSolved || IsSolved(board));
            }
        }

        [TestMethod]
        public void Solve_ValidSolvedBoards_True()
        {
            // Arrange
            string[] boardsArray = GetBoardsFromFile("BoardsFiles\\ValidSolvedBoards.txt");

            // Act
            foreach (string boardStr in boardsArray)
            {
                ISudokuBoard board = new ArraySudokuBoard(boardStr);
                ISudokuSolver solver = new DancingLinksSolver(board);
                SolvingResult solvingResult = solver.Solve();

                // Assert
                Assert.IsTrue(solvingResult.IsSolved && IsSolved(board));
            }
        }

        [TestMethod]
        public void Solve_InvalidSolvedBoards_False()
        {
            // Arrange
            string[] boardsArray = GetBoardsFromFile("BoardsFiles\\InvalidSolvedBoards.txt");

            // Act
            foreach (string boardStr in boardsArray)
            {
                ISudokuBoard board = new ArraySudokuBoard(boardStr);
                ISudokuSolver solver = new DancingLinksSolver(board);
                SolvingResult solvingResult = solver.Solve();

                // Assert
                Assert.IsFalse(solvingResult.IsSolved || IsSolved(board));
            }
        }

        [TestMethod]
        public void ArraySudokuBoard_InvalidStringBoards_InvalidBoardException()
        {
            // Arrange
            string[] boardsArray = GetBoardsFromFile("BoardsFiles\\InvalidBoardStrings.txt");

            // Act
            foreach (string boardStr in boardsArray)
            {
                bool exceptionThrown = false;
                try
                {
                    ISudokuBoard board = new ArraySudokuBoard(boardStr);

                }
                catch (InvalidBoardString)
                {
                    exceptionThrown = true;
                }

                // Assert
                Assert.IsTrue(exceptionThrown);
            }
        }

        [TestMethod]
        public void Files_InvalidFile_FileNotFoundException()
        {
            // Arrange
            string invalidFileName = "NotExist.txt";
            bool exceptionThrown = false;

            // Act
            try
            {
                string[] boardsArray = GetBoardsFromFile(invalidFileName);
            }
            catch (System.IO.FileNotFoundException)
            {
                exceptionThrown = true;
            }

            // Assert
            Assert.IsTrue(exceptionThrown);
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

        private static string[] GetBoardsFromFile(string file)
        {
            // get the file path root.
            string rootDirectory = System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName; ;
            string path = System.IO.Path.Combine(rootDirectory, file);
            // create input handler
            IInput inputHandler = new FileInput(path);

            // gettting boards to test and create array of board strings.
            string boardsString = inputHandler.GetString();
            char[] delims = new[] { '\r', '\n' };
            string[] boardsArray = boardsString.Split(delims, StringSplitOptions.RemoveEmptyEntries);
            return boardsArray;
        }
    }
}
