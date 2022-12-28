using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Exceptions
{
    /// <summary>
    /// Exception for invalid board string
    /// </summary>
    internal class InvalidBoardString : ArgumentException
    {
        public InvalidBoardString(string message) : base(message)
        {

        }
    }

}
