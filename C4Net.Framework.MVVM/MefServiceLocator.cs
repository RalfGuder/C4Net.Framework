using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace C4Net.Framework.MVVM
{
    /// <summary>
    /// Class for a service locator, that search for instances in the container.
    /// </summary>
    [Export(typeof(IServiceLocator))]
    public class MefServiceLocator : IServiceLocator
    {
        #region - Fields -

        /// <summary>
        /// The composition container.
        /// </summary>
        private readonly CompositionContainer compositionContainer;

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="MefServiceLocator"/> class.
        /// </summary>
        /// <param name="compositionContainer">The composition container.</param>
        [ImportingConstructor]
        public MefServiceLocator(CompositionContainer compositionContainer)
        {
            this.compositionContainer = compositionContainer;
        }

        #endregion

        #region - Methods -

        /// <summary>
        /// Gets the instance of the given type stored at the container.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public T GetInstance<T>() where T : class
        {
            T instance = this.compositionContainer.GetExportedValue<T>();
            if (instance != null)
            {
                return instance;
            }
            throw new Exception(string.Format("Could not locate any instances of contract {0}.", typeof(T)));
        }

        #endregion
    }
}
