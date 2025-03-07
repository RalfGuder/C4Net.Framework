using System;

namespace C4Net.Framework.Data.Attributes
{
    public class DataLengthAttribute : Attribute
    {
        #region - Properties -

        public int Value { get; private set; }

        #endregion

        #region - Constructors -

        public DataLengthAttribute(int value)
        {
            this.Value = value;
        }

        #endregion
    }
}
