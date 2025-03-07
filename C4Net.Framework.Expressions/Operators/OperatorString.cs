using System;

namespace C4Net.Framework.Expressions.Operators
{
    /// <summary>
    /// Converts expression operator to string for SQL.
    /// </summary>
    public static class OperatorString
    {
        /// <summary>
        /// Gets the string for binary operators.
        /// </summary>
        /// <param name="binaryOperator">The binary operator.</param>
        /// <returns></returns>
        /// <exception cref="System.NotSupportedException">Unknown Binary Operator</exception>
        public static string GetOperatorStr(BinaryOperator binaryOperator)
        {
            switch (binaryOperator)
            {
                case BinaryOperator.Plus: return " + ";
                case BinaryOperator.Minus: return " - ";
                case BinaryOperator.Multiply: return " * ";
                case BinaryOperator.Divide: return " / ";
                case BinaryOperator.Modulo: return " % ";
                default: throw new NotSupportedException("Unknown Binary Operator");
            }
        }

        /// <summary>
        /// Gets the string for element operators.
        /// </summary>
        /// <param name="elementOperator">The element operator.</param>
        /// <returns></returns>
        /// <exception cref="System.NotSupportedException">Unknown Element Operator</exception>
        public static string GetOperatorStr(ElementOperator elementOperator)
        {
            switch (elementOperator)
            {
                case ElementOperator.Equal: return " = ";
                case ElementOperator.NotEqual: return " <> ";
                case ElementOperator.Greater: return " > ";
                case ElementOperator.GreaterOrEqual: return " >= ";
                case ElementOperator.Less: return " < ";
                case ElementOperator.LessOrEqual: return " <= ";
                case ElementOperator.IsNull: return " IS NULL";
                case ElementOperator.IsNotNull: return " IS NOT NULL";
                case ElementOperator.Like: return " LIKE ";
                case ElementOperator.NotLike: return " NOT LIKE ";
                case ElementOperator.Between: return " BETWEEN ";
                case ElementOperator.NotBetween: return " NOT BETWEEN ";
                case ElementOperator.In: return " IN ";
                case ElementOperator.NotIn: return " NOT IN ";
                default: throw new NotSupportedException("Unknown Element Operator");
            }
        }

        /// <summary>
        /// Gets the string for function operators.
        /// </summary>
        /// <param name="functionOperator">The function operator.</param>
        /// <returns></returns>
        /// <exception cref="System.NotSupportedException">Unknown Function Operator</exception>
        public static string GetOperatorStr(FunctionOperator functionOperator)
        {
            switch (functionOperator)
            {
                case FunctionOperator.Avg: return "AVG";
                case FunctionOperator.Count: return "COUNT";
                case FunctionOperator.Max: return "MAX";
                case FunctionOperator.Min: return "MIN";
                case FunctionOperator.Sum: return "SUM";
                default: throw new NotSupportedException("Unknown Function Operator");
            }
        }

        /// <summary>
        /// Gets the string for relation operators.
        /// </summary>
        /// <param name="relationOperator">The relation operator.</param>
        /// <returns></returns>
        /// <exception cref="System.NotSupportedException">Unknown Relation Operator</exception>
        public static string GetOperatorStr(RelationOperator relationOperator)
        {
            switch (relationOperator)
            {
                case RelationOperator.And: return " AND ";
                case RelationOperator.Or: return " OR ";
                case RelationOperator.Not: return " NOT ";
                case RelationOperator.None: return string.Empty;
                default: throw new NotSupportedException("Unknown Relation Operator");
            }
        }
    }
}
