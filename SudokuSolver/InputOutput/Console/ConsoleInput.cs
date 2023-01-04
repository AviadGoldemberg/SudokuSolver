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
        private const int BUFFER_MAX_SIZE = 25 * 25 + 1; // buffer size to be able to get large string
        public ConsoleInput()
        {
            // set the console to be able to get large string
            System.Console.SetIn(new System.IO.StreamReader(System.Console.OpenStandardInput(BUFFER_MAX_SIZE)));
        }
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
        /// <returns>string that the user insert</returns>
        public string GetString()
        {
            return System.Console.ReadLine();
        }
    }

}
