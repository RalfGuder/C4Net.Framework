using System;
using System.Data;
using System.Reflection;
using System.Xml;
using C4Net.Framework.Core.Types;
using C4Net.Framework.Core.Utils;

namespace C4Net.Framework.Data.Configuration
{
    /// <summary>
    /// Class for the information of a database provider.
    /// </summary>
    [Serializable]
    public class DbProviderInfo : BaseXmlManagerItem
    {
        #region - Fields -

        /// <summary>
        /// The parameter prefix.
        /// </summary>
        private string parameterPrefix;

        /// <summary>
        /// The command builder type.
        /// </summary>
        private Type commandBuilderType = null;

        /// <summary>
        /// The parameter DbType.
        /// </summary>
        private Type parameterDbType = null;

        /// <summary>
        /// The template data adapter.
        /// </summary>
        private IDbDataAdapter templateDataAdapter = null;

        /// <summary>
        /// Indicates if the template data adapter implements ICloneable.
        /// </summary>
        private bool templateDataAdapterIsICloneable = false;

        /// <summary>
        /// The template connection.
        /// </summary>
        private IDbConnection templateConnection = null;

        /// <summary>
        /// Indicates if the template connection implements ICloneable.
        /// </summary>
        private bool templateConnectionIsICloneable = false;

        #endregion

        #region - Properties -

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the name of the assembly.
        /// </summary>
        /// <value>
        /// The name of the assembly.
        /// </value>
        public string AssemblyName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [allow mars].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [allow mars]; otherwise, <c>false</c>.
        /// </value>
        public bool AllowMars { get; set; }

        /// <summary>
        /// Gets or sets the db connection class.
        /// </summary>
        /// <value>
        /// The db connection class.
        /// </value>
        public string DbConnectionClass { get; set; }

        /// <summary>
        /// Gets or sets the db command class.
        /// </summary>
        /// <value>
        /// The db command class.
        /// </value>
        public string DbCommandClass { get; set; }

        /// <summary>
        /// Gets or sets the parameter db type class.
        /// </summary>
        /// <value>
        /// The parameter db type class.
        /// </value>
        public string ParameterDbTypeClass { get; set; }

        /// <summary>
        /// Gets or sets the data adapter class.
        /// </summary>
        /// <value>
        /// The data adapter class.
        /// </value>
        public string DataAdapterClass { get; set; }

        /// <summary>
        /// Gets or sets the command builder class.
        /// </summary>
        /// <value>
        /// The command builder class.
        /// </value>
        public string CommandBuilderClass { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [use parameter prefix in SQL].
        /// </summary>
        /// <value>
        /// <c>true</c> if [use parameter prefix in SQL]; otherwise, <c>false</c>.
        /// </value>
        public bool UseParameterPrefixInSQL { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [use parameter prefix in parameter].
        /// </summary>
        /// <value>
        /// <c>true</c> if [use parameter prefix in parameter]; otherwise, <c>false</c>.
        /// </value>
        public bool UseParameterPrefixInParameter { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [use positional parameters].
        /// </summary>
        /// <value>
        /// <c>true</c> if [use positional parameters]; otherwise, <c>false</c>.
        /// </value>
        public bool UsePositionalParameters { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [set db parameter size].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [set db parameter size]; otherwise, <c>false</c>.
        /// </value>
        public bool SetDbParameterSize { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [set db parameter precision].
        /// </summary>
        /// <value>
        /// <c>true</c> if [set db parameter precision]; otherwise, <c>false</c>.
        /// </value>
        public bool SetDbParameterPrecision { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [set db parameter scale].
        /// </summary>
        /// <value>
        /// <c>true</c> if [set db parameter scale]; otherwise, <c>false</c>.
        /// </value>
        public bool SetDbParameterScale { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [use derive parameters].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use derive parameters]; otherwise, <c>false</c>.
        /// </value>
        public bool UseDeriveParameters { get; set; }

        /// <summary>
        /// Gets or sets the parameter prefix.
        /// </summary>
        /// <value>
        /// The parameter prefix.
        /// </value>
        public string ParameterPrefix
        {
            get
            {
                return this.parameterPrefix;
            }
            set
            {
                this.parameterPrefix = value == null ? string.Empty : value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is ODBC.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is ODBC; otherwise, <c>false</c>.
        /// </value>
        public bool IsODBC
        {
            get
            {
                return (this.DbConnectionClass != null && this.DbConnectionClass.IndexOf(".Odbc.") > 0);
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is OLEDB.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is OLEDB; otherwise, <c>false</c>.
        /// </value>
        public bool IsOLEDB
        {
            get
            {
                return (this.DbConnectionClass != null && this.DbConnectionClass.IndexOf(".OleDb.") > 0);
            }
        }

        /// <summary>
        /// Gets the type of the command builder.
        /// </summary>
        /// <value>
        /// The type of the command builder.
        /// </value>
        public Type CommandBuilderType
        {
            get
            {
                return this.commandBuilderType;
            }
        }

        /// <summary>
        /// Gets the type of the parameter db.
        /// </summary>
        /// <value>
        /// The type of the parameter db.
        /// </value>
        public Type ParameterDbType
        {
            get
            {
                return this.parameterDbType;
            }
        }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="DbProviderInfo"/> class.
        /// </summary>
        /// <param name="node">The node.</param>
        public DbProviderInfo(XmlNode node)
            : base(node)
        {
        }

        #endregion

        #region - Methods -

        /// <summary>
        /// Inner load of the item.
        /// </summary>
        protected override void InnerLoad(NodeAttributes attributes)
        {
            this.Name = attributes.AsString("name");
            this.Description = attributes.AsString("description", this.Name);
            this.IsEnabled = attributes.AsBool("enabled", true);
            this.AssemblyName = attributes.AsString("assemblyName");
            this.DbConnectionClass = attributes.AsString("connectionClass");
            this.CommandBuilderClass = attributes.AsString("commandBuilderClass");
            this.DbCommandClass = attributes.AsString("commandClass");
            this.DataAdapterClass = attributes.AsString("dataAdapterClass");
            this.ParameterDbTypeClass = attributes.AsString("parameterDbTypeClass");
            this.ParameterPrefix = attributes.AsString("parameterPrefix");
            this.AllowMars = attributes.AsBool("allowMARS", false);
            this.UsePositionalParameters = attributes.AsBool("usePositionalParameters", false);
            this.UseParameterPrefixInParameter = attributes.AsBool("useParameterPrefixInParameter", true);
            this.UseParameterPrefixInSQL = attributes.AsBool("useParameterPrefixInSql", true);
            this.SetDbParameterPrecision = attributes.AsBool("setDbParameterPrecision", true);
            this.SetDbParameterScale = attributes.AsBool("setDbParameterScale", true);
            this.SetDbParameterSize = attributes.AsBool("setDbParameterSize", true);
            this.UseDeriveParameters = attributes.AsBool("useDeriveParameters", true);
        }

        /// <summary>
        /// Initializes this provider, and resolve types.
        /// </summary>
        /// <exception cref="System.ApplicationException"></exception>
        public void Initialize()
        {
            Assembly assembly = null;
            try
            {
                try
                {
                    assembly = Assembly.Load(this.AssemblyName);
                }
                catch
                {
                    assembly = null;
                }
                Type type = TypesManager.ResolveType(this.DataAdapterClass, assembly);
                this.AssertPropertyType("DataAdapterClass", typeof(IDbDataAdapter), type);
                this.templateDataAdapter = (IDbDataAdapter)type.GetConstructor(Type.EmptyTypes).Invoke(null);
                this.templateDataAdapterIsICloneable = this.templateDataAdapter is ICloneable;
                if (this.templateDataAdapterIsICloneable)
                {
                    try
                    {
                        ((ICloneable)this.templateDataAdapter).Clone();
                    }
                    catch
                    {
                        this.templateDataAdapterIsICloneable = false;
                    }
                }
                type = TypesManager.ResolveType(this.DbConnectionClass, assembly);
                this.AssertPropertyType("DbConnectionClass", typeof(IDbConnection), type);
                this.templateConnection = (IDbConnection)type.GetConstructor(Type.EmptyTypes).Invoke(null);
                this.templateConnectionIsICloneable = this.templateConnection is ICloneable;
                if (this.templateConnectionIsICloneable)
                {
                    try
                    {
                        ((ICloneable)this.templateConnection).Clone();
                    }
                    catch
                    {
                        this.templateConnectionIsICloneable = false;
                    }
                }
                this.commandBuilderType = TypesManager.ResolveType(this.CommandBuilderClass, assembly);
                if (this.ParameterDbTypeClass.IndexOf(',') > 0)
                {
                    this.parameterDbType = TypesManager.ResolveType(this.ParameterDbTypeClass);
                }
                else
                {
                    this.parameterDbType = TypesManager.ResolveType(this.ParameterDbTypeClass, assembly);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("Could not configure providers. Unable to load provider named \"{0}\" not found, failed. Cause: {1}", this.Name, ex.Message), ex);
            }
        }

        /// <summary>
        /// Asserts that the type of the property inherits from the exptected type.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="expectedType">The expected type.</param>
        /// <param name="actualType">The actual type.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.ArgumentException"></exception>
        private void AssertPropertyType(string propertyName, Type expectedType, Type actualType)
        {
            if (actualType == null)
            {
                throw new ArgumentNullException(string.Format("The {0} property cannot be set to a null value.", propertyName), propertyName);
            }
            if (!expectedType.IsAssignableFrom(actualType))
            {
                throw new ArgumentException(string.Format("The Type passed to the {0} property must be an {1} implementation.", propertyName, expectedType.Name), propertyName);
            }
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj != null && obj is DbProviderInfo)
            {
                DbProviderInfo copy = (DbProviderInfo)obj;
                return (this.Name.Equals(copy.Name) && this.AssemblyName.Equals(copy.AssemblyName) && (this.DbConnectionClass == copy.DbConnectionClass));
            }
            return false;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return (this.Name.GetHashCode() ^ this.AssemblyName.GetHashCode() ^ this.DbConnectionClass.GetHashCode());
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("DbProviderInfo {0}", this.Name);
        }

        /// <summary>
        /// Creates a new connection.
        /// </summary>
        /// <returns></returns>
        public IDbConnection CreateConnection()
        {
            return (IDbConnection)(this.templateConnectionIsICloneable ? ((ICloneable)this.templateConnection).Clone() : Activator.CreateInstance(this.templateConnection.GetType()));
        }

        /// <summary>
        /// Creates a new command.
        /// </summary>
        /// <returns></returns>
        public IDbCommand CreateCommand()
        {
            return this.templateConnection.CreateCommand();
        }

        /// <summary>
        /// Creates a new data adapter.
        /// </summary>
        /// <returns></returns>
        public IDbDataAdapter CreateDataAdapter()
        {
            return (IDbDataAdapter)(this.templateDataAdapterIsICloneable ? ((ICloneable)this.templateDataAdapter).Clone() : Activator.CreateInstance(this.templateDataAdapter.GetType()));
        }

        /// <summary>
        /// Creates a new data parameter.
        /// </summary>
        /// <returns></returns>
        public IDbDataParameter CreateDataParameter()
        {
            return this.templateConnection.CreateCommand().CreateParameter();
        }

        /// <summary>
        /// Creates a new command builder.
        /// </summary>
        /// <param name="dataAdapter">The data adapter.</param>
        /// <returns></returns>
        public virtual object CreateCommandBuilder(IDbDataAdapter dataAdapter)
        {
            return dataAdapter == null ? null : this.commandBuilderType.GetConstructor(new Type[] { dataAdapter.GetType() }).Invoke(new object[] { dataAdapter });
        }

        #endregion
    }
}
