using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public DancingLinksNode LinkDown(DancingLinksNode node)
        {
            node.Down = Down;
            node.Down.Up = node;
            node.Up = this;
            Down = node;
            return node;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public DancingLinksNode LinkRight(DancingLinksNode node)
        {
            node.Right = Right;
            node.Right.Left = node;
            node.Left = this;
            Right = node;
            return node;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveLeftRight()
        {
            Left.Right = Right;
            Right.Left = Left;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReinsertLeftRight()
        {
            Left.Right = this;
            Right.Left = this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveTopBottom()
        {
            Up.Down = Down;
            Down.Up = Up;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReinsertTopBottom()
        {
            Up.Down = this;
            Down.Up = this;
        }
    }
}
