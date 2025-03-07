using System.Collections.Generic;
using System.Linq.Expressions;

namespace C4Net.Framework.Data.Expressions
{
    /// <summary>
    /// Class for a expression evaluator.
    /// </summary>
    public class ExprEvaluator : ExprReviewer
    {
        #region - Fields -

        /// <summary>
        /// The candidate expression collection.
        /// </summary>
        private HashSet<Expression> candidates;

        #endregion

        #region - Methods -

        /// <summary>
        /// Evaluates the specified expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public static Expression Evaluate(Expression expression)
        {
            ExprEvaluator instance = new ExprEvaluator();
            return instance.InnerEvaluate(expression);
        }

        /// <summary>
        /// Internal evaluation of the expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        protected virtual Expression InnerEvaluate(Expression expression)
        {
            this.candidates = ExprCandidates.GetCandidates(expression);
            return this.Review(expression);
        }

        /// <summary>
        /// Reviews the specified expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        protected override Expression Review(Expression expression)
        {
            if (expression == null)
            {
                return null;
            }
            if (candidates.Count > 0 && this.candidates.Contains(expression))
            {
                return expression.NodeType == ExpressionType.Constant ? expression : Expression.Constant((object)((Expression.Lambda(expression)).Compile()).DynamicInvoke(null), expression.Type);
            }
            return base.Review(expression);
        }

        #endregion
    }
}
