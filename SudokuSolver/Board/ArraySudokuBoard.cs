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

        /// <summary>
        /// constructor to array board
        /// </summary>
        /// <param name="boardSize">board size</param>
        /// <param name="boardString">string which represent the board</param>
        /// <exception cref="InvalidBoardString"></exception>
        public ArraySudokuBoard(int boardSize, string boardString)
        {
            _boardSize = boardSize;
            _board = new Cell[boardSize, boardSize];

            // check board size
            if (boardString.Length != _boardSize * _boardSize)
            {
                throw new InvalidBoardString("Board string is invalid.");
            }

            // init the board
            for (int row = 0; row < _boardSize; row++)
            {
                for (int col = 0; col < _boardSize; col++)
                {
                    char currChar = boardString[row * _boardSize + col];
                    if (!char.IsNumber(currChar))
                    {
                        throw new InvalidBoardString("Board string must contain only numbers!");
                    }
                    _board[row, col] = new Cell(row, col, currChar - '0', true);
                }
            }
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
                    result += _board[row, col].Val.ToString();
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
    }

}
