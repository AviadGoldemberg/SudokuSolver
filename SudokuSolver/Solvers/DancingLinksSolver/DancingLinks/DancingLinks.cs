using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Solvers.DancingLinksSolver.DancingLinks
{
    internal class DancingLinks
    {
        private DancingLinksColumnNode _head;
        private List<DancingLinksNode> _solutions;

        /// <summary>
        /// Remove the current node's column from the matrix, and remove the rows
        /// of all the nodes in the current coulmn.
        /// </summary>
        /// <param name="node">Current node.</param>
        private void Cover(DancingLinksColumnNode node)
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
                    rightNode.Column.Size--;
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
        private void Uncover(DancingLinksColumnNode node)
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
                    leftNode.Column.Size++;
                    leftNode = leftNode.Left;
                }
                rowNode = rowNode.Up;
            }

            // add the current node's column back to the matrix
            node.Right.Left = node;
            node.Left.Right = node;
        }

        /// <summary>
        /// Find solution to the exact cover problem.
        /// </summary>
        /// <param name="k">current depth of the search.</param>
        private void Search(int k)
        {
            // if solution is found
            if (_head.Right == _head)
            {
                return;
            }
            // find column with the least amount of values.
            DancingLinksColumnNode column = Choose();
            // remove column and all rows that contain a value in that column
            Cover(column);

            for (DancingLinksNode row = column.Down; row != column; row = row.Down)
            {
                // add row to the solution.
                _solutions.Add(row);

                // remove columns in the row and all rows that contain a value in those columns.
                for (DancingLinksNode rowNode = row.Right; rowNode != row; rowNode = row.Right)
                {
                    Cover(rowNode.Column);
                }
                // search for a solution
                Search(k + 1);

                // no solution found. remove last row added to the solution and restore columns in the row.
                row = _solutions[_solutions.Count - 1];
                _solutions.RemoveAt(_solutions.Count - 1);
                column = row.Column;
                for ( DancingLinksNode rowNode = row.Left; rowNode != row; rowNode = rowNode.Left)
                {
                    Uncover(rowNode.Column);
                }
            }
            // restore selected column and all rows that contain a value in that column.
            Uncover(column);
        }

        /// <summary>
        /// Choose the column with the minimum size.
        /// </summary>
        /// <returns>Column with the minimum size.</returns>
        private DancingLinksColumnNode Choose()
        {
            // init values.
            int min = int.MaxValue;
            DancingLinksColumnNode columnNode = null;

            // iterate all columns from the right of the head node.
            for (DancingLinksColumnNode column = (DancingLinksColumnNode)_head.Right; column != _head; column = (DancingLinksColumnNode)column.Right)
            {
                // need to found the column with the minimum size.
                if(column.Size < min)
                {
                    min = column.Size;
                    columnNode = column;
                }
            }
            return columnNode;
        }
    }
}
