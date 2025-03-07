using System;
using System.Collections.Generic;
using System.Data;

namespace C4Net.Framework.Data.Base
{
    /// <summary>
    /// Class for a collection of parameters.
    /// </summary>
    [Serializable]
    public class BaseParameterCollection : List<BaseParameter>
    {
        /// <summary>
        /// Adds a parameter.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public BaseParameter Add(string name, object value)
        {
            BaseParameter result = new BaseParameter(name, value);
            this.Add(result);
            return result;
        }

        /// <summary>
        /// Adds a parameter.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="dbType">Type of the db.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public BaseParameter Add(string name, DbType dbType, object value)
        {
            BaseParameter result = new BaseParameter(name, dbType, value);
            this.Add(result);
            return result;
        }
    }
}
