using System;

namespace C4Net.Framework.Expressions
{
    /// <summary>
    /// Expression for a column of a table.
    /// </summary>
    [Serializable]
    public class TableColumnExpression : ColumnExpression
    {
        #region - Properties -

        /// <summary>
        /// Gets the table expression.
        /// </summary>
        /// <value>The table expression.</value>
        public TableExpression TableExpression { get; private set; }

        #endregion

        #region - Constructor -

        /// <summary>
        /// Initializes a new instance of the <see cref="TableColumnExpression"/> class.
        /// </summary>
        /// <param name="tableExpression">The table expression.</param>
        /// <param name="columnName">Name of the column.</param>
        public TableColumnExpression(TableExpression tableExpression, string columnName)
            : base(columnName)
        {
            this.TableExpression = tableExpression;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TableColumnExpression"/> class.
        /// </summary>
        /// <param name="tableExpression">The table expression.</param>
        /// <param name="columnExpression">The column expression.</param>
        public TableColumnExpression(TableExpression tableExpression, ColumnExpression columnExpression)
            : base((string)columnExpression)
        {
            this.TableExpression = tableExpression;
        }

        #endregion
    }
}
