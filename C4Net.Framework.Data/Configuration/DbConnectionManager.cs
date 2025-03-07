using System;
using System.Collections.Generic;
using System.Data;
using System.Xml;
using C4Net.Framework.Core.Utils;

namespace C4Net.Framework.Data.Configuration
{
    /// <summary>
    /// Manager for connections.
    /// </summary>
    public class DbConnectionManager : BaseXmlManager, IDbConnectionManager
    {
        #region - Constants -

        /// <summary>
        /// The default connection name
        /// </summary>
        private const string DefaultConnectionName = "DefaultConnection";

        #endregion

        #region - Fields -

        /// <summary>
        /// The connection info dictionary.
        /// </summary>
        private readonly Dictionary<string, DbConnectionInfo> connectionInfos = new Dictionary<string, DbConnectionInfo>();

        /// <summary>
        /// The database connection cache.
        /// </summary>
        private readonly Dictionary<string, IDbConnection> connectionCache = new Dictionary<string, IDbConnection>();

        #endregion

        #region - Methods -

        /// <summary>
        /// Process the node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public override void ProcessNode(XmlNode node)
        {
            this.RegisterConnection(new DbConnectionInfo(node));
        }

        /// <summary>
        /// Registers the connection.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <exception cref="System.ApplicationException"></exception>
        public void RegisterConnection(DbConnectionInfo connection)
        {
            if (connection != null)
            {
                connection.Initialize();
                this.connectionInfos.Add(connection.Name, connection);
                if (connection.IsDefault)
                {
                    if (this.connectionInfos.ContainsKey(DefaultConnectionName))
                    {
                        throw new ApplicationException(string.Format("Error while configuring the Connection named \"{0}\" There can be only one default Connection.", connection.Name));
                    }
                    this.connectionInfos.Add(DefaultConnectionName, connection);
                }
            }
        }

        /// <summary>
        /// Gets the connection info for a given name.
        /// </summary>
        /// <param name="connectionName">Name of the connection.</param>
        /// <returns></returns>
        public DbConnectionInfo GetConnectionInfo(string connectionName = "")
        {
            if (string.IsNullOrEmpty(connectionName))
            {
                connectionName = DefaultConnectionName;
            }
            return this.connectionInfos.ContainsKey(connectionName) ? this.connectionInfos[connectionName] : null;
        }

        /// <summary>
        /// Creates a new connection.
        /// </summary>
        /// <param name="info">The info.</param>
        /// <returns></returns>
        protected IDbConnection NewConnection(DbConnectionInfo info)
        {
            IDbConnection connection = info.Provider.CreateConnection();
            if (connection != null)
            {
                connection.ConnectionString = info.ConnectionString;
            }
            return connection;
        }

        /// <summary>
        /// Gets the connection for a given connection name.
        /// </summary>
        /// <param name="connectionName">Name of the connection.</param>
        /// <returns></returns>
        public IDbConnection GetConnection(string connectionName = "")
        {
            DbConnectionInfo info = GetConnectionInfo(connectionName);
            if (info == null)
            {
                return null;
            }
            if (!info.ReuseConnection)
            {
                return NewConnection(info);
            }
            else
            {
                if (string.IsNullOrEmpty(connectionName))
                {
                    connectionName = DefaultConnectionName;
                }
                if (this.connectionCache.ContainsKey(connectionName))
                {
                    return this.connectionCache[connectionName];
                }
                IDbConnection connection = NewConnection(info);
                if (connection != null)
                {
                    this.connectionCache.Add(connectionName, connection);
                }
                return connection;
            }
        }

        /// <summary>
        /// Gets the full source name of a connection, including default schema if exists.
        /// </summary>
        /// <param name="connectionName">Name of the connection.</param>
        /// <param name="sourceName">Name of the source.</param>
        /// <returns></returns>
        public string GetFullSourceName(string connectionName, string sourceName)
        {
            DbConnectionInfo info = this.GetConnectionInfo(connectionName);
            if ((info == null) || (sourceName.Contains(".")))
            {
                return sourceName;
            }
            return string.IsNullOrEmpty(info.DefaultSchema) ? sourceName : info.DefaultSchema + "." + sourceName;
        }

        #endregion
    }
}
