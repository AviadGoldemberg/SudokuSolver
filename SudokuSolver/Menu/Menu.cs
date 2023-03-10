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
        private const string SOLVED_BOARD_RESULT_FILE = FILES_DIRECTORY + "\\Result.txt";
        private bool _saveResultToFile;
        private const int MAX_EMPTY_CELLS_FOR_BACKTRACKING = 15;
        private OutputBoardFormat _outputBoardFormat = OutputBoardFormat.BoardString;
        

        /// <summary>
        /// Enum which represent user choices.
        /// </summary>
        private enum Choices
        {
            SaveResultInFile = 1,

            ConsoleInput = 1,
            FileInput = 2,

            ConsoleOutput = 1,
            FileOutput = 2,

            Solve = 1,
            Benchmark = 2,
            OptimizedAlgorithm = 3,

            DancingLinksAlgorithm = 1, 
            BacktrackingAlgorithm = 2
        }

        /// <summary>
        /// Represent the output format to the board.
        /// </summary>
        private enum OutputBoardFormat
        {
            VisualBoard = 1,
            BoardString = 2
        }

        public Menu()
        {
            // init default input and output handlers. 
            _defaultInput = new ConsoleInput();
            _defaultOutput = new ConsoleOutput();
        }

        /// <summary>
        /// Method which check with the user if he want to save the result in file.
        /// </summary>
        /// <returns>True if need to save the result in file, else false.</returns>
        private bool SaveResultToFileChecker()
        {
            int userChoice = GetUserIntChoice($"Enter {(int)Choices.SaveResultInFile} if you want to save the solved board result in: {SOLVED_BOARD_RESULT_FILE}\nPress any other number to show result only at console.\nEnter here your choice: ");
            if (userChoice == (int)Choices.SaveResultInFile)
                return true;
            return false;

        }

        /// <summary>
        /// Method which init the board output format by the user choice.
        /// </summary>
        private void InitOutputFormat()
        {
            int userChoice = GetUserIntChoice($"Enter {(int)OutputBoardFormat.VisualBoard} to get board in visual mode.\nEnter {(int)OutputBoardFormat.BoardString} to get board as string.\nEnter here your choice: ");
            switch (userChoice)
            {
                case (int)OutputBoardFormat.VisualBoard:
                    _outputBoardFormat = OutputBoardFormat.VisualBoard;
                    break;
                case (int)OutputBoardFormat.BoardString:
                    _outputBoardFormat = OutputBoardFormat.BoardString;
                    break;
                default:
                    _defaultOutput.OutputLine("Got invalid format. Use the defult format which is board string.");
                    break;
            }
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
            InitOutputFormat();
            _saveResultToFile = SaveResultToFileChecker();
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
            _defaultOutput.OutputLine("To get the best results, please use Release mode instead of Debug mode.\n");
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
            int choice = GetUserIntChoice($"Enter {(int)Choices.DancingLinksAlgorithm} to solve with dancing links.\nEnter {(int)Choices.BacktrackingAlgorithm} to solve with Backtracking.\nEnter {(int)Choices.OptimizedAlgorithm} to solve with optimized algorithm.\nEnter your choice: ");
            ISudokuSolver solver = null;
            switch (choice)
            {
                case (int)Choices.DancingLinksAlgorithm:
                    solver = new DancingLinksSolver(board);
                    _defaultOutput.OutputLine("Solving with Dancing Links...");
                    break;
                case (int)Choices.BacktrackingAlgorithm:
                    solver = new BacktrackingSolver(board);
                    _defaultOutput.OutputLine("Solving with Backtracking...");
                    break;
                case (int)Choices.OptimizedAlgorithm:
                    solver = GetOptimizedAlgorithm(board);
                    if (solver is DancingLinksSolver)
                        _defaultOutput.OutputLine("Solving with Dancing Links...");
                    else if (solver is BacktrackingSolver)
                        _defaultOutput.OutputLine("Solving with Backtracking...");
                    break;
                default:
                    _defaultOutput.Output("Got invalid algorithm code. Use optimized algorithm by default...\n");
                    // by default the solver will be dancing links algorithm.
                    solver = GetOptimizedAlgorithm(board);
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

            // get solver from user and try to solve the board.
            ISudokuSolver solver = GetSudokuSolver(board);
            SolvingResult solvingResult = solver.Solve();
            
            // get the board in the selected format
            string boardInSelectedFormat = "";
            if (_outputBoardFormat == OutputBoardFormat.VisualBoard)
                boardInSelectedFormat = board.BoardOutput();
            else
                boardInSelectedFormat = board.GetBoardString();

            // output result
            if (solvingResult.IsSolved)
                _defaultOutput.Output($"Solved in [{solvingResult.SolvingTime}ms]\n{boardInSelectedFormat}\n");
            else
                throw new InvalidBoardString($"The board is unsolvable. Time took: [{solvingResult.SolvingTime}ms]");
            
            // save to file if there is need to
            if (_saveResultToFile)
            {
                IOutput fileOutput = new FileOutput(Path.Combine(GetRootDirectory(), SOLVED_BOARD_RESULT_FILE));
                fileOutput.Output(board.GetBoardString());
            }
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
            int unsolvedBoards = 0;
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

                // if there is unsolved board which is should not happend, update counter.
                if (!solvingResult.IsSolved)
                    unsolvedBoards++;

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
            resultOutput += $"[Unsolved boards: {unsolvedBoards}ms]\n";
            resultOutput += $"[Benchmark running time in milliseconds: {stopwatch.ElapsedMilliseconds} ms]\n";
            resultOutput += $"[Benchmark running time in seconds: {stopwatch.Elapsed.TotalSeconds} Seconds]\n";
            benchmarkResultOutput.Output(resultOutput);

            _defaultOutput.Output("Finish benchmark.\n");
        }

        /// <summary>
        /// Method which return the optimized algorithm for the current board.
        /// </summary>
        /// <param name="board"></param>
        private static ISudokuSolver GetOptimizedAlgorithm(ISudokuBoard board)
        {
            int emptyCellsCount = 0;
            // count the number of empty cells.
            for (int row = 0; row < board.GetBoardSize(); row++)
                for (int col = 0; col < board.GetBoardSize(); col++)
                    if (board[row, col].Val == 0)
                        emptyCellsCount++;
            // if backtracking is probably the most efficient algorithm return it.
            if (emptyCellsCount < MAX_EMPTY_CELLS_FOR_BACKTRACKING)
                return new BacktrackingSolver(board);
            // else Dancing Links is probably the most efficient algorithm.
            return new DancingLinksSolver(board);
        }
    }
}
