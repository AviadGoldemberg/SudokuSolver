using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Solvers.DancingLinksSolver
{
    internal class DancingLinksNode
    {
        public DancingLinksNode Left { get; set; }
        public DancingLinksNode Right { get; set; }
        public DancingLinksNode Up { get; set; }
        public DancingLinksNode Down { get; set; }
        public int Size { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
    }
}
