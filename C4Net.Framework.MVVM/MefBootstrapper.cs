using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Windows;
using Caliburn.Micro;

namespace C4Net.Framework.MVVM
{
    /// <summary>
    /// Abstract class for a MEF Bootstrapper.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class MefBootstrapper<T> : Bootstrapper<T>
    {
        #region - Fields -

        ///// <summary>
        ///// The container of composable parts.
        ///// </summary>
        private CompositionContainer container;

        #endregion

        #region - Methods -

        /// <summary>
        /// Override to configure the framework and setup your IoC container.
        /// </summary>
        protected override void Configure()
        {
            // Create the composition container and select all the composable parts from the assembly catalog.
            this.container = new CompositionContainer(new AggregateCatalog(AssemblySource.Instance.Select(x => new AssemblyCatalog(x)).OfType<ComposablePartCatalog>()));
            // Composition parts.
            CompositionBatch batch = new CompositionBatch();
            batch.AddExportedValue<IWindowManager>(new WindowManager());
            batch.AddExportedValue<IEventAggregator>(new EventAggregator());
            batch.AddExportedValue<IServiceLocator>(new MefServiceLocator(this.container));
            IThemeManager themeManager = new ThemeManager();
            batch.AddExportedValue<IThemeManager>(themeManager);
            batch.AddExportedValue<IViewLocator>(new MefViewLocator(themeManager));
            batch.AddExportedValue(this.container);
            batch.AddExportedValue(this.container.Catalog);
            this.container.Compose(batch);
        }

        /// <summary>
        /// Gets all instances.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns></returns>
        protected override IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return this.container.GetExportedValues<object>(AttributedModelServices.GetContractName(serviceType));
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        protected override object GetInstance(Type serviceType, string key)
        {
            string contract = string.IsNullOrEmpty(key) ? AttributedModelServices.GetContractName(serviceType) : key;
            IEnumerable<object> exports = this.container.GetExportedValues<object>(contract);
            if (exports.Any())
            {
                return exports.First();
            }
            throw new Exception(string.Format("Could not locate any instances of contract {0}.", contract));
        }

        /// <summary>
        /// Override this to provide an IoC specific implementation.
        /// </summary>
        /// <param name="instance">The instance to perform injection on.</param>
        protected override void BuildUp(object instance)
        {
            this.container.SatisfyImportsOnce(instance);
        }

        /// <summary>
        /// Called when [startup].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="StartupEventArgs"/> instance containing the event data.</param>
        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            this.ConfigureIoC();
            this.GetAllInstances(typeof(StartupTask)).Cast<ExportedDelegate>()
                .Select(exportedDelegate => (StartupTask)exportedDelegate.CreateDelegate(typeof(StartupTask))).Apply(s => s());
            base.OnStartup(sender, e);
        }

        /// <summary>
        /// Configures the container.
        /// </summary>
        protected abstract void ConfigureIoC();

        #endregion
    }
}
