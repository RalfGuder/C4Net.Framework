using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using C4Net.Framework.Data.Adapters;
using C4Net.Framework.Data.Base;
using C4Net.Framework.Data.Builders;

namespace C4Net.Framework.Data.Expressions
{
    public class QueryableObject<T> : IQueryableObject<T>, IQueryProvider, IListSource
    {
        #region - Fields -

        private List<T> list = null;

        private IDataContext adapter = null;

        private Expression expression = null;

        #endregion

        #region - Constructors -

        public QueryableObject(IDataContext adapter, Expression expression)
        {
            this.adapter = adapter;
            this.expression = expression;
        }

        public QueryableObject(IDataContext adapter)
            : this(adapter, null)
        {
        }

        #endregion

        #region - Aux methods -

        private Type GetDataType(Expression expression)
        {
            if (expression is MethodCallExpression)
            {
                string methodName = (expression as MethodCallExpression).Method.Name;
                if ((methodName == "Select") || (methodName == "Count") || (methodName == "Average")
                    || (methodName == "Max") || (methodName == "Min") || (methodName == "Sum"))
                {
                    return this.GetDataType((expression as MethodCallExpression).Arguments[0]);
                }
            }
            return expression.Type.IsGenericType ? expression.Type.GetGenericArguments()[0] : expression.Type;
        }

        #endregion

        #region - IListSource -

        public bool ContainsListCollection
        {
            get { return false; }
        }

        public IList GetList()
        {
            if (list == null)
            {
                list = new List<T>();
                foreach (T item in this)
                {
                    list.Add(item);
                }
            }
            return list;
        }

        #endregion

        #region - IQueryProvider -

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new QueryableObject<TElement>(this.Adapter, expression);
        }

        public IQueryable CreateQuery(Expression expression)
        {
            return new QueryableObject<T>(this.Adapter, expression);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            return (TResult)Convert.ChangeType(this.Execute(expression), typeof(TResult));
        }

        public object Execute(Expression expression)
        {
            DataObjectAdapter objectAdapter = this.adapter.GetDataObjectAdapter(this.GetDataType(expression));
            return objectAdapter.ExecuteScalarCommand(ExpressionCommandBuilder.GetCommand(expression, objectAdapter.Definition));
        }

        #endregion

        #region - IQueryableObject -

        public IDataContext Adapter
        {
            get { return this.adapter; }
        }

        public Type ElementType
        {
            get { return typeof(T); }
        }

        public Expression Expression
        {
            get
            {
                return this.expression == null ? Expression.Constant(this) : this.expression;
            }
        }

        public IQueryProvider Provider
        {
            get { return this; }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this.ExecuteQuery();
        }

        #endregion

        #region - Methods -

        private IEnumerator<T> ExecuteQuery()
        {
            Type type = this.GetDataType(this.expression);
            DataObjectAdapter objectAdapter = this.adapter.GetDataObjectAdapter(type);
            BaseCommand command = ExpressionCommandBuilder.GetCommand(this.expression, objectAdapter.Definition);
            IEnumerator<T> enumerator = this.adapter.GetDataObjectAdapter(type).GetEnumerator<T>(command).GetEnumerator();
            return (IEnumerator<T>)enumerator;
        }

        #endregion
    }
}
