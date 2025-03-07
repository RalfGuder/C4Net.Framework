using System.Collections.Generic;
using System.Linq.Expressions;

namespace C4Net.Framework.Data.Expressions
{
    /// <summary>
    /// Search the candidate expressions from an expression tree.
    /// </summary>
    public class ExprCandidates : ExprReviewer
    {
        #region - Fields -

        /// <summary>
        /// The candidate list.
        /// </summary>
        private HashSet<Expression> candidates = new HashSet<Expression>();

        private bool isNominated = true;

        #endregion

        #region - Methods -

        /// <summary>
        /// Gets the candidate list from an expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public static HashSet<Expression> GetCandidates(Expression expression)
        {
            ExprCandidates instance = new ExprCandidates();
            instance.Review(expression);
            return instance.candidates;
        }

        /// <summary>
        /// Determines whether the specified expression is evaluable.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>
        ///   <c>true</c> if the specified expression is evaluable; otherwise, <c>false</c>.
        /// </returns>
        protected virtual bool IsEvaluable(Expression expression)
        {
            return expression.NodeType != ExpressionType.Parameter;
        }


        /// <summary>
        /// Reviews the specified expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        protected override Expression Review(Expression expression)
        {
            bool oldNominated = this.isNominated;
            try
            {
                this.isNominated = true;
                if (expression == null)
                {
                    return null;
                }
                base.Review(expression);
                if (this.isNominated)
                {
                    if (this.IsEvaluable(expression))
                    {
                        this.candidates.Add(expression);
                        this.isNominated = true;
                    }
                    else
                    {
                        this.isNominated = false;
                    }
                }
                return expression;
            }
            finally
            {
                this.isNominated &= oldNominated;
            }
        }

        #endregion
    }
}
