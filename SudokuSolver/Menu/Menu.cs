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

        private enum Choices
        {
            Solve = 1,
            Test = 2,

            ConsoleInput = 1,
            FileInput = 2,

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
                        // get board string and create board object.
                        string boardString = _defaultInput.GetString(_defaultOutput, "Enter board string: ");
                        // check if user choose to exit.
                        if (boardString == "exit")
                            continueLoop = false;
                        else
                            SolveWithUserInput(boardString);
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
        /// Method which return sudoku solver by user choice.
        /// </summary>
        /// <param name="boardString">Board string.</param>
        /// <returns>Sudoku solver.</returns>
        public ISudokuSolver GetSudokuSolver(ISudokuBoard board)
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
        public int GetUserIntChoice(string message)
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
        /// Method which solve board with user input.
        /// </summary>
        /// <param name="boardString">Board string</param>
        public void SolveWithUserInput(string boardString)
        {
            ISudokuBoard board = new ArraySudokuBoard(boardString);
            // get sudoku solver from user.
            ISudokuSolver solver = GetSudokuSolver(board);
            _defaultOutput.Output("Solving...\n");
            // solve the board.
            SolvingResult solvingResult = solver.Solve();
            // output result
            if (solvingResult.IsSolved)
            {
                _defaultOutput.Output($"Solved in [{solvingResult.SolvingTime}ms]\n{board.BoardOutput()}\n");
            }
            else
            {
                _defaultOutput.Output($"Can't solve the board. time took: [{solvingResult.SolvingTime}ms]\n");
            }
        }
    }
}
