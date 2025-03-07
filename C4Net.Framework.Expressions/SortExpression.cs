using System;

namespace C4Net.Framework.Expressions
{
    /// <summary>
    /// Expression for the sorting.
    /// </summary>
    [Serializable]
    public class SortExpression
    {
        #region - Properties -

        /// <summary>
        /// Gets or sets the name of the column.
        /// </summary>
        /// <value>The name of the column.</value>
        public string ColumnName { get; set; }

        /// <summary>
        /// Gets or sets the sorting direction.
        /// </summary>
        /// <value>The sorting direction.</value>
        public SortDirection SortDirection { get; set; }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="SortExpression"/> class.
        /// </summary>
        public SortExpression()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SortExpression"/> class.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        public SortExpression(string columnName)
        {
            this.ColumnName = columnName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SortExpression"/> class.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="sortDirection">The sorting direction.</param>
        public SortExpression(string columnName, SortDirection sortDirection)
        {
            this.ColumnName = columnName;
            this.SortDirection = sortDirection;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SortExpression"/> class.
        /// </summary>
        /// <param name="columnExpression">The column expression.</param>
        public SortExpression(ColumnExpression columnExpression)
        {
            this.ColumnName = (string)columnExpression;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SortExpression"/> class.
        /// </summary>
        /// <param name="columnExpression">The column expression.</param>
        /// <param name="sortDirection">The sorting direction.</param>
        public SortExpression(ColumnExpression columnExpression, SortDirection sortingDirection)
        {
            this.ColumnName = (string)columnExpression;
            this.SortDirection = sortingDirection;
        }

        #endregion

        #region - Operators -

        /// <summary>
        /// Implements the operator !.
        /// </summary>
        /// <param name="sortExpression">The sorting expression.</param>
        /// <returns>The result of the operator.</returns>
        public static SortExpression operator !(SortExpression sortExpression)
        {
            if (sortExpression.SortDirection == SortDirection.Ascending)
            {
                sortExpression.SortDirection = SortDirection.Descending;
            }
            else
            {
                sortExpression.SortDirection = SortDirection.Ascending;
            }
            return sortExpression;
        }

        #endregion
    }
}
