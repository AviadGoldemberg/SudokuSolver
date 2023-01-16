using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Solvers.DancingLinksSolver.DancingLinks
{
    /// <summary>
    /// Represent Dancing Links column in the Dancing Links data structre.
    /// </summary>
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

        /// <summary>
        /// Method in the Dancing Links algorithm which used to remove a column from the matrix representation of a problem being solved.
        /// This is done by covering the column by removing its header node from the doubly-linked list structure and then iterating through
        /// the nodes in the column and removing them from their respective row lists.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

        /// <summary>
        /// Method which reverse the <see cref="Cover"/> method. 
        /// Used to restore a covered column back into the matrix. 
        /// This is done by reinserting the header node for the column back into the doubly-linked list structure and then iterating
        /// through the nodes in the column and reinserting them into their respective row lists.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
