using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C4Net.Framework.Data.Attributes
{
    public class DataDecimalsAttribute : Attribute
    {
        #region - Properties -

        public int Value { get; private set; }

        #endregion

        #region - Constructors -

        public DataDecimalsAttribute(int value)
        {
            this.Value = value;
        }

        #endregion
    }
}
