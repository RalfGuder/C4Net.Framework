using System.Data;

namespace C4Net.Framework.Data.Configuration
{
    /// <summary>
    /// Interface for the Database provider manager.
    /// </summary>
    public interface IDbProviderManager
    {
        #region - Methods -

        /// <summary>
        /// Registers a provider.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <exception cref="System.ApplicationException"></exception>
        void RegisterProvider(DbProviderInfo provider);

        /// <summary>
        /// Gets a provider by name.
        /// </summary>
        /// <param name="providerName">Name of the provider.</param>
        /// <returns></returns>
        DbProviderInfo GetProvider(string providerName);

        /// <summary>
        /// Indicates if exists a provider with the given name.
        /// </summary>
        /// <param name="providerName">Name of the provider.</param>
        /// <returns></returns>
        bool ProviderExists(string providerName);

        /// <summary>
        /// Creates a new database connection, searching by provider name over the provider list and the factories.
        /// </summary>
        /// <param name="providerName">Name of the provider.</param>
        /// <returns></returns>
        IDbConnection CreateConnection(string providerName);

        /// <summary>
        /// Formats a parameter name with the provider information about parameters.
        /// </summary>
        /// <param name="providerName">Name of the provider.</param>
        /// <param name="commonParameterName">Common name of the parameter.</param>
        /// <param name="commonParameterPrefix">Common prefix of the parameter.</param>
        /// <param name="parameterNameInCommandText">The parameter name in command text.</param>
        /// <returns></returns>
        string GetParameterName(string providerName, string commonParameterName, string commonParameterPrefix, out string parameterNameInCommandText);

        #endregion
    }
}
