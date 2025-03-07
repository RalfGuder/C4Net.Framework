using System;

namespace C4Net.Framework.Expressions
{
    /// <summary>
    /// Expression for queries
    /// </summary>
    [Serializable]
    public class QueryExpression : IQueryExpression
    {
        #region - Properties -

        /// <summary>
        /// Gets the table.
        /// </summary>
        /// <value>The table.</value>
        public TableExpression TableExpr { get; set; }

        /// <summary>
        /// Gets the condition.
        /// </summary>
        /// <value>The condition.</value>
        public ConditionExpression ConditionExpr { get; set; }

        /// <summary>
        /// Gets the columns.
        /// </summary>
        /// <value>The columns.</value>
        public ColumnExpressionList ColumnsExpr { get; set; }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryExpression"/> class.
        /// </summary>
        public QueryExpression()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryExpression"/> class.
        /// </summary>
        /// <param name="table">The table.</param>
        public QueryExpression(TableExpression table)
        {
            this.TableExpr = table;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryExpression"/> class.
        /// </summary>
        /// <param name="condition">The condition.</param>
        public QueryExpression(ConditionExpression condition)
        {
            this.ConditionExpr = condition;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryExpression"/> class.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="condition">The condition.</param>
        public QueryExpression(TableExpression table, ConditionExpression condition)
        {
            this.TableExpr = table;
            this.ConditionExpr = condition;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryExpression"/> class.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="columns">The columns.</param>
        public QueryExpression(TableExpression table, ConditionExpression condition, ColumnExpressionList columns)
        {
            this.TableExpr = table;
            this.ConditionExpr = condition;
            this.ColumnsExpr = columns;
        }

        #endregion

    }
}
