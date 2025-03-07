using System;
using System.Data;

namespace C4Net.Framework.Data.Base
{
    /// <summary>
    /// Base class for a Parameter.
    /// </summary>
    [Serializable]
    public class BaseParameter
    {
        #region - Fields -

        /// <summary>
        /// Field for a value.
        /// </summary>
        private object value = DBNull.Value;

        #endregion

        #region - Properties -

        /// <summary>
        /// Gets or sets the direction.
        /// </summary>
        /// <value>
        /// The direction.
        /// </value>
        public ParameterDirection Direction { get; set; }

        /// <summary>
        /// Gets or sets the type of the db.
        /// </summary>
        /// <value>
        /// The type of the db.
        /// </value>
        public DbType DbType { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public object Value
        {
            get { return this.value; }
            set { this.value = (value == null) ? DBNull.Value : value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is nullable.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is nullable; otherwise, <c>false</c>.
        /// </value>
        public bool IsNullable { get; set; }

        /// <summary>
        /// Gets or sets the source version.
        /// </summary>
        /// <value>
        /// The source version.
        /// </value>
        public DataRowVersion SourceVersion { get; set; }

        /// <summary>
        /// Gets or sets the name of the parameter.
        /// </summary>
        /// <value>
        /// The name of the parameter.
        /// </value>
        public string ParameterName { get; set; }

        /// <summary>
        /// Gets or sets the source column.
        /// </summary>
        /// <value>
        /// The source column.
        /// </value>
        public string SourceColumn { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [source column null mapping].
        /// </summary>
        /// <value>
        /// <c>true</c> if [source column null mapping]; otherwise, <c>false</c>.
        /// </value>
        public bool SourceColumnNullMapping { get; set; }

        /// <summary>
        /// Gets or sets the precision.
        /// </summary>
        /// <value>
        /// The precision.
        /// </value>
        public byte Precision { get; set; }

        /// <summary>
        /// Gets or sets the scale.
        /// </summary>
        /// <value>
        /// The scale.
        /// </value>
        public byte Scale { get; set; }

        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        /// <value>
        /// The size.
        /// </value>
        public int Size { get; set; }

        #endregion

        #region - Constuctors -

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseParameter"/> class.
        /// </summary>
        public BaseParameter()
        {
            this.Direction = ParameterDirection.Input;
            this.DbType = DbType.String;
            this.IsNullable = true;
            this.SourceVersion = DataRowVersion.Default;
            this.ParameterName = string.Empty;
            this.SourceColumn = string.Empty;
            this.SourceColumnNullMapping = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseParameter"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public BaseParameter(string name, object value)
            : this()
        {
            this.ParameterName = name;
            this.Value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseParameter"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="dbType">Type of the db.</param>
        /// <param name="value">The value.</param>
        public BaseParameter(string name, DbType dbType, object value)
            : this(name, value)
        {
            this.DbType = dbType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseParameter"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="dbType">Type of the db.</param>
        /// <param name="direction">The direction.</param>
        /// <param name="sourceColumn">The source column.</param>
        /// <param name="sourceVersion">The source version.</param>
        /// <param name="sourceColumnNullMapping">if set to <c>true</c> [source column null mapping].</param>
        public BaseParameter(string name, DbType dbType, ParameterDirection direction, string sourceColumn, DataRowVersion sourceVersion, bool sourceColumnNullMapping)
            : this()
        {
            this.ParameterName = name;
            this.DbType = dbType;
            this.Direction = direction;
            this.SourceColumn = sourceColumn;
            this.SourceVersion = sourceVersion;
            this.SourceColumnNullMapping = sourceColumnNullMapping;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseParameter"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="dbType">Type of the db.</param>
        /// <param name="sourceColumn">The source column.</param>
        /// <param name="sourceColumnNullMapping">if set to <c>true</c> [source column null mapping].</param>
        public BaseParameter(string name, DbType dbType, string sourceColumn, bool sourceColumnNullMapping)
            : this(name, dbType, ParameterDirection.Input, sourceColumn, DataRowVersion.Original, sourceColumnNullMapping)
        {
        }

        #endregion
    }
}
