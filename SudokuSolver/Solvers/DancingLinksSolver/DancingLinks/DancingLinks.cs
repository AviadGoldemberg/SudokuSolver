using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Solvers.DancingLinksSolver.DancingLinks
{
    internal class DancingLinks
    {
        /// <summary>
        /// Remove the current node's column from the matrix, and remove the rows
        /// of all the nodes in the current coulmn.
        /// </summary>
        /// <param name="node">Current node.</param>
        private void Cover(DancingLinksNode node)
        {
            // remove the current node's column from the matrix
            node.Right.Left = node.Left;
            node.Left.Right = node.Right;

            // remove the rows from the nodes in the current column
            DancingLinksNode rowNode = node.Down;
            while(rowNode != node)
            {
                DancingLinksNode rightNode = rowNode.Right;
                while(rightNode != rowNode)
                {
                    rightNode.Down.Up = rightNode.Up;
                    rightNode.Up.Down = rightNode.Down;
                    rightNode.Size--;
                    rightNode = rightNode.Right;
                }
                rowNode = rowNode.Right;
            }
        }

        /// <summary>
        /// Adds the rows of all the nodes in the current column back, and adds the 
        /// current nodes column back to the matrix.
        /// </summary>
        /// <param name="node">Current node.</param>
        private void Uncover(DancingLinksNode node)
        {
            // add the rows back to the nodes in the current column
            DancingLinksNode rowNode = node.Up;
            while(rowNode != node)
            {
                DancingLinksNode leftNode = rowNode.Left;
                while(leftNode != rowNode)
                {
                    leftNode.Down.Up = leftNode;
                    leftNode.Up.Down = leftNode;
                    leftNode.Size++;
                    leftNode = leftNode.Left;
                }
                rowNode = rowNode.Up;
            }

            // add the current node's column back to the matrix
            node.Right.Left = node;
            node.Left.Right = node;
        }
    }
}
