using System;
using System.Data;

namespace C4Net.Framework.Core.Conversions
{
    /// <summary>
    /// Interface for a Conversion Manager.
    /// </summary>
    public interface IConversionManager
    {
        #region - Methods -

        /// <summary>
        /// Gets the conversion DbType -> Type
        /// </summary>
        /// <param name="dbType">Type of the db.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        ConversionDelegate GetConversion(DbType dbType, Type type);

        /// <summary>
        /// Gets the conversion Type -> DbType.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="dbType">Type of the db.</param>
        /// <returns></returns>
        ConversionDelegate GetConversion(Type type, DbType dbType);

        /// <summary>
        /// Converts the specified value from DbType to Type.
        /// </summary>
        /// <param name="dbType">Type of the db.</param>
        /// <param name="type">The type.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        object Convert(DbType dbType, Type type, object value);

        /// <summary>
        /// Converts the specified value from Type to DbType.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="dbType">Type of the db.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        object Convert(Type type, DbType dbType, object value);

        #endregion
    }
}
