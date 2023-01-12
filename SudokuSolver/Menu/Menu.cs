﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using SudokuSolver.InputOutput;
using SudokuSolver.InputOutput.Console;
using SudokuSolver.InputOutput.Files;
using SudokuSolver.Exceptions;
using SudokuSolver.Solvers.DancingLinksSolver;
using SudokuSolver.Board;
using SudokuSolver.Solvers;
using SudokuSolver.Solvers.BacktrackingSolver;
using System.IO;

namespace SudokuSolver.Menu
{
    /// <summary>
    /// Menu class.
    /// </summary>
    internal class Menu
    {
        private IInput _defaultInput;
        private IOutput _defaultOutput;
        private const string FILES_DIRECTORY = "BoardsFiles";
        private const string BENCHMARK_FILE_PATH = "Benchmark\\boardsDataset.txt";
        private const string BENCHMARK_LOG_FILE_PATH = "Benchmark\\BenchmarkLogs.txt";
        private const string BENCHMARK_RESULT_FILE_PATH = "Benchmark\\BenchmarkResult.txt";

        /// <summary>
        /// Enum which represent user choices.
        /// </summary>
        private enum Choices
        {
            ConsoleInput = 1,
            FileInput = 2,

            ConsoleOutput = 1,
            FileOutput = 2,

            Solve = 1,
            Benchmark = 2,

            DancingLinksAlgorithm = 1, 
            BacktrackingAlgorithm = 2
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
            int userChoice = 0;
            bool continueLoop = true;
            OutputWelcomeMessage();
            while (continueLoop)
            {
                // getting choice from input.
                userChoice = GetUserIntChoice($"Choose {(int)Choices.Solve} for solve a sudoku board.\nChoose {(int)Choices.Benchmark} for benchmark.\nEnter here your choice: ");
                try
                {
                    RunUserOption(userChoice);
                }
                catch (InvalidBoardString e)
                {
                    _defaultOutput.OutputLine(e.Message);
                }
                catch (InvalidChoice e)
                {
                    _defaultOutput.OutputLine(e.Message);
                }
                catch (FileNotFoundException e)
                {
                    _defaultOutput.OutputLine(e.Message + " is not valid file path.");
                }
            }
        }

        private void OutputWelcomeMessage()
        {
            string WelcomeMessage = @"   ___        _   _           _             _    ____            _       _             ____        _                
  / _ \ _ __ | |_(_)_ __ ___ (_)_______  __| |  / ___| _   _  __| | ___ | | ___   _   / ___|  ___ | |_   _____ _ __ 
 | | | | '_ \| __| | '_ ` _ \| |_  / _ \/ _` |  \___ \| | | |/ _` |/ _ \| |/ / | | |  \___ \ / _ \| \ \ / / _ \ '__|
 | |_| | |_) | |_| | | | | | | |/ /  __/ (_| |   ___) | |_| | (_| | (_) |   <| |_| |   ___) | (_) | |\ V /  __/ |   
  \___/| .__/ \__|_|_| |_| |_|_/___\___|\__,_|  |____/ \__,_|\__,_|\___/|_|\_\\__,_|  |____/ \___/|_| \_/ \___|_|   
       |_|     ";
            _defaultOutput.OutputLine(WelcomeMessage + "\n\n");
        }

        /// <summary>
        /// Method which run user choice.
        /// Helper function to <see cref="Start"/>
        /// </summary>
        /// <param name="userChoice">User choice.</param>
        private void RunUserOption(int userChoice)
        {
            switch (userChoice)
            {
                case (int)Choices.Solve:
                    // get board from input and solve.
                    IInput inputHandler = GetBoardInputHandler();
                    SolveBoardString(inputHandler.GetString());
                    break;
                case (int)Choices.Benchmark:
                    Benchmark();
                    break;
                default:
                    throw new InvalidChoice("Got invalid choice.");
            }
        }

        /// <summary>
        /// Method which get from user input handler.
        /// </summary>
        /// <returns>Input handler for the Sudoku board.</returns>
        private IInput GetBoardInputHandler()
        {
            int choice = GetUserIntChoice($"Enter {(int)Choices.ConsoleInput} to console input.\nEnter {(int)Choices.FileInput} to file input.\nEnter your choice: ");
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
                        throw new InvalidChoice("Got invalid choice.");

                }
            }
            return inputHandler;
        }

        /// <summary>
        /// Method which get file path from user to get a sudoku board string file.
        /// </summary>
        /// <returns>Sudoku board file path.</returns>
        private string GetFilePath()
        {
            // get the file path
            string fileName = _defaultInput.GetString(_defaultOutput, $"Enter file path (file path should be in [{FILES_DIRECTORY}] directory): ");
            fileName = FILES_DIRECTORY + "\\" + fileName;
            return System.IO.Path.GetFullPath(System.IO.Path.Combine(GetRootDirectory(), fileName));
        }

        /// <summary>
        /// Method which return sudoku solver by user choice.
        /// </summary>
        /// <param name="boardString">Board string.</param>
        /// <returns>Sudoku solver.</returns>
        private ISudokuSolver GetSudokuSolver(ISudokuBoard board)
        {
            int choice = GetUserIntChoice($"Enter {(int)Choices.DancingLinksAlgorithm} to solve with dancing links.\nEnter {(int)Choices.BacktrackingAlgorithm} to solve with Backtracking.\nEnter your choice: ");
            ISudokuSolver solver = null;
            switch (choice)
            {
                case (int)Choices.DancingLinksAlgorithm:
                    solver = new DancingLinksSolver(board);
                    break;
                case (int)Choices.BacktrackingAlgorithm:
                    solver = new BacktrackingSolver(board);
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
            if (solvingResult.IsSolved)
                _defaultOutput.Output($"Solved in [{solvingResult.SolvingTime}ms]\n{board.BoardOutput()}\n");
            else
                _defaultOutput.Output($"Can't solve the board. time took: [{solvingResult.SolvingTime}ms]\n");
        }

        /// <summary>
        /// Method which get the benchmark file path.
        /// </summary>
        /// <returns>Benchmark file path.</returns>
        private string GetBenchmarkFile()
        {
            string path = FILES_DIRECTORY + "\\" + BENCHMARK_FILE_PATH;
            return System.IO.Path.GetFullPath(System.IO.Path.Combine(GetRootDirectory(), path));
        }

        /// <summary>
        /// Method which get the benchmark logging file.
        /// </summary>
        /// <returns>Benchmark logging file.</returns>
        private string GetBenchmarkLogFile()
        {
            string path = FILES_DIRECTORY + "\\" + BENCHMARK_LOG_FILE_PATH;
            return System.IO.Path.GetFullPath(System.IO.Path.Combine(GetRootDirectory(), path));
        }

        /// <summary>
        /// Method which get the benchmark result log file.
        /// </summary>
        /// <returns>Benchmark result log file.</returns>
        private string GetBenchmarkResultFile()
        {
            string path = FILES_DIRECTORY + "\\" + BENCHMARK_RESULT_FILE_PATH;
            return System.IO.Path.GetFullPath(System.IO.Path.Combine(GetRootDirectory(), path));
        }

        /// <summary>
        /// Method which get the root directory.
        /// </summary>
        /// <returns>Root directory.</returns>
        private static string GetRootDirectory()
        {
            string exeDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string binDirectory = System.IO.Directory.GetParent(exeDirectory).FullName;
            string rootDirectory = System.IO.Directory.GetParent(binDirectory).FullName;
            return rootDirectory;
        }

        /// <summary>
        /// Method which benchamark the Sudoku solving algorithm.
        /// The method save the results in two files.
        /// One file is log for each board and another to the result in general.
        /// </summary>
        private void Benchmark()
        {
            // getting dataset input handler.
            IInput boardsInput = new FileInput(GetBenchmarkFile());
            // get output file to log the results.
            IOutput logger = new FileOutput(GetBenchmarkLogFile());
            // get benchmartk file to store the result.
            IOutput benchmarkResultOutput = new FileOutput(GetBenchmarkResultFile());

            string dataset = boardsInput.GetString();
            // split the dataset to board and solution.
            string[] boardSAndSolutions = dataset.Split('\n');

            // result parametrs:
            ulong averageTime = 0;
            ulong count = 0;
            // init min and max solve time.
            long minSolveTime = long.MaxValue;
            long maxSolveTime = long.MinValue;
            string loggerString = "[" + DateTime.Now.ToString("dd/MM/yyyy h:mm") + " Benchmark result]\n";

            // start stopwatch
            Stopwatch stopwatch = Stopwatch.StartNew();
            stopwatch.Start();

            // iterate each board.
            foreach (string currentBoardAndSolution in boardSAndSolutions)
            {
                // getting board string, create board object and solve it with Dancing Links algorithm.
                string boardString = currentBoardAndSolution.Split(',')[0];
                ISudokuBoard board = new ArraySudokuBoard(boardString);
                DancingLinksSolver solver = new DancingLinksSolver(board);
                SolvingResult solvingResult = solver.Solve();

                // append to the average the time and increase the count.
                averageTime += (ulong)solvingResult.SolvingTime;
                count++;

                // update minimum and maximum solving time.
                if (solvingResult.SolvingTime < minSolveTime)
                    minSolveTime = solvingResult.SolvingTime;
                if (solvingResult.SolvingTime > maxSolveTime)
                    maxSolveTime = solvingResult.SolvingTime;

                loggerString += $"Board {count} Solved in [{solvingResult.SolvingTime}ms]\n";
            }

            stopwatch.Stop();

            // output to logger the logs.
            logger.Output(loggerString);

            // update result file.
            string resultOutput = "Result for " + DateTime.Now.ToString("dd/MM/yyyy h:mm") + "Benchmark\n";
            resultOutput += $"Solve {count} different boards.\n";
            resultOutput += $"Number of boards benchmarked: {count}\n";
            resultOutput += $"[Average solving time: {averageTime / count}ms]\n";
            resultOutput += $"[Minimum solving time: {minSolveTime}ms]\n";
            resultOutput += $"[Maximum solving time: {maxSolveTime}ms]\n";
            resultOutput += $"[Benchmark running time in milliseconds: {stopwatch.ElapsedMilliseconds} ms]\n";
            resultOutput += $"[Benchmark running time in seconds: {stopwatch.Elapsed.TotalSeconds} Seconds]\n";
            benchmarkResultOutput.Output(resultOutput);

            _defaultOutput.Output("Finish benchmark.\n");
        }
    }
}
