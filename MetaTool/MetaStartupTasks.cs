using System.ComponentModel.Composition;
using C4Net.Framework.MVVM;

namespace MetaTool
{
    /// <summary>
    /// Startup Tasks for this application.
    /// </summary>
    public class MetaStartupTasks : StartupTasks
    {
        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="MetaStartupTasks"/> class.
        /// </summary>
        /// <param name="serviceLocator">The service locator.</param>
        [ImportingConstructor]
        public MetaStartupTasks(IServiceLocator serviceLocator)
            : base(serviceLocator)
        {
        }

        #endregion

        #region - Methods -

        /// <summary>
        /// Applies the view locator override.
        /// </summary>
        [Export(typeof(StartupTask))]
        public override void ApplyViewLocatorOverride()
        {
            base.ApplyViewLocatorOverride();
        }

        /// <summary>
        /// Applies the binding scope override.
        /// </summary>
        [Export(typeof(StartupTask))]
        public override void ApplyBindingScopeOverride()
        {
            base.ApplyBindingScopeOverride();
        }

        #endregion
    }
}
