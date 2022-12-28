using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Board
{
    /// <summary>
    /// Represent cell in the board
    /// </summary>
    internal class Cell
    {
        public int Row { get; set; }
        public int Colunm { get; set; }
        public int Val { get; set; }
        public bool IsConst { get; }

        public Cell(int row, int colunm, int val, bool isConst)
        {
            Row = row;
            Colunm = colunm;
            Val = val;
            IsConst = isConst;
        }
    }

}
