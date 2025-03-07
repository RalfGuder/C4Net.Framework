using System;
using System.Collections.Generic;
using System.Data;
using C4Net.Framework.Core.Conversions.Common;
using C4Net.Framework.Core.Types;

namespace C4Net.Framework.Core.Conversions
{
    /// <summary>
    /// Class for a manager of conversions.
    /// </summary>
    public class ConversionManager : IConversionManager
    {
        #region - Fields -

        /// <summary>
        /// The conversions
        /// </summary>
        protected Dictionary<DbType, IConversion> conversions = new Dictionary<DbType, IConversion>();

        /// <summary>
        /// The enum conversions
        /// </summary>
        protected Dictionary<Type, IConversion> enumConversions = new Dictionary<Type, IConversion>();

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="ConversionManager"/> class.
        /// </summary>
        public ConversionManager()
        {
            this.FillDefault();
        }

        #endregion

        #region - Methods -

        /// <summary>
        /// Fills the default.
        /// </summary>
        protected virtual void FillDefault()
        {
            this.conversions.Add(DbType.String, new StringConversion());
            this.conversions.Add(DbType.Boolean, new BoolConversion());
        }

        /// <summary>
        /// Gets the enum conversion.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public IConversion GetEnumConversion(Type type)
        {
            if (this.enumConversions.ContainsKey(type))
            {
                return this.enumConversions[type];
            }
            IConversion result = new EnumConversion(type);
            this.enumConversions.Add(type, result);
            return result;
        }

        /// <summary>
        /// Gets the conversion DbType -> Type
        /// </summary>
        /// <param name="dbType">Type of the db.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public ConversionDelegate GetConversion(DbType dbType, Type type)
        {
            if (this.conversions.ContainsKey(dbType))
            {
                IConversion conversion = this.conversions[dbType];
                if (conversion.CanConvertTo(type))
                {
                    return conversion.GetDelegateTo(type);
                }
            }
            if (type.IsEnum)
            {
                if ((dbType == DbType.Int32) || (dbType == DbType.String))
                {
                    IConversion conversion = this.GetEnumConversion(type);
                    switch (dbType)
                    {
                        case DbType.Int32: return conversion.GetDelegateFrom(typeof(int));
                        case DbType.String: return conversion.GetDelegateFrom(typeof(string));
                    }
                }
            }
            if ((type.IsGenericType) && (type.GetGenericTypeDefinition() == typeof(Nullable)))
            {
                Type argumentsType = type.GetGenericArguments()[0];
                if ((argumentsType.IsEnum) || (TypesManager.IsCommonType(argumentsType)))
                {
                    return this.GetConversion(dbType, argumentsType);
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the conversion Type -> DbType.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="dbType">Type of the db.</param>
        /// <returns></returns>
        public ConversionDelegate GetConversion(Type type, DbType dbType)
        {
            if (this.conversions.ContainsKey(dbType))
            {
                IConversion conversion = this.conversions[dbType];
                if (conversion.CanConvertFrom(type))
                {
                    return conversion.GetDelegateFrom(type);
                }
            }
            if (type.IsEnum)
            {
                if ((dbType == DbType.Int32) || (dbType == DbType.String))
                {
                    IConversion conversion = this.GetEnumConversion(type);
                    switch (dbType)
                    {
                        case DbType.Int32: return conversion.GetDelegateTo(typeof(int));
                        case DbType.String: return conversion.GetDelegateTo(typeof(string));
                    }
                }
            }
            if ((type.IsGenericType) && (type.GetGenericTypeDefinition() == typeof(Nullable)))
            {
                Type argumentsType = type.GetGenericArguments()[0];
                if ((argumentsType.IsEnum) || (TypesManager.IsCommonType(argumentsType)))
                {
                    return this.GetConversion(argumentsType, dbType);
                }
            }
            return null;
        }

        /// <summary>
        /// Converts the specified value from DbType to Type.
        /// </summary>
        /// <param name="dbType">Type of the db.</param>
        /// <param name="type">The type.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public object Convert(DbType dbType, Type type, object value)
        {
            ConversionDelegate conversionDelegate = this.GetConversion(dbType, type);
            if (conversionDelegate != null)
            {
                return conversionDelegate(value);
            }
            return value;
        }

        /// <summary>
        /// Converts the specified value from Type to DbType.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="dbType">Type of the db.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public object Convert(Type type, DbType dbType, object value)
        {
            ConversionDelegate conversionDelegate = this.GetConversion(type, dbType);
            if (conversionDelegate != null)
            {
                return conversionDelegate(value);
            }
            return value;
        }

        #endregion
    }
}
