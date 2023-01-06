using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuSolver.Solvers.DancingLinksSolver;
using SudokuSolver.Solvers;
using SudokuSolver.Board;
using SudokuSolver.Menu;
using System.Threading;

namespace SudokuSolver
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Menu.Menu menu = new Menu.Menu();
            menu.Start();
        }
    }
}
