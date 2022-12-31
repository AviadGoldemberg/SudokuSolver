using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuSolver.InputOutput;
using SudokuSolver.InputOutput.Console;
using SudokuSolver.InputOutput.Files;
using SudokuSolver.Exceptions;

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
            FileInput = 2
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
                        Console.WriteLine("You choose to solve!");
                        continueLoop = false;
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
        public void SolveBoard(string boardInput, IOutput outputHandler)
        {
            return;
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
    }
}
