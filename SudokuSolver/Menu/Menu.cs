using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuSolver.InputOutput;
using SudokuSolver.InputOutput.Console;
using SudokuSolver.InputOutput.Files;
using SudokuSolver.Exceptions;
using SudokuSolver.Solvers.DancingLinksSolver;
using SudokuSolver.Board;
using SudokuSolver.Solvers;

namespace SudokuSolver.Menu
{
    internal class Menu
    {
        private IInput _defaultInput;
        private IOutput _defaultOutput;
        private const string FILES_DIRECTORY = "BoardsFiles";

        private enum Choices
        {
            ConsoleInput = 1,
            FileInput = 2,

            ConsoleOutput = 1,
            FileOutput = 2,

            Solve = 1,
            Test = 2,

            DancingLinksAlgorithm = 1
        }

        public Menu()
        {
            // init default input and output handlers. 
            _defaultInput = new ConsoleInput();
            _defaultOutput = new ConsoleOutput();
        }

        /// <summary>
        /// Method which start the menu.
        /// </summary>
        public void Start()
        {
            // getting user choice
            int userChoice = GetUserIntChoice("Welcome to Sudoku solver.\nChoose 1 for solve a sudoku board.\nChoose 2 for test sudoku solver.\nEnter here your choice: ");
            bool continueLoop = true;
            while (continueLoop)
            {
                switch (userChoice)
                {
                    case (int)Choices.Solve:
                        // get board from input and solve.
                        IInput inputHandler = GetBoardInputHandler();
                        SolveBoardString(inputHandler.GetString());
                        break;
                    case (int)Choices.Test:
                        Console.WriteLine("You choose to test!");
                        continueLoop = false;
                        break;
                    default:
                        // if the choice is not valid, force the user enter valid choice.
                        Console.WriteLine("Please enter a valid number.");
                        userChoice = GetUserIntChoice("Enter your choice: ");
                        break;
                }
            }
        }

        /// <summary>
        /// Method which get from user input handler.
        /// </summary>
        /// <returns>Input handler for the Sudoku board.</returns>
        private IInput GetBoardInputHandler()
        {
            int choice = GetUserIntChoice("Enter 1 to console input.\nEnter 2 to file input.\nEnter your choice: ");
            bool continueLoop = true;
            IInput inputHandler = null;
            while (continueLoop)
            {
                switch (choice)
                {
                    case (int)Choices.ConsoleInput:
                        inputHandler =  new ConsoleInput();
                        _defaultOutput.Output("Enter board string: ");
                        continueLoop = false;
                        break;
                    case (int)Choices.FileInput:
                        // get the file path
                        string path = GetFilePath();
                        inputHandler =  new FileInput(path);
                        continueLoop = false;
                        break;
                    default:
                        choice = GetUserIntChoice("Please enter valid input: ");
                        break;

                }
            }
            return inputHandler;
        }

        /// <summary>
        /// Method which get file path from user to get a sudoku board string.
        /// </summary>
        /// <returns>Sudoku board file path.</returns>
        private string GetFilePath()
        {
            // get the file path
            string fileName = _defaultInput.GetString(_defaultOutput, $"Enter file path (file path should be in [{FILES_DIRECTORY}] directory): ");
            fileName = FILES_DIRECTORY + "\\" + fileName;
            string exeDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string binDirectory = System.IO.Directory.GetParent(exeDirectory).FullName;
            string rootDirectory = System.IO.Directory.GetParent(binDirectory).FullName;
            return System.IO.Path.GetFullPath(System.IO.Path.Combine(rootDirectory, fileName));
        }

        /// <summary>
        /// Method which return sudoku solver by user choice.
        /// </summary>
        /// <param name="boardString">Board string.</param>
        /// <returns>Sudoku solver.</returns>
        private ISudokuSolver GetSudokuSolver(ISudokuBoard board)
        {
            int choice = GetUserIntChoice("Enter 1 to solve with dancing links.\nEnter your choice: ");
            ISudokuSolver solver = null;
            switch (choice)
            {
                case (int)Choices.DancingLinksAlgorithm:
                    solver = new DancingLinksSolver(board);
                    break;
                default:
                    _defaultOutput.Output("Got invalid algorithm code. Use Dancing Links by default...\n");
                    // by default the solver will be dancing links algorithm.
                    solver = new DancingLinksSolver(board);
                    break;
            }
            return solver;
        }

        /// <summary>
        /// Method which get int input from the user input handler.
        /// </summary>
        /// <param name="message">Which message output to the user before the input.</param>
        /// <returns></returns>
        private int GetUserIntChoice(string message)
        {
            // getting user choice
            string userInput = _defaultInput.GetString(_defaultOutput, message);
            int choice = 0;
            // force the user to enter input.
            while (!int.TryParse(userInput, out choice))
            {
                userInput = _defaultInput.GetString(_defaultOutput, "Please enter a number: ");
            }
            return choice;
            
        }

        /// <summary>
        /// Method which solve board string.
        /// </summary>
        /// <param name="boardString">Board string</param>
        private void SolveBoardString(string boardString)
        {
            ISudokuBoard board = new ArraySudokuBoard(boardString);
            // get sudoku solver from user.
            ISudokuSolver solver = GetSudokuSolver(board);
            _defaultOutput.Output("Solving...\n");
            // solve the board.
            SolvingResult solvingResult = solver.Solve();
            // output result
            if (solvingResult.IsSolved && IsSoved(board))
            {
                _defaultOutput.Output($"Solved in [{solvingResult.SolvingTime}ms]\n{board.BoardOutput()}\n");
            }
            else if (solvingResult.IsSolved && !IsSoved(board))
            {
                _defaultOutput.Output("Solver assume that the board is solved, but is not!.");
            }
            else
            {
                _defaultOutput.Output($"Can't solve the board. time took: [{solvingResult.SolvingTime}ms]\n");
            }
        }
        /// <summary>
        /// Method which check if sudoku board is solved.
        /// </summary>
        /// <param name="board">Sudoku board.</param>
        /// <returns>True if the board is solved, otherwise false.</returns>
        private static bool IsSoved(ISudokuBoard board)
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
