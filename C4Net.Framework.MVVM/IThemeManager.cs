using System.Windows;

namespace C4Net.Framework.MVVM
{
    /// <summary>
    /// Interface for a Theme Manager that can store several themes and has one active.
    /// </summary>
    public interface IThemeManager
    {
        #region - Properties -

        /// <summary>
        /// Gets or sets the current theme name.
        /// </summary>
        /// <value>
        /// The current theme.
        /// </value>
        string CurrentTheme { get; set; }

        /// <summary>
        /// Gets the theme resources.
        /// </summary>
        /// <value>
        /// The theme resources.
        /// </value>
        ResourceDictionary ThemeResources { get; }

        #endregion

        #region - Methods -

        /// <summary>
        /// Registers one theme.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="path">The path.</param>
        void RegisterTheme(string name, string path);

        #endregion
    }
}
