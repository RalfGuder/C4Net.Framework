using C4Net.Framework.MVVM;

namespace MetaTool
{
    /// <summary>
    /// Interface for the shell of the application.
    /// </summary>
    public interface IShell
    {
        #region - Properties -

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <value>
        /// The parameters.
        /// </value>
        ObservableKeyValue<string, string> Parameters { get; }

        #endregion
    }
}
