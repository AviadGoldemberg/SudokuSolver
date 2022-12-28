using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SudokuSolver.InputOutput.Files
{
    /// <summary>
    /// File input handler
    /// </summary>
    internal class FileInput : IInput
    {
        private string _filePath = "";

        public FileInput(string filePath)
        {
            _filePath = filePath;
        }

        /// <summary>
        /// method which get string from file
        /// </summary>
        /// <param name="output">output handler to output message</param>
        /// <param name="str">message to output</param>
        /// <returns>string that was read from the file</returns>
        public string GetString(IOutput output, string str)
        {
            output.Output(str);
            return GetStringFromFile(_filePath);
        }

        /// <summary>
        /// method which get string from file without show any message
        /// </summary>
        /// <returns>string that was read from the file</returns>
        public string GetString()
        {
            return GetStringFromFile(_filePath);
        }

        /// <summary>
        /// method which get string from file.
        /// </summary>
        /// <param name="filePath">file path to the file</param>
        /// <returns>string that was read from the file</returns>
        /// <exception cref="FileNotFoundException"></exception>
        private static string GetStringFromFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                return File.ReadAllText(filePath);
            }
            throw new FileNotFoundException(filePath);
        }
    }

}
