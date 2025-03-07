using System.Data;

namespace C4Net.Framework.Data.Configuration
{
    /// <summary>
    /// Manager for connections to database.
    /// </summary>
    public interface IDbConnectionManager
    {
        #region - Methods -

        /// <summary>
        /// Registers the connection.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <exception cref="System.ApplicationException"></exception>
        void RegisterConnection(DbConnectionInfo connection);

        /// <summary>
        /// Gets the connection info for a given name.
        /// </summary>
        /// <param name="connectionName">Name of the connection.</param>
        /// <returns></returns>
        DbConnectionInfo GetConnectionInfo(string connectionName = "");

        /// <summary>
        /// Gets the connection for a given connection name.
        /// </summary>
        /// <param name="connectionName">Name of the connection.</param>
        /// <returns></returns>
        IDbConnection GetConnection(string connectionName = "");

        /// <summary>
        /// Gets the full source name of a connection, including default schema if exists.
        /// </summary>
        /// <param name="connectionName">Name of the connection.</param>
        /// <param name="sourceName">Name of the source.</param>
        /// <returns></returns>
        string GetFullSourceName(string connectionName, string sourceName);

        #endregion
    }
}
