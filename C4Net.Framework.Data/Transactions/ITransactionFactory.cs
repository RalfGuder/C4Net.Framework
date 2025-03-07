using System.Data;

namespace C4Net.Framework.Data.Transactions
{
    /// <summary>
    /// Interface for a transaction factory.
    /// </summary>
    public interface ITransactionFactory
    {
        #region - Methods -

        /// <summary>
        /// Gets the transaction manager by the connection.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <returns></returns>
        ITransactionManager GetTransactionManager(IDbConnection connection);

        /// <summary>
        /// Removes a transaction manager.
        /// </summary>
        /// <param name="connection">The connection.</param>
        void RemoveTransactionManager(IDbConnection connection);

        #endregion
    }
}
