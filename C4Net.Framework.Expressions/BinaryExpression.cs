using System;
using System.Collections.Generic;
using C4Net.Framework.Expressions.Operators;

namespace C4Net.Framework.Expressions
{
    /// <summary>
    /// Expression for binary operations.
    /// </summary>
    [Serializable]
    public class BinaryExpression : OperationElement
    {
        #region - Fields -

        private List<BinaryExpression> sons = new List<BinaryExpression>();

        #endregion

        #region - Properties -

        /// <summary>
        /// Gets or sets the binary operator.
        /// </summary>
        /// <value>The binary operator.</value>
        public BinaryOperator BinaryOperator { get; set; }

        /// <summary>
        /// Gets or sets the operation element.
        /// </summary>
        /// <value>The operation element.</value>
        public OperationElement OperationElement { get; set; }

        /// <summary>
        /// Gets the sons.
        /// </summary>
        /// <value>The sons.</value>
        public List<BinaryExpression> Sons
        {
            get { return this.sons; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has sons.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has sons; otherwise, <c>false</c>.
        /// </value>
        public bool HasSons
        {
            get { return this.Sons.Count > 0; }
        }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryExpression"/> class.
        /// </summary>
        public BinaryExpression()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryExpression"/> class.
        /// </summary>
        /// <param name="operationElement">The operation element.</param>
        public BinaryExpression(OperationElement operationElement)
            : this()
        {
            this.OperationElement = operationElement;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryExpression"/> class.
        /// </summary>
        /// <param name="operationElement1">The operation element1.</param>
        /// <param name="binaryOperator">The binary operator.</param>
        /// <param name="operationElement2">The operation element2.</param>
        public BinaryExpression(OperationElement operationElement1, BinaryOperator binaryOperator, OperationElement operationElement2)
            : this()
        {
            BinaryExpression binaryExpression1 = new BinaryExpression(operationElement1);
            BinaryExpression binaryExpression2 = new BinaryExpression(operationElement2);
            this.Sons.Add(binaryExpression1);
            binaryExpression2.BinaryOperator = binaryOperator;
            this.Sons.Add(binaryExpression2);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryExpression"/> class.
        /// </summary>
        /// <param name="binaryExpression1">The binary expression1.</param>
        /// <param name="binaryOperator">The binary operator.</param>
        /// <param name="binaryExpression2">The binary expression2.</param>
        public BinaryExpression(BinaryExpression binaryExpression1, BinaryOperator binaryOperator, BinaryExpression binaryExpression2)
            : this()
        {
            this.Sons.Add(binaryExpression1);
            binaryExpression2.BinaryOperator = binaryOperator;
            this.Sons.Add(binaryExpression2);
        }

        #endregion

        #region - Methods -

        /// <summary>
        /// Adds the expression.
        /// </summary>
        /// <param name="binaryExpression">The binary expression.</param>
        public void AddExpression(BinaryExpression binaryExpression)
        {
            this.Sons.Add(binaryExpression);
        }

        /// <summary>
        /// Adds the expression.
        /// </summary>
        /// <param name="operationElement">The operation element.</param>
        public void AddExpression(OperationElement operationElement)
        {
            BinaryExpression binaryExpression = new BinaryExpression(operationElement);
            this.AddExpression(binaryExpression);
        }

        /// <summary>
        /// Adds the expression.
        /// </summary>
        /// <param name="binaryOperator">The binary operator.</param>
        /// <param name="operationElement">The operation element.</param>
        public void AddExpression(BinaryOperator binaryOperator, OperationElement operationElement)
        {
            BinaryExpression binaryExpression = new BinaryExpression(operationElement);
            binaryExpression.BinaryOperator = binaryOperator;
            this.AddExpression(binaryExpression);
        }

        /// <summary>
        /// Adds the plus expression.
        /// </summary>
        /// <param name="binaryExpressions">The binary expressions.</param>
        public void AddPlusExpression(params BinaryExpression[] binaryExpressions)
        {
            foreach (BinaryExpression binaryExpression in binaryExpressions)
            {
                binaryExpression.BinaryOperator = BinaryOperator.Plus;
                this.Sons.Add(binaryExpression);
            }
        }

        /// <summary>
        /// Adds the plus expression.
        /// </summary>
        /// <param name="operationElements">The operation elements.</param>
        public void AddPlusExpression(params OperationElement[] operationElements)
        {
            foreach (OperationElement operationElement in operationElements)
            {
                BinaryExpression binaryExpression = new BinaryExpression(operationElement);
                binaryExpression.BinaryOperator = BinaryOperator.Plus;
                this.Sons.Add(binaryExpression);
            }
        }

        /// <summary>
        /// Adds the minus expression.
        /// </summary>
        /// <param name="binaryExpressions">The binary expressions.</param>
        public void AddMinusExpression(params BinaryExpression[] binaryExpressions)
        {
            foreach (BinaryExpression binaryExpression in binaryExpressions)
            {
                binaryExpression.BinaryOperator = BinaryOperator.Minus;
                this.Sons.Add(binaryExpression);
            }
        }

        /// <summary>
        /// Adds the minus expression.
        /// </summary>
        /// <param name="operationElements">The operation elements.</param>
        public void AddMinusExpression(params OperationElement[] operationElements)
        {
            foreach (OperationElement operationElement in operationElements)
            {
                BinaryExpression binaryExpression = new BinaryExpression(operationElement);
                binaryExpression.BinaryOperator = BinaryOperator.Minus;
                this.Sons.Add(binaryExpression);
            }
        }

        /// <summary>
        /// Adds the multiply expression.
        /// </summary>
        /// <param name="binaryExpressions">The binary expressions.</param>
        public void AddMultiplyExpression(params BinaryExpression[] binaryExpressions)
        {
            foreach (BinaryExpression binaryExpression in binaryExpressions)
            {
                binaryExpression.BinaryOperator = BinaryOperator.Multiply;
                this.Sons.Add(binaryExpression);
            }
        }

        /// <summary>
        /// Adds the multiply expression.
        /// </summary>
        /// <param name="operationElements">The operation elements.</param>
        public void AddMultiplyExpression(params OperationElement[] operationElements)
        {
            foreach (OperationElement operationElement in operationElements)
            {
                BinaryExpression binaryExpression = new BinaryExpression(operationElement);
                binaryExpression.BinaryOperator = BinaryOperator.Multiply;
                this.Sons.Add(binaryExpression);
            }
        }

        /// <summary>
        /// Adds the divide expression.
        /// </summary>
        /// <param name="binaryExpressions">The binary expressions.</param>
        public void AddDivideExpression(params BinaryExpression[] binaryExpressions)
        {
            foreach (BinaryExpression binaryExpression in binaryExpressions)
            {
                binaryExpression.BinaryOperator = BinaryOperator.Divide;
                this.Sons.Add(binaryExpression);
            }
        }

        /// <summary>
        /// Adds the divide expression.
        /// </summary>
        /// <param name="operationElements">The operation elements.</param>
        public void AddDivideExpression(params OperationElement[] operationElements)
        {
            foreach (OperationElement operationElement in operationElements)
            {
                BinaryExpression binaryExpression = new BinaryExpression(operationElement);
                binaryExpression.BinaryOperator = BinaryOperator.Divide;
                this.Sons.Add(binaryExpression);
            }
        }

        /// <summary>
        /// Adds the modulo expression.
        /// </summary>
        /// <param name="binaryExpressions">The binary expressions.</param>
        public void AddModuloExpression(params BinaryExpression[] binaryExpressions)
        {
            foreach (BinaryExpression binaryExpression in binaryExpressions)
            {
                binaryExpression.BinaryOperator = BinaryOperator.Modulo;
                this.Sons.Add(binaryExpression);
            }
        }

        /// <summary>
        /// Adds the modulo expression.
        /// </summary>
        /// <param name="operationElements">The operation elements.</param>
        public void AddModuloExpression(params OperationElement[] operationElements)
        {
            foreach (OperationElement operationElement in operationElements)
            {
                BinaryExpression binaryExpression = new BinaryExpression(operationElement);
                binaryExpression.BinaryOperator = BinaryOperator.Modulo;
                this.Sons.Add(binaryExpression);
            }
        }

        /// <summary>
        /// Pluses the specified binary expression.
        /// </summary>
        /// <param name="binaryExpression">The binary expression.</param>
        public void Plus(BinaryExpression binaryExpression)
        {
            binaryExpression.BinaryOperator = BinaryOperator.Plus;
            this.Sons.Add(binaryExpression);
        }

        /// <summary>
        /// Pluses the specified operation element.
        /// </summary>
        /// <param name="operationElement">The operation element.</param>
        public new void Plus(OperationElement operationElement)
        {
            BinaryExpression binaryExpression = new BinaryExpression(operationElement);
            this.Plus(binaryExpression);
        }

        /// <summary>
        /// Minuses the specified binary expression.
        /// </summary>
        /// <param name="binaryExpression">The binary expression.</param>
        public void Minus(BinaryExpression binaryExpression)
        {
            binaryExpression.BinaryOperator = BinaryOperator.Minus;
            this.Sons.Add(binaryExpression);
        }

        /// <summary>
        /// Minuses the specified operation element.
        /// </summary>
        /// <param name="operationElement">The operation element.</param>
        public new void Minus(OperationElement operationElement)
        {
            BinaryExpression binaryExpression = new BinaryExpression(operationElement);
            this.Minus(binaryExpression);
        }

        /// <summary>
        /// Multiplies the specified binary expression.
        /// </summary>
        /// <param name="binaryExpression">The binary expression.</param>
        public void Multiply(BinaryExpression binaryExpression)
        {
            binaryExpression.BinaryOperator = BinaryOperator.Multiply;
            this.Sons.Add(binaryExpression);
        }

        /// <summary>
        /// Multiplies the specified operation element.
        /// </summary>
        /// <param name="operationElement">The operation element.</param>
        public new void Multiply(OperationElement operationElement)
        {
            BinaryExpression binaryExpression = new BinaryExpression(operationElement);
            this.Multiply(binaryExpression);
        }

        /// <summary>
        /// Divides the specified binary expression.
        /// </summary>
        /// <param name="binaryExpression">The binary expression.</param>
        public void Divide(BinaryExpression binaryExpression)
        {
            binaryExpression.BinaryOperator = BinaryOperator.Divide;
            this.Sons.Add(binaryExpression);
        }

        /// <summary>
        /// Divides the specified operation element.
        /// </summary>
        /// <param name="operationElement">The operation element.</param>
        public new void Divide(OperationElement operationElement)
        {
            BinaryExpression binaryExpression = new BinaryExpression(operationElement);
            this.Divide(binaryExpression);
        }

        /// <summary>
        /// Moduloes the specified binary expression.
        /// </summary>
        /// <param name="binaryExpression">The binary expression.</param>
        public void Modulo(BinaryExpression binaryExpression)
        {
            binaryExpression.BinaryOperator = BinaryOperator.Modulo;
            this.Sons.Add(binaryExpression);
        }

        /// <summary>
        /// Moduloes the specified operation element.
        /// </summary>
        /// <param name="operationElement">The operation element.</param>
        public new void Modulo(OperationElement operationElement)
        {
            BinaryExpression binaryExpression = new BinaryExpression(operationElement);
            this.Modulo(binaryExpression);
        }

        #endregion

        #region - Operators -

        /// <summary>
        /// Performs an implicit conversion from <see cref="PVP.Core.Expressions.ColumnExpression"/> to <see cref="PVP.Core.Expressions.BinaryExpression"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator BinaryExpression(ColumnExpression value)
        {
            return new BinaryExpression(value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="PVP.Core.Expressions.FunctionExpression"/> to <see cref="PVP.Core.Expressions.BinaryExpression"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator BinaryExpression(FunctionExpression value)
        {
            return new BinaryExpression(value);
        }

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="binaryExpression1">The binary expression1.</param>
        /// <param name="binaryExpression2">The binary expression2.</param>
        /// <returns>The result of the operator.</returns>
        public static BinaryExpression operator +(BinaryExpression binaryExpression1, BinaryExpression binaryExpression2)
        {
            return new BinaryExpression(binaryExpression1, BinaryOperator.Plus, binaryExpression2);
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="binaryExpression1">The binary expression1.</param>
        /// <param name="binaryExpression2">The binary expression2.</param>
        /// <returns>The result of the operator.</returns>
        public static BinaryExpression operator -(BinaryExpression binaryExpression1, BinaryExpression binaryExpression2)
        {
            return new BinaryExpression(binaryExpression1, BinaryOperator.Minus, binaryExpression2);
        }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="binaryExpression1">The binary expression1.</param>
        /// <param name="binaryExpression2">The binary expression2.</param>
        /// <returns>The result of the operator.</returns>
        public static BinaryExpression operator *(BinaryExpression binaryExpression1, BinaryExpression binaryExpression2)
        {
            return new BinaryExpression(binaryExpression1, BinaryOperator.Multiply, binaryExpression2);
        }

        /// <summary>
        /// Implements the operator /.
        /// </summary>
        /// <param name="binaryExpression1">The binary expression1.</param>
        /// <param name="binaryExpression2">The binary expression2.</param>
        /// <returns>The result of the operator.</returns>
        public static BinaryExpression operator /(BinaryExpression binaryExpression1, BinaryExpression binaryExpression2)
        {
            return new BinaryExpression(binaryExpression1, BinaryOperator.Divide, binaryExpression2);
        }

        /// <summary>
        /// Implements the operator %.
        /// </summary>
        /// <param name="binaryExpression1">The binary expression1.</param>
        /// <param name="binaryExpression2">The binary expression2.</param>
        /// <returns>The result of the operator.</returns>
        public static BinaryExpression operator %(BinaryExpression binaryExpression1, BinaryExpression binaryExpression2)
        {
            return new BinaryExpression(binaryExpression1, BinaryOperator.Modulo, binaryExpression2);
        }

        #endregion
    }
}
