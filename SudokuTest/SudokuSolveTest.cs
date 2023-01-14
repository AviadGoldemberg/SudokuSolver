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
                Assert.IsTrue(solvingResult.IsSolved && board.IsSolved());
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
                Assert.IsFalse(solvingResult.IsSolved || board.IsSolved());
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
                Assert.IsTrue(solvingResult.IsSolved && board.IsSolved());
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
                Assert.IsFalse(solvingResult.IsSolved || board.IsSolved());
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
