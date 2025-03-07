using System;

namespace C4Net.Framework.Core.Conversions
{
    /// <summary>
    /// Delegate for converting an instance to another.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    public delegate object ConversionDelegate(object value);

    /// <summary>
    /// Interface for a type conversion.
    /// </summary>
    public interface IConversion
    {
        #region - Methods -

        /// <summary>
        /// Determines whether this instance [can convert to] the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///   <c>true</c> if this instance [can convert to] the specified type; otherwise, <c>false</c>.
        /// </returns>
        bool CanConvertTo(Type type);

        /// <summary>
        /// Determines whether this instance [can convert from] the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///   <c>true</c> if this instance [can convert from] the specified type; otherwise, <c>false</c>.
        /// </returns>
        bool CanConvertFrom(Type type);

        /// <summary>
        /// Converts to.
        /// </summary>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        object ConvertTo(Type targetType, object instance);

        /// <summary>
        /// Converts from.
        /// </summary>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        object ConvertFrom(Type targetType, object instance);

        /// <summary>
        /// Gets the delegate to.
        /// </summary>
        /// <param name="targetType">Type of the target.</param>
        /// <returns></returns>
        ConversionDelegate GetDelegateTo(Type targetType);

        /// <summary>
        /// Gets the delegate from.
        /// </summary>
        /// <param name="targetType">Type of the target.</param>
        /// <returns></returns>
        ConversionDelegate GetDelegateFrom(Type targetType);

        #endregion
    }
}
