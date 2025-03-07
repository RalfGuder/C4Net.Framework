using System;

namespace C4Net.Framework.Data.Attributes
{
    public class MaxValueAttribute : Attribute
    {
        #region - Properties -

        public string Value { get; private set; }

        #endregion

        #region - Constructors -

        public MaxValueAttribute(string value)
        {
            this.Value = value;
        }

        #endregion
    }
}
