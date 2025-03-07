using System;
using System.ComponentModel;
using C4Net.Framework.Core.Utils;

namespace C4Net.Framework.Core.Conversions.Common
{
    /// <summary>
    /// Class for conversion of enumerateds.
    /// </summary>
    public class EnumConversion : BaseConversion
    {
        #region - Fields -

        /// <summary>
        /// The enum type.
        /// </summary>
        private Type type;

        /// <summary>
        /// The converter.
        /// </summary>
        private EnumConverter converter = null;

        #endregion

        #region - Properties -

        /// <summary>
        /// Gets the converter.
        /// </summary>
        /// <value>
        /// The converter.
        /// </value>
        protected EnumConverter Converter
        {
            get
            {
                if (this.converter == null)
                {
                    this.converter = new EnumConverter(this.type);
                }
                return this.converter;
            }
        }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumConversion"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public EnumConversion(Type type)
        {
            this.type = type;
        }

        #endregion

        #region - Methods -

        /// <summary>
        /// Fills the default.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public override void FillDefault()
        {
            this.AddConversion(typeof(int), ConvertToInt, ConvertFromInt);
            this.AddConversion(typeof(string), ConvertToString, ConvertFromString);
        }

        /// <summary>
        /// Converts to int.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public object ConvertToInt(object instance)
        {
            return (int)instance;
        }

        /// <summary>
        /// Converts from int.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public object ConvertFromInt(object instance)
        {
            return Enum.ToObject(this.type, instance);
        }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public object ConvertToString(object instance)
        {
            foreach (var item in Enum.GetValues(this.type))
            {
                StringValueAttribute[] attributes = (StringValueAttribute[])item.GetType().GetField(item.ToString()).GetCustomAttributes(typeof(StringValueAttribute), false);
                if ((attributes != null) && (attributes.Length > 0) && (attributes[0].Value.Equals(instance)))
                {
                    return item;
                }
            }
            return this.Converter.ConvertToString(instance);
        }

        /// <summary>
        /// Converts from string.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public object ConvertFromString(object instance)
        {
            StringValueAttribute[] attributes = (StringValueAttribute[])instance.GetType().GetField(instance.ToString()).GetCustomAttributes(typeof(StringValueAttribute), false);
            if ((attributes != null) && (attributes.Length > 0))
            {
                return attributes[0].Value;
            }
            return this.Converter.ConvertFromString(instance as string);
        }

        #endregion
    }
}
