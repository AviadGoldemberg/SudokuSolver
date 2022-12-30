using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Solvers.DancingLinksSolver.DancingLinks
{
    internal class DancingLinksResult
    {
        public List<DancingLinksNode> Solution { get; set; }
        public bool IsSolved { get; set; }
        public DancingLinksResult (List<DancingLinksNode> solution, bool isSolved)
        {
            Solution = solution;
            IsSolved = isSolved;
        }
    }
}
