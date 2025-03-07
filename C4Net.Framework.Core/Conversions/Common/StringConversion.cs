using System;
using System.Drawing;

namespace C4Net.Framework.Core.Conversions.Common
{
    /// <summary>
    /// Common class for String Conversion.
    /// </summary>
    public class StringConversion : BaseConversion
    {
        #region - Fields -

        /// <summary>
        /// Color converter.
        /// </summary>
        private readonly ColorConverter colorConverter = new ColorConverter();

        #endregion

        #region - Methods -

        /// <summary>
        /// Fills the default.
        /// </summary>
        public override void FillDefault()
        {
            this.AddConversion(typeof(bool), ConvertToBool, ConvertFromAny);
            this.AddConversion(typeof(byte), ConvertToByte, ConvertFromAny);
            this.AddConversion(typeof(DateTime), ConvertToDateTime, ConvertFromAny);
            this.AddConversion(typeof(decimal), ConvertToDecimal, ConvertFromAny);
            this.AddConversion(typeof(double), ConvertToDouble, ConvertFromAny);
            this.AddConversion(typeof(float), ConvertToFloat, ConvertFromAny);
            this.AddConversion(typeof(Guid), ConvertToGuid, ConvertFromAny);
            this.AddConversion(typeof(int), ConvertToInt, ConvertFromAny);
            this.AddConversion(typeof(long), ConvertToLong, ConvertFromAny);
            this.AddConversion(typeof(short), ConvertToShort, ConvertFromAny);
            this.AddConversion(typeof(Color), ConvertToColor, ConvertFromColor);
        }

        /// <summary>
        /// Converts from any primitive type.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        private object ConvertFromAny(object instance)
        {
            return Convert.ToString(instance);
        }

        /// <summary>
        /// Converts to bool.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        private object ConvertToBool(object instance)
        {
            return bool.Parse(instance as string);
        }

        /// <summary>
        /// Converts to byte.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        private object ConvertToByte(object instance)
        {
            return byte.Parse(instance as string);
        }

        /// <summary>
        /// Converts to date time.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        private object ConvertToDateTime(object instance)
        {
            return DateTime.Parse(instance as string);
        }

        /// <summary>
        /// Converts to decimal.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        private object ConvertToDecimal(object instance)
        {
            return decimal.Parse(instance as string);
        }

        /// <summary>
        /// Converts to double.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        private object ConvertToDouble(object instance)
        {
            return double.Parse(instance as string);
        }

        /// <summary>
        /// Converts to float.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        private object ConvertToFloat(object instance)
        {
            return float.Parse(instance as string);
        }

        /// <summary>
        /// Converts to GUID.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        private object ConvertToGuid(object instance)
        {
            return new Guid(instance as string);
        }

        /// <summary>
        /// Converts to int.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        private object ConvertToInt(object instance)
        {
            return int.Parse(instance as string);
        }

        /// <summary>
        /// Converts to long.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        private object ConvertToLong(object instance)
        {
            return long.Parse(instance as string);
        }

        /// <summary>
        /// Converts to short.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        private object ConvertToShort(object instance)
        {
            return short.Parse(instance as string);
        }

        /// <summary>
        /// Converts to color.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        private object ConvertToColor(object instance)
        {
            return this.colorConverter.ConvertFromString(instance as string);
        }

        /// <summary>
        /// Converts back from color.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        private object ConvertFromColor(object instance)
        {
            return this.colorConverter.ConvertToString(instance);
        }

        #endregion
    }
}
