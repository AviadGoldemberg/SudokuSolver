using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.InputOutput.Console
{
    /// <summary>
    /// Console output handler
    /// </summary>
    internal class ConsoleOutput : IOutput
    {
        /// <summary>
        /// output string to console
        /// </summary>
        /// <param name="str">string to output to console</param>
        public void Output(string str)
        {
            System.Console.Write(str);
        }

        /// <summary>
        /// output string with new line at the end to console
        /// </summary>
        /// <param name="str">string to output to the console</param>
        public void OutputLine(string str = "")
        {
            System.Console.WriteLine(str);
        }
    }

}
