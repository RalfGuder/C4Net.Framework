using System;
using System.Collections.Generic;
using C4Net.Framework.Data.DAO;
using C4Net.Framework.Data.Expressions;

namespace C4Net.Framework.Data
{
    /// <summary>
    /// Class for the Unit of Work pattern.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        #region - Fields -

        /// <summary>
        /// The repositories dictionary.
        /// </summary>
        private Dictionary<Type, object> repositories = new Dictionary<Type, object>();

        #endregion

        #region - Properties -

        /// <summary>
        /// Gets or sets the data context.
        /// </summary>
        /// <value>
        /// The context.
        /// </value>
        public IDataContext Context { get; set; }

        /// <summary>
        /// Gets the transaction id.
        /// </summary>
        /// <value>
        /// The transaction id.
        /// </value>
        public string TransactionId { get; private set; }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public UnitOfWork(IDataContext context)
        {
            this.Context = context;
            this.StartTransaction();
        }

        #endregion

        #region - Methods -

        /// <summary>
        /// Starts a new transaction.
        /// </summary>
        public void StartTransaction()
        {
            this.TransactionId = this.Context.BeginTransaction();
        }

        /// <summary>
        /// Gets the repository for the given type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IRepository<T> GetRepository<T>() where T : IDAO
        {
            if (this.repositories.ContainsKey(typeof(T)))
            {
                return (IRepository<T>)this.repositories[typeof(T)];
            }
            IRepository<T> result = new Repository<T>(this.Context, this.TransactionId);
            this.repositories.Add(typeof(T), result);
            return result;
        }

        public IQueryableObject<T> GetQuery<T>() where T : IDAO
        {
            return new QueryableObject<T>(this.Context);
        }

        /// <summary>
        /// Applies the changes.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public void ApplyChanges()
        {
            this.Context.CommitTransaction(this.TransactionId);
            this.repositories.Clear();
            this.StartTransaction();
        }

        /// <summary>
        /// Cancels the changes.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public void CancelChanges()
        {
            this.Context.RollbackTransaction(this.TransactionId);
            this.repositories.Clear();
            this.StartTransaction();
        }

        #endregion
    }
}
