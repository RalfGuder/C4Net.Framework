using System;
using System.Collections.Generic;
using System.Data;

namespace C4Net.Framework.Data.Transactions
{
    /// <summary>
    /// Manager for transactions of a connection.
    /// </summary>
    public class TransactionManager : ITransactionManager
    {
        #region - Fields -

        /// <summary>
        /// Connection of the transaction manager
        /// </summary>
        protected IDbConnection connection;

        /// <summary>
        /// Dictionary of database transactions
        /// </summary>
        protected Dictionary<string, IDbTransaction> transactions = new Dictionary<string, IDbTransaction>();

        /// <summary>
        /// Indicates if the connection must be closed when transaction count is 0.
        /// </summary>
        protected bool mustCloseConnection = false;

        /// <summary>
        /// Stack for transactions
        /// </summary>
        protected Stack<string> transactionStack = new Stack<string>();

        /// <summary>
        /// default transaction ID
        /// </summary>
        protected string defaultTransactionID = null;

        #endregion

        #region - Properties -

        /// <summary>
        /// Gets or sets the default isolation level.
        /// </summary>
        /// <value>
        /// The default isolation level.
        /// </value>
        public IsolationLevel DefaultIsolationLevel { get; set; }

        public IDbConnection Connection
        {
            get { return this.connection; }
        }

        #endregion

        #region - Constructor -

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionManager"/> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="defaultIsolationLevel">The default isolation level.</param>
        public TransactionManager(IDbConnection connection, IsolationLevel defaultIsolationLevel = IsolationLevel.ReadUncommitted)
        {
            this.connection = connection;
            this.DefaultIsolationLevel = defaultIsolationLevel;
        }

        #endregion

        #region - Methods -

        /// <summary>
        /// Begins the transaction.
        /// </summary>
        /// <returns></returns>
        public string BeginTransaction()
        {
            return this.BeginTransaction(this.DefaultIsolationLevel);
        }

        /// <summary>
        /// Begins the transaction.
        /// </summary>
        /// <param name="isolationLevel">The isolation level.</param>
        /// <returns></returns>
        public string BeginTransaction(IsolationLevel isolationLevel)
        {
            string transactionID = null;
            if ((this.connection.State & ConnectionState.Open) != ConnectionState.Open)
            {
                this.connection.Open();
                if (this.transactions.Count == 0)
                {
                    this.mustCloseConnection = true;
                }
            }
            IDbTransaction transaction = this.connection.BeginTransaction(isolationLevel);
            transactionID = Guid.NewGuid().ToString();
            this.transactions.Add(transactionID, transaction);
            if (string.IsNullOrEmpty(this.defaultTransactionID))
            {
                this.transactionStack.Push(this.defaultTransactionID);
            }
            this.defaultTransactionID = transactionID;
            return transactionID;
        }

        /// <summary>
        /// Commits the transaction.
        /// </summary>
        /// <param name="transactionID">The transaction ID.</param>
        /// <exception cref="System.InvalidOperationException">There are no transactions pending.</exception>
        /// <exception cref="System.ArgumentException">The given transactionID is not correct.</exception>
        public void CommitTransaction(string transactionID = null)
        {
            if (string.IsNullOrEmpty(transactionID))
            {
                transactionID = this.defaultTransactionID;
            }
            if (string.IsNullOrEmpty(transactionID))
            {
                throw new InvalidOperationException("There are no transactions pending.");
            }
            IDbTransaction transaction = this.transactions[transactionID];
            if (transaction == null)
            {
                throw new ArgumentException("The given transactionID is not correct.");
            }
            transaction.Commit();
            this.transactions.Remove(transactionID);
            if (transactionID == defaultTransactionID)
            {
                if (this.transactionStack.Count > 0)
                {
                    this.defaultTransactionID = this.transactionStack.Pop();
                }
                else
                {
                    this.defaultTransactionID = null;
                }
            }
        }

        /// <summary>
        /// Rollbacks the transaction.
        /// </summary>
        /// <param name="transactionID">The transaction ID.</param>
        /// <exception cref="System.InvalidOperationException">There are no transactions pending.</exception>
        /// <exception cref="System.ArgumentException">The given transactionID is not correct.</exception>
        public void RollbackTransaction(string transactionID = null)
        {
            if (string.IsNullOrEmpty(transactionID))
            {
                transactionID = this.defaultTransactionID;
            }
            if (string.IsNullOrEmpty(transactionID))
            {
                throw new InvalidOperationException("There are no transactions pending.");
            }
            IDbTransaction transaction = this.transactions[transactionID];
            if (transaction == null)
            {
                throw new ArgumentException("The given transactionID is not correct.");
            }
            transaction.Rollback();
            this.transactions.Remove(transactionID);
            if (transactionID == defaultTransactionID)
            {
                if (this.transactionStack.Count > 0)
                {
                    this.defaultTransactionID = this.transactionStack.Pop();
                }
                else
                {
                    this.defaultTransactionID = null;
                }
            }
        }

        /// <summary>
        /// Gets a transaction by its ID.
        /// </summary>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public IDbTransaction GetTransaction(string transactionID = null)
        {
            if (string.IsNullOrEmpty(transactionID))
            {
                transactionID = this.defaultTransactionID;
            }
            if (string.IsNullOrEmpty(transactionID))
            {
                return null;
            }
            return this.transactions[transactionID];
        }

        #endregion
    }
}
