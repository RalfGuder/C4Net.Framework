using System;
using C4Net.Framework.Expressions.Operators;

namespace C4Net.Framework.Expressions
{
    /// <summary>
    /// Expression for operations
    /// </summary>
    [Serializable]
    public class OperationExpression : ExpressionElement
    {
        #region - Properties -

        /// <summary>
        /// Gets or sets the element.
        /// </summary>
        /// <value>The element.</value>
        public OperationElement Element { get; set; }

        /// <summary>
        /// Gets or sets the operator.
        /// </summary>
        /// <value>The operator.</value>
        public ElementOperator Operator { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public object Value { get; set; }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationExpression"/> class.
        /// </summary>
        public OperationExpression()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationExpression"/> class.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="expressionOperator">The expression operator.</param>
        /// <param name="value">The value.</param>
        public OperationExpression(string columnName, ElementOperator expressionOperator, object value)
        {
            this.Element = new ColumnExpression(columnName);
            this.Operator = expressionOperator;
            this.Value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationExpression"/> class.
        /// </summary>
        /// <param name="operationElement">The operation element.</param>
        /// <param name="expressionOperator">The expression operator.</param>
        /// <param name="value">The value.</param>
        public OperationExpression(OperationElement operationElement, ElementOperator expressionOperator, object value)
        {
            this.Element = operationElement;
            this.Operator = expressionOperator;
            this.Value = value;
        }

        #endregion

        #region - Operators -

        /// <summary>
        /// Implements the operator &amp;.
        /// </summary>
        /// <param name="operationExpression1">The operation expression1.</param>
        /// <param name="operationExpression2">The operation expression2.</param>
        /// <returns>The result of the operator.</returns>
        public static RelationExpression operator &(OperationExpression operationExpression1, OperationExpression operationExpression2)
        {
            return new RelationExpression(operationExpression1, RelationOperator.And, operationExpression2);
        }

        /// <summary>
        /// Implements the operator &amp;.
        /// </summary>
        /// <param name="operationExpression">The operation expression.</param>
        /// <param name="relationExpression">The relation expression.</param>
        /// <returns>The result of the operator.</returns>
        public static RelationExpression operator &(OperationExpression operationExpression, RelationExpression relationExpression)
        {
            return new RelationExpression(operationExpression, RelationOperator.And, relationExpression);
        }

        /// <summary>
        /// Implements the operator |.
        /// </summary>
        /// <param name="operationExpression1">The operation expression1.</param>
        /// <param name="operationExpression2">The operation expression2.</param>
        /// <returns>The result of the operator.</returns>
        public static RelationExpression operator |(OperationExpression operationExpression1, OperationExpression operationExpression2)
        {
            return new RelationExpression(operationExpression1, RelationOperator.Or, operationExpression2);
        }

        /// <summary>
        /// Implements the operator |.
        /// </summary>
        /// <param name="operationExpression">The operation expression.</param>
        /// <param name="relationExpression">The relation expression.</param>
        /// <returns>The result of the operator.</returns>
        public static RelationExpression operator |(OperationExpression operationExpression, RelationExpression relationExpression)
        {
            return new RelationExpression(operationExpression, RelationOperator.Or, relationExpression);
        }

        /// <summary>
        /// Implements the operator !.
        /// </summary>
        /// <param name="operationExpression">The operation expression.</param>
        /// <returns>The result of the operator.</returns>
        public static RelationExpression operator !(OperationExpression operationExpression)
        {
            return new RelationExpression(RelationOperator.Not, operationExpression);
        }

        /// <summary>
        /// Implements the operator ~.
        /// </summary>
        /// <param name="operationExpression">The operation expression.</param>
        /// <returns>The result of the operator.</returns>
        public static OperationExpression operator ~(OperationExpression operationExpression)
        {
            switch (operationExpression.Operator)
            {
                case ElementOperator.Equal:
                    {
                        operationExpression.Operator = ElementOperator.NotEqual;
                        break;
                    }
                case ElementOperator.NotEqual:
                    {
                        operationExpression.Operator = ElementOperator.Equal;
                        break;
                    }
                case ElementOperator.Greater:
                    {
                        operationExpression.Operator = ElementOperator.LessOrEqual;
                        break;
                    }
                case ElementOperator.LessOrEqual:
                    {
                        operationExpression.Operator = ElementOperator.Greater;
                        break;
                    }
                case ElementOperator.Less:
                    {
                        operationExpression.Operator = ElementOperator.GreaterOrEqual;
                        break;
                    }
                case ElementOperator.GreaterOrEqual:
                    {
                        operationExpression.Operator = ElementOperator.Less;
                        break;
                    }
                case ElementOperator.IsNull:
                    {
                        operationExpression.Operator = ElementOperator.IsNotNull;
                        break;
                    }
                case ElementOperator.IsNotNull:
                    {
                        operationExpression.Operator = ElementOperator.IsNull;
                        break;
                    }
                case ElementOperator.Like:
                    {
                        operationExpression.Operator = ElementOperator.NotLike;
                        break;
                    }
                case ElementOperator.NotLike:
                    {
                        operationExpression.Operator = ElementOperator.Like;
                        break;
                    }
                case ElementOperator.Between:
                    {
                        operationExpression.Operator = ElementOperator.NotBetween;
                        break;
                    }
                case ElementOperator.NotBetween:
                    {
                        operationExpression.Operator = ElementOperator.Between;
                        break;
                    }
                case ElementOperator.In:
                    {
                        operationExpression.Operator = ElementOperator.NotIn;
                        break;
                    }
                case ElementOperator.NotIn:
                    {
                        operationExpression.Operator = ElementOperator.In;
                        break;
                    }
            }
            return operationExpression;
        }

        /// <summary>
        /// Implements the operator ^.
        /// </summary>
        /// <param name="operationExpression">The operation expression.</param>
        /// <param name="sortExpression">The sorting expression.</param>
        /// <returns>The result of the operator.</returns>
        public static ConditionExpression operator ^(OperationExpression operationExpression, SortExpression sortExpression)
        {
            return (new ConditionExpression(operationExpression)).OrderBy(sortExpression);
        }

        /// <summary>
        /// Implements the operator ^.
        /// </summary>
        /// <param name="operationExpression">The operation expression.</param>
        /// <param name="columnExpression">The column expression.</param>
        /// <returns>The result of the operator.</returns>
        public static ConditionExpression operator ^(OperationExpression operationExpression, ColumnExpression columnExpression)
        {
            return (new ConditionExpression(operationExpression)).OrderBy(columnExpression);
        }

        /// <summary>
        /// Implements the operator ^.
        /// </summary>
        /// <param name="operationExpression">The operation expression.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns>The result of the operator.</returns>
        public static ConditionExpression operator ^(OperationExpression operationExpression, string columnName)
        {
            return (new ConditionExpression(operationExpression)).OrderBy(columnName);
        }

        #endregion
    }
}
