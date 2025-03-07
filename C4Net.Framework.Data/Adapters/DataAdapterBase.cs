using System.Data;
using C4Net.Framework.Core.Log;
using C4Net.Framework.Data.Transactions;

namespace C4Net.Framework.Data.Adapters
{
    /// <summary>
    /// Base for Data Adapters with log.
    /// </summary>
    public class DataAdapterBase : Transactionable
    {
        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="DataAdapterBase"/> class.
        /// </summary>
        /// <param name="transactionManager">The transaction manager.</param>
        public DataAdapterBase(ITransactionManager transactionManager)
            : base(transactionManager)
        {
        }

        public DataAdapterBase(string connectionName = null)
            : base(connectionName)
        {
        }

        #endregion

        #region - Methods -

        /// <summary>
        /// Logs the adapter commands.
        /// </summary>
        /// <param name="severity">The severity.</param>
        /// <param name="adapter">The adapter.</param>
        public virtual void LogAdapter(LogSeverity severity, IDbDataAdapter adapter)
        {
            this.WriteLog(severity, "[DataAdapter]");
            this.LogCommand(severity, "Select command:", adapter.SelectCommand);
            this.LogCommand(severity, "Insert command:", adapter.InsertCommand);
            this.LogCommand(severity, "Update command:", adapter.UpdateCommand);
            this.LogCommand(severity, "Delete command:", adapter.DeleteCommand);
            this.WriteLog(severity, "[/DataAdapter]");
        }

        /// <summary>
        /// Logs the adapter commands.
        /// </summary>
        /// <param name="adapter">The adapter.</param>
        public virtual void LogAdapter(IDbDataAdapter adapter)
        {
            this.LogAdapter(LogSeverity.Debug, adapter);
        }

        #endregion
    }
}
