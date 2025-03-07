using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Xml;
using C4Net.Framework.Core.Utils;

namespace C4Net.Framework.Data.Configuration
{
    /// <summary>
    /// Manager for database providers.
    /// </summary>
    public class DbProviderManager : BaseXmlManager, IDbProviderManager
    {
        #region - Fields -

        /// <summary>
        /// The DbProviderInfo dictionary.
        /// </summary>
        private readonly Dictionary<string, DbProviderInfo> providers = new Dictionary<string, DbProviderInfo>();

        /// <summary>
        /// Cache of provider factories already used.
        /// </summary>
        private readonly Dictionary<string, DbProviderFactory> factories = new Dictionary<string, DbProviderFactory>();

        #endregion

        #region - Methods -

        /// <summary>
        /// Process the node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public override void ProcessNode(XmlNode node)
        {
            this.RegisterProvider(new DbProviderInfo(node));
        }

        /// <summary>
        /// Registers a provider.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <exception cref="System.ApplicationException"></exception>
        public void RegisterProvider(DbProviderInfo provider)
        {
            if (provider.IsEnabled)
            {
                provider.Initialize();
                this.providers.Add(provider.Name, provider);
            }
        }

        /// <summary>
        /// Gets a provider by name. If no name is provided, returns default provider.
        /// </summary>
        /// <param name="providerName">Name of the provider.</param>
        /// <returns></returns>
        public DbProviderInfo GetProvider(string providerName)
        {
            if (string.IsNullOrEmpty(providerName))
            {
                return null;
            }
            return this.providers.ContainsKey(providerName) ? this.providers[providerName] : null;
        }

        /// <summary>
        /// Indicates if exists a provider with the given name.
        /// </summary>
        /// <param name="providerName">Name of the provider.</param>
        /// <returns></returns>
        public bool ProviderExists(string providerName)
        {
            return (!string.IsNullOrEmpty(providerName) && this.providers.ContainsKey(providerName));
        }

        /// <summary>
        /// Creates a new database connection, searching by provider name over the provider list and the factories.
        /// </summary>
        /// <param name="providerName">Name of the provider.</param>
        /// <returns></returns>
        public IDbConnection CreateConnection(string providerName)
        {
            DbProviderInfo provider = this.GetProvider(providerName);
            IDbConnection result = null;
            if (provider != null)
            {
                result = provider.CreateConnection();
            }
            else
            {
                DbProviderFactory factory;
                if (this.factories.ContainsKey(providerName))
                {
                    factory = this.factories[providerName];
                }
                else
                {
                    factory = DbProviderFactories.GetFactory(providerName);
                    if (factory != null)
                    {
                        this.factories.Add(providerName, factory);
                    }
                }
                if (factory != null)
                {
                    result = factory.CreateConnection();
                }
            }
            return result;
        }

        /// <summary>
        /// Formats a parameter name with the provider information about parameters.
        /// </summary>
        /// <param name="providerName">Name of the provider.</param>
        /// <param name="commonParameterName">Common name of the parameter.</param>
        /// <param name="commonParameterPrefix">Common prefix of the parameter.</param>
        /// <param name="parameterNameInCommandText">The parameter name in command text.</param>
        /// <returns></returns>
        public string GetParameterName(string providerName, string commonParameterName, string commonParameterPrefix, out string parameterNameInCommandText)
        {
            string parameterName = commonParameterName;
            parameterNameInCommandText = commonParameterName;
            DbProviderInfo provider = this.GetProvider(providerName);
            if (provider == null)
            {
                return null;
            }
            if (provider.UsePositionalParameters)
            {
                parameterName = "?";
                parameterNameInCommandText = "?";
            }
            else
            {
                if (provider.UseParameterPrefixInParameter)
                {
                    if (provider.ParameterPrefix != commonParameterPrefix)
                    {
                        parameterName = provider.ParameterPrefix + StringUtil.TrimStringStart(commonParameterName, commonParameterPrefix);
                    }
                }
                else
                {
                    parameterName = StringUtil.TrimStringStart(commonParameterName, commonParameterPrefix);
                }
                if (provider.UseParameterPrefixInSQL)
                {
                    if (provider.ParameterPrefix != commonParameterPrefix)
                    {
                        parameterNameInCommandText = provider.ParameterPrefix + StringUtil.TrimStringStart(commonParameterName, commonParameterPrefix);
                    }
                }
                else
                {
                    parameterNameInCommandText = StringUtil.TrimStringStart(commonParameterName, commonParameterPrefix);
                }
            }
            return parameterName;
        }

        #endregion
    }
}
