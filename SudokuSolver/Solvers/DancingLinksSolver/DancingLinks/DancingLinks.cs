using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Concurrent;

namespace SudokuSolver.Solvers.DancingLinksSolver.DancingLinks
{
    /// <summary>
    /// Implemntation of Dancing Links algorithm.
    /// The algorithm support find number of solution to the problem.
    /// </summary>
    internal class DancingLinks
    {
        private DancingLinksColumnNode _head;
        private Stack<DancingLinksNode> _solutions;
        private List<Stack<DancingLinksNode>> _allSolutions;
        private readonly int _solutionCount;

        /// <summary>
        /// Constructor to Dancing Links algorithm.
        /// </summary>
        /// <param name="matrix">Sparse matrix which represent exact cover problem.</param>
        /// <param name="solutionCount">Number of solution to get.</param>
        public DancingLinks(SparseMatrix matrix, int solutionCount)
        {
            _solutionCount = solutionCount;
            _solutions = new Stack<DancingLinksNode>();
            _allSolutions = new List<Stack<DancingLinksNode>>();
            // Create the Dancing Links matrix.
            initDLX(matrix);
        }

        /// <summary>
        /// Method which solve the exact cover problem using Dancing Links algorithm.
        /// </summary>
        /// <returns>If the problem solved or not.</returns>
        public DancingLinksResult Solve()
        {
            bool isSolved = Search();
            return new DancingLinksResult(_allSolutions, isSolved);
        }

        /// <summary>
        /// Recursive function that searches for N solutions to the exact cover problem.
        /// N is amount of solutions that was passed in Dancing Links Constructor.
        /// </summary>
        /// <returns>True if a solution is found, false otherwise.</returns>
        private bool Search()
        {
            // if solution is found
            if (_head.Right == _head)
            {
                // add the solution to list of solutions
                _allSolutions.Add(new Stack<DancingLinksNode>(_solutions));
                // if we didn't get the amount of solutions we need, return false to search for more solutions.
                if (_allSolutions.Count != _solutionCount)
                    return false;
                // if we get the amount of solutions we need, return true to stop.
                else
                    return true;
            }
            // find best next column.
            DancingLinksColumnNode column = Choose();
            // cover the selected column.
            column.Cover();
            for (DancingLinksNode row = column.Down; row != column; row = row.Down)
            {
                _solutions.Push(row);

                // cover nodes in row.
                for (DancingLinksNode rowNode = row.Right; rowNode != row; rowNode = rowNode.Right)
                {
                    rowNode.Column.Cover();
                }
                // search for a solution. if we found all solutions that need, stop.
                if (Search())
                    return true;

                // no solution found. remove last row added to the solution and restore columns in the row.
                row = _solutions.Pop();
                column = row.Column;
                
                // uncover to nodes that were covered.
                for (DancingLinksNode rowNode = row.Left; rowNode != row; rowNode = rowNode.Left)
                {
                    rowNode.Column.Uncover();
                }
            }
            // uncover the column that was selected.
            column.Uncover();
            return false;
        }

        /// <summary>
        /// Choose the column with the minimum size, or that have size 1.
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
                // after benchmark, I found that if the column size is one or zero,
                // in every case that I check the search method run faster.
                // if I found column with size 1, I can to return and avoid the loops that not necessary.
                if (column.Size <= 1)
                    return column;

                // need to found the column with the minimum size.
                if (column.Size < min)
                {
                    min = column.Size;
                    columnNode = column;
                }
            }
            return columnNode;
        }

        /// <summary>
        /// Method which create Dancing Links data structure by the SparseMatrix which represent the problem.
        /// </summary>
        /// <param name="matrix">SparseMatrix which represent the problem to solve.</param>
        private void initDLX(SparseMatrix matrix)
        {
            int columnsNumber = matrix.Length2;

            _head = new DancingLinksColumnNode("ROOT");
            Dictionary<int, DancingLinksColumnNode> columnNodes = new Dictionary<int, DancingLinksColumnNode>();

            // link all column nodes
            for (int i = 0; i < columnsNumber; i++)
            {
                DancingLinksColumnNode newNode = new DancingLinksColumnNode(i.ToString());
                columnNodes[i] = newNode;
                _head = (DancingLinksColumnNode)_head.LinkRight(newNode);
            }
            // set head to the first node
            _head = _head.Right.Column;

            // get every index in the sparse matrix and connect the nodes.
            foreach (int row in matrix.GetSparseMatrixRows())
            {
                DancingLinksNode prevNode = null;
                foreach (int col in matrix.GetSparseMatrixColumn(row))
                {

                    DancingLinksColumnNode column = columnNodes[col];
                    DancingLinksNode newNode = new DancingLinksNode(column);
                    if (prevNode == null)
                    {
                        prevNode = newNode;
                    }
                    column.Up.LinkDown(newNode);
                    prevNode = prevNode.LinkRight(newNode);
                    column.Size++;
                }
            }
            _head.Size = columnsNumber;
        }
    }
}