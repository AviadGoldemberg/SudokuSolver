using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SudokuSolver.InputOutput.Files
{
    /// <summary>
    /// File output handler
    /// </summary>
    internal class FileOutput : IOutput
    {
        private string _filePath;

        public FileOutput(string filePath)
        {
            _filePath = filePath;
        }

        /// <summary>
        /// method which write string to file
        /// </summary>
        /// <param name="str">string to write to file</param>
        public void Output(string str)
        {
            WriteToFile(_filePath, str);
        }

        /// <summary>
        /// method wihch write string to file with new line at the end
        /// </summary>
        /// <param name="str">string to write to file</param>
        public void OutputLine(string str = "")
        {
            WriteToFile(_filePath, str + "\n");
        }

        /// <summary>
        /// method which write string to file
        /// </summary>
        /// <param name="filePath">path to the file</param>
        /// <param name="str">string to write to the file</param>
        /// <exception cref="FileNotFoundException"></exception>
        private static void WriteToFile(string filePath, string str)
        {
            if (!File.Exists(str))
            {
                throw new FileNotFoundException(filePath);
            }
            File.WriteAllText(filePath, str);
        }

    }

}
