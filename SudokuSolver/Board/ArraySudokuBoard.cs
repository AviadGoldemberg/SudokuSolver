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

        public bool IsSolved()
        {
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
                string row = getRowOutput(i);
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
            double squareBoardSize = Math.Sqrt(stringLength);
            // if the square is not integer, the board size is invalid.
            if (squareBoardSize % 1 != 0)
            {
                throw new InvalidBoardString("Size of the board string is invalid.");
            }
            if (squareBoardSize > _maxBoardSize)
            {
                throw new InvalidBoardString("Board size is not supported.");
            }
            _boardSize = (int)squareBoardSize;
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
                    _board[row, col] = new Cell(row, col, currentChar - '0', true);
                }
            }
        }

        /// <summary>
        /// Getting row output.
        /// Helper method to <see cref="BoardOutput"/>
        /// </summary>
        /// <param name="row">Row to get ouptut for.</param>
        /// <returns>The row string.</returns>
        private string getRowOutput(int row)
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
    }

}
