using System;
using System.Linq;
using System.Reflection;

namespace C4Net.Framework.Expressions
{
    /// <summary>
    /// Base expression for querying entities.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseEntityExpression<T> : BaseExpression<T> where T : TableExpression
    {
        #region - Fields -

        /// <summary>
        /// The columns.
        /// </summary>
        protected ColumnExpression[] columns = null;

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseEntityExpression{T}"/> class.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="type">The type.</param>
        public BaseEntityExpression(string tableName, Type type)
            : base(tableName)
        {
            columns = new ColumnExpression[type.GetProperties().Count()];
            int i = 0;
            foreach (PropertyInfo info in type.GetProperties())
            {
                columns[i++] = new ColumnExpression(info.Name);
            }
        }

        #endregion

        #region - Methods -

        /// <summary>
        /// Gets the column expression.
        /// </summary>
        /// <returns></returns>
        public override ColumnExpression[] GetColumnExpression()
        {
            return this.columns;
        }

        #endregion
    }
}
