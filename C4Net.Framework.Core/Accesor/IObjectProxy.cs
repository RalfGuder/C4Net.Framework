using System;

namespace C4Net.Framework.Core.Accesor
{
    /// <summary>
    /// Interface for the proxy of an object, that allows to know the property type by name, gets and sets properties
    /// by name, and create new instances of the object or collection of objects.
    /// </summary>
    public interface IObjectProxy : IObjectAccessor, IObjectCreator
    {
        #region - Properties -

        /// <summary>
        /// Gets the property names.
        /// </summary>
        /// <value>
        /// The property names.
        /// </value>
        string[] PropertyNames { get; }

        #endregion

        #region - Methods -

        /// <summary>
        /// Gets the type of the property.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <returns></returns>
        Type GetPropertyType(string name);

        #endregion
    }
}
