using System;
using System.Collections;
using System.Data;
using System.Text;
using C4Net.Framework.Core.Types;
using C4Net.Framework.Data.Base;
using C4Net.Framework.Data.Definitions;
using C4Net.Framework.Expressions;
using C4Net.Framework.Expressions.Operators;

namespace C4Net.Framework.Data.Builders
{
    /// <summary>
    /// Build a condition command from a condition expression.
    /// </summary>
    public class ConditionCommandBuilder
    {
        #region - Properties -

        /// <summary>
        /// Gets or sets the condition expression.
        /// </summary>
        /// <value>
        /// The condition expression.
        /// </value>
        public ConditionExpression ConditionExpression { get; set; }

        /// <summary>
        /// Gets the definition.
        /// </summary>
        /// <value>
        /// The definition.
        /// </value>
        public EntityDefinition Definition { get; private set; }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="ConditionCommandBuilder" /> class.
        /// </summary>
        /// <param name="conditionExpression">The condition expression.</param>
        /// <param name="definition">The definition.</param>
        public ConditionCommandBuilder(ConditionExpression conditionExpression, EntityDefinition definition)
        {
            this.ConditionExpression = conditionExpression;
            this.Definition = definition;
        }

        #endregion

        #region - Methods -

        #region - Aux methods -

        /// <summary>
        /// Gets the parameter name from the parameter number.
        /// </summary>
        /// <param name="paramNumber">The param number.</param>
        /// <returns></returns>
        private string GetParameterStr(int paramNumber)
        {
            return "@pc" + paramNumber.ToString();
        }

        /// <summary>
        /// Gets the data type of the object.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        private DbType GetDbType(object value)
        {
            return value == null ? DbType.String : TypesManager.GetDefaultDbType(value.GetType());
        }

        /// <summary>
        /// Gets the data type of the operation element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        private DbType GetDbType(OperationElement element)
        {
            string columnName = string.Empty;
            if (element is TableColumnExpression)
            {
                columnName = (element as TableColumnExpression).TableExpression + "." + (element as TableColumnExpression);
            }
            else if (element is ColumnExpression)
            {
                columnName = (string)(element as ColumnExpression);
            }
            else if (element is ValueExpression)
            {
                return this.GetDbType((element as ValueExpression).Value);
            }
            if (this.Definition.AttributeDict.ContainsKey(columnName))
            {
                return this.Definition.AttributeDict[columnName].DataType;
            }
            return DbType.String;
        }

        #endregion

        #region - String getters -

        /// <summary>
        /// Gets the expression string for a column expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        private string GetExpressionStr(ColumnExpression expression)
        {
            if (expression is TableColumnExpression)
            {
                return ".[" + (expression as TableColumnExpression).TableExpression + "].[" + (expression as TableColumnExpression) + "]";
            }
            else
            {
                return this.Definition.GetAttribute((string)expression).ColumnName;
            }
        }

        /// <summary>
        /// Gets the expression string for a function expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        private string GetExpressionStr(FunctionExpression expression)
        {
            return "(SELECT " + OperatorString.GetOperatorStr(expression.FunctionOperator) + "("
                + this.GetExpressionStr(expression.ColumnExpression) + ") FROM "
                + ".[" + expression + "])";
        }

        /// <summary>
        /// Gets the expression string for a sorting expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        private string GetExpressionStr(SortExpression expression)
        {
            if (expression.SortDirection == SortDirection.Descending)
            {
                return "[" + this.Definition.GetAttribute(expression.ColumnName).ColumnName + "] DESC";
            }
            else
            {
                return "[" + this.Definition.GetAttribute(expression.ColumnName).ColumnName + "] ASC";
            }
        }

        #endregion

        #region - Command getters -

        /// <summary>
        /// Gets the grade of the operator (number of members). 3 represents more than 2.
        /// </summary>
        /// <param name="elementOperator">The element operator.</param>
        /// <returns></returns>
        private int OperatorGrade(ElementOperator elementOperator)
        {
            if ((elementOperator == ElementOperator.IsNull) || (elementOperator == ElementOperator.IsNotNull))
            {
                return 0;
            }
            if ((elementOperator == ElementOperator.Between) || (elementOperator == ElementOperator.NotBetween))
            {
                return 2;
            }
            if ((elementOperator == ElementOperator.In) || (elementOperator == ElementOperator.NotIn))
            {
                return 3;
            }
            return 1;
        }

        /// <summary>
        /// Gets the command for an operation expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="paramNumber">The param number.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">
        /// Data type of element is not a collection.
        /// or
        /// Between or NotBetween needs two parameters
        /// </exception>
        private BaseCommand GetOperationCommand(OperationExpression expression, ref int paramNumber)
        {
            BaseCommand result = new BaseCommand();
            StringBuilder builder = new StringBuilder();
            this.AppendExpressionCommand(builder, result, expression.Element, ref paramNumber);
            builder.Append(OperatorString.GetOperatorStr(expression.Operator));
            int grade = OperatorGrade(expression.Operator);
            if (grade == 1)
            {
                if (expression.Value is OperationElement)
                {
                    this.AppendExpressionCommand(builder, result, expression.Value as OperationElement, ref paramNumber);
                }
                else
                {
                    this.AppendExpressionCommand(builder, result, new ValueExpression(expression.Value), ref paramNumber);
                }
            }
            else if (grade > 1)
            {
                if (!(expression.Value is ICollection))
                {
                    throw new ArgumentException("Data type of element is not a collection.");
                }
                ICollection collection = expression.Value as ICollection;
                if ((grade == 2) && (collection.Count != 2))
                {
                    throw new ArgumentException("Between or NotBetween needs two parameters");
                }
                if (grade == 3)
                {
                    builder.Append("(");
                }
                bool isFirst = true;
                foreach (object value in collection)
                {
                    if (isFirst)
                    {
                        isFirst = false;
                    }
                    else
                    {
                        builder.Append(grade == 2 ? " AND " : ", ");
                    }
                    BaseParameter parameter = new BaseParameter(this.GetParameterStr(paramNumber++), value);
                    result.Parameters.Add(parameter);
                    builder.Append(parameter.ParameterName);
                }
                if (grade == 3)
                {
                    builder.Append(")");
                }
            }
            result.CommandText = builder.ToString();
            return result;
        }

        /// <summary>
        /// Gets the command for a relation expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="paramNumber">The param number.</param>
        /// <returns></returns>
        private BaseCommand GetRelationCommand(RelationExpression expression, ref int paramNumber)
        {
            BaseCommand result = new BaseCommand();
            StringBuilder builder = new StringBuilder();
            ExpressionElement element = expression.ExpressionElement;
            if (expression.HasSons)
            {
                if (element != null)
                {
                    if ((expression.RelationOperator == RelationOperator.And) || (expression.RelationOperator == RelationOperator.Or))
                    {
                        builder.Append("(");
                        this.AppendExpressionCommand(builder, result, element, ref paramNumber);
                        builder.Append(")");
                        builder.Append(OperatorString.GetOperatorStr(expression.RelationOperator));
                        builder.Append("(");
                        this.AppendChildExpressionCommand(builder, result, expression, ref paramNumber);
                        builder.Append(")");
                    }
                }
                else
                {
                    builder.Append("(");
                    this.AppendChildExpressionCommand(builder, result, expression, ref paramNumber);
                    builder.Append(")");
                }
            }
            else
            {
                if (element != null)
                {
                    if (expression.RelationOperator == RelationOperator.Not)
                    {
                        builder.Append("( NOT (");
                        this.AppendExpressionCommand(builder, result, element, ref paramNumber);
                        builder.Append("))");
                    }
                    else
                    {
                        this.AppendExpressionCommand(builder, result, element, ref paramNumber);
                    }
                }
            }
            result.CommandText = builder.ToString();
            return result;
        }

        /// <summary>
        /// Gets the command for a binary expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="paramNumber">The param number.</param>
        /// <returns></returns>
        private BaseCommand GetBinaryCommand(BinaryExpression expression, ref int paramNumber)
        {
            BaseCommand result = new BaseCommand();
            StringBuilder builder = new StringBuilder();
            if (expression.HasSons)
            {
                builder.Append("(");
                if (object.Equals(expression.OperationElement, null))
                {
                    this.AppendChildExpressionCommand(builder, result, expression, ref paramNumber);
                }
                else
                {
                    this.AppendExpressionCommand(builder, result, expression.OperationElement, ref paramNumber);
                    string operatorStr = OperatorString.GetOperatorStr(expression.BinaryOperator);
                    if (operatorStr.Length > 0)
                    {
                        builder.Append(") " + operatorStr + " (");
                    }
                    this.AppendChildExpressionCommand(builder, result, expression, ref paramNumber);
                }
                builder.Append(")");
            }
            else if (!object.Equals(expression.OperationElement, null))
            {
                this.AppendExpressionCommand(builder, result, expression.OperationElement, ref paramNumber);
            }
            result.CommandText = builder.ToString();
            return result;
        }

        #endregion

        #region - Appenders -

        /// <summary>
        /// Appends two commadns.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="command">The command.</param>
        /// <param name="otherCommand">The other command.</param>
        private void AppendCommand(StringBuilder builder, BaseCommand command, BaseCommand otherCommand)
        {
            builder.Append(otherCommand.CommandText);
            CommandBuilder.AddParameters(command, otherCommand);
        }

        /// <summary>
        /// Appends an expression to a command.
        /// </summary>
        /// <param name="stringBuilder">The string builder.</param>
        /// <param name="command">The command.</param>
        /// <param name="element">The element.</param>
        /// <param name="paramNumber">The param number.</param>
        /// <exception cref="System.NotSupportedException">Expression type not applicable.</exception>
        private void AppendExpressionCommand(StringBuilder stringBuilder, BaseCommand command, ExpressionElement element, ref int paramNumber)
        {
            if (element is OperationExpression)
            {
                this.AppendCommand(stringBuilder, command, this.GetOperationCommand((element as OperationExpression), ref paramNumber));
            }
            else if (element is RelationExpression)
            {
                this.AppendCommand(stringBuilder, command, this.GetRelationCommand((element as RelationExpression), ref paramNumber));
            }
            else
            {
                throw new NotSupportedException("Expression type not applicable.");
            }
        }

        /// <summary>
        /// Appends an expression element to a command.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="command">The command.</param>
        /// <param name="element">The element.</param>
        /// <param name="paramNumber">The param number.</param>
        private void AppendExpressionCommand(StringBuilder builder, BaseCommand command, OperationElement element, ref int paramNumber)
        {
            if (element is ColumnExpression)
            {
                builder.Append(this.GetExpressionStr(element as ColumnExpression));
            }
            else if (element is FunctionExpression)
            {
                builder.Append(this.GetExpressionStr(element as FunctionExpression));
            }
            else if (element is BinaryExpression)
            {
                this.AppendCommand(builder, command, this.GetBinaryCommand(element as BinaryExpression, ref paramNumber));
            }
            else if (element is ValueExpression)
            {
                BaseParameter parameter = new BaseParameter(this.GetParameterStr(paramNumber++), (element as ValueExpression).Value);
                command.Parameters.Add(parameter);
                builder.Append(parameter.ParameterName);
            }
        }

        /// <summary>
        /// Appends child expressions to a command.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="command">The command.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="paramNumber">The param number.</param>
        private void AppendChildExpressionCommand(StringBuilder builder, BaseCommand command, BinaryExpression expression, ref int paramNumber)
        {
            bool isFirst = true;
            foreach (BinaryExpression son in expression.Sons)
            {
                if (isFirst)
                {
                    isFirst = false;
                }
                else
                {
                    builder.Append(OperatorString.GetOperatorStr(son.BinaryOperator));
                }
                this.AppendExpressionCommand(builder, command, son, ref paramNumber);
            }
        }

        /// <summary>
        /// Appends child expressions to a command.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="command">The command.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="paramNumber">The param number.</param>
        private void AppendChildExpressionCommand(StringBuilder builder, BaseCommand command, RelationExpression expression, ref int paramNumber)
        {
            bool isFirst = true;
            foreach (RelationExpression son in expression.Sons)
            {
                if (isFirst)
                {
                    isFirst = false;
                }
                else
                {
                    if ((son.RelationOperator == RelationOperator.And) || (son.RelationOperator == RelationOperator.Or))
                    {
                        builder.Append(") " + OperatorString.GetOperatorStr(son.RelationOperator) + "(");
                    }
                }
                this.AppendExpressionCommand(builder, command, son, ref paramNumber);
            }
        }

        #endregion

        /// <summary>
        /// Gets the expression command.
        /// </summary>
        /// <returns></returns>
        public BaseCommand GetExpressionCommand()
        {
            if (this.ConditionExpression != null)
            {
                BaseCommand result = new BaseCommand();
                StringBuilder stringBuilder = new StringBuilder();
                // Build the where part.
                if (this.ConditionExpression.ExpressionElement != null)
                {
                    stringBuilder.Append("WHERE ");
                    int paramNumber = 1;
                    this.AppendExpressionCommand(stringBuilder, result, this.ConditionExpression.ExpressionElement, ref paramNumber);
                }
                // Build the order by part.
                if (this.ConditionExpression.HasSortExpressions)
                {
                    if (stringBuilder.Length > 0)
                    {
                        stringBuilder.Append(" ");
                    }
                    stringBuilder.Append("ORDER BY ");
                    bool isFirst = true;
                    foreach (SortExpression expression in this.ConditionExpression.SortExpressions)
                    {
                        if (isFirst)
                        {
                            isFirst = false;
                        }
                        else
                        {
                            stringBuilder.Append(", ");
                        }
                        stringBuilder.Append(this.GetExpressionStr(expression));
                    }
                }
                result.CommandText = stringBuilder.ToString();
                return result;
            }
            return null;
        }

        /// <summary>
        /// Gets the command.
        /// </summary>
        /// <param name="conditionExpression">The condition expression.</param>
        /// <param name="dbTypes">The db types.</param>
        /// <param name="tableExpression">The table expression.</param>
        /// <returns></returns>
        public static BaseCommand GetCommand(ConditionExpression conditionExpression, EntityDefinition definition)
        {
            ConditionCommandBuilder builder = new ConditionCommandBuilder(conditionExpression, definition);
            return builder.GetExpressionCommand();
        }

        #endregion
    }
}
