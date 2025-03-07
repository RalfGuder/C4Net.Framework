using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using C4Net.Framework.Data.Base;
using C4Net.Framework.Data.DAO;
using C4Net.Framework.Data.Expressions;
using C4Net.Framework.Expressions;

namespace C4Net.Framework.Data
{
    /// <summary>
    /// Class for a Repository of a given entity class.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Repository<T> : IRepository<T> where T : IDAO
    {
        #region - Properties -

        /// <summary>
        /// Gets or sets the context.
        /// </summary>
        /// <value>
        /// The context.
        /// </value>
        public IDataContext Context { get; set; }

        /// <summary>
        /// Gets or sets the transaction id.
        /// </summary>
        /// <value>
        /// The transaction id.
        /// </value>
        public string TransactionId { get; set; }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository{T}"/> class.
        /// </summary>
        /// <param name="dataContext">The data context.</param>
        /// <param name="transactionId">The transaction id.</param>
        public Repository(IDataContext dataContext, string transactionId)
        {
            this.Context = dataContext;
            this.TransactionId = transactionId;
        }

        #endregion

        #region - Methods -

        /// <summary>
        /// Selects the query.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public IQueryable<T> SelectQuery(Expression expression)
        {
            QueryableObject<T> qryEntity = new QueryableObject<T>(this.Context);
            return qryEntity.CreateQuery<T>(expression);
        }

        /// <summary>
        /// Selects the list.
        /// </summary>
        /// <returns></returns>
        public IList<T> SelectList()
        {
            return this.Context.SelectList<T>(this.TransactionId);
        }

        /// <summary>
        /// Selects the list.
        /// </summary>
        /// <param name="conditionExpression">The condition expression.</param>
        /// <returns></returns>
        public IList<T> SelectList(ConditionExpression conditionExpression)
        {
            if (conditionExpression == null)
            {
                return this.SelectList();
            }
            return this.Context.SelectList<T>(conditionExpression, this.TransactionId);
        }

        /// <summary>
        /// Selects the list.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <returns></returns>
        public IList<T> SelectList(BaseCommand condition)
        {
            if (condition == null)
            {
                return this.SelectList();
            }
            return this.Context.SelectList<T>(condition, this.TransactionId);
        }

        /// <summary>
        /// Selects the list.
        /// </summary>
        /// <param name="startRecord">The start record.</param>
        /// <param name="maxRecords">The max records.</param>
        /// <returns></returns>
        public IList<T> SelectList(int startRecord, int maxRecords)
        {
            return this.Context.SelectList<T>(startRecord, maxRecords, this.TransactionId);
        }

        /// <summary>
        /// Selects the list.
        /// </summary>
        /// <param name="conditionExpression">The condition expression.</param>
        /// <param name="startRecord">The start record.</param>
        /// <param name="maxRecords">The max records.</param>
        /// <returns></returns>
        public IList<T> SelectList(ConditionExpression conditionExpression, int startRecord, int maxRecords)
        {
            if (conditionExpression == null)
            {
                return this.SelectList(startRecord, maxRecords);
            }
            return this.Context.SelectList<T>(conditionExpression, startRecord, maxRecords, this.TransactionId);
        }

        /// <summary>
        /// Selects the list.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <param name="startRecord">The start record.</param>
        /// <param name="maxRecords">The max records.</param>
        /// <returns></returns>
        public IList<T> SelectList(BaseCommand condition, int startRecord, int maxRecords)
        {
            if (condition == null)
            {
                return this.SelectList(startRecord, maxRecords);
            }
            return this.Context.SelectList<T>(condition, startRecord, maxRecords, this.TransactionId);
        }

        /// <summary>
        /// Selects the first.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public T SelectFirst()
        {
            IList<T> list = (IList<T>)this.Context.SelectList<T>(0, 1, this.TransactionId);
            if ((list != null) && (list.Count > 0))
            {
                return list[0];
            }
            else
            {
                return default(T);
            }
        }

        /// <summary>
        /// Selects the first.
        /// </summary>
        /// <param name="conditionExpression">The condition expression.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public T SelectFirst(ConditionExpression conditionExpression)
        {
            return this.Context.Select<T>(conditionExpression, this.TransactionId);
        }

        public T SelectFirst(BaseCommand condition)
        {
            return this.Context.Select<T>(condition, this.TransactionId);
        }

        public bool Exists(T entity)
        {
            return this.Context.Exists<T>(entity, this.TransactionId);
        }

        public void Insert(T entity)
        {
            this.Context.Insert<T>(entity, this.TransactionId);
        }

        public void Update(T entity)
        {
            this.Context.Update<T>(entity, this.TransactionId);
        }

        public void Save(T entity)
        {
            this.Context.Save<T>(entity, this.TransactionId);
        }

        public void Delete(T entity)
        {
            this.Context.Save<T>(entity, this.TransactionId);
        }

        #endregion
    }
}
