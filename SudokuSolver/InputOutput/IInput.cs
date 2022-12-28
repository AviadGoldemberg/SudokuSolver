using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.InputOutput
{
    /// <summary>
    /// Input interface
    /// </summary>
    internal interface IInput
    {
        /// <summary>
        /// get string as input
        /// </summary>
        /// <param name="output">output handler to show message</param>
        /// <param name="str">message to show</param>
        /// <returns>input string</returns>
        string GetString(IOutput output, string str);

        /// <summary>
        /// get string as input without message.
        /// </summary>
        /// <returns>input string</returns>
        string GetString();
    }

}
