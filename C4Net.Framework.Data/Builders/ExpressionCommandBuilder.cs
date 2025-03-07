using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using C4Net.Framework.Data.Base;
using C4Net.Framework.Data.Definitions;
using C4Net.Framework.Data.Expressions;
using C4Net.Framework.Expressions;

namespace C4Net.Framework.Data.Builders
{
    /// <summary>
    /// Class for building commands from lambda expressions.
    /// </summary>
    public class ExpressionCommandBuilder : ExprReviewer
    {
        #region - Fields -

        private BaseCommand command = null;

        private Stack<BaseCommand> commands = new Stack<BaseCommand>();

        private Stack<ColumnExpression> columns = new Stack<ColumnExpression>();

        private List<SortExpression> sortings = new List<SortExpression>();

        private List<ColumnExpression> columnExpressions = new List<ColumnExpression>();

        private int paramNumber = 1;

        private EntityDefinition definition;

        #endregion

        #region - Properties -

        public Expression Expression { get; private set; }

        #endregion

        #region - Constructors -

        public ExpressionCommandBuilder(Expression expression, EntityDefinition definition)
            : base()
        {
            this.Expression = expression;
            this.definition = definition;
        }

        #endregion

        #region - Methods -

        #region - Aux Methods -

        /// <summary>
        /// Gets the parameter name from the parameter number.
        /// </summary>
        /// <param name="paramNumber">The param number.</param>
        /// <returns></returns>
        private string GetParameterStr()
        {
            string result = "@pe" + this.paramNumber.ToString();
            this.paramNumber++;
            return result;
        }

        private string GetOperatorStr(ExpressionType type)
        {
            switch (type)
            {
                case ExpressionType.Equal: return " = ";
                case ExpressionType.NotEqual: return " <> ";
                case ExpressionType.GreaterThan: return " > ";
                case ExpressionType.GreaterThanOrEqual: return " >= ";
                case ExpressionType.LessThan: return " < ";
                case ExpressionType.LessThanOrEqual: return " <= ";
                case ExpressionType.AndAlso: return " AND ";
                case ExpressionType.OrElse: return " OR ";
                case ExpressionType.Add: return " + ";
                case ExpressionType.Subtract: return " - ";
                case ExpressionType.Multiply: return " * ";
                case ExpressionType.Divide: return " / ";
                case ExpressionType.Modulo: return " % ";
                default: throw new NotSupportedException("Expression type " + type + " is not supported");
            }
        }

        public ColumnExpression[] GetColumnExpressions(Expression expression)
        {
            if ((expression is UnaryExpression) && (expression.NodeType == ExpressionType.Quote))
            {
                expression = (expression as UnaryExpression).Operand;
            }
            if (!((expression is LambdaExpression) && (expression.NodeType == ExpressionType.Lambda)))
            {
                return null;
            }
            expression = (expression as LambdaExpression).Body;
            if (expression.NodeType == ExpressionType.Convert)
            {
                expression = (expression as UnaryExpression).Operand;
            }
            List<ColumnExpression> list = new List<ColumnExpression>();
            if ((expression is MemberExpression) && (expression.NodeType == ExpressionType.MemberAccess))
            {
                list.Add(new ColumnExpression(this.definition.GetAttribute((expression as MemberExpression).Member.Name).ColumnName));
            }
            else if ((expression is NewExpression) && (expression.NodeType == ExpressionType.New))
            {
                NewExpression newExpression = expression as NewExpression;
                foreach (Expression arg in newExpression.Arguments)
                {
                    if ((arg is MemberExpression) && (arg.NodeType == ExpressionType.MemberAccess))
                    {
                        list.Add(new ColumnExpression(this.definition.GetAttribute((arg as MemberExpression).Member.Name).ColumnName));
                    }
                }
            }
            else if ((expression is NewArrayExpression) && (expression.NodeType == ExpressionType.NewArrayInit))
            {
                NewArrayExpression newArrExpression = expression as NewArrayExpression;
                foreach (Expression arg in newArrExpression.Expressions)
                {
                    if ((arg is MemberExpression) && (arg.NodeType == ExpressionType.MemberAccess))
                    {
                        list.Add(new ColumnExpression(this.definition.GetAttribute((arg as MemberExpression).Member.Name).ColumnName));
                    }
                }
            }
            else
            {
                return null;
            }
            return list.ToArray();
        }

        #endregion

        #region - Override Methods -

        protected override Expression ReviewConstant(ConstantExpression expression)
        {
            if (expression != null)
            {
                string paramName = this.GetParameterStr();
                BaseCommand command = new BaseCommand(paramName);
                command.Parameters.Add(paramName, expression.Value);
                this.commands.Push(command);
            }
            return expression;
        }

        protected override Expression ReviewMember(MemberExpression expression)
        {
            if (expression != null)
            {
                PropertyInfo info = expression.Member as PropertyInfo;
                if (info != null)
                {
                    this.commands.Push(new BaseCommand("[" + this.definition.GetAttribute(info.Name).ColumnName + "]"));
                    this.columns.Push(new ColumnExpression(this.definition.GetAttribute(info.Name).ColumnName));
                }
            }
            return expression;
        }

        protected override Expression ReviewBinary(System.Linq.Expressions.BinaryExpression expression)
        {
            if (expression == null)
            {
                return null;
            }
            this.Review(expression.Left);
            this.Review(expression.Right);
            BaseCommand rightCommand = this.commands.Pop();
            BaseCommand leftCommand = this.commands.Pop();
            BaseCommand command = new BaseCommand("(" + leftCommand + this.GetOperatorStr(expression.NodeType) + rightCommand + ")");
            CommandBuilder.AddParameters(command, leftCommand);
            CommandBuilder.AddParameters(command, rightCommand);
            this.commands.Push(command);
            return expression;
        }

        protected override Expression ReviewMethodCall(MethodCallExpression expression)
        {
            if (expression == null)
            {
                return null;
            }
            MethodInfo info = expression.Method;
            if (info != null)
            {
                switch (info.Name)
                {
                    case "OrderBy":
                    case "ThenBy":
                    case "OrderByDescending":
                    case "ThenByDescending":
                        {
                            if ((expression.Arguments != null) && (expression.Arguments.Count > 1))
                            {
                                this.Review(expression.Arguments[0]);
                                this.Review(expression.Arguments[1]);
                                if (this.columns.Count > 0)
                                {
                                    SortDirection direction = ((info.Name == "OrderByDescending") || (info.Name == "ThenByDescending")) ? SortDirection.Descending : SortDirection.Ascending;
                                    this.sortings.Add(new SortExpression(this.columns.Pop(), direction));
                                    this.commands.Pop();
                                }
                            }
                            return expression;
                        }
                    case "Select":
                        {
                            if ((expression.Arguments != null) && (expression.Arguments.Count > 1))
                            {
                                this.Review(expression.Arguments[0]);
                                ColumnExpression[] columns = this.GetColumnExpressions(expression.Arguments[1]);
                                if (columns != null)
                                {
                                    this.columnExpressions.AddRange(columns);
                                }
                            }
                            return expression;
                        }
                }
            }
            return base.ReviewMethodCall(expression);
        }

        #endregion

        public static BaseCommand GetCommand(Expression expression, EntityDefinition definition)
        {
            ExpressionCommandBuilder builder = new ExpressionCommandBuilder(expression, definition);
            return builder.InnerGetCommand();
        }

        public BaseCommand InnerGetCommand()
        {
            if (this.Expression == null)
            {
                return null;
            }
            if (this.command == null)
            {
                this.Build();
            }
            return this.command;
        }

        private string GetExpressionStr(SortExpression expression)
        {
            if (expression.SortDirection == SortDirection.Descending)
            {
                return "[" + expression.ColumnName + "] DESC";
            }
            else
            {
                return "[" + expression.ColumnName + "] ASC";
            }
        }

        private void Build()
        {
            this.commands.Clear();
            this.columns.Clear();
            this.sortings.Clear();
            this.columnExpressions.Clear();
            this.paramNumber = 1;
            this.Review(ExprEvaluator.Evaluate(this.Expression));
            BaseCommand condition = this.commands.Count > 0 ? this.commands.Pop() : null;
            this.command = new BaseCommand();
            string commandText = string.Empty;
            if ((condition != null) && (!string.IsNullOrEmpty(condition.CommandText)))
            {
                commandText = "WHERE " + condition.CommandText;
                CommandBuilder.AddParameters(command, condition);
            }
            if (this.sortings.Count > 0)
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("ORDER BY ");
                bool isFirst = true;
                foreach (SortExpression sort in this.sortings)
                {
                    if (isFirst)
                    {
                        isFirst = false;
                    }
                    else
                    {
                        builder.Append(", ");
                    }
                    builder.Append(this.GetExpressionStr(sort));
                }
                commandText = string.IsNullOrEmpty(commandText) ? builder.ToString() : commandText + " " + builder.ToString();
            }
            command.CommandText = commandText;
        }

        #endregion
    }
}
