using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuSolver.Exceptions;

namespace SudokuSolver.Board
{
    /// <summary>
    /// Class which represent a sudoku board.
    /// The class save the board as array.
    /// </summary>
    internal class ArraySudokuBoard : ISudokuBoard
    {
        private int _boardSize;
        private Cell[,] _board;
        private const int _maxBoardSize = 25;

        /// <summary>
        /// constructor to array board
        /// </summary>
        /// <param name="boardSize">board size</param>
        /// <param name="boardString">string which represent the board</param>
        /// <exception cref="InvalidBoardString">If the board string is invalid.</exception>
        public ArraySudokuBoard(string boardString)
        {
            // check if size is valid
            InitBoardSize(boardString.Length);
            // create the board.
            CreateBoardFromString(boardString);
        }

        /// <summary>
        /// method which return cell at specific index
        /// </summary>
        /// <param name="row">row of the index in the board</param>
        /// <param name="column">column of the index in the board</param>
        /// <returns></returns>
        public Cell this[int row, int column]
        {
            get { return _board[row, column]; }
            set { _board[row, column] = value; }
        }

        /// <summary>
        /// method which get the board string
        /// </summary>
        /// <returns>board string</returns>
        public string GetBoardString()
        {
            string result = "";
            for (int row = 0; row < _boardSize; row++)
            {
                for (int col = 0; col < _boardSize; col++)
                {
                    result += (char)(_board[row, col].Val + '0');
                }
            }
            return result;
        }

        /// <summary>
        /// method which return the board size
        /// </summary>
        /// <returns>board size</returns>
        public int GetBoardSize()
        {
            return _boardSize;
        }

        /// <summary>
        /// Method which check if the current board is solved or not.
        /// </summary>
        /// <returns>True if board solved, else false.</returns>
        public bool IsSolved()
        {
            // check each row
            for (int i = 0; i < _boardSize; i++)
            {
                bool[] seen = new bool[_boardSize];
                for (int j = 0; j < _boardSize; j++)
                {
                    // if value in row is less then one or larger then the board size or was already in row
                    if (_board[i, j].Val < 1 || _board[i, j].Val > _boardSize || seen[_board[i, j].Val - 1])
                        return false;
                    seen[_board[i, j].Val - 1] = true;
                }
            }

            // check each column
            for (int j = 0; j < _boardSize; j++)
            {
                bool[] seen = new bool[_boardSize];
                for (int i = 0; i < _boardSize; i++)
                {
                    // if value in column is less then one or larger then the board size or was already in column
                    if (_board[i, j].Val < 1 || _board[i, j].Val > _boardSize || seen[_board[i, j].Val - 1])
                    {
                        return false;
                    }
                    seen[_board[i, j].Val - 1] = true;
                }
            }

            // check each box
            int subgridSize = (int)Math.Sqrt(_boardSize);
            for (int i = 0; i < _boardSize; i += subgridSize)
            {
                for (int j = 0; j < _boardSize; j += subgridSize)
                {
                    bool[] seen = new bool[_boardSize];
                    for (int k = 0; k < _boardSize; k++)
                    {
                        int row = i + k / subgridSize;
                        int col = j + k % subgridSize;
                        // if value in box is less then one or larger then the board size or was already in box
                        if (_board[row, col].Val < 1 || _board[row, col].Val > _boardSize || seen[_board[row, col].Val - 1])
                        {
                            return false;
                        }
                        seen[_board[row, col].Val - 1] = true;
                    }
                }
            }

            // if all checks pass, the board is solved
            return true;
        }

        /// <summary>
        /// Method which return board string for output.
        /// </summary>
        /// <returns>Board to output as string.</returns>
        public string BoardOutput()
        {
            string output = "";
            string rowAbove = "";
            int squareSize = (int)Math.Sqrt(_boardSize);
            // getting row to put above every row
            for (int i = 0; i <= 6 * _boardSize + squareSize + 1; i++)
            {
                rowAbove += '-';
            }
            output += rowAbove + "\n";

            // getting the output
            for (int i = 0; i < _boardSize; i++)
            {
                // getting the current row output
                string row = GetRowOutput(i);
                output += rowAbove + "\n" + row + "\n";
                // in board square
                if ((i+1) % squareSize == 0)
                {
                    output += rowAbove + "\n";
                }
            }
            output += rowAbove + "\n";
            return output;
        }

        /// <summary>
        /// Method which check if board size is valid or not. 
        /// If board size is invalid the method throws <see cref="InvalidBoardString"/> exception.
        /// </summary>
        /// <exception cref="InvalidBoardString">If the board size is invalid.</exception>
        private void InitBoardSize(int stringLength)
        {
            double boardSize = Math.Sqrt(stringLength);
            // if the square is not integer, the board size is invalid.
            if (boardSize % 1 != 0)
            {
                throw new InvalidBoardString("Size of the board string is invalid.");
            }
            // if the square of the board size is not integer, the board size is invalid.
            if (Math.Sqrt(boardSize) % 1 != 0)
            {
                throw new InvalidBoardString("Size of the board string is invalid.");
            }
            if (boardSize > _maxBoardSize)
            {
                throw new InvalidBoardString("Board size is not supported.");
            }
            _boardSize = (int)boardSize;
        }

        /// <summary>
        /// Method which create board from given string.
        /// </summary>
        /// <param name="boardString">String which represent the board.</param>
        /// <exception cref="InvalidBoardString">If chars in string is invalid for the current board size.</exception>
        private void CreateBoardFromString(string boardString)
        {
            // calculate the minimum ascii and maximum ascii that can be in the string
            char minAsciiInString = '0';
            char maxAsciiInString = (char)('0' + _boardSize);

            // init board
            _board = new Cell[_boardSize, _boardSize];

            // loop which add chars in string to the board.
            for (int row = 0; row < _boardSize; row++)
            {
                for (int col = 0; col < _boardSize; col++)
                {
                    char currentChar = boardString[row * _boardSize + col];
                    if (currentChar < minAsciiInString || currentChar > maxAsciiInString)
                    {
                        throw new InvalidBoardString("Board string contains invalid characters.");
                    }
                    bool isConst = currentChar != '0';
                    _board[row, col] = new Cell(row, col, currentChar - '0', isConst);
                }
            }
            InitPossibleValues();
        }

        /// <summary>
        /// Getting row output.
        /// Helper method to <see cref="BoardOutput"/>
        /// </summary>
        /// <param name="row">Row to get ouptut for.</param>
        /// <returns>The row string.</returns>
        private string GetRowOutput(int row)
        {
            string rowString = "|";
            for (int col = 0; col < _boardSize; col++)
            {
                int val = _board[row, col].Val;
                if (val < 10)
                {
                    rowString += "|  " + val.ToString() + "  ";
                }
                else
                {
                    rowString += "| " + val.ToString() + "  ";
                }
                // in board square
                if ((col + 1) % Math.Sqrt(_boardSize) == 0)
                {
                    rowString += '|';
                }
            }
            rowString += "|";
            return rowString;
        }

        /// <summary>
        /// Method which init possible values for each cell.
        /// </summary>
        private void InitPossibleValues()
        {
            // Init possible values for each cell in the board.
            for (int row = 0; row < _boardSize; row++)
                for (int col = 0; col < _boardSize; col++)
                    if (!_board[row, col].IsConst)
                        for (int num = 1; num <= _boardSize; num++)
                            if (IsValid(num, row, col))
                                _board[row, col].PossibleValues.Add(num);
        }

        /// <summary>
        /// Method which check if number is valid in row and col.
        /// </summary>
        /// <param name="num">Number to check.</param>
        /// <param name="checkRow">Row to check.</param>
        /// <param name="checkCol">Column to check.</param>
        /// <returns></returns>
        public bool IsValid(int num, int checkRow, int checkCol)
        {
            // check row and col.
            for (int index = 0; index < _boardSize; index++)
            {
                if (_board[checkRow, index].Val == num) { return false; }
                if (_board[index, checkCol].Val == num) { return false; }
            }

            // check box.
            int _subgridSize = (int)Math.Sqrt(_boardSize);
            int rowBoxStart = _subgridSize * (checkRow / _subgridSize);
            int colBoxStart = _subgridSize * (checkCol / _subgridSize);
            int rowBoxEnd = rowBoxStart + _subgridSize;
            int colBoxEnd = colBoxStart + _subgridSize;
            for (int row = rowBoxStart; row < rowBoxEnd; row++)
                for (int col = colBoxStart; col < colBoxEnd; col++)
                    if (_board[row, col].Val == num)
                        return false;

            // if there is no number in the row, column and box, num is valid.
            return true;
        }
    }
}
