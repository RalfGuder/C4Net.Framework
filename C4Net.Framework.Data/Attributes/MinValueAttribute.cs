using System;

namespace C4Net.Framework.Data.Attributes
{
    public class MinValueAttribute : Attribute
    {
        #region - Properties -

        public string Value { get; private set; }

        #endregion

        #region - Constructors -

        public MinValueAttribute(string value)
        {
            this.Value = value;
        }

        #endregion
    }
}
