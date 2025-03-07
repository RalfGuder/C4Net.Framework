using System;
using C4Net.Framework.Expressions.Operators;

namespace C4Net.Framework.Expressions
{
    /// <summary>
    /// Expression for a column of a table.
    /// </summary>
    [Serializable]
    public class ColumnExpression : OperationElement
    {
        #region - Fields -

        /// <summary>
        /// The column name.
        /// </summary>
        private readonly string columnName;

        #endregion

        #region - Properties -

        /// <summary>
        /// Gets the avg.
        /// </summary>
        /// <value>The avg.</value>
        public FunctionExpression Avg
        {
            get
            {
                return new FunctionExpression(FunctionOperator.Avg, this);
            }
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>The count.</value>
        public FunctionExpression Count
        {
            get
            {
                return new FunctionExpression(FunctionOperator.Count, this);
            }
        }

        /// <summary>
        /// Gets the max.
        /// </summary>
        /// <value>The max.</value>
        public FunctionExpression Max
        {
            get
            {
                return new FunctionExpression(FunctionOperator.Max, this);
            }
        }

        /// <summary>
        /// Gets the min.
        /// </summary>
        /// <value>The min.</value>
        public FunctionExpression Min
        {
            get
            {
                return new FunctionExpression(FunctionOperator.Min, this);
            }
        }

        /// <summary>
        /// Gets the sum.
        /// </summary>
        /// <value>The sum.</value>
        public FunctionExpression Sum
        {
            get
            {
                return new FunctionExpression(FunctionOperator.Sum, this);
            }
        }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnExpression"/> class.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        public ColumnExpression(string columnName)
        {
            this.columnName = columnName;
        }

        #endregion

        #region - Methods -

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.columnName;
        }

        /// <summary>
        /// Gets the sorting expression
        /// </summary>
        /// <returns></returns>
        public SortExpression OrderBy()
        {
            return new SortExpression(this);
        }

        /// <summary>
        /// Orders the by.
        /// </summary>
        /// <param name="sortDirection">The sorting direction.</param>
        /// <returns></returns>
        public SortExpression OrderBy(SortDirection sortDirection)
        {
            return new SortExpression(this, sortDirection);
        }

        #endregion

        #region - Operators -

        /// <summary>
        /// Performs an explicit conversion from <see cref="PVP.Core.Expressions.ColumnExpression"/> to <see cref="System.String"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator string(ColumnExpression value)
        {
            return value.columnName;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.String"/> to <see cref="PVP.Core.Expressions.ColumnExpression"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator ColumnExpression(string value)
        {
            return new ColumnExpression(value);
        }

        /// <summary>
        /// Implements the operator !.
        /// </summary>
        /// <param name="columnExpression">The column expression.</param>
        /// <returns>The result of the operator.</returns>
        public static SortExpression operator !(ColumnExpression columnExpression)
        {
            return (!columnExpression.OrderBy());
        }

        #endregion

    }
}
