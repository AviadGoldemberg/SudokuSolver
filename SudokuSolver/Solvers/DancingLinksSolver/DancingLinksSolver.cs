using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuSolver.Board;

namespace SudokuSolver.Solvers.DancingLinksSolver
{
    internal class DancingLinksSolver : ISudokuSolver
    {
        private readonly int _size;
        private readonly int _sectorSize;
        public DancingLinksSolver(ISudokuBoard board) : base(board)
        {
            _size = board.GetBoardSize();
            _sectorSize = (int)Math.Sqrt(_size);
        }

        public override SolvingResult Solve()
        {
            SolvingResult solvingResult = new SolvingResult();
            int[,] matrix = {
                {1,0,0,1,0,0,1 },
                {1,0,0,1,0,0,0 },
                {0,0,0,0,1,0,0 },
                {0,0,1,0,1,1,0 },
                {0,1,1,0,0,1,0 },
                {0,1,0,0,0,0,1 }
            };
            DancingLinks.DancingLinks dlx = new DancingLinks.DancingLinks(matrix);
            dlx.Solve();

            return solvingResult;
        }
    }
}
