using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Exceptions
{
    /// <summary>
    /// Exception for invalid choice
    /// </summary>
    internal class InvalidChoice : ArgumentException
    {
        public InvalidChoice(string message) : base(message)
        {

        }
    }
}
