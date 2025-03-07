namespace C4Net.Framework.Expressions
{
    public abstract class BaseExpression<T> : TableExpression, IQueryExpression, IQueryExpression<T> where T : TableExpression
    {
        #region - Properties -

        /// <summary>
        /// Gets the table.
        /// </summary>
        /// <value>
        /// The table.
        /// </value>
        public TableExpression TableExpr
        {
            get { return this; }
        }

        /// <summary>
        /// Gets the condition.
        /// </summary>
        /// <value>
        /// The condition.
        /// </value>
        public ConditionExpression ConditionExpr
        {
            get { return new ConditionExpression(); }
        }

        /// <summary>
        /// Gets the columns.
        /// </summary>
        /// <value>
        /// The columns.
        /// </value>
        public ColumnExpressionList ColumnsExpr
        {
            get { return new ColumnExpressionList(this.GetColumnExpression()); }

        }

        #endregion

        #region - Constructor -

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseExpression"/> class.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        public BaseExpression(string tableName)
            : base(tableName)
        {
        }

        #endregion

        #region - Methods -

        /// <summary>
        /// Gets the column expression.
        /// </summary>
        /// <returns></returns>
        public abstract ColumnExpression[] GetColumnExpression();

        /// <summary>
        /// Gets an OrderBy expression ascending for this table.
        /// </summary>
        /// <param name="columnExpression">The column expression.</param>
        /// <returns></returns>
        public virtual ConditionExpression OrderBy(ColumnExpression columnExpression)
        {
            ConditionExpression conditionExpression = new ConditionExpression();
            return conditionExpression.OrderBy(columnExpression);
        }

        /// <summary>
        /// Gets an OrderBy expression for this table.
        /// </summary>
        /// <param name="columnExpression">The column expression.</param>
        /// <param name="sortingDirection">The sorting direction.</param>
        /// <returns></returns>
        public virtual ConditionExpression OrderBy(ColumnExpression columnExpression, SortDirection sortingDirection)
        {
            ConditionExpression conditionExpression = new ConditionExpression();
            return conditionExpression.OrderBy(columnExpression, sortingDirection);
        }

        /// <summary>
        /// Gets a where condition for this table.
        /// </summary>
        /// <param name="expressionElement">The expression element.</param>
        /// <returns></returns>
        public virtual ConditionExpression Where(ExpressionElement expressionElement)
        {
            ConditionExpression conditionExpression = new ConditionExpression();
            return conditionExpression.Where(expressionElement);
        }

        /// <summary>
        /// Gets an except columns expression for this table.
        /// </summary>
        /// <param name="columns">The columns.</param>
        /// <returns></returns>
        public virtual ColumnExpressionList Except(params ColumnExpression[] columns)
        {
            ColumnExpressionList columnExpressionCollection = this.ColumnsExpr;
            return columnExpressionCollection.Except(columns);
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public T Value
        {
            get { return (T)(this as TableExpression); }
        }

        #endregion

    }
}
