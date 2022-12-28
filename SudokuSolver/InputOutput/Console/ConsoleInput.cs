using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.InputOutput.Console
{
    /// <summary>
    /// Console input handler
    /// </summary>
    internal class ConsoleInput : IInput
    {
        /// <summary>
        /// get string as input from console
        /// </summary>
        /// <param name="output">output handler to output the message</param>
        /// <param name="str">string to output before the input</param>
        /// <returns>string that the user insert</returns>
        public string GetString(IOutput output, string str)
        {
            output.Output(str);
            return System.Console.ReadLine();
            
        }

        /// <summary>
        /// get string as input from console without any message
        /// </summary>
        /// <returns>string that the uuser insert</returns>
        public string GetString()
        {
            return System.Console.ReadLine();
        }
    }

}
