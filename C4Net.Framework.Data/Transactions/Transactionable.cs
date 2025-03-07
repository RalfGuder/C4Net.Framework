using System;
using System.Data;
using C4Net.Framework.Core.IoC;
using C4Net.Framework.Core.Log;
using C4Net.Framework.Data.Configuration;

namespace C4Net.Framework.Data.Transactions
{
    /// <summary>
    /// Base class for a loggable and transactionable object.
    /// </summary>
    [Serializable]
    public class Transactionable : LoggableClass, ITransactionManager
    {
        #region - Properties -

        /// <summary>
        /// Gets the transaction manager.
        /// </summary>
        /// <value>
        /// The transaction manager.
        /// </value>
        public ITransactionManager TransactionManager { get; private set; }

        public IDbConnection Connection
        {
            get { return this.TransactionManager.Connection; }
        }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="Transactionable"/> class.
        /// </summary>
        /// <param name="transactionManager">The transaction manager.</param>
        public Transactionable(ITransactionManager transactionManager)
        {
            this.TransactionManager = transactionManager;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Transactionable"/> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        public Transactionable(IDbConnection connection)
        {
            this.TransactionManager = IoCDefault.Get<ITransactionFactory>().GetTransactionManager(connection);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Transactionable"/> class.
        /// </summary>
        /// <param name="connectionName">Name of the connection.</param>
        public Transactionable(string connectionName)
            : this(IoCDefault.Get<IDbConnectionManager>().GetConnection(connectionName))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Transactionable"/> class.
        /// </summary>
        public Transactionable()
            : this(IoCDefault.Get<IDbConnectionManager>().GetConnection())
        {
        }

        #endregion

        #region - Methods -

        /// <summary>
        /// Begins the transaction.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public string BeginTransaction()
        {
            return this.TransactionManager.BeginTransaction();
        }

        /// <summary>
        /// Begins the transaction.
        /// </summary>
        /// <param name="isolationLevel">The isolation level.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public string BeginTransaction(IsolationLevel isolationLevel)
        {
            return this.TransactionManager.BeginTransaction(isolationLevel);
        }

        /// <summary>
        /// Commits the transaction.
        /// </summary>
        /// <param name="transactionID">The transaction ID.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void CommitTransaction(string transactionID = null)
        {
            this.TransactionManager.CommitTransaction(transactionID);
        }

        /// <summary>
        /// Rollbacks the transaction.
        /// </summary>
        /// <param name="transactionID">The transaction ID.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void RollbackTransaction(string transactionID = null)
        {
            this.TransactionManager.RollbackTransaction(transactionID);
        }

        /// <summary>
        /// Gets a transaction by its ID.
        /// </summary>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IDbTransaction GetTransaction(string transactionID = null)
        {
            return this.TransactionManager.GetTransaction(transactionID);
        }

        /// <summary>
        /// Writes a command and parameters into log.
        /// </summary>
        /// <param name="severity">The severity.</param>
        /// <param name="command">The command.</param>
        public virtual void LogCommand(LogSeverity severity, string prefix, IDbCommand command)
        {
            if (command != null)
            {
                this.WriteLog(severity, prefix + " " + command.CommandText);
                foreach (IDbDataParameter parameter in command.Parameters)
                {
                    this.WriteLog(severity, string.Format(" Parameter {0}: {1} {2} (Size = {3}, Prec = {4}, Scale = {5}) [{6}]",
                        parameter.ParameterName, parameter.Direction, parameter.DbType, parameter.Size, parameter.Precision,
                        parameter.Scale, parameter.Value));
                }
            }
        }

        /// <summary>
        /// Writes a command and parameters into log.
        /// </summary>
        /// <param name="severity">The severity.</param>
        /// <param name="command">The command.</param>
        public virtual void LogCommand(LogSeverity severity, IDbCommand command)
        {
            this.LogCommand(severity, "Executing command with command text", command);
        }

        /// <summary>
        /// Writes a command and parameters into log with default severity debug.
        /// </summary>
        /// <param name="command">The command.</param>
        public virtual void LogCommand(IDbCommand command)
        {
            this.LogCommand(LogSeverity.Debug, command);
        }

        #endregion
    }
}
