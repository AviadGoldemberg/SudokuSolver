using System;
using System.Collections.Generic;
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
        private int _sectorSize;
        private const int NUMBER_OF_SOLUTIONS = 1;
        private const string FILES_DIRECTORY = "BoardsFiles\\Testing";
        private const int GENERATED_BOARDS_COUNT = 20;

        public DancingLinksSolver(ISudokuBoard board) : base(board)
        {
            _size = board.GetBoardSize();
            _sectorSize = (int)Math.Sqrt(_size);
        }
        public DancingLinksSolver() : base(null)
        {

        }

        /// <summary>
        /// Method which try to solve the current sudoku board.
        /// </summary>
        /// <returns>Solving result.</returns>
        public override SolvingResult Solve()
        {
            // solve the board and also calculate the time.
            Stopwatch stopwatch = Stopwatch.StartNew();
            stopwatch.Start();

            // get the matrix for the Dancing Links algorithm.
            byte[,] matrix = ConvertBoardToMatrix();
            // create Dancing Links solver.
            DancingLinks.DancingLinks DLX = new DancingLinks.DancingLinks(matrix, NUMBER_OF_SOLUTIONS);

            DancingLinksResult DLXResult = DLX.Solve();
            stopwatch.Stop();

            // add the DLX solution to the board.
            if (DLXResult.IsSolved)
            {
                AddSolutionToBoard(DLXResult.AllSolutions[0]);
            }

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
        /// Initializes and creates a matrix used in the Dancing Links algorithm for solving Sudoku puzzles.
        /// The matrix includes constraints for each cell, row, column, and box in the Sudoku board.
        /// Helper method to <see cref="ConvertBoardToMatrix(ISudokuBoard)"/>
        /// </summary>
        /// <returns>A 2D integer array representing the matrix used in the Dancing Links algorithm.</returns>
        private byte[,] CreateMatrix()
        {
            byte[,] matrix = new byte[_size * _size * _size, _size * _size * 4];

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
        private void CreateBoxConstraints(byte[,] matrix, ref int header)
        {
            for (int row = 1; row <= _size; row += _sectorSize)
            {
                for (int column = 1; column <= _size; column += _sectorSize)
                {
                    for (int num = 1; num <= _size; num++, header++)
                    {
                        for (int rowDelta = 0; rowDelta < _sectorSize; rowDelta++)
                        {
                            for (int columnDelta = 0; columnDelta < _sectorSize; columnDelta++)
                            {
                                int index = IndexInCoverMatrix(row + rowDelta, column + columnDelta, num);
                                matrix[index, header] = 1;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Adds constraints for each column in the Sudoku board to the matrix used in the Dancing Links algorithm.
        /// These constraints ensure that each cell in a column is filled with a number that is unique within the column.
        /// Helper method to <see cref="CreateMatrix"/>
        /// </summary>
        /// <param name="matrix">The matrix used in the Dancing Links algorithm.</param>
        /// <param name="header">The current header index in the matrix, where the constraints will be added.</param>
        private void CreateColumnConstraints(byte[,] matrix, ref int header)
        {
            for (int column = 1; column <= _size; column++)
            {
                for (int num = 1; num <= _size; num++, header++)
                {
                    for (int row = 1; row <= _size; row++)
                    {
                        int index = IndexInCoverMatrix(row, column, num);
                        matrix[index,header] = 1;
                    }
                }
            }
        }

        /// <summary>
        /// Adds constraints for each row in the Sudoku board to the matrix used in the Dancing Links algorithm.
        /// These constraints ensure that each cell in a column is filled with a number that is unique within the row.
        /// Helper method to <see cref="CreateMatrix"/>
        /// </summary>
        /// <param name="matrix">The matrix used in the Dancing Links algorithm.</param>
        /// <param name="header">The current header index in the matrix, where the constraints will be added.</param>
        private void CreateRowConstraints(byte[,] matrix, ref int header)
        {
            for (int row = 1; row <= _size; row++)
            {
                for (int num = 1; num <= _size; num++, header++)
                {
                    for (int column = 1; column <= _size; column++)
                    {
                        int index = IndexInCoverMatrix(row, column, num);
                        matrix[index, header] = 1;
                    }
                }
            }
        }

        /// <summary>
        /// Adds constraints for each cell in the Sudoku board to the matrix used in the Dancing Links algorithm.
        /// These constraints ensure that each cell in a column is filled with a number that is unique within the cell.
        /// Helper method to <see cref="CreateMatrix"/>
        /// </summary>
        /// <param name="matrix">The matrix used in the Dancing Links algorithm.</param>
        /// <param name="header">The current header index in the matrix, where the constraints will be added.</param>
        private void CreateCellConstraints(byte[,] matrix, ref int header)
        {
            for (int row = 1; row <= _size; row++)
            {
                for (int column = 1; column <= _size; column++, header++)
                {
                    for (int num = 1; num <= _size; num++)
                    {
                        int index = IndexInCoverMatrix(row, column, num);
                        matrix[index, header] = 1;
                    }
                }
            }
        }

        /// <summary>
        /// Method which return Sudoku board as matrix for the use of the Dancing Links algorithm.
        /// The method create matrix which represent the Sudoku board as 1's and 0's for the Dancing Links algorithm.
        /// </summary>
        /// <param name="board">Sudoku board to create a matrix for it.</param>
        /// <returns>Matrix for the Dancing Links algorithm.</returns>
        private byte[,] ConvertBoardToMatrix()
        {
            byte[,] matrix = CreateMatrix();

            // Taking into account the values already entered in Sudoku's board instance
            for (int row = 1; row <= _size; row++)
            {
                for (int column = 1; column <= _size; column++)
                {
                    int currentNumber = _board[row - 1, column - 1].Val;

                    if (currentNumber != 0)
                    {
                        for (int num = 1; num <= _size; num++)
                        {
                            if (num != currentNumber)
                            {
                                int fillRow = IndexInCoverMatrix(row, column, num);
                                for (int fillCol = 0; fillCol < matrix.GetLength(1); fillCol++)
                                {
                                    matrix[fillRow, fillCol] = 0;
                                }
                            }
                        }
                    }
                }
            }

            return matrix;
        }

        /// <summary>
        /// Method which add the solution from the Dancing Links algorithm to the board.
        /// </summary>
        /// <param name="solution">Solution that Dancing Links return.</param>
        private void AddSolutionToBoard(List<DancingLinksNode> solution)
        {
            foreach (DancingLinksNode node in solution)
            {
                // find the dlx node in same row as 'node' with smallest column name, which represents
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

        /// <summary>
        /// Method which generate boards and save them on file.
        /// </summary>
        public void GenerateBoards()
        {
            // init empty boards in all valid sizes.
            Dictionary<string, int> boardsAndCount = new Dictionary<string, int>();
            boardsAndCount[GetEmptyBoard(4)] = 287;
            boardsAndCount[GetEmptyBoard(9)] = GENERATED_BOARDS_COUNT;
            boardsAndCount[GetEmptyBoard(16)] = GENERATED_BOARDS_COUNT;
            boardsAndCount[GetEmptyBoard(25)] = GENERATED_BOARDS_COUNT;
            // getting the output file path
            string path = GetFilePath();
            IOutput outputFile = new FileOutput(path);
            string output = "";

            // generate boards
            foreach(string boardString in boardsAndCount.Keys)
            {
                // init the current board.
                _board = new ArraySudokuBoard(boardString);
                _size = _board.GetBoardSize();
                _sectorSize = (int)Math.Sqrt(_size);
                // get the matrix for the Dancing Links algorithm.
                byte[,] matrix = ConvertBoardToMatrix();
                // create Dancing Links solver.
                DancingLinks.DancingLinks DLX = new DancingLinks.DancingLinks(matrix, boardsAndCount[boardString]);
                // solve the board
                DancingLinksResult DLXResult = DLX.Solve();
                for(int j = 0; j < DLXResult.AllSolutions.Count; j++)
                {
                    AddSolutionToBoard(DLXResult.AllSolutions[j]);
                    output += _board.GetBoardString() + "\n";
                }
            }
            outputFile.Output(output);

        }

        /// <summary>
        /// Method which return empty board.
        /// </summary>
        /// <param name="size">Size of the board.</param>
        /// <returns>Empty baord.</returns>
        private string GetEmptyBoard(int size)
        {
            string board = "";
            for (int i = 0; i < size * size; i++)
                    board += '0';
            return board;
        }

        /// <summary>
        /// Merthod which get the file path of the testing file.
        /// </summary>
        /// <returns>Path of the testing file.</returns>
        public static string GetFilePath()
        {
            // getting the path of the file
            string path = FILES_DIRECTORY + "\\" + "TestingBoards.txt";
            string exeDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string binDirectory = System.IO.Directory.GetParent(exeDirectory).FullName;
            string rootDirectory = System.IO.Directory.GetParent(binDirectory).FullName;
            return System.IO.Path.GetFullPath(System.IO.Path.Combine(rootDirectory, path));
        }
    }
}
