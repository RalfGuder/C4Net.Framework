using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace C4Net.Framework.Data.Expressions
{
    /// <summary>
    /// Class for reviewing expression trees visiting each of the processable nodes.
    /// </summary>
    public class ExprReviewer
    {
        #region - Methods -

        /// <summary>
        /// Reviews the specified expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        protected virtual Expression Review(Expression expression)
        {
            if (expression == null)
            {
                return null;
            }
            if (expression is UnaryExpression)
            {
                return this.ReviewUnary(expression as UnaryExpression);
            }
            if (expression is BinaryExpression)
            {
                return this.ReviewBinary(expression as BinaryExpression);
            }
            if (expression is TypeBinaryExpression)
            {
                return this.ReviewTypeBinary(expression as TypeBinaryExpression);
            }
            if (expression is ConditionalExpression)
            {
                return this.ReviewConditional(expression as ConditionalExpression);
            }
            if (expression is ConstantExpression)
            {
                return this.ReviewConstant(expression as ConstantExpression);
            }
            if (expression is ParameterExpression)
            {
                return this.ReviewParameter(expression as ParameterExpression);
            }
            if (expression is MemberExpression)
            {
                return this.ReviewMember(expression as MemberExpression);
            }
            if (expression is MethodCallExpression)
            {
                return this.ReviewMethodCall(expression as MethodCallExpression);
            }
            if (expression is LambdaExpression)
            {
                return this.ReviewLambda(expression as LambdaExpression);
            }
            if (expression is NewExpression)
            {
                return this.ReviewNew(expression as NewExpression);
            }
            if (expression is NewArrayExpression)
            {
                return this.ReviewNewArray(expression as NewArrayExpression);
            }
            if (expression is InvocationExpression)
            {
                return this.ReviewInvocation(expression as InvocationExpression);
            }
            if (expression is MemberInitExpression)
            {
                return this.ReviewMemberInit(expression as MemberInitExpression);
            }
            if (expression is ListInitExpression)
            {
                return this.ReviewListInit(expression as ListInitExpression);
            }
            throw new Exception(string.Format("Unhandled expression node type: '{0}'", expression.NodeType));
        }

        /// <summary>
        /// Reviews an unary expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        protected virtual Expression ReviewUnary(UnaryExpression expression)
        {
            Expression operand = this.Review(expression.Operand);
            return operand == expression.Operand ? expression : Expression.MakeUnary(expression.NodeType, operand, expression.Type, expression.Method);
        }

        /// <summary>
        /// Reviews a binary expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        protected virtual Expression ReviewBinary(BinaryExpression expression)
        {
            Expression left = this.Review(expression.Left);
            Expression right = this.Review(expression.Right);
            Expression conversion = this.Review(expression.Conversion);
            if (left != expression.Left || right != expression.Right || conversion != expression.Conversion)
            {
                if (expression.NodeType == ExpressionType.Coalesce && expression.Conversion != null)
                {
                    return Expression.Coalesce(left, right, conversion as LambdaExpression);
                }
                else
                {
                    return Expression.MakeBinary(expression.NodeType, left, right, expression.IsLiftedToNull, expression.Method);
                }
            }
            return expression;
        }

        /// <summary>
        /// Reviews a type binary expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        protected virtual Expression ReviewTypeBinary(TypeBinaryExpression expression)
        {
            Expression expr = this.Review(expression.Expression);
            return expr == expression.Expression ? expression : Expression.TypeIs(expr, expression.TypeOperand);
        }

        /// <summary>
        /// Reviews a conditional expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        protected virtual Expression ReviewConditional(ConditionalExpression expression)
        {
            Expression test = this.Review(expression.Test);
            Expression ifTrue = this.Review(expression.IfTrue);
            Expression ifFalse = this.Review(expression.IfFalse);
            return (test != expression.Test || ifTrue != expression.IfTrue || ifFalse != expression.IfFalse)
                ? Expression.Condition(test, ifTrue, ifFalse) : expression;
        }

        /// <summary>
        /// Reviews a constant expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        protected virtual Expression ReviewConstant(ConstantExpression expression)
        {
            return expression;
        }

        /// <summary>
        /// Reviews a parameter expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        protected virtual Expression ReviewParameter(ParameterExpression expression)
        {
            return expression;
        }

        /// <summary>
        /// Reviews a member expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        protected virtual Expression ReviewMember(MemberExpression expression)
        {
            Expression expr = this.Review(expression.Expression);
            return expr == expression.Expression ? expression : Expression.MakeMemberAccess(expr, expression.Member);
        }

        /// <summary>
        /// Reviews a method call expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        protected virtual Expression ReviewMethodCall(MethodCallExpression expression)
        {
            Expression obj = this.Review(expression.Object);
            ReadOnlyCollection<Expression> args = this.ReviewExpressionList(expression.Arguments);
            return (obj != expression.Object || args != expression.Arguments)
                ? Expression.Call(obj, expression.Method, args) : expression;
        }

        /// <summary>
        /// Reviews an expression collection.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        protected virtual ReadOnlyCollection<Expression> ReviewExpressionList(ReadOnlyCollection<Expression> source)
        {
            List<Expression> target = new List<Expression>();
            bool changed = false;
            foreach (Expression item in source)
            {
                Expression copy = this.Review(item);
                target.Add(copy);
                if (copy != item && !changed)
                {
                    changed = true;
                }
            }
            return changed ? target.AsReadOnly() : source;
        }

        /// <summary>
        /// Reviews a lambda expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        protected virtual Expression ReviewLambda(LambdaExpression expression)
        {
            Expression body = this.Review(expression.Body);
            return body == expression.Body ? expression : Expression.Lambda(expression.Type, body, expression.Parameters);
        }

        /// <summary>
        /// Reviews a new instance expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        protected virtual NewExpression ReviewNew(NewExpression expression)
        {
            ReadOnlyCollection<Expression> args = this.ReviewExpressionList(expression.Arguments);
            if (args != expression.Arguments)
            {
                if (expression.Members != null)
                {
                    return Expression.New(expression.Constructor, args, expression.Members);
                }
                else
                {
                    return Expression.New(expression.Constructor, args);
                }
            }
            return expression;
        }

        /// <summary>
        /// Reviews a new array expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        protected virtual Expression ReviewNewArray(NewArrayExpression expression)
        {
            ReadOnlyCollection<Expression> exprList = this.ReviewExpressionList(expression.Expressions);
            if (exprList != expression.Expressions)
            {
                if (expression.NodeType == ExpressionType.NewArrayInit)
                {
                    return Expression.NewArrayInit(expression.Type.GetElementType(), exprList);
                }
                else
                {
                    return Expression.NewArrayBounds(expression.Type.GetElementType(), exprList);
                }
            }
            return expression;
        }

        /// <summary>
        /// Reviews an invocation expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        protected virtual Expression ReviewInvocation(InvocationExpression expression)
        {
            ReadOnlyCollection<Expression> args = this.ReviewExpressionList(expression.Arguments);
            Expression expr = this.Review(expression.Expression);
            return (args != expression.Arguments || expr != expression.Expression) ? Expression.Invoke(expr, args) : expression;
        }

        /// <summary>
        /// Reviews a member binding.
        /// </summary>
        /// <param name="binding">The binding.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        protected virtual MemberBinding ReviewBinding(MemberBinding binding)
        {
            switch (binding.BindingType)
            {
                case MemberBindingType.Assignment:
                    return this.ReviewMemberAssignment(binding as MemberAssignment);
                case MemberBindingType.MemberBinding:
                    return this.ReviewMemberBinding(binding as MemberMemberBinding);
                case MemberBindingType.ListBinding:
                    return this.ReviewMemberListBinding(binding as MemberListBinding);
                default:
                    throw new Exception(string.Format("Unhandled binding type '{0}'", binding.BindingType));

            }
        }

        /// <summary>
        /// Reviews a member binding collection.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        protected virtual ReadOnlyCollection<MemberBinding> ReviewBindings(ReadOnlyCollection<MemberBinding> source)
        {
            List<MemberBinding> list = new List<MemberBinding>();
            bool changed = false;
            foreach (MemberBinding item in source)
            {
                MemberBinding copy = this.ReviewBinding(item);
                if (copy != item && !changed)
                {
                    changed = true;
                }
                list.Add(copy);
            }
            return changed ? list.AsReadOnly() : source;
        }

        /// <summary>
        /// Reviews a member init expression
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        protected virtual Expression ReviewMemberInit(MemberInitExpression expression)
        {
            NewExpression newExp = this.ReviewNew(expression.NewExpression);
            ReadOnlyCollection<MemberBinding> bindings = this.ReviewBindings(expression.Bindings);
            return (newExp != expression.NewExpression || bindings != expression.Bindings)
                ? Expression.MemberInit(newExp, bindings) : expression;
        }

        /// <summary>
        /// Reviews a member assignment.
        /// </summary>
        /// <param name="assignment">The assignment.</param>
        /// <returns></returns>
        protected virtual MemberAssignment ReviewMemberAssignment(MemberAssignment assignment)
        {
            Expression expression = this.Review(assignment.Expression);
            return expression == assignment.Expression ? assignment : Expression.Bind(assignment.Member, expression);
        }

        /// <summary>
        /// Reviews a member binding.
        /// </summary>
        /// <param name="binding">The binding.</param>
        /// <returns></returns>
        protected virtual MemberMemberBinding ReviewMemberBinding(MemberMemberBinding binding)
        {
            ReadOnlyCollection<MemberBinding> bindings = this.ReviewBindings(binding.Bindings);
            return bindings == binding.Bindings ? binding : Expression.MemberBind(binding.Member, bindings);
        }

        /// <summary>
        /// Reviews a member list binding.
        /// </summary>
        /// <param name="binding">The binding.</param>
        /// <returns></returns>
        protected virtual MemberListBinding ReviewMemberListBinding(MemberListBinding binding)
        {
            ReadOnlyCollection<ElementInit> initializers = this.ReviewElementInitializers(binding.Initializers);
            return initializers == binding.Initializers ? binding : Expression.ListBind(binding.Member, initializers);
        }

        /// <summary>
        /// Reviews the element initializer.
        /// </summary>
        /// <param name="initializer">The initializer.</param>
        /// <returns></returns>
        protected virtual ElementInit ReviewElementInit(ElementInit initializer)
        {
            ReadOnlyCollection<Expression> args = this.ReviewExpressionList(initializer.Arguments);
            return args == initializer.Arguments ? initializer : Expression.ElementInit(initializer.AddMethod, args);
        }

        /// <summary>
        /// Reviews an element initializer collection.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        protected virtual ReadOnlyCollection<ElementInit> ReviewElementInitializers(ReadOnlyCollection<ElementInit> source)
        {
            List<ElementInit> list = new List<ElementInit>();
            bool changed = false;
            foreach (ElementInit item in source)
            {
                ElementInit copy = this.ReviewElementInit(item);
                if (copy != item && !changed)
                {
                    changed = true;
                }
                list.Add(copy);
            }
            return changed ? list.AsReadOnly() : source;
        }

        /// <summary>
        /// Reviews a list initialization expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        protected virtual Expression ReviewListInit(ListInitExpression expression)
        {
            NewExpression newExp = this.ReviewNew(expression.NewExpression);
            ReadOnlyCollection<ElementInit> initializers = this.ReviewElementInitializers(expression.Initializers);
            return (newExp != expression.NewExpression || initializers != expression.Initializers)
                ? Expression.ListInit(newExp, initializers) : expression;
        }

        #endregion
    }
}
