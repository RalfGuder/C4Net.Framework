using System;

namespace C4Net.Framework.Core.Conversions.Common
{
    /// <summary>
    /// Conversion of boolean.
    /// </summary>
    public class BoolConversion : BaseConversion
    {
        /// <summary>
        /// The sons will implement this filling the dictionaries with the delegates for the conversions.
        /// </summary>
        public override void FillDefault()
        {
            this.AddConversion(typeof(int), ConvertToInt, ConvertFromInt);
        }

        /// <summary>
        /// Converts to int.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        private object ConvertToInt(object instance)
        {
            return Convert.ToInt32(instance);
        }

        /// <summary>
        /// Converts back from int.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        private object ConvertFromInt(object instance)
        {
            return Convert.ToBoolean(instance);
        }
    }
}
