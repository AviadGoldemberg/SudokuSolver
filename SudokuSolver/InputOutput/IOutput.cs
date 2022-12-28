using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.InputOutput
{
    /// <summary>
    /// Output interface
    /// </summary>
    internal interface IOutput
    {
        /// <summary>
        /// output string
        /// </summary>
        /// <param name="str">string to output</param>
        void Output(string str);

        /// <summary>
        /// output string with new line at the end
        /// </summary>
        /// <param name="str">string to output</param>
        void OutputLine(string str = "");
    }

}
