using System;
using System.Collections.Generic;

namespace C4Net.Framework.Expressions
{
    /// <summary>
    /// List of column expressions.
    /// </summary>
    [Serializable]
    public class ColumnExpressionList : List<ColumnExpression>
    {
        #region - Constructor -

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnExpressionList"/> class.
        /// </summary>
        /// <param name="columns">The columns.</param>
        public ColumnExpressionList(ColumnExpression[] columns)
            : base(columns)
        {
        }

        #endregion

        #region - Methods -

        /// <summary>
        /// Excepts the specified columns.
        /// </summary>
        /// <param name="columns">The columns.</param>
        /// <returns></returns>
        public ColumnExpressionList Except(params ColumnExpression[] columns)
        {
            ColumnExpressionList columnExpressionCollection = new ColumnExpressionList(this);
            if ((columns != null) && (columns.Length > 0))
            {
                foreach (ColumnExpression column in columns)
                {
                    columnExpressionCollection.Remove(column);
                }
            }
            return columnExpressionCollection;
        }

        #endregion

        #region - Operators -

        /// <summary>
        /// Performs an implicit conversion from array of <see cref="PVP.Core.Expressions.ColumnExpression"/> to <see cref="PVP.Core.Expressions.ColumnExpressionList"/>.
        /// </summary>
        /// <param name="columns">The columns.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator ColumnExpressionList(ColumnExpression[] columns)
        {
            return new ColumnExpressionList(columns);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="PVP.Core.Expressions.ColumnExpressionList"/> to array of <see cref="PVP.Core.Expressions.ColumnExpression"/>.
        /// </summary>
        /// <param name="columnExpressionList">The column expression list.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator ColumnExpression[](ColumnExpressionList columnExpressionList)
        {
            if (columnExpressionList != null)
            {
                return columnExpressionList.ToArray();
            }
            return null;
        }

        #endregion
    }
}
