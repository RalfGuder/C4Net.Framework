using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Caliburn.Micro;

namespace C4Net.Framework.MVVM
{
    /// <summary>
    /// Class for the View Locator for our MEF bootstrapper.
    /// </summary>
    [Export(typeof(IViewLocator))]
    public class MefViewLocator : IViewLocator
    {
        #region - Fields -

        /// <summary>
        /// The theme manager.
        /// </summary>
        private readonly IThemeManager themeManager;

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="MefViewLocator"/> class.
        /// </summary>
        /// <param name="themeManager">The theme manager.</param>
        [ImportingConstructor]
        public MefViewLocator(IThemeManager themeManager)
        {
            this.themeManager = themeManager;
        }

        #endregion

        #region - Methods -

        /// <summary>
        /// If the view locator has an element of the type in the cache, then return this element, else create it
        /// and assign the theme.
        /// </summary>
        /// <param name="viewType">Type of the view.</param>
        /// <returns></returns>
        public UIElement GetOrCreateViewType(Type viewType)
        {
            UIElement cached = IoC.GetAllInstances(viewType).OfType<UIElement>().FirstOrDefault();
            if (cached != null)
            {
                Caliburn.Micro.ViewLocator.InitializeComponent(cached);
                return cached;
            }
            if (viewType.IsInterface || viewType.IsAbstract || !typeof(UIElement).IsAssignableFrom(viewType))
            {
                return new TextBlock { Text = string.Format("Cannot create {0}.", viewType.FullName) };
            }
            UIElement newInstance = (UIElement)Activator.CreateInstance(viewType);
            FrameworkElement frameworkElement = newInstance as FrameworkElement;
            if (frameworkElement != null)
            {
                frameworkElement.Resources.MergedDictionaries.Add(this.themeManager.ThemeResources);
            }
            ViewLocator.InitializeComponent(newInstance);
            return newInstance;
        }

        #endregion
    }
}
