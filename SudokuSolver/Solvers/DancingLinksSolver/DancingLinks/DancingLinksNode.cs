using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuSolver.Solvers.DancingLinksSolver.DancingLinks;

namespace SudokuSolver.Solvers.DancingLinksSolver
{
    /// <summary>
    /// Represent Dancing Links node in Dancing Links data structre.
    /// </summary>
    internal class DancingLinksNode
    {
        public DancingLinksNode Left { get; set; }
        public DancingLinksNode Right { get; set; }
        public DancingLinksNode Up { get; set; }
        public DancingLinksNode Down { get; set; }
        public DancingLinksColumnNode Column { get; set; }
        public DancingLinksNode(DancingLinksColumnNode column)
        {
            Left = this;
            Right = this;
            Up = this;
            Down = this;
            Column = column;
        }

        public DancingLinksNode LinkDown(DancingLinksNode node)
        {
            node.Down = Down;
            node.Down.Up = node;
            node.Up = this;
            Down = node;
            return node;
        }

        public DancingLinksNode LinkRight(DancingLinksNode node)
        {
            node.Right = Right;
            node.Right.Left = node;
            node.Left = this;
            Right = node;
            return node;
        }

        public void RemoveLeftRight()
        {
            Left.Right = Right;
            Right.Left = Left;
        }

        public void ReinsertLeftRight()
        {
            Left.Right = this;
            Right.Left = this;
        }

        public void RemoveTopBottom()
        {
            Up.Down = Down;
            Down.Up = Up;
        }

        public void ReinsertTopBottom()
        {
            Up.Down = this;
            Down.Up = this;
        }
    }
}
