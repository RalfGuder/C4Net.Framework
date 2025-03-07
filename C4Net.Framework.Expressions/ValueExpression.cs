using System;

namespace C4Net.Framework.Expressions
{
    /// <summary>
    /// Expression for a value.
    /// </summary>
    [Serializable]
    public class ValueExpression : OperationElement
    {
        #region - Properties -

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public object Value { get; set; }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueExpression"/> class.
        /// </summary>
        public ValueExpression()
        {
            this.Value = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueExpression"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public ValueExpression(object value)
        {
            this.Value = value;
        }

        #endregion

    }
}
