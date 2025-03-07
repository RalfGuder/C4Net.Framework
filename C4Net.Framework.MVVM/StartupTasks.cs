using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Windows;
using Caliburn.Micro;
using MahApps.Metro.Controls;

namespace C4Net.Framework.MVVM
{
    /// <summary>
    /// Delegate for a startup task
    /// </summary>
    public delegate void StartupTask();

    /// <summary>
    /// Class for the startup tasks that override the View Locator and the binding scope.
    /// </summary>
    public class StartupTasks
    {
        #region - Fields -

        /// <summary>
        /// The service locator
        /// </summary>
        private readonly IServiceLocator serviceLocator;

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="StartupTasks"/> class.
        /// </summary>
        /// <param name="serviceLocator">The service locator.</param>
        [ImportingConstructor]
        public StartupTasks(IServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        #endregion

        #region - Methods -

        /// <summary>
        /// Applies the view locator override.
        /// </summary>
        [Export(typeof(StartupTask))]
        public virtual void ApplyViewLocatorOverride()
        {
            IViewLocator viewLocator = this.serviceLocator.GetInstance<IViewLocator>();
            ViewLocator.GetOrCreateViewType = viewLocator.GetOrCreateViewType;
        }

        /// <summary>
        /// Applies the binding scope override.
        /// </summary>
        [Export(typeof(StartupTask))]
        public virtual void ApplyBindingScopeOverride()
        {
            var getNamedElements = BindingScope.GetNamedElements;
            BindingScope.GetNamedElements = o =>
            {
                MetroWindow metroWindow = o as MetroWindow;
                if (metroWindow == null)
                {
                    return getNamedElements(o);
                }

                List<FrameworkElement> list = new List<FrameworkElement>(getNamedElements(o));
                Type type = o.GetType();
                IEnumerable<FieldInfo> fields = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic).Where(f => f.DeclaringType == type);
                list.AddRange(fields.Where(f => f.FieldType == typeof(FlyoutsControl)).Select(f => f.GetValue(o)).Cast<FlyoutsControl>());
                return list;
            };
        }

        #endregion
    }
}
