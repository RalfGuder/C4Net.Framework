using C4Net.Framework.Core.Conversions;
using C4Net.Framework.Core.IoC;
using C4Net.Framework.Core.Log;
using C4Net.Framework.Data.Configuration;
using C4Net.Framework.Data.Transactions;
using C4Net.Framework.MVVM;
using C4Net.Framework.NLoggable;
using Caliburn.Micro;

namespace MetaTool
{
    /// <summary>
    /// Bootstrapper for the application.
    /// </summary>
    public class MetaBootstrapper : MefBootstrapper<IShell>
    {
        #region - Methods -

        /// <summary>
        /// Configures the container.
        /// </summary>
        protected override void ConfigureIoC()
        {
            IThemeManager themeManager = IoC.Get<IThemeManager>();
            themeManager.RegisterTheme("DefaultTheme", "pack://application:,,,/Resources/MetaToolTheme.xaml");
            IoCDefault.RegisterSingleton<ILoggable>(new NLoggable());
            IoCDefault.RegisterSingleton<ITransactionFactory>(new TransactionFactory());
            IoCDefault.RegisterSingleton<IConversionManager>(new ConversionManager());
            DbProviderManager providerManager = new DbProviderManager();
            providerManager.LoadElements("Providers.config", "configuration/providers/provider");
            IoCDefault.RegisterSingleton<IDbProviderManager>(providerManager);
            DbConnectionManager connectionManager = new DbConnectionManager();
            IoCDefault.RegisterSingleton<IDbConnectionManager>(connectionManager);
        }

        #endregion
    }
}
