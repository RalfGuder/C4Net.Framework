using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows;

namespace C4Net.Framework.MVVM
{
    /// <summary>
    /// Class for a theme manager.
    /// </summary>
    [Export(typeof(IThemeManager))]
    public class ThemeManager : IThemeManager
    {
        #region - Fields -

        /// <summary>
        /// The current theme name.
        /// </summary>
        private string currentTheme = string.Empty;

        /// <summary>
        /// Dictionary of themes.
        /// </summary>
        private Dictionary<string, ResourceDictionary> themes = new Dictionary<string, ResourceDictionary>();

        #endregion

        #region - Properties -

        /// <summary>
        /// Gets the current theme resources.
        /// </summary>
        /// <value>
        /// The current theme resources.
        /// </value>
        public ResourceDictionary ThemeResources { get; private set; }

        /// <summary>
        /// Gets or sets the current theme name.
        /// </summary>
        /// <value>
        /// The current theme.
        /// </value>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">Theme not found</exception>
        public string CurrentTheme
        {
            get { return this.currentTheme; }
            set
            {
                if (this.themes.ContainsKey(value))
                {
                    this.currentTheme = value;
                    this.ThemeResources = this.themes[value];
                }
                else
                {
                    throw new KeyNotFoundException("Theme not found");
                }
            }
        }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="ThemeManager"/> class.
        /// </summary>
        public ThemeManager()
        {
        }

        #endregion

        #region - Methods -

        /// <summary>
        /// Registers one theme.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="path">The path.</param>
        public void RegisterTheme(string name, string path)
        {
            ResourceDictionary theme = new ResourceDictionary
            {
                Source = new Uri(path)
            };
            if (this.themes.ContainsKey(name))
            {
                this.themes[name] = theme;
            }
            else
            {
                this.themes.Add(name, theme);
                if (this.themes.Count == 1)
                {
                    this.currentTheme = name;
                    this.ThemeResources = theme;
                }
            }
        }

        #endregion
    }
}
