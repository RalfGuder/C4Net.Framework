namespace C4Net.Framework.MVVM
{
    /// <summary>
    /// Interface for a Service locator, locates exported values at the container.
    /// </summary>
    public interface IServiceLocator
    {
        #region - Methods -

        /// <summary>
        /// Gets the instance of the given type stored at the container.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetInstance<T>() where T : class;

        #endregion
    }
}
