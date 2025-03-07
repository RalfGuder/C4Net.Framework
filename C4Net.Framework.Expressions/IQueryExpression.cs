namespace C4Net.Framework.Expressions
{
    /// <summary>
    /// Interface for the query expression
    /// </summary>
    public interface IQueryExpression
    {
        #region - Properties -

        /// <summary>
        /// Gets the table.
        /// </summary>
        /// <value>The table.</value>
        TableExpression TableExpr { get; }

        /// <summary>
        /// Gets the condition.
        /// </summary>
        /// <value>The condition.</value>
        ConditionExpression ConditionExpr { get; }

        /// <summary>
        /// Gets the columns.
        /// </summary>
        /// <value>The columns.</value>
        ColumnExpressionList ColumnsExpr { get; }

        #endregion
    }
}
