using System;
using System.Collections.Generic;

namespace C4Net.Framework.Core.Conversions
{
    /// <summary>
    /// Abstract class for implementing IConversion, with dictionaries for the delegates.
    /// </summary>
    public abstract class BaseConversion : IConversion
    {
        #region - Fields -

        /// <summary>
        /// Dictionary for the delegates of convert to.
        /// </summary>
        private Dictionary<Type, ConversionDelegate> delegatesTo = new Dictionary<Type, ConversionDelegate>();

        /// <summary>
        /// Dictionary for the delegates of convert from.
        /// </summary>
        private Dictionary<Type, ConversionDelegate> delegatesFrom = new Dictionary<Type, ConversionDelegate>();

        #endregion

        #region - Constructor -

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseConversion"/> class.
        /// </summary>
        public BaseConversion()
        {
            this.FillDefault();
        }

        #endregion

        #region - Methods -

        /// <summary>
        /// The sons will implement this filling the dictionaries with the delegates for the conversions.
        /// </summary>
        public abstract void FillDefault();

        /// <summary>
        /// Determines whether this instance [can convert to] the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///   <c>true</c> if this instance [can convert to] the specified type; otherwise, <c>false</c>.
        /// </returns>
        public bool CanConvertTo(Type type)
        {
            return this.delegatesTo.ContainsKey(type);
        }

        /// <summary>
        /// Determines whether this instance [can convert from] the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///   <c>true</c> if this instance [can convert from] the specified type; otherwise, <c>false</c>.
        /// </returns>
        public bool CanConvertFrom(Type type)
        {
            return this.delegatesFrom.ContainsKey(type);
        }

        /// <summary>
        /// Converts to.
        /// </summary>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Cannot convert to type</exception>
        public object ConvertTo(Type targetType, object instance)
        {
            if (this.CanConvertTo(targetType))
            {
                return this.delegatesTo[targetType](instance);
            }
            throw new ArgumentOutOfRangeException("Cannot convert to type");
        }

        /// <summary>
        /// Converts from.
        /// </summary>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Cannot convert from type</exception>
        public object ConvertFrom(Type targetType, object instance)
        {
            if (this.CanConvertFrom(targetType))
            {
                return this.delegatesFrom[targetType](instance);
            }
            throw new ArgumentOutOfRangeException("Cannot convert from type");
        }

        /// <summary>
        /// Gets the delegate to.
        /// </summary>
        /// <param name="targetType">Type of the target.</param>
        /// <returns></returns>
        public ConversionDelegate GetDelegateTo(Type targetType)
        {
            if (this.CanConvertTo(targetType))
            {
                return this.delegatesTo[targetType];
            }
            return null;
        }

        /// <summary>
        /// Gets the delegate from.
        /// </summary>
        /// <param name="targetType">Type of the target.</param>
        /// <returns></returns>
        public ConversionDelegate GetDelegateFrom(Type targetType)
        {
            if (this.CanConvertFrom(targetType))
            {
                return this.delegatesFrom[targetType];
            }
            return null;
        }

        /// <summary>
        /// Adds the conversion.
        /// </summary>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="delegateTo">The delegate to.</param>
        /// <param name="delegateFrom">The delegate from.</param>
        public void AddConversion(Type targetType, ConversionDelegate delegateTo, ConversionDelegate delegateFrom)
        {
            if (delegateTo != null)
            {
                this.delegatesTo[targetType] = delegateTo;
            }
            if (delegateFrom != null)
            {
                this.delegatesFrom[targetType] = delegateFrom;
            }
        }

        #endregion
    }
}
