﻿using System;
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
            //string boardStr = "903000047065014000800920000042000061100075300090000802720049005058037009000100008";
            //string boardStr = "10023400<06000700080007003009:6;0<00:0010=0;00>0300?200>000900<0=000800:0<201?000;76000@000?005=000:05?0040800;0@0059<00100000800200000=00<580030=00?0300>80@000580010002000=9?000<406@0=00700050300<0006004;00@0700@050>0010020;1?900=002000>000>000;0200=3500<";
            //string boardStr = "0E003000000000F000<0000000=00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000600000000000000009000000400000000000000000000000000000000000000000000000000000500000000000000000000000700000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000B000000000000000000000000000000000000000000000000000000000000000000C00000000000000000000000000000000000000000000A000";
            /*while (true)
            {
                ISudokuBoard board = new ArraySudokuBoard(boardStr);
                DancingLinksSolver dlx = new DancingLinksSolver(board);
                SolvingResult solvingResult = dlx.Solve();
                Console.WriteLine(board.BoardOutput());
            }*/
        }
    }
}
