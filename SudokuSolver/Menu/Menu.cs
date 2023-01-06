using System;
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

namespace SudokuSolver.Menu
{
    internal class Menu
    {
        private IInput _defaultInput;
        private IOutput _defaultOutput;
        private const string FILES_DIRECTORY = "BoardsFiles";
        private const string BENCHMARK_FILE_PATH = "Benchmark\\boardsDataset.txt";
        private const string BENCHMARK_LOG_FILE_PATH = "Benchmark\\BenchmarkLogs.txt";
        private const string BENCHMARK_RESULT_FILE_PATH = "Benchmark\\BenchmarkResult.txt";
        private enum Choices
        {
            ConsoleInput = 1,
            FileInput = 2,

            ConsoleOutput = 1,
            FileOutput = 2,

            Solve = 1,
            Benchmark = 2,

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
            int userChoice = GetUserIntChoice("Welcome to Sudoku solver.\nChoose 1 for solve a sudoku board.\nChoose 2 for benchmark.\nEnter here your choice: ");
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
                    case (int)Choices.Benchmark:
                        Benchmark();
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
            return System.IO.Path.GetFullPath(System.IO.Path.Combine(GetRootDirectory(), fileName));
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
                // getting board string, create board object and solve it.
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
