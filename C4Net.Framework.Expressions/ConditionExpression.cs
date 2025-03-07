using System;
using System.Collections.Generic;
using C4Net.Framework.Expressions.Operators;

namespace C4Net.Framework.Expressions
{
    /// <summary>
    /// Expression for a Condition.
    /// </summary>
    [Serializable]
    public class ConditionExpression
    {
        #region - Fields -

        private List<SortExpression> sortExpressions = new List<SortExpression>();

        #endregion

        #region - Properties -

        /// <summary>
        /// Gets or sets the expression element.
        /// </summary>
        /// <value>The expression element.</value>
        public ExpressionElement ExpressionElement { get; set; }

        /// <summary>
        /// Gets the sorting expressions.
        /// </summary>
        /// <value>The sorting expressions.</value>
        public List<SortExpression> SortExpressions
        {
            get { return this.sortExpressions; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has sorting expressions.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has sorting expressions; otherwise, <c>false</c>.
        /// </value>
        public bool HasSortExpressions
        {
            get { return this.sortExpressions.Count > 0; }
        }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="ConditionExpression"/> class.
        /// </summary>
        public ConditionExpression()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConditionExpression"/> class.
        /// </summary>
        /// <param name="expressionElement">The expression element.</param>
        public ConditionExpression(ExpressionElement expressionElement)
            : this()
        {
            this.ExpressionElement = expressionElement;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConditionExpression"/> class.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="expressionOperator">The expression operator.</param>
        /// <param name="value">The value.</param>
        public ConditionExpression(string columnName, ElementOperator expressionOperator, object value)
            : this()
        {
            this.ExpressionElement = new OperationExpression(columnName, expressionOperator, value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConditionExpression"/> class.
        /// </summary>
        /// <param name="operationElement">The operation element.</param>
        /// <param name="expressionOperator">The expression operator.</param>
        /// <param name="value">The value.</param>
        public ConditionExpression(OperationElement operationElement, ElementOperator expressionOperator, object value)
            : this()
        {
            this.ExpressionElement = new OperationExpression(operationElement, expressionOperator, value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConditionExpression"/> class.
        /// </summary>
        /// <param name="sortExpression">The sorting expression.</param>
        public ConditionExpression(SortExpression sortExpression)
            : this()
        {
            this.OrderBy(sortExpression);
        }

        #endregion

        #region - Methods -

        /// <summary>
        /// Wheres the specified expression element.
        /// </summary>
        /// <param name="expressionElement">The expression element.</param>
        /// <returns></returns>
        public ConditionExpression Where(ExpressionElement expressionElement)
        {
            this.ExpressionElement = expressionElement;
            return this;
        }

        /// <summary>
        /// Wheres the specified expression element1.
        /// </summary>
        /// <param name="expressionElement1">The expression element1.</param>
        /// <param name="relationOperator">The relation operator.</param>
        /// <param name="expressionElement2">The expression element2.</param>
        /// <returns></returns>
        public ConditionExpression Where(ExpressionElement expressionElement1, RelationOperator relationOperator, ExpressionElement expressionElement2)
        {
            if (expressionElement1 is RelationExpression)
            {
                if (expressionElement2 is RelationExpression)
                {
                    this.ExpressionElement = new RelationExpression(expressionElement1 as RelationExpression, relationOperator, expressionElement2 as RelationExpression);
                }
                else
                {
                    this.ExpressionElement = new RelationExpression(expressionElement1 as RelationExpression, relationOperator, expressionElement2 as OperationExpression);
                }
            }
            else
            {
                if (expressionElement2 is RelationExpression)
                {
                    this.ExpressionElement = new RelationExpression(expressionElement1 as OperationExpression, relationOperator, expressionElement2 as RelationExpression);
                }
                else
                {
                    this.ExpressionElement = new RelationExpression(expressionElement1 as OperationExpression, relationOperator, expressionElement2 as OperationExpression);
                }
            }
            return this;
        }

        /// <summary>
        /// Get the order by.
        /// </summary>
        /// <param name="sortingExpression">The sorting expression.</param>
        /// <returns></returns>
        public ConditionExpression OrderBy(SortExpression sortExpression)
        {
            if (sortExpression != null)
            {
                this.SortExpressions.Add(sortExpression);
                return this;
            }
            else
            {
                throw new ArgumentNullException("sortingExpression");
            }
        }

        /// <summary>
        /// Orders the by.
        /// </summary>
        /// <param name="columnExpression">The column expression.</param>
        /// <returns></returns>
        public ConditionExpression OrderBy(ColumnExpression columnExpression)
        {
            if (!object.Equals(columnExpression, null))
            {
                this.SortExpressions.Add(new SortExpression((string)columnExpression));
                return this;
            }
            else
            {
                throw new ArgumentNullException("columnExpression");
            }
        }

        /// <summary>
        /// Orders the by.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        public ConditionExpression OrderBy(string columnName)
        {
            if (columnName != null)
            {
                this.SortExpressions.Add(new SortExpression(columnName));
                return this;
            }
            else
            {
                throw new ArgumentNullException("columnName");
            }
        }

        /// <summary>
        /// Orders the by.
        /// </summary>
        /// <param name="columnExpression">The column expression.</param>
        /// <param name="sortDirection">The sorting direction.</param>
        /// <returns></returns>
        public ConditionExpression OrderBy(ColumnExpression columnExpression, SortDirection sortDirection)
        {
            if (!object.Equals(columnExpression, null))
            {
                this.SortExpressions.Add(new SortExpression((string)columnExpression, sortDirection));
                return this;
            }
            else
            {
                throw new ArgumentNullException("columnExpression");
            }
        }

        /// <summary>
        /// Orders the by.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="sortDirection">The sorting direction.</param>
        /// <returns></returns>
        public ConditionExpression OrderBy(string columnName, SortDirection sortDirection)
        {
            if (columnName != null)
            {
                this.SortExpressions.Add(new SortExpression(columnName, sortDirection));
                return this;
            }
            else
            {
                throw new ArgumentNullException("columnName");
            }
        }

        #endregion

        #region - Operators -

        /// <summary>
        /// Performs an implicit conversion from <see cref="PVP.Core.Expressions.OperationExpression"/> to <see cref="PVP.Core.Expressions.ConditionExpression"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator ConditionExpression(OperationExpression value)
        {
            return new ConditionExpression(value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="PVP.Core.Expressions.RelationExpression"/> to <see cref="PVP.Core.Expressions.ConditionExpression"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator ConditionExpression(RelationExpression value)
        {
            return new ConditionExpression(value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="PVP.Core.Expressions.SortingExpression"/> to <see cref="PVP.Core.Expressions.ConditionExpression"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator ConditionExpression(SortExpression value)
        {
            return new ConditionExpression(value);
        }

        /// <summary>
        /// Implements the operator ^.
        /// </summary>
        /// <param name="conditionExpression">The condition expression.</param>
        /// <param name="sortingExpression">The sorting expression.</param>
        /// <returns>The result of the operator.</returns>
        public static ConditionExpression operator ^(ConditionExpression conditionExpression, SortExpression sortingExpression)
        {
            return conditionExpression.OrderBy(sortingExpression);
        }

        /// <summary>
        /// Implements the operator ^.
        /// </summary>
        /// <param name="conditionExpression">The condition expression.</param>
        /// <param name="columnExpression">The column expression.</param>
        /// <returns>The result of the operator.</returns>
        public static ConditionExpression operator ^(ConditionExpression conditionExpression, ColumnExpression columnExpression)
        {
            return conditionExpression.OrderBy(columnExpression);
        }

        /// <summary>
        /// Implements the operator ^.
        /// </summary>
        /// <param name="conditionExpression">The condition expression.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns>The result of the operator.</returns>
        public static ConditionExpression operator ^(ConditionExpression conditionExpression, string columnName)
        {
            return conditionExpression.OrderBy(columnName);
        }

        #endregion
    }
}
