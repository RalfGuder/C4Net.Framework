using System.Collections.Generic;
using System.Data;

namespace C4Net.Framework.Data.Transactions
{
    /// <summary>
    /// Class for the transaction factory.
    /// </summary>
    public class TransactionFactory : ITransactionFactory
    {
        #region - Fields -

        /// <summary>
        /// The transaction manager dictionary, relates connections with managers.
        /// </summary>
        private Dictionary<IDbConnection, ITransactionManager> managers = new Dictionary<IDbConnection, ITransactionManager>();

        #endregion

        #region - Methods -

        /// <summary>
        /// Gets the transaction manager by the connection.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <returns></returns>
        public ITransactionManager GetTransactionManager(IDbConnection connection)
        {
            if (managers.ContainsKey(connection))
            {
                return managers[connection];
            }
            ITransactionManager result = new TransactionManager(connection);
            managers.Add(connection, result);
            return result;
        }

        /// <summary>
        /// Removes a transaction manager.
        /// </summary>
        /// <param name="connection">The connection.</param>
        public void RemoveTransactionManager(IDbConnection connection)
        {
            managers.Remove(connection);
        }

        #endregion
    }
}
