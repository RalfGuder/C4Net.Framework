using System;
using System.Xml;
using C4Net.Framework.Core.IoC;
using C4Net.Framework.Core.Utils;

namespace C4Net.Framework.Data.Configuration
{
    /// <summary>
    /// Class for the information of a connection.
    /// </summary>
    [Serializable]
    public class DbConnectionInfo : BaseXmlManagerItem
    {
        #region - Properties -

        /// <summary>
        /// Gets the name of the connection
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the name of the provider.
        /// </summary>
        /// <value>
        /// The name of the provider.
        /// </value>
        public string ProviderName { get; set; }

        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        /// <value>
        /// The connection string.
        /// </value>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the default schema.
        /// </summary>
        /// <value>
        /// The default schema.
        /// </value>
        public string DefaultSchema { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is default.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is default; otherwise, <c>false</c>.
        /// </value>
        public bool IsDefault { get; set; }

        /// <summary>
        /// Gets the provider.
        /// </summary>
        /// <value>
        /// The provider.
        /// </value>
        public DbProviderInfo Provider { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether [reuse connection].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [reuse connection]; otherwise, <c>false</c>.
        /// </value>
        public bool ReuseConnection { get; set; }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="DbConnectionInfo"/> class.
        /// </summary>
        /// <param name="node">The node.</param>
        public DbConnectionInfo(XmlNode node)
            : base(node)
        {
        }

        public DbConnectionInfo()
            : base(null)
        {
        }

        #endregion

        #region - Methods -

        /// <summary>
        /// Inner load of the item.
        /// </summary>
        protected override void InnerLoad(NodeAttributes attributes)
        {
            this.Name = attributes.AsString("name");
            this.ProviderName = attributes.AsString("providerName");
            this.DefaultSchema = attributes.AsString("defaultSchema", string.Empty);
            this.IsDefault = attributes.AsBool("default", false);
            this.ConnectionString = attributes.Node.InnerText;
            this.ReuseConnection = attributes.AsBool("reuseConnection", true);
            this.Provider = null;
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public void Initialize()
        {
            this.Provider = IoCDefault.Get<IDbProviderManager>().GetProvider(this.ProviderName);
        }

        #endregion
    }
}
