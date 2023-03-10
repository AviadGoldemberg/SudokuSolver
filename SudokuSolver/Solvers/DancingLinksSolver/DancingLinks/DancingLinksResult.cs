using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Solvers.DancingLinksSolver.DancingLinks
{
    /// <summary>
    /// Represent Dancing Links solving result.
    /// </summary>
    internal class DancingLinksResult
    {
        public List<Stack<DancingLinksNode>> AllSolutions { get; set; }
        public bool IsSolved { get; set; }
        public DancingLinksResult (List<Stack<DancingLinksNode>> solution, bool isSolved)
        {
            AllSolutions = solution;
            IsSolved = isSolved;
        }
    }
}
