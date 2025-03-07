using System;
using System.Data;
using System.Data.Common;
using System.Text;
using C4Net.Framework.Core.Accesor;
using C4Net.Framework.Core.IoC;
using C4Net.Framework.Data.Base;
using C4Net.Framework.Data.Configuration;
using C4Net.Framework.Data.Definitions;

namespace C4Net.Framework.Data.Builders
{
    public class CommandBuilder
    {
        #region - Fields -

        /// <summary>
        /// The connection name.
        /// </summary>
        private string connectionName = string.Empty;

        /// <summary>
        /// The connection manager.
        /// </summary>
        private IDbConnectionManager connectionManager = null;

        /// <summary>
        /// The database connection.
        /// </summary>
        private IDbConnection connection;

        /// <summary>
        /// The provider name for the connection.
        /// </summary>
        private string providerName;

        private DataTableMapping defaultTableMapping = null;

        #endregion

        #region - Properties -

        /// <summary>
        /// Gets the definition of the table.
        /// </summary>
        /// <value>
        /// The definition.
        /// </value>
        public EntityDefinition Definition { get; private set; }

        public IObjectProxy Proxy { get; private set; }

        public DataTableMapping DefaultTableMapping
        {
            get
            {
                if (this.defaultTableMapping == null)
                {
                    this.defaultTableMapping = this.GetDataTableMapping(null);
                }
                return this.defaultTableMapping;
            }
        }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandBuilder"/> class.
        /// </summary>
        /// <param name="connectionName">Name of the connection.</param>
        /// <param name="type">The type.</param>
        public CommandBuilder(string connectionName, Type type, IDbConnection connection)
        {
            this.connectionName = connectionName;
            this.Definition = DefinitionManager.GetDefinition(type);
            this.connectionManager = IoCDefault.Get<IDbConnectionManager>();
            this.connection = connection;
            this.providerName = connectionManager.GetConnectionInfo(this.connectionName).ProviderName;
            this.Proxy = ObjectProxyFactory.GetByType(type);
        }
        
        #endregion

        #region - Methods -

        #region - Aux -
        
        public DataTableMapping GetDataTableMapping(string[] columnNames)
        {
            DataTableMapping result = new DataTableMapping(this.Definition.TableName, this.Definition.Name);
            foreach (AttributeDefinition columnDefinition in this.Definition.Attributes)
            {
                if ((columnNames == null) || (Array.IndexOf(columnNames, columnDefinition.Name) >= 0))
                {
                    result.ColumnMappings.Add(columnDefinition.ColumnName, columnDefinition.Name);
                }
            }
            return result;
        }

        /// <summary>
        /// Adds the parameters from target to source.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="source">The source.</param>
        public static void AddParameters(BaseCommand target, BaseCommand source)
        {
            foreach (BaseParameter parameter in source.Parameters)
            {
                target.Parameters.Add(parameter);
            }
        }

        /// <summary>
        /// Appends a command to another command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="conditionCommand">The condition command.</param>
        /// <returns></returns>
        public BaseCommand AppendCommand(BaseCommand command, BaseCommand conditionCommand)
        {
            command.CommandText = command.CommandText + " " + conditionCommand.CommandText;
            CommandBuilder.AddParameters(command, conditionCommand);
            return command;
        }

        /// <summary>
        /// Converts the base command to IDbCommand for this connection and provider.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        public IDbCommand ConvertCommand(BaseCommand command)
        {
            return CommandConverter.Convert(command, this.connection, this.providerName);
        }

        /// <summary>
        /// Removes characters at the end of a string builder.
        /// </summary>
        /// <param name="stringBuilder">The string builder.</param>
        /// <param name="length">The length.</param>
        private void RemoveEnd(StringBuilder stringBuilder, int length)
        {
            stringBuilder.Remove(stringBuilder.Length - length, length);
        }

        /// <summary>
        /// Gets the full name of the physical table of the actual type.
        /// </summary>
        /// <returns></returns>
        private string GetFullSourceName()
        {
            return this.connectionManager.GetFullSourceName(this.connectionName, this.Definition.TableName);
        }

        /// <summary>
        /// Gets the name of a formatted parameter.
        /// </summary>
        /// <param name="paramNumber">The param number.</param>
        /// <returns></returns>
        private string GetParameterStr(int paramNumber)
        {
            return "@p" + paramNumber.ToString();
        }

        /// <summary>
        /// Builds the condition command for a primary key. If no PK then go for all columns.
        /// </summary>
        /// <param name="mapping">The mapping.</param>
        /// <param name="paramNumber">The param number.</param>
        /// <returns></returns>
        private BaseCommand BuildConditionCommand(DataTableMapping mapping, ref int paramNumber)
        {
            BaseCommand result = new BaseCommand();
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("WHERE ");
            if (this.Definition.PrimaryKeys.Count > 0)
            {
                for (int i = 0; i < this.Definition.PrimaryKeys.Count; i++)
                {
                    string parameterName = this.GetParameterStr(paramNumber++);
                    AttributeDefinition columnDefinition = this.Definition.PrimaryKeys[i];
                    DataColumnMapping columnMapping = mapping.ColumnMappings.GetByDataSetColumn(columnDefinition.Name);
                    result.Parameters.Add(new BaseParameter(parameterName, columnDefinition.DataType, ParameterDirection.Input, columnMapping.DataSetColumn, DataRowVersion.Original, false));
                    stringBuilder.Append(columnDefinition.ColumnName + " = " + parameterName);
                    if (i < this.Definition.PrimaryKeys.Count - 1)
                    {
                        stringBuilder.Append(" AND ");
                    }
                }
            }
            else
            {
                for (int i = 0; i < mapping.ColumnMappings.Count; i++)
                {
                    DataColumnMapping columnMapping = mapping.ColumnMappings[i];
                    string columnName = columnMapping.DataSetColumn;
                    AttributeDefinition columnDefinition = this.Definition.GetAttribute(columnName);
                    string parameterName = this.GetParameterStr(paramNumber++);
                    if (!columnDefinition.IsMandatory)
                    {
                        string isNullParameterName = parameterName;
                        parameterName = this.GetParameterStr(paramNumber++);
                        result.Parameters.Add(new BaseParameter(isNullParameterName, DbType.Int32, columnMapping.DataSetColumn, true));
                        stringBuilder.Append("((" + isNullParameterName + " = 1 AND " + columnDefinition.ColumnName + " IS NULL) OR (" + columnDefinition.ColumnName + " = " + parameterName + "))");
                    }
                    else
                    {
                        stringBuilder.Append("(" + columnDefinition.ColumnName + " = " + parameterName + ")");
                    }
                    result.Parameters.Add(new BaseParameter(parameterName, columnDefinition.DataType, columnMapping.DataSetColumn, false));
                    if (i < mapping.ColumnMappings.Count - 1)
                    {
                        stringBuilder.Append(" AND ");
                    }
                }
            }
            result.CommandText = stringBuilder.ToString();
            return result;
        }

        /// <summary>
        /// Builds the condition command for a primary key. If no PK then go for all columns.
        /// </summary>
        /// <param name="paramNumber">The param number.</param>
        /// <returns></returns>
        private BaseCommand BuildConditionCommand(ref int paramNumber)
        {
            return this.BuildConditionCommand(this.DefaultTableMapping, ref paramNumber);
        }

        #endregion

        #region - Select -

        /// <summary>
        /// Builds the select SQL for the given column names (if null, then use all the columns).
        /// </summary>
        /// <param name="columnNames">The column names.</param>
        /// <param name="mapping">The mapping.</param>
        /// <returns></returns>
        private string BuildSelectSQL(string[] columnNames, out DataTableMapping mapping)
        {
            mapping = new DataTableMapping(this.Definition.Name, this.Definition.TableName);
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("SELECT ");
            foreach (AttributeDefinition columnDefinition in this.Definition.Attributes)
            {
                if ((columnNames == null) || (Array.IndexOf(columnNames, columnDefinition.ColumnName) >= 0))
                {
                    mapping.ColumnMappings.Add(columnDefinition.Name, columnDefinition.ColumnName);
                    stringBuilder.Append(columnDefinition.ColumnName);
                    stringBuilder.Append(", ");
                }
            }
            RemoveEnd(stringBuilder, 2);
            stringBuilder.Append(" FROM " + this.GetFullSourceName());
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Builds the select SQL.
        /// </summary>
        /// <param name="mapping">The mapping.</param>
        /// <returns></returns>
        private string BuildSelectSQL(out DataTableMapping mapping)
        {
            return this.BuildSelectSQL(null, out mapping);
        }

        /// <summary>
        /// Gets the select command.
        /// </summary>
        /// <param name="mapping">The mapping.</param>
        /// <returns></returns>
        private BaseCommand GetSelectCommandBase(out DataTableMapping mapping)
        {
            return new BaseCommand(this.BuildSelectSQL(out mapping));
        }

        /// <summary>
        /// Gets the select command.
        /// </summary>
        /// <param name="mapping">The mapping.</param>
        /// <returns></returns>
        public IDbCommand GetSelectCommand(out DataTableMapping mapping)
        {
            return this.ConvertCommand(this.GetSelectCommandBase(out mapping));
        }

        /// <summary>
        /// Gets the select command.
        /// </summary>
        /// <param name="columnNames">The column names.</param>
        /// <param name="mapping">The mapping.</param>
        /// <returns></returns>
        private BaseCommand GetSelectCommandBase(string[] columnNames, out DataTableMapping mapping)
        {
            return new BaseCommand(this.BuildSelectSQL(columnNames, out mapping));
        }

        /// <summary>
        /// Gets the select command.
        /// </summary>
        /// <param name="columnNames">The column names.</param>
        /// <param name="mapping">The mapping.</param>
        /// <returns></returns>
        public IDbCommand GetSelectCommand(string[] columnNames, out DataTableMapping mapping)
        {
            return this.ConvertCommand(this.GetSelectCommandBase(columnNames, out mapping));
        }

        /// <summary>
        /// Gets the select command.
        /// </summary>
        /// <param name="conditionCommand">The condition command.</param>
        /// <param name="mapping">The mapping.</param>
        /// <returns></returns>
        private BaseCommand GetSelectCommandBase(BaseCommand conditionCommand, out DataTableMapping mapping)
        {
            return this.AppendCommand(new BaseCommand(this.BuildSelectSQL(out mapping)), conditionCommand);
        }

        /// <summary>
        /// Gets the select command.
        /// </summary>
        /// <param name="conditionCommand">The condition command.</param>
        /// <param name="mapping">The mapping.</param>
        /// <returns></returns>
        public IDbCommand GetSelectCommand(BaseCommand conditionCommand, out DataTableMapping mapping)
        {
            return this.ConvertCommand(this.GetSelectCommandBase(conditionCommand, out mapping));
        }

        /// <summary>
        /// Gets the select command.
        /// </summary>
        /// <param name="columnNames">The column names.</param>
        /// <param name="conditionCommand">The condition command.</param>
        /// <param name="mapping">The mapping.</param>
        /// <returns></returns>
        private BaseCommand GetSelectCommandBase(string[] columnNames, BaseCommand conditionCommand, out DataTableMapping mapping)
        {
            return this.AppendCommand(new BaseCommand(this.BuildSelectSQL(columnNames, out mapping)), conditionCommand);
        }

        /// <summary>
        /// Gets the select command.
        /// </summary>
        /// <param name="columnNames">The column names.</param>
        /// <param name="conditionCommand">The condition command.</param>
        /// <param name="mapping">The mapping.</param>
        /// <returns></returns>
        public IDbCommand GetSelectCommand(string[] columnNames, BaseCommand conditionCommand, out DataTableMapping mapping)
        {
            return this.ConvertCommand(this.GetSelectCommandBase(columnNames, conditionCommand, out mapping));
        }

        /// <summary>
        /// Gets the base command for a select with condition for primary key of the object.
        /// </summary>
        /// <param name="mapping">The mapping.</param>
        /// <returns></returns>
        private BaseCommand GetConditionSelectCommandBase(out DataTableMapping mapping)
        {
            int paramNumber = 1;
            BaseCommand result = this.GetSelectCommandBase(out mapping);
            BaseCommand condition = this.BuildConditionCommand(this.DefaultTableMapping, ref paramNumber);
            return this.AppendCommand(result, condition);
        }

        /// <summary>
        /// Gets the command for a select with condition for primary key of the object.
        /// </summary>
        /// <param name="mapping">The mapping.</param>
        /// <returns></returns>
        public IDbCommand GetConditionSelectCommand(out DataTableMapping mapping)
        {
            return this.ConvertCommand(this.GetConditionSelectCommandBase(out mapping));
        }

        /// <summary>
        /// Gets the base command for a select with condition for primary key of the object, getting only selected
        /// columns.
        /// </summary>
        /// <param name="columnNames">The column names.</param>
        /// <param name="mapping">The mapping.</param>
        /// <returns></returns>
        private BaseCommand GetConditionSelectCommandBase(string[] columnNames, out DataTableMapping mapping)
        {
            int paramNumber = 1;
            BaseCommand result = this.GetSelectCommandBase(columnNames, out mapping);
            BaseCommand condition = this.BuildConditionCommand(this.DefaultTableMapping, ref paramNumber);
            return this.AppendCommand(result, condition);
        }

        /// <summary>
        /// Gets the command for a select with condition for primary key of the object, getting only selected
        /// columns.
        /// </summary>
        /// <param name="columnNames">The column names.</param>
        /// <param name="mapping">The mapping.</param>
        /// <returns></returns>
        public IDbCommand GetConditionSelectCommand(string[] columnNames, out DataTableMapping mapping)
        {
            return this.ConvertCommand(this.GetConditionSelectCommandBase(columnNames, out mapping));
        }

        #endregion

        #region - Insert -

        /// <summary>
        /// Gets the insert command.
        /// </summary>
        /// <param name="mapping">The mapping.</param>
        /// <returns></returns>
        private BaseCommand GetInsertCommandBase(DataTableMapping mapping = null)
        {
            if (mapping == null)
            {
                mapping = this.DefaultTableMapping;
            }
            BaseCommand result = new BaseCommand();
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("INSERT INTO " + this.GetFullSourceName());
            StringBuilder columnStringBuilder = new StringBuilder();
            StringBuilder paramStringBuilder = new StringBuilder();
            int paramNumber = 1;
            foreach (DataColumnMapping columnMapping in mapping.ColumnMappings)
            {
                string columnName = columnMapping.DataSetColumn;
                AttributeDefinition columnDefinition = this.Definition.GetAttribute(columnName);
                //if (!columnDefinition.IsAutoIncrement)
                //{
                columnStringBuilder.Append(columnDefinition.ColumnName);
                string parameterName = this.GetParameterStr(paramNumber++);
                paramStringBuilder.Append(parameterName);
                result.Parameters.Add(new BaseParameter(parameterName, columnDefinition.DataType, columnMapping.DataSetColumn, false));
                columnStringBuilder.Append(", ");
                paramStringBuilder.Append(", ");
                //}
            }
            RemoveEnd(columnStringBuilder, 2);
            RemoveEnd(paramStringBuilder, 2);
            stringBuilder.Append("(");
            stringBuilder.Append(columnStringBuilder);
            stringBuilder.Append(") VALUES (");
            stringBuilder.Append(paramStringBuilder);
            stringBuilder.Append(")");
            result.CommandText = stringBuilder.ToString();
            return result;
        }

        /// <summary>
        /// Gets the insert command.
        /// </summary>
        /// <param name="mapping">The mapping.</param>
        /// <returns></returns>
        public IDbCommand GetInsertCommand(DataTableMapping mapping = null)
        {
            return this.ConvertCommand(this.GetInsertCommandBase(mapping));
        }

        #endregion

        #region - Update -

        /// <summary>
        /// Gets the update command.
        /// </summary>
        /// <param name="mapping">The mapping.</param>
        /// <param name="paramNumber">The param number.</param>
        /// <returns></returns>
        private BaseCommand GetUpdateCommandBase(DataTableMapping mapping, ref int paramNumber)
        {
            if (mapping == null)
            {
                mapping = this.DefaultTableMapping;
            }
            BaseCommand result = new BaseCommand();
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("UPDATE ");
            stringBuilder.Append(this.GetFullSourceName());
            stringBuilder.Append(" SET ");
            for (int i = 0; i < mapping.ColumnMappings.Count; i++)
            {
                DataColumnMapping columnMapping = mapping.ColumnMappings[i];
                string columnName = columnMapping.DataSetColumn;
                AttributeDefinition columnDefinition = this.Definition.GetAttribute(columnName);
                string parameterName = this.GetParameterStr(paramNumber++);
                stringBuilder.Append(columnDefinition.ColumnName);
                stringBuilder.Append(" = ");
                stringBuilder.Append(parameterName);
                result.Parameters.Add(new BaseParameter(parameterName, columnDefinition.DataType, columnMapping.DataSetColumn, false));
                if (i < mapping.ColumnMappings.Count - 1)
                {
                    stringBuilder.Append(", ");
                }
            }
            result.CommandText = stringBuilder.ToString();
            return result;
        }

        /// <summary>
        /// Gets the update command.
        /// </summary>
        /// <param name="mapping">The mapping.</param>
        /// <returns></returns>
        private BaseCommand GetUpdateCommandBase(BaseCommand conditionCommand, DataTableMapping mapping = null)
        {
            int paramNumber = 1;
            BaseCommand result = this.GetUpdateCommandBase(mapping, ref paramNumber);
            if (conditionCommand == null)
            {
                conditionCommand = this.BuildConditionCommand(ref paramNumber);
            }
            return this.AppendCommand(result, conditionCommand);
        }

        /// <summary>
        /// Gets the update command.
        /// </summary>
        /// <param name="mapping">The mapping.</param>
        /// <param name="conditionCommand">The condition command.</param>
        /// <returns></returns>
        public IDbCommand GetUpdateCommand(BaseCommand conditionCommand, DataTableMapping mapping = null)
        {
            return this.ConvertCommand(this.GetUpdateCommandBase(conditionCommand, mapping));
        }

        #endregion

        #region - Delete -

        /// <summary>
        /// Gets the delete all base command.
        /// </summary>
        /// <returns></returns>
        private BaseCommand GetDeleteAllCommandBase()
        {
            return new BaseCommand("DELETE FROM " + this.GetFullSourceName());
        }

        /// <summary>
        /// Gets the delete all command.
        /// </summary>
        /// <returns></returns>
        public IDbCommand GetDeleteAllCommand()
        {
            return this.ConvertCommand(this.GetDeleteAllCommandBase());
        }

        /// <summary>
        /// Gets the delete command.
        /// </summary>
        /// <returns></returns>
        private BaseCommand GetDeleteCommandBase(BaseCommand condition)
        {
            if (condition == null)
            {
                int paramNumber = 1;
                condition = this.BuildConditionCommand(ref paramNumber);
            }
            return this.AppendCommand(new BaseCommand("DELETE FROM " + this.GetFullSourceName()), condition);
        }

        /// <summary>
        /// Gets the delete command.
        /// </summary>
        /// <returns></returns>
        public IDbCommand GetDeleteCommand(BaseCommand conditionCommand = null)
        {
            return this.ConvertCommand(this.GetDeleteCommandBase(conditionCommand));
        }

        #endregion

        #region - Aggregates -

        /// <summary>
        /// Gets the count command.
        /// </summary>
        /// <returns></returns>
        private BaseCommand GetCountCommandBase()
        {
            return new BaseCommand("SELECT COUNT(*) FROM " + this.GetFullSourceName());
        }

        /// <summary>
        /// Gets the count command.
        /// </summary>
        /// <returns></returns>
        public IDbCommand GetCountCommand()
        {
            return this.ConvertCommand(this.GetCountCommandBase());
        }

        /// <summary>
        /// Gets the count command.
        /// </summary>
        /// <param name="conditionCommand">The condition command.</param>
        /// <returns></returns>
        private BaseCommand GetCountCommandBase(BaseCommand conditionCommand)
        {
            return this.AppendCommand(this.GetCountCommandBase(), conditionCommand);
        }

        /// <summary>
        /// Gets the count command.
        /// </summary>
        /// <param name="conditionCommand">The condition command.</param>
        /// <returns></returns>
        public IDbCommand GetCountCommand(BaseCommand conditionCommand)
        {
            return this.ConvertCommand(this.GetCountCommandBase(conditionCommand));
        }

        /// <summary>
        /// Gets the sum command.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        private BaseCommand GetSumCommandBase(string columnName)
        {
            return new BaseCommand("SELECT SUM(" + this.Definition.GetAttribute(columnName).ColumnName + ") FROM " + this.GetFullSourceName());
        }

        /// <summary>
        /// Gets the sum command.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        public IDbCommand GetSumCommand(string columnName)
        {
            return this.ConvertCommand(this.GetSumCommandBase(columnName));
        }

        /// <summary>
        /// Gets the sum command.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="conditionCommand">The condition command.</param>
        /// <returns></returns>
        private BaseCommand GetSumCommandBase(string columnName, BaseCommand conditionCommand)
        {
            return this.AppendCommand(this.GetSumCommandBase(columnName), conditionCommand);
        }

        /// <summary>
        /// Gets the sum command.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="conditionCommand">The condition command.</param>
        /// <returns></returns>
        public IDbCommand GetSumCommand(string columnName, BaseCommand conditionCommand)
        {
            return this.ConvertCommand(this.GetSumCommandBase(columnName, conditionCommand));
        }

        /// <summary>
        /// Gets the avg command.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        private BaseCommand GetAvgCommandBase(string columnName)
        {
            return new BaseCommand("SELECT AVG(" + this.Definition.GetAttribute(columnName).ColumnName + ") FROM " + this.GetFullSourceName());
        }

        /// <summary>
        /// Gets the avg command.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        public IDbCommand GetAvgCommand(string columnName)
        {
            return this.ConvertCommand(this.GetAvgCommandBase(columnName));
        }

        /// <summary>
        /// Gets the avg command.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="conditionCommand">The condition command.</param>
        /// <returns></returns>
        private BaseCommand GetAvgCommandBase(string columnName, BaseCommand conditionCommand)
        {
            return this.AppendCommand(this.GetAvgCommandBase(columnName), conditionCommand);
        }

        /// <summary>
        /// Gets the avg command.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="conditionCommand">The condition command.</param>
        /// <returns></returns>
        public IDbCommand GetAvgCommand(string columnName, BaseCommand conditionCommand)
        {
            return this.ConvertCommand(this.GetAvgCommandBase(columnName, conditionCommand));
        }

        /// <summary>
        /// Gets the max command.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        private BaseCommand GetMaxCommandBase(string columnName)
        {
            return new BaseCommand("SELECT MAX(" + this.Definition.GetAttribute(columnName).ColumnName + ") FROM " + this.GetFullSourceName());
        }

        /// <summary>
        /// Gets the max command.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        public IDbCommand GetMaxCommand(string columnName)
        {
            return this.ConvertCommand(this.GetMaxCommandBase(columnName));
        }

        /// <summary>
        /// Gets the max command.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="conditionCommand">The condition command.</param>
        /// <returns></returns>
        private BaseCommand GetMaxCommandBase(string columnName, BaseCommand conditionCommand)
        {
            return this.AppendCommand(this.GetMaxCommandBase(columnName), conditionCommand);
        }

        /// <summary>
        /// Gets the max command.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="conditionCommand">The condition command.</param>
        /// <returns></returns>
        public IDbCommand GetMaxCommand(string columnName, BaseCommand conditionCommand)
        {
            return this.ConvertCommand(this.GetMaxCommandBase(columnName, conditionCommand));
        }

        /// <summary>
        /// Gets the min command.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        private BaseCommand GetMinCommandBase(string columnName)
        {
            return new BaseCommand("SELECT MIN(" + this.Definition.GetAttribute(columnName).ColumnName + ") FROM " + this.GetFullSourceName());
        }

        /// <summary>
        /// Gets the min command.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        public IDbCommand GetMinCommand(string columnName)
        {
            return this.ConvertCommand(this.GetMinCommandBase(columnName));
        }

        /// <summary>
        /// Gets the min command.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="conditionCommand">The condition command.</param>
        /// <returns></returns>
        private BaseCommand GetMinCommandBase(string columnName, BaseCommand conditionCommand)
        {
            return this.AppendCommand(this.GetMinCommandBase(columnName), conditionCommand);
        }

        /// <summary>
        /// Gets the min command.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="conditionCommand">The condition command.</param>
        /// <returns></returns>
        public IDbCommand GetMinCommand(string columnName, BaseCommand conditionCommand)
        {
            return this.ConvertCommand(this.GetMinCommandBase(columnName, conditionCommand));
        }

        #endregion

        #region - Exists

        /// <summary>
        /// Gets the exists command.
        /// </summary>
        /// <returns></returns>
        private BaseCommand GetExistsCommandBase()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("SELECT COUNT(*) FROM ");
            stringBuilder.Append(this.GetFullSourceName());
            int paramNumber = 1;
            return this.AppendCommand(new BaseCommand(stringBuilder.ToString()), this.BuildConditionCommand(ref paramNumber));
        }

        /// <summary>
        /// Gets the exists command.
        /// </summary>
        /// <returns></returns>
        public IDbCommand GetExistsCommand()
        {
            return this.ConvertCommand(this.GetExistsCommandBase());
        }

        #endregion

        #endregion
    }
}
