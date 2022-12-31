using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Board
{
    internal interface ISudokuBoard
    {
        Cell this[int row, int column] { get; set; }
        int GetBoardSize();
        string GetBoardString();
        bool IsSolved();
        void PrintBoard();
    }

}
