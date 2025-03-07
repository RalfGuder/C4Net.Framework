using System;
using System.Collections.Generic;
using C4Net.Framework.Expressions.Operators;

namespace C4Net.Framework.Expressions
{
    /// <summary>
    /// Expression for a relation.
    /// </summary>
    [Serializable]
    public class RelationExpression : ExpressionElement
    {
        #region - Fields -

        /// <summary>
        /// Children.
        /// </summary>
        private List<RelationExpression> sons = new List<RelationExpression>();

        #endregion

        #region - Properties -

        /// <summary>
        /// Gets or sets the relation operator.
        /// </summary>
        /// <value>The relation operator.</value>
        public RelationOperator RelationOperator { get; set; }

        /// <summary>
        /// Gets or sets the expression element.
        /// </summary>
        /// <value>The expression element.</value>
        public ExpressionElement ExpressionElement { get; set; }

        /// <summary>
        /// Gets the sons.
        /// </summary>
        /// <value>The sons.</value>
        public List<RelationExpression> Sons
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
            get { return this.sons.Count > 0; }
        }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="RelationExpression"/> class.
        /// </summary>
        public RelationExpression()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelationExpression"/> class.
        /// </summary>
        /// <param name="expressionElement">The expression element.</param>
        public RelationExpression(ExpressionElement expressionElement)
            : this()
        {
            this.ExpressionElement = expressionElement;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelationExpression"/> class.
        /// </summary>
        /// <param name="relationOperator">The relation operator.</param>
        /// <param name="expressionElement">The expression element.</param>
        public RelationExpression(RelationOperator relationOperator, ExpressionElement expressionElement)
            : this()
        {
            this.RelationOperator = relationOperator;
            this.ExpressionElement = expressionElement;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelationExpression"/> class.
        /// </summary>
        /// <param name="operation1">The operation1.</param>
        /// <param name="relation">The relation.</param>
        /// <param name="operation2">The operation2.</param>
        public RelationExpression(OperationExpression operation1, RelationOperator relation, OperationExpression operation2)
            : this()
        {
            RelationExpression relation1 = new RelationExpression(operation1);
            this.Sons.Add(relation1);
            RelationExpression relation2 = new RelationExpression(operation2);
            relation2.RelationOperator = relation;
            this.Sons.Add(relation2);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelationExpression"/> class.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <param name="relation">The relation.</param>
        /// <param name="relationExpression">The relation expression.</param>
        public RelationExpression(OperationExpression operation, RelationOperator relation, RelationExpression relationExpression)
            : this()
        {
            RelationExpression relationExpression1 = new RelationExpression(operation);
            this.Sons.Add(relationExpression1);
            RelationExpression relationExpression2;
            if (relationExpression.RelationOperator == RelationOperator.None)
            {
                relationExpression2 = relationExpression;
            }
            else
            {
                relationExpression2 = new RelationExpression(relationExpression);
            }
            relationExpression2.RelationOperator = relation;
            this.Sons.Add(relationExpression2);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelationExpression"/> class.
        /// </summary>
        /// <param name="relationExpression1">The relation expression1.</param>
        /// <param name="relationOperator">The relation operator.</param>
        /// <param name="relationExpression2">The relation expression2.</param>
        public RelationExpression(RelationExpression relationExpression1, RelationOperator relationOperator, RelationExpression relationExpression2)
            : this()
        {
            this.Sons.Add(relationExpression1);

            if (relationExpression2.RelationOperator != RelationOperator.None)
            {
                relationExpression2 = new RelationExpression(relationExpression2);
            }
            relationExpression2.RelationOperator = relationOperator;
            this.Sons.Add(relationExpression2);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelationExpression"/> class.
        /// </summary>
        /// <param name="relationExpression">The relation expression.</param>
        /// <param name="relationOperator">The relation operator.</param>
        /// <param name="operationExpression">The operation expression.</param>
        public RelationExpression(RelationExpression relationExpression, RelationOperator relationOperator, OperationExpression operationExpression)
            : this()
        {
            RelationExpression relationExpression1 = relationExpression;
            this.Sons.Add(relationExpression1);
            RelationExpression relationExpression2 = new RelationExpression(operationExpression);
            relationExpression2.RelationOperator = relationOperator;
            this.Sons.Add(relationExpression2);
        }

        #endregion

        #region - Methods -

        /// <summary>
        /// Adds the expression.
        /// </summary>
        /// <param name="relationExpression">The relation expression.</param>
        public void AddExpression(RelationExpression relationExpression)
        {
            this.Sons.Add(relationExpression);
        }

        /// <summary>
        /// Adds the expression.
        /// </summary>
        /// <param name="operationExpression">The operation expression.</param>
        public void AddExpression(OperationExpression operationExpression)
        {
            RelationExpression relationExpression = new RelationExpression(operationExpression);
            this.AddExpression(relationExpression);
        }

        /// <summary>
        /// Adds the expression.
        /// </summary>
        /// <param name="relationOperator">The relation operator.</param>
        /// <param name="operationExpression">The operation expression.</param>
        public void AddExpression(RelationOperator relationOperator, OperationExpression operationExpression)
        {
            RelationExpression relationExpression = new RelationExpression(operationExpression);
            relationExpression.RelationOperator = relationOperator;
            this.AddExpression(relationExpression);
        }

        /// <summary>
        /// Adds the and expression.
        /// </summary>
        /// <param name="relationExpressions">The relation expressions.</param>
        public void AddAndExpression(params RelationExpression[] relationExpressions)
        {
            foreach (RelationExpression relationExpression in relationExpressions)
            {
                relationExpression.RelationOperator = RelationOperator.And;
                this.Sons.Add(relationExpression);
            }
        }

        /// <summary>
        /// Adds the and expression.
        /// </summary>
        /// <param name="operationExpressions">The operation expressions.</param>
        public void AddAndExpression(params OperationExpression[] operationExpressions)
        {
            foreach (OperationExpression operationExpression in operationExpressions)
            {
                RelationExpression relationExpression = new RelationExpression(operationExpression);
                relationExpression.RelationOperator = RelationOperator.And;
                this.Sons.Add(relationExpression);
            }
        }

        /// <summary>
        /// Adds the or expression.
        /// </summary>
        /// <param name="relationExpressions">The relation expressions.</param>
        public void AddOrExpression(params RelationExpression[] relationExpressions)
        {
            foreach (RelationExpression relationExpression in relationExpressions)
            {
                relationExpression.RelationOperator = RelationOperator.Or;
                this.Sons.Add(relationExpression);
            }
        }

        /// <summary>
        /// Adds the or expression.
        /// </summary>
        /// <param name="operationExpressions">The operation expressions.</param>
        public void AddOrExpression(params OperationExpression[] operationExpressions)
        {
            foreach (OperationExpression operationExpression in operationExpressions)
            {
                RelationExpression relationExpression = new RelationExpression(operationExpression);
                relationExpression.RelationOperator = RelationOperator.Or;
                this.Sons.Add(relationExpression);
            }
        }

        /// <summary>
        /// Ands the specified relation expression.
        /// </summary>
        /// <param name="relationExpression">The relation expression.</param>
        public void And(RelationExpression relationExpression)
        {
            if (relationExpression.RelationOperator != RelationOperator.None)
            {
                relationExpression = new RelationExpression(relationExpression);
            }
            relationExpression.RelationOperator = RelationOperator.And;
            this.Sons.Add(relationExpression);
        }

        /// <summary>
        /// Ands the specified operation expression.
        /// </summary>
        /// <param name="operationExpression">The operation expression.</param>
        public void And(OperationExpression operationExpression)
        {
            RelationExpression relationExpression = new RelationExpression(operationExpression);
            this.And(relationExpression);
        }

        /// <summary>
        /// Ors the specified relation expression.
        /// </summary>
        /// <param name="relationExpression">The relation expression.</param>
        public void Or(RelationExpression relationExpression)
        {
            if (relationExpression.RelationOperator != RelationOperator.None)
            {
                relationExpression = new RelationExpression(relationExpression);
            }
            relationExpression.RelationOperator = RelationOperator.Or;
            this.Sons.Add(relationExpression);
        }

        /// <summary>
        /// Ors the specified operation expression.
        /// </summary>
        /// <param name="operationExpression">The operation expression.</param>
        public void Or(OperationExpression operationExpression)
        {
            RelationExpression relationExpression = new RelationExpression(operationExpression);
            this.Or(relationExpression);
        }

        #endregion

        #region - Operators -

        /// <summary>
        /// Implements the operator &amp;.
        /// </summary>
        /// <param name="relationExpression1">The relation expression1.</param>
        /// <param name="relationExpression2">The relation expression2.</param>
        /// <returns>The result of the operator.</returns>
        public static RelationExpression operator &(RelationExpression relationExpression1, RelationExpression relationExpression2)
        {
            return new RelationExpression(relationExpression1, RelationOperator.And, relationExpression1);
        }

        /// <summary>
        /// Implements the operator &amp;.
        /// </summary>
        /// <param name="relationExpression">The relation expression.</param>
        /// <param name="operationExpression">The operation expression.</param>
        /// <returns>The result of the operator.</returns>
        public static RelationExpression operator &(RelationExpression relationExpression, OperationExpression operationExpression)
        {
            return new RelationExpression(relationExpression, RelationOperator.And, operationExpression);
        }

        /// <summary>
        /// Implements the operator |.
        /// </summary>
        /// <param name="relationExpression1">The relation expression1.</param>
        /// <param name="relationExpression2">The relation expression2.</param>
        /// <returns>The result of the operator.</returns>
        public static RelationExpression operator |(RelationExpression relationExpression1, RelationExpression relationExpression2)
        {
            return new RelationExpression(relationExpression1, RelationOperator.Or, relationExpression2);
        }

        /// <summary>
        /// Implements the operator |.
        /// </summary>
        /// <param name="relationExpression">The relation expression.</param>
        /// <param name="operationExpression">The operation expression.</param>
        /// <returns>The result of the operator.</returns>
        public static RelationExpression operator |(RelationExpression relationExpression, OperationExpression operationExpression)
        {
            return new RelationExpression(relationExpression, RelationOperator.Or, operationExpression);
        }

        /// <summary>
        /// Implements the operator !.
        /// </summary>
        /// <param name="relationExpression">The relation expression.</param>
        /// <returns>The result of the operator.</returns>
        public static RelationExpression operator !(RelationExpression relationExpression)
        {
            return new RelationExpression(RelationOperator.Not, relationExpression);
        }

        /// <summary>
        /// Implements the operator ^.
        /// </summary>
        /// <param name="relationExpression">The relation expression.</param>
        /// <param name="sortExpression">The sorting expression.</param>
        /// <returns>The result of the operator.</returns>
        public static ConditionExpression operator ^(RelationExpression relationExpression, SortExpression sortExpression)
        {
            return (new ConditionExpression(relationExpression)).OrderBy(sortExpression);
        }

        /// <summary>
        /// Implements the operator ^.
        /// </summary>
        /// <param name="relationExpression">The relation expression.</param>
        /// <param name="columnExpression">The column expression.</param>
        /// <returns>The result of the operator.</returns>
        public static ConditionExpression operator ^(RelationExpression relationExpression, ColumnExpression columnExpression)
        {
            return (new ConditionExpression(relationExpression)).OrderBy(columnExpression);
        }

        /// <summary>
        /// Implements the operator ^.
        /// </summary>
        /// <param name="relationExpression">The relation expression.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns>The result of the operator.</returns>
        public static ConditionExpression operator ^(RelationExpression relationExpression, string columnName)
        {
            return (new ConditionExpression(relationExpression)).OrderBy(columnName);
        }

        #endregion
    }
}
