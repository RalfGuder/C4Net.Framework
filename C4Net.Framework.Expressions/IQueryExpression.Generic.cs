namespace C4Net.Framework.Expressions
{
    /// <summary>
    /// Interface for the query expression, generic
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IQueryExpression<T> : IQueryExpression
    {
        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>The value.</value>
        T Value { get; }
    }
}
