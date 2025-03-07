using System;
using C4Net.Framework.Expressions.Operators;

namespace C4Net.Framework.Expressions
{
    /// <summary>
    /// Expression for an aggregate function.
    /// </summary>
    [Serializable]
    public class FunctionExpression : OperationElement
    {
        #region - Properties -

        /// <summary>
        /// Gets or sets the function operator.
        /// </summary>
        /// <value>The function operator.</value>
        public FunctionOperator FunctionOperator { get; set; }

        /// <summary>
        /// Gets or sets the column expression.
        /// </summary>
        /// <value>The column expression.</value>
        public ColumnExpression ColumnExpression { get; set; }

        #endregion

        #region - Constructor -

        /// <summary>
        /// Initializes a new instance of the <see cref="FunctionExpression"/> class.
        /// </summary>
        public FunctionExpression()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FunctionExpression"/> class.
        /// </summary>
        /// <param name="functionOperator">The function operator.</param>
        /// <param name="columnName">Name of the column.</param>
        public FunctionExpression(FunctionOperator functionOperator, string columnName)
        {
            this.FunctionOperator = functionOperator;
            this.ColumnExpression = new ColumnExpression(columnName);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FunctionExpression"/> class.
        /// </summary>
        /// <param name="functionOperator">The function operator.</param>
        /// <param name="columnExpression">The column expression.</param>
        public FunctionExpression(FunctionOperator functionOperator, ColumnExpression columnExpression)
        {
            this.FunctionOperator = functionOperator;
            this.ColumnExpression = columnExpression;
        }

        #endregion
    }
}
