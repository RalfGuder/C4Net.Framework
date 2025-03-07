using System;

namespace C4Net.Framework.Expressions
{
    /// <summary>
    /// Expression for a table.
    /// </summary>
    [Serializable]
    public class TableExpression
    {
        #region - Properties -

        /// <summary>
        /// Gets or sets the name of the table.
        /// </summary>
        /// <value>The name of the table.</value>
        public string TableName { get; set; }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="TableExpression"/> class.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        public TableExpression(string tableName)
        {
            this.TableName = tableName;
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
            return this.TableName;
        }

        #endregion

        #region - Operators -

        /// <summary>
        /// Performs an explicit conversion from <see cref="PVP.Core.Expressions.TableExpression"/> to <see cref="System.String"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator string(TableExpression value)
        {
            return value.TableName;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.String"/> to <see cref="PVP.Core.Expressions.TableExpression"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator TableExpression(string value)
        {
            return new TableExpression(value);
        }

        #endregion

    }
}
