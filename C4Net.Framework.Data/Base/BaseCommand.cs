using System;
using System.Data;

namespace C4Net.Framework.Data.Base
{
    /// <summary>
    /// Base class for a command.
    /// </summary>
    [Serializable]
    public class BaseCommand
    {
        #region - Fields -

        /// <summary>
        /// Parameter collection.
        /// </summary>
        private BaseParameterCollection parameters = null;

        #endregion

        #region - Properties -

        /// <summary>
        /// Gets or sets the command text.
        /// </summary>
        /// <value>
        /// The command text.
        /// </value>
        public string CommandText { get; set; }

        /// <summary>
        /// Gets or sets the type of the command.
        /// </summary>
        /// <value>
        /// The type of the command.
        /// </value>
        public CommandType CommandType { get; set; }

        /// <summary>
        /// Gets or sets the command timeout.
        /// </summary>
        /// <value>
        /// The command timeout.
        /// </value>
        public int CommandTimeout { get; set; }

        /// <summary>
        /// Gets or sets the parameter prefix.
        /// </summary>
        /// <value>
        /// The parameter prefix.
        /// </value>
        public string ParameterPrefix { get; set; }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <value>
        /// The parameters.
        /// </value>
        public BaseParameterCollection Parameters
        {
            get
            {
                if (this.parameters == null)
                {
                    this.parameters = new BaseParameterCollection();
                }
                return this.parameters;
            }
        }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseCommand"/> class.
        /// </summary>
        public BaseCommand()
        {
            this.CommandText = string.Empty;
            this.CommandType = CommandType.Text;
            this.CommandTimeout = 30;
            this.ParameterPrefix = "@";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseCommand"/> class.
        /// </summary>
        /// <param name="commandText">The command text.</param>
        public BaseCommand(string commandText)
            : this()
        {
            this.CommandText = commandText;
        }

        #endregion

        #region - Methods -

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.CommandText;
        }

        /// <summary>
        /// Gets a value indicating whether this instance has parameters.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has parameters; otherwise, <c>false</c>.
        /// </value>
        public bool HasParameters
        {
            get
            {
                return ((this.parameters != null) && (this.parameters.Count > 0));
            }
        }

        /// <summary>
        /// Adds a parameter.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="dbType">Type of the db.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public BaseParameter AddParameter(string name, DbType dbType, object value)
        {
            return this.parameters.Add(name, dbType, value);
        }

        /// <summary>
        /// Adds a parameter.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public BaseParameter AddParameter(string name, object value)
        {
            return this.parameters.Add(name, value);
        }

        #endregion
    }
}
