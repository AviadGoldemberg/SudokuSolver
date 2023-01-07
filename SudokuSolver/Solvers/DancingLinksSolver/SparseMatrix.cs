using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Solvers.DancingLinksSolver
{
    internal class SparseMatrixIndex : IComparable<SparseMatrixIndex>
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public SparseMatrixIndex(int row, int col)
        {
            Row = row;
            Column = col;
        }

        public int CompareTo(SparseMatrixIndex other)
        {
            // first compare the rows
            int result = Row.CompareTo(other.Row);
            if (result != 0)
                return result;
            // if rows are equal compare the columns
            return Column.CompareTo(other.Column);
        }
    }
    internal class SparseMatrix
    {
        // dictionary to store the values. key is row, value is sorted set of columns.
        private Dictionary<int, SortedSet<int>> sparseMatrix;
        public int Length1 { get; set; }
        internal int Length2 { get; set; }
        
        public SparseMatrix(int length1, int length2)
        {
            sparseMatrix = new Dictionary<int, SortedSet<int>>();
            Length1 = length1;
            Length2 = length2;
        }

        /// <summary>
        /// Add index to the sparse matrix.
        /// </summary>
        /// <param name="index">Index to add.</param>
        public void Add(SparseMatrixIndex index)
        {
            if (sparseMatrix.ContainsKey(index.Row))
            {
                sparseMatrix[index.Row].Add(index.Column);
            }
            else
            {
                sparseMatrix[index.Row] = new SortedSet<int>();
                sparseMatrix[index.Row].Add(index.Column);
            }
        }

        /// <summary>
        /// Remove row from the matrix.
        /// </summary>
        /// <param name="row">Row to remove.</param>
        public void Remove(int row)
        {
            if (sparseMatrix.ContainsKey(row))
                sparseMatrix.Remove(row);
        }

        /// <summary>
        /// Method which return the columns in row at the matrix.
        /// </summary>
        /// <param name="row">Row to get.</param>
        /// <returns></returns>
        public SortedSet<int> GetSparseMatrixColumn(int row)
        {
            if (sparseMatrix.ContainsKey(row))
                return sparseMatrix[row];
            return new SortedSet<int>();
        }
    }
}
