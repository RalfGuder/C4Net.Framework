using System;
using System.Windows;

namespace C4Net.Framework.MVVM
{
    /// <summary>
    /// Interface for a view locator, able to cache UIElements and to create them assigning the current theme.
    /// </summary>
    public interface IViewLocator
    {
        #region - Methods -

        /// <summary>
        /// If the view locator has an element of the type in the cache, then return this element, else create it
        /// and assign the theme.
        /// </summary>
        /// <param name="viewType">Type of the view.</param>
        /// <returns></returns>
        UIElement GetOrCreateViewType(Type viewType);

        #endregion
    }
}
