using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Solvers.DancingLinksSolver.DancingLinks
{
    internal class DancingLinksColumnNode : DancingLinksNode
    {
        public int Size { get; set; }
        public string Name { get; set; }
        public DancingLinksColumnNode(string name) : base(null)
        {
            Name = name;
            Size = 0;
            Column = this;
        }


        public void Cover()
        {
            RemoveLeftRight();

            for (DancingLinksNode i = Down; i != this; i = i.Down)
            {
                for (DancingLinksNode j = i.Right; j != i; j = j.Right)
                {
                    j.RemoveTopBottom();
                    j.Column.Size--;
                }
            }
        }

        public void Uncover()
        {
            for (DancingLinksNode i = Up; i != this; i = i.Up)
            {
                for (DancingLinksNode j = i.Left; j != i; j = j.Left)
                {
                    j.Column.Size++;
                    j.ReinsertTopBottom();
                }
            }

            ReinsertLeftRight();
        }
    }
}
