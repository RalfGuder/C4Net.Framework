using System;
using System.Collections;
using C4Net.Framework.Expressions.Operators;

namespace C4Net.Framework.Expressions
{
    /// <summary>
    /// Abstract class for an element of an operation.
    /// </summary>
    [Serializable]
    public abstract class OperationElement
    {
        #region - Methods -

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Equals the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public OperationExpression Equal(object value)
        {
            return new OperationExpression(this, ElementOperator.Equal, value);
        }

        /// <summary>
        /// Nots the equal.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public OperationExpression NotEqual(object value)
        {
            return new OperationExpression(this, ElementOperator.NotEqual, value);
        }

        /// <summary>
        /// Lesses the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public OperationExpression Less(object value)
        {
            return new OperationExpression(this, ElementOperator.Less, value);
        }

        /// <summary>
        /// Greaters the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public OperationExpression Greater(object value)
        {
            return new OperationExpression(this, ElementOperator.Greater, value);
        }

        /// <summary>
        /// Lesses the or equal.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public OperationExpression LessOrEqual(object value)
        {
            return new OperationExpression(this, ElementOperator.LessOrEqual, value);
        }

        /// <summary>
        /// Greaters the or equal.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public OperationExpression GreaterOrEqual(object value)
        {
            return new OperationExpression(this, ElementOperator.GreaterOrEqual, value);
        }

        /// <summary>
        /// Determines whether this instance is null.
        /// </summary>
        /// <returns></returns>
        public OperationExpression IsNull()
        {
            return new OperationExpression(this, ElementOperator.IsNull, null);
        }

        /// <summary>
        /// Determines whether [is not null].
        /// </summary>
        /// <returns></returns>
        public OperationExpression IsNotNull()
        {
            return new OperationExpression(this, ElementOperator.IsNotNull, null);
        }

        /// <summary>
        /// Likes the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public OperationExpression Like(string value)
        {
            return new OperationExpression(this, ElementOperator.Like, value);
        }

        /// <summary>
        /// Likes the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="matchOperator">The match operator.</param>
        /// <returns></returns>
        public OperationExpression Like(string value, MatchOperator matchOperator)
        {
            string matchValue = null;
            switch (matchOperator)
            {
                case MatchOperator.Any:
                    {
                        matchValue = string.Format("%{0}%", value);
                        break;
                    }
                case MatchOperator.Left:
                    {
                        matchValue = string.Format("{0}%", value);
                        break;
                    }
                case MatchOperator.Right:
                    {
                        matchValue = string.Format("%{0}", value);
                        break;
                    }
            }
            return new OperationExpression(this, ElementOperator.Like, matchValue);
        }

        /// <summary>
        /// Nots the like.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public OperationExpression NotLike(string value)
        {
            return new OperationExpression(this, ElementOperator.NotLike, value);
        }

        /// <summary>
        /// Nots the like.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="matchOperator">The match operator.</param>
        /// <returns></returns>
        public OperationExpression NotLike(string value, MatchOperator matchOperator)
        {
            string matchValue = null;
            switch (matchOperator)
            {
                case MatchOperator.Any:
                    {
                        matchValue = string.Format("%{0}%", value);
                        break;
                    }
                case MatchOperator.Left:
                    {
                        matchValue = string.Format("{0}%", value);
                        break;
                    }
                case MatchOperator.Right:
                    {
                        matchValue = string.Format("%{0}", value);
                        break;
                    }
            }
            return new OperationExpression(this, ElementOperator.NotLike, matchValue);
        }

        /// <summary>
        /// Betweens the specified min value.
        /// </summary>
        /// <param name="minValue">The min value.</param>
        /// <param name="maxValue">The max value.</param>
        /// <returns></returns>
        public OperationExpression Between(object minValue, object maxValue)
        {
            return new OperationExpression(this, ElementOperator.Between, new object[] { minValue, maxValue });
        }

        /// <summary>
        /// Betweens the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public OperationExpression Between(ICollection values)
        {
            if (values != null)
            {
                if (values.Count >= 2)
                {
                    return new OperationExpression(this, ElementOperator.Between, values);
                }
                else
                {
                    throw new ArgumentException("Between expression must have at least 2 values");
                }
            }
            else
            {
                throw new ArgumentNullException("values");
            }
        }

        /// <summary>
        /// Nots the between.
        /// </summary>
        /// <param name="minValue">The min value.</param>
        /// <param name="maxValue">The max value.</param>
        /// <returns></returns>
        public OperationExpression NotBetween(object minValue, object maxValue)
        {
            return new OperationExpression(this, ElementOperator.NotBetween, new object[] { minValue, maxValue });
        }

        /// <summary>
        /// Nots the between.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public OperationExpression NotBetween(ICollection values)
        {
            if (values != null)
            {
                if (values.Count >= 2)
                {
                    return new OperationExpression(this, ElementOperator.NotBetween, values);
                }
                else
                {
                    throw new ArgumentException("Not Between expression must have at least 2 values");
                }
            }
            else
            {
                throw new ArgumentNullException("values");
            }
        }

        /// <summary>
        /// Ins the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public OperationExpression In(ICollection values)
        {
            if (values != null)
            {
                return new OperationExpression(this, ElementOperator.In, values);
            }
            else
            {
                throw new ArgumentNullException("values");
            }
        }

        /// <summary>
        /// Nots the in.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public OperationExpression NotIn(ICollection values)
        {
            if (values != null)
            {
                return new OperationExpression(this, ElementOperator.NotIn, values);
            }
            else
            {
                throw new ArgumentNullException("values");
            }
        }

        /// <summary>
        /// Pluses the specified operation element.
        /// </summary>
        /// <param name="operationElement">The operation element.</param>
        /// <returns></returns>
        public BinaryExpression Plus(OperationElement operationElement)
        {
            return new BinaryExpression(this, BinaryOperator.Plus, operationElement);
        }

        /// <summary>
        /// Pluses the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public BinaryExpression Plus(object value)
        {
            return new BinaryExpression(this, BinaryOperator.Plus, new ValueExpression(value));
        }

        /// <summary>
        /// Minuses the specified operation element.
        /// </summary>
        /// <param name="operationElement">The operation element.</param>
        /// <returns></returns>
        public BinaryExpression Minus(OperationElement operationElement)
        {
            return new BinaryExpression(this, BinaryOperator.Minus, operationElement);
        }

        /// <summary>
        /// Minuses the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public BinaryExpression Minus(object value)
        {
            return new BinaryExpression(this, BinaryOperator.Minus, new ValueExpression(value));
        }

        /// <summary>
        /// Multiplies the specified operation element.
        /// </summary>
        /// <param name="operationElement">The operation element.</param>
        /// <returns></returns>
        public BinaryExpression Multiply(OperationElement operationElement)
        {
            return new BinaryExpression(this, BinaryOperator.Multiply, operationElement);
        }

        /// <summary>
        /// Multiplies the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public BinaryExpression Multiply(object value)
        {
            return new BinaryExpression(this, BinaryOperator.Multiply, new ValueExpression(value));
        }

        /// <summary>
        /// Divides the specified operation element.
        /// </summary>
        /// <param name="operationElement">The operation element.</param>
        /// <returns></returns>
        public BinaryExpression Divide(OperationElement operationElement)
        {
            return new BinaryExpression(this, BinaryOperator.Divide, operationElement);
        }

        /// <summary>
        /// Divides the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public BinaryExpression Divide(object value)
        {
            return new BinaryExpression(this, BinaryOperator.Divide, new ValueExpression(value));
        }

        /// <summary>
        /// Moduloes the specified operation element.
        /// </summary>
        /// <param name="operationElement">The operation element.</param>
        /// <returns></returns>
        public BinaryExpression Modulo(OperationElement operationElement)
        {
            return new BinaryExpression(this, BinaryOperator.Modulo, operationElement);
        }

        /// <summary>
        /// Moduloes the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public BinaryExpression Modulo(object value)
        {
            return new BinaryExpression(this, BinaryOperator.Modulo, new ValueExpression(value));
        }

        #endregion

        #region - Operators -

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="operationElement">The operation element.</param>
        /// <param name="value">The value.</param>
        /// <returns>The result of the operator.</returns>
        public static OperationExpression operator ==(OperationElement operationElement, object value)
        {
            return new OperationExpression(operationElement, ElementOperator.Equal, value);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="operationElement">The operation element.</param>
        /// <param name="value">The value.</param>
        /// <returns>The result of the operator.</returns>
        public static OperationExpression operator !=(OperationElement operationElement, object value)
        {
            return new OperationExpression(operationElement, ElementOperator.NotEqual, value);
        }

        /// <summary>
        /// Implements the operator &lt;.
        /// </summary>
        /// <param name="operationElement">The operation element.</param>
        /// <param name="value">The value.</param>
        /// <returns>The result of the operator.</returns>
        public static OperationExpression operator <(OperationElement operationElement, object value)
        {
            return new OperationExpression(operationElement, ElementOperator.Less, value);
        }

        /// <summary>
        /// Implements the operator &gt;.
        /// </summary>
        /// <param name="operationElement">The operation element.</param>
        /// <param name="value">The value.</param>
        /// <returns>The result of the operator.</returns>
        public static OperationExpression operator >(OperationElement operationElement, object value)
        {
            return new OperationExpression(operationElement, ElementOperator.Greater, value);
        }

        /// <summary>
        /// Implements the operator &lt;=.
        /// </summary>
        /// <param name="operationElement">The operation element.</param>
        /// <param name="value">The value.</param>
        /// <returns>The result of the operator.</returns>
        public static OperationExpression operator <=(OperationElement operationElement, object value)
        {
            return new OperationExpression(operationElement, ElementOperator.LessOrEqual, value);
        }

        /// <summary>
        /// Implements the operator &gt;=.
        /// </summary>
        /// <param name="operationElement">The operation element.</param>
        /// <param name="value">The value.</param>
        /// <returns>The result of the operator.</returns>
        public static OperationExpression operator >=(OperationElement operationElement, object value)
        {
            return new OperationExpression(operationElement, ElementOperator.GreaterOrEqual, value);
        }

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="operationElement1">The operation element1.</param>
        /// <param name="operationElement2">The operation element2.</param>
        /// <returns>The result of the operator.</returns>
        public static BinaryExpression operator +(OperationElement operationElement1, OperationElement operationElement2)
        {
            return new BinaryExpression(operationElement1, BinaryOperator.Plus, operationElement2);
        }

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="operationElement">The operation element.</param>
        /// <param name="value">The value.</param>
        /// <returns>The result of the operator.</returns>
        public static BinaryExpression operator +(OperationElement operationElement, object value)
        {
            return new BinaryExpression(operationElement, BinaryOperator.Plus, new ValueExpression(value));
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="operationElement1">The operation element1.</param>
        /// <param name="operationElement2">The operation element2.</param>
        /// <returns>The result of the operator.</returns>
        public static BinaryExpression operator -(OperationElement operationElement1, OperationElement operationElement2)
        {
            return new BinaryExpression(operationElement1, BinaryOperator.Minus, operationElement2);
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="operationElement">The operation element.</param>
        /// <param name="value">The value.</param>
        /// <returns>The result of the operator.</returns>
        public static BinaryExpression operator -(OperationElement operationElement, object value)
        {
            return new BinaryExpression(operationElement, BinaryOperator.Minus, new ValueExpression(value));
        }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="operationElement1">The operation element1.</param>
        /// <param name="operationElement2">The operation element2.</param>
        /// <returns>The result of the operator.</returns>
        public static BinaryExpression operator *(OperationElement operationElement1, OperationElement operationElement2)
        {
            return new BinaryExpression(operationElement1, BinaryOperator.Multiply, operationElement2);
        }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="operationElement">The operation element.</param>
        /// <param name="value">The value.</param>
        /// <returns>The result of the operator.</returns>
        public static BinaryExpression operator *(OperationElement operationElement, object value)
        {
            return new BinaryExpression(operationElement, BinaryOperator.Multiply, new ValueExpression(value));
        }

        /// <summary>
        /// Implements the operator /.
        /// </summary>
        /// <param name="operationElement1">The operation element1.</param>
        /// <param name="operationElement2">The operation element2.</param>
        /// <returns>The result of the operator.</returns>
        public static BinaryExpression operator /(OperationElement operationElement1, OperationElement operationElement2)
        {
            return new BinaryExpression(operationElement1, BinaryOperator.Divide, operationElement2);
        }

        /// <summary>
        /// Implements the operator /.
        /// </summary>
        /// <param name="operationElement">The operation element.</param>
        /// <param name="value">The value.</param>
        /// <returns>The result of the operator.</returns>
        public static BinaryExpression operator /(OperationElement operationElement, object value)
        {
            return new BinaryExpression(operationElement, BinaryOperator.Divide, new ValueExpression(value));
        }

        /// <summary>
        /// Implements the operator %.
        /// </summary>
        /// <param name="operationElement1">The operation element1.</param>
        /// <param name="operationElement2">The operation element2.</param>
        /// <returns>The result of the operator.</returns>
        public static BinaryExpression operator %(OperationElement operationElement1, OperationElement operationElement2)
        {
            return new BinaryExpression(operationElement1, BinaryOperator.Modulo, operationElement2);
        }

        /// <summary>
        /// Implements the operator %.
        /// </summary>
        /// <param name="operationElement">The operation element.</param>
        /// <param name="value">The value.</param>
        /// <returns>The result of the operator.</returns>
        public static BinaryExpression operator %(OperationElement operationElement, object value)
        {
            return new BinaryExpression(operationElement, BinaryOperator.Modulo, new ValueExpression(value));
        }

        #endregion
    }
}
