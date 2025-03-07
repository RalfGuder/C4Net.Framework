using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;
using C4Net.Framework.Core.Accesor;
using C4Net.Framework.Core.Conversions;
using C4Net.Framework.Core.IoC;
using C4Net.Framework.Data.Base;
using C4Net.Framework.Data.Builders;
using C4Net.Framework.Data.Definitions;
using C4Net.Framework.Data.Transactions;
using C4Net.Framework.Expressions;

namespace C4Net.Framework.Data.Adapters
{
    /// <summary>
    /// Data adapter for objects.
    /// </summary>
    public class DataObjectAdapter : DataAdapterBase
    {
        #region - Fields -

        /// <summary>
        /// The command builder used by the adapter.
        /// </summary>
        private CommandBuilder commandBuilder;

        public EntityDefinition Definition
        {
            get { return this.commandBuilder.Definition; }
        }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="DataObjectAdapter"/> class.
        /// </summary>
        /// <param name="connectionName">Name of the connection.</param>
        /// <param name="type">The type.</param>
        /// <param name="transactionManager">The transaction manager.</param>
        public DataObjectAdapter(string connectionName, Type type, ITransactionManager transactionManager)
            : base(transactionManager)
        {
            this.commandBuilder = new CommandBuilder(connectionName, type, transactionManager.Connection);
        }

        #endregion

        #region - Methods -

        #region - Aux methods -

        /// <summary>
        /// Gets the primary key values contained in the instance.
        /// </summary>
        /// <param name="item">The instance.</param>
        /// <returns></returns>
        public object[] GetPrimaryKeys(object item)
        {
            IObjectProxy accesor = this.commandBuilder.Proxy;
            object[] result = new object[this.commandBuilder.Definition.PrimaryKeys.Count];
            for (int i = 0; i < this.commandBuilder.Definition.PrimaryKeys.Count; i++)
            {
                result[i] = accesor.GetValue(item, this.commandBuilder.Definition.PrimaryKeys[i].Name);
            }
            return result;
        }

        /// <summary>
        /// Fills the parameters of a command with the objects in the array.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="values">The values.</param>
        private void FillParameters(IDbCommand command, object[] values)
        {
            for (int i = 0; i < command.Parameters.Count; i++)
            {
                (command.Parameters[i] as IDbDataParameter).Value = values[i];
            }
        }

        /// <summary>
        /// Fills the parameters of a command with the properties of an item.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="item">The item.</param>
        private void FillParameters(IDbCommand command, object item)
        {
            IObjectProxy accesor = this.commandBuilder.Proxy;
            foreach (IDbDataParameter parameter in command.Parameters)
            {
                string columnName = parameter.SourceColumn;
                object value = accesor.GetValue(item, columnName);
                if (value == null)
                {
                    parameter.Value = DBNull.Value;
                }
                else
                {
                    parameter.Value = IoCDefault.Get<IConversionManager>().Convert(accesor.GetPropertyType(columnName), parameter.DbType, value);
                }
            }
        }

        /// <summary>
        /// Gets the column names from a column expression.
        /// </summary>
        /// <param name="columnExpressions">The column expressions.</param>
        /// <returns></returns>
        public string[] GetColumnNames(ColumnExpression[] columnExpressions)
        {
            string[] result = new string[columnExpressions.Length];
            for (int i = 0; i < columnExpressions.Length; i++)
            {
                result[i] = (string)columnExpressions[i];
            }
            return result;
        }

        /// <summary>
        /// Gets the value of a column from the daa reader actual row, and return the value casted to a .net 
        /// type from the database type.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="dbType">Type of the db.</param>
        /// <returns></returns>
        public object GetReaderValue(IDataReader reader, string columnName, DbType dbType)
        {
            int i = reader.GetOrdinal(columnName);
            if (reader.IsDBNull(i))
            {
                return DBNull.Value;
            }
            object value = reader.GetValue(i);
            switch (dbType)
            {
                case DbType.String: return Convert.ToString(value);
                case DbType.Boolean: return Convert.ToBoolean(value);
                case DbType.Int32: return Convert.ToInt32(value);
                case DbType.Decimal: return Convert.ToDecimal(value);
                case DbType.DateTime: return Convert.ToDateTime(value);
                case DbType.Single: return Convert.ToSingle(value);
                case DbType.Double: return Convert.ToDouble(value);
                case DbType.Byte: return Convert.ToByte(value);
                case DbType.Int16: return Convert.ToInt16(value);
                case DbType.Int64: return Convert.ToInt64(value);
                case DbType.Guid:
                    if (value is string)
                    {
                        return new Guid(value as string);
                    }
                    else if (value is byte[])
                    {
                        return new Guid(value as byte[]);
                    }
                    else
                    {
                        return value;
                    }
                default: return value;
            }
        }

        #endregion

        #region - Exists -

        /// <summary>
        /// Returns if the item exists at database using a transaction.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public bool Exists(object item, string transactionID = null)
        {
            object[] primaryKeys = this.GetPrimaryKeys(item);
            return this.Exists(primaryKeys, transactionID);
        }

        /// <summary>
        /// Returns if the item exists at database, located by primary key values, using a transaction.
        /// </summary>
        /// <param name="primaryKeys">The primary keys.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public bool Exists(object[] primaryKeys, string transactionID = null)
        {
            IDbCommand command = this.commandBuilder.GetExistsCommand();
            this.FillParameters(command, primaryKeys);
            this.LogCommand(command);
            return Convert.ToInt32(BaseDAL.ExecuteScalarCommand(command, this.TransactionManager.GetTransaction(transactionID))) > 0;
        }

        /// <summary>
        /// Returns if the item exists at datababase, located by a primary key of one column.
        /// </summary>
        /// <param name="primaryKey">The primary key.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public bool ExistsByPrimaryKey(object primaryKey, string transactionID = null)
        {
            return this.Exists(new object[] { primaryKey }, transactionID);
        }

        #endregion

        #region - Aggregate functions -

        /// <summary>
        /// Gets the count of records of a table by a conditon.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public long GetCount(BaseCommand condition = null, string transactionID = null)
        {
            IDbCommand command = condition == null ? this.commandBuilder.GetCountCommand() : this.commandBuilder.GetCountCommand(condition);
            this.LogCommand(command);
            return Convert.ToInt64(BaseDAL.ExecuteScalarCommand(command, this.TransactionManager.GetTransaction(transactionID)));
        }

        /// <summary>
        /// Gets the sum of a column.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public object GetSum(string columnName, BaseCommand condition = null, string transactionID = null)
        {
            IDbCommand command = condition == null ? this.commandBuilder.GetSumCommand(columnName) : this.commandBuilder.GetSumCommand(columnName, condition);
            this.LogCommand(command);
            return BaseDAL.ExecuteScalarCommand(command, this.TransactionManager.GetTransaction(transactionID));
        }

        /// <summary>
        /// Gets the max value for a column.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public object GetMax(string columnName, BaseCommand condition = null, string transactionID = null)
        {
            IDbCommand command = condition == null ? this.commandBuilder.GetMaxCommand(columnName) : this.commandBuilder.GetMaxCommand(columnName, condition);
            this.LogCommand(command);
            return BaseDAL.ExecuteScalarCommand(command, this.TransactionManager.GetTransaction(transactionID));
        }

        /// <summary>
        /// Gets the min value for a column.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public object GetMin(string columnName, BaseCommand condition = null, string transactionID = null)
        {
            IDbCommand command = condition == null ? this.commandBuilder.GetMinCommand(columnName) : this.commandBuilder.GetMinCommand(columnName, condition);
            this.LogCommand(command);
            return BaseDAL.ExecuteScalarCommand(command, this.TransactionManager.GetTransaction(transactionID));
        }

        /// <summary>
        /// Gets the average value for a column.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public object GetAvg(string columnName, BaseCommand condition = null, string transactionID = null)
        {
            IDbCommand command = condition == null ? this.commandBuilder.GetAvgCommand(columnName) : this.commandBuilder.GetAvgCommand(columnName, condition);
            this.LogCommand(command);
            return BaseDAL.ExecuteScalarCommand(command, this.TransactionManager.GetTransaction(transactionID));
        }

        #endregion

        #region - Insert -

        /// <summary>
        /// Inner insert for a single object.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="mapping">The mapping.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        private int InnerInsert(object item, DataTableMapping mapping, string transactionID = null)
        {
            IDbCommand command = this.commandBuilder.GetInsertCommand(mapping);
            this.FillParameters(command, item);
            this.LogCommand(command);
            return BaseDAL.ExecuteNonQueryCommand(command, this.TransactionManager.GetTransaction(transactionID));
        }

        /// <summary>
        /// Inners insert for a collection of objects.
        /// </summary>
        /// <param name="itemList">The item list.</param>
        /// <param name="mapping">The mapping.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        private int InnerInsertList(ICollection itemList, DataTableMapping mapping, string transactionID = null)
        {
            int result = 0;
            IDbCommand command = this.commandBuilder.GetInsertCommand(mapping);
            IDbTransaction transaction = this.TransactionManager.GetTransaction(transactionID);
            foreach (object item in itemList)
            {
                this.FillParameters(command, item);
                this.LogCommand(command);
                result += BaseDAL.ExecuteNonQueryCommand(command, transaction);
            }
            return result;
        }

        /// <summary>
        /// Inserts the specified item into database.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public int Insert(object item, string transactionID = null)
        {
            return this.InnerInsert(item, null, transactionID);
        }

        /// <summary>
        /// Inserts the specified item into database using only specified columns.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="columnNames">The column names.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public int Insert(object item, string[] columnNames, string transactionID = null)
        {
            return this.InnerInsert(item, this.commandBuilder.GetDataTableMapping(columnNames), transactionID);
        }

        /// <summary>
        /// Inserts the specified item into database using only the columns of the expression.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="columnExpressions">The column expressions.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public int Insert(object item, ColumnExpression[] columnExpressions, string transactionID = null)
        {
            return this.Insert(item, this.GetColumnNames(columnExpressions), transactionID);
        }

        /// <summary>
        /// Inserts the collection of items into database.
        /// </summary>
        /// <param name="itemList">The item list.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public int InsertList(ICollection itemList, string transactionID = null)
        {
            return this.InnerInsertList(itemList, null, transactionID);
        }

        /// <summary>
        /// Inserts the collection of items into database using only the specified columns.
        /// </summary>
        /// <param name="itemList">The item list.</param>
        /// <param name="columnNames">The column names.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public int InsertList(ICollection itemList, string[] columnNames, string transactionID = null)
        {
            return this.InnerInsertList(itemList, this.commandBuilder.GetDataTableMapping(columnNames), transactionID);
        }

        /// <summary>
        /// Inners the collection of items into database
        /// </summary>
        /// <param name="itemList">The item list.</param>
        /// <param name="columnExpressions">The column expressions.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public int InsertList(ICollection itemList, ColumnExpression[] columnExpressions, string transactionID = null)
        {
            return this.InsertList(itemList, this.GetColumnNames(columnExpressions), transactionID);
        }

        #endregion

        #region - Update -

        /// <summary>
        /// Inner update for a single object.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="mapping">The mapping.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        private int InnerUpdate(object item, DataTableMapping mapping, BaseCommand conditionCommand, string transactionID = null)
        {
            IDbCommand command = this.commandBuilder.GetUpdateCommand(conditionCommand, mapping);
            this.FillParameters(command, item);
            this.LogCommand(command);
            return BaseDAL.ExecuteNonQueryCommand(command, this.TransactionManager.GetTransaction(transactionID));
        }

        /// <summary>
        /// Inners update for a collection of objects.
        /// </summary>
        /// <param name="itemList">The item list.</param>
        /// <param name="mapping">The mapping.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        private int InnerUpdateList(ICollection itemList, DataTableMapping mapping, string transactionID = null)
        {
            int result = 0;
            IDbCommand command = this.commandBuilder.GetUpdateCommand(null, mapping);
            IDbTransaction transaction = this.TransactionManager.GetTransaction(transactionID);
            foreach (object item in itemList)
            {
                this.FillParameters(command, item);
                this.LogCommand(command);
                result += BaseDAL.ExecuteNonQueryCommand(command, transaction);
            }
            return result;
        }

        /// <summary>
        /// Updates the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public int Update(object item, string transactionID = null)
        {
            return this.InnerUpdate(item, null, null, transactionID);
        }

        /// <summary>
        /// Updates the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="columnNames">The column names.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public int Update(object item, string[] columnNames, string transactionID = null)
        {
            return this.InnerUpdate(item, this.commandBuilder.GetDataTableMapping(columnNames), null, transactionID);
        }

        /// <summary>
        /// Updates the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="columnExpressions">The column expressions.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public int Update(object item, ColumnExpression[] columnExpressions, string transactionID = null)
        {
            return this.Update(item, this.GetColumnNames(columnExpressions), transactionID);
        }

        /// <summary>
        /// Updates the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public int Update(object item, BaseCommand condition, string transactionID = null)
        {
            return this.InnerUpdate(item, null, condition, transactionID);
        }

        /// <summary>
        /// Updates the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="columnNames">The column names.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public int Update(object item, string[] columnNames, BaseCommand condition, string transactionID = null)
        {
            return this.InnerUpdate(item, this.commandBuilder.GetDataTableMapping(columnNames), condition, transactionID);
        }

        /// <summary>
        /// Updates the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="columnExpressions">The column expressions.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public int Update(object item, ColumnExpression[] columnExpressions, BaseCommand condition, string transactionID = null)
        {
            return this.Update(item, this.GetColumnNames(columnExpressions), condition, transactionID);
        }

        /// <summary>
        /// Updates the specified collection of items.
        /// </summary>
        /// <param name="itemList">The item list.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public int UpdateList(ICollection itemList, string transactionID = null)
        {
            return this.InnerUpdateList(itemList, null, transactionID);
        }

        /// <summary>
        /// Updates the specified collection of items.
        /// </summary>
        /// <param name="itemList">The item list.</param>
        /// <param name="columnNames">The column names.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public int UpdateList(ICollection itemList, string[] columnNames, string transactionID = null)
        {
            return this.InnerUpdateList(itemList, this.commandBuilder.GetDataTableMapping(columnNames), transactionID);
        }

        /// <summary>
        /// Updates the specified collection of items.
        /// </summary>
        /// <param name="itemList">The item list.</param>
        /// <param name="columnExpressions">The column expressions.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public int UpdateList(ICollection itemList, ColumnExpression[] columnExpressions, string transactionID = null)
        {
            return this.UpdateList(itemList, this.GetColumnNames(columnExpressions), transactionID);
        }

        #endregion

        #region - Delete -

        /// <summary>
        /// Deletes the specified item from database table (using its primary key).
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public int Delete(object item, string transactionID = null)
        {
            return this.Delete(this.GetPrimaryKeys(item), transactionID);
        }

        /// <summary>
        /// Deletes a row from database table using the primary key.
        /// </summary>
        /// <param name="primaryKeys">The primary keys.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public int Delete(object[] primaryKeys, string transactionID = null)
        {
            IDbCommand command = this.commandBuilder.GetDeleteCommand();
            this.FillParameters(command, primaryKeys);
            this.LogCommand(command);
            return BaseDAL.ExecuteNonQueryCommand(command, this.TransactionManager.GetTransaction(transactionID));
        }

        /// <summary>
        /// Delete rows from database table using a condition stored in a command.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public int Delete(BaseCommand condition, string transactionID = null)
        {
            IDbCommand command = this.commandBuilder.GetDeleteCommand(condition);
            this.LogCommand(command);
            return BaseDAL.ExecuteNonQueryCommand(command, this.TransactionManager.GetTransaction(transactionID));
        }

        /// <summary>
        /// Deletes from database using a condition expression.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public int Delete(ConditionExpression condition, string transactionID = null)
        {
            BaseCommand command = ConditionCommandBuilder.GetCommand(condition, this.commandBuilder.Definition);
            return this.Delete(command, transactionID);
        }

        /// <summary>
        /// Deletes all the rows from databse table.
        /// </summary>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public int DeleteAll(string transactionID = null)
        {
            IDbCommand command = this.commandBuilder.GetDeleteAllCommand();
            this.LogCommand(command);
            return BaseDAL.ExecuteNonQueryCommand(command, this.TransactionManager.GetTransaction(transactionID));
        }

        #endregion

        #region - Save -

        /// <summary>
        /// Saves the specified item into database (if exists then update else insert).
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public int Save(object item, string transactionID = null)
        {
            return this.Exists(item, transactionID) ? this.Update(item, transactionID) : this.Insert(item, transactionID);
        }

        /// <summary>
        /// Saves the specified item into database (if exists then update else insert).
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="columnNames">The column names.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public int Save(object item, string[] columnNames, string transactionID = null)
        {
            return this.Exists(item, transactionID) ? this.Update(item, columnNames, transactionID) : this.Insert(item, columnNames, transactionID);
        }

        /// <summary>
        /// Saves the specified item into database (if exists then update else insert).
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="columnExpressions">The column expressions.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public int Save(object item, ColumnExpression[] columnExpressions, string transactionID = null)
        {
            return this.Exists(item, transactionID) ? this.Update(item, columnExpressions, transactionID) : this.Insert(item, columnExpressions, transactionID);
        }

        /// <summary>
        /// Saves a list of items into database.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public int SaveList(ICollection items, string transactionID = null)
        {
            int result = 0;
            foreach (object item in items)
            {
                result += this.Save(item, transactionID);
            }
            return result;
        }

        /// <summary>
        /// Saves a list of items into database.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="columnNames">The column names.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public int SaveList(ICollection items, string[] columnNames, string transactionID = null)
        {
            int result = 0;
            foreach (object item in items)
            {
                result += this.Save(item, columnNames, transactionID);
            }
            return result;
        }

        /// <summary>
        /// Saves a list of items into database.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="columnExpressions">The column expressions.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public int SaveList(ICollection items, ColumnExpression[] columnExpressions, string transactionID = null)
        {
            int result = 0;
            foreach (object item in items)
            {
                result += this.Save(item, columnExpressions, transactionID);
            }
            return result;
        }

        #endregion

        #region - Fill -

        /// <summary>
        /// Execute a command and fill the item with the first record retrieved, mapping columns with 
        /// the mapping information.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="command">The command.</param>
        /// <param name="mapping">The mapping.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        private bool FillObject(object item, IDbCommand command, DataTableMapping mapping, string transactionID = null)
        {
            IDataReader reader = BaseDAL.ExecuteReaderCommand(command, this.TransactionManager.GetTransaction(transactionID));
            try
            {
                if (reader.Read())
                {
                    IObjectProxy proxy = this.commandBuilder.Proxy;
                    if (mapping == null)
                    {
                        mapping = this.commandBuilder.DefaultTableMapping;
                    }
                    foreach (DataColumnMapping columnMapping in mapping.ColumnMappings)
                    {
                        string columnName = columnMapping.DataSetColumn;
                        string propertyName = columnMapping.SourceColumn;
                        AttributeDefinition columnDefinition = this.commandBuilder.Definition.GetAttribute(propertyName);
                        object value = this.GetReaderValue(reader, columnName, columnDefinition.DataType);
                        if (value == DBNull.Value)
                        {
                            proxy.SetValue(item, propertyName, null);
                        }
                        else
                        {
                            value = IoCDefault.Get<IConversionManager>().Convert(proxy.GetPropertyType(propertyName), columnDefinition.DataType, value);
                            proxy.SetValue(item, propertyName, value);
                        }
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        /// <summary>
        /// Executes the command and append objects to the list, using the mapping, and paging by start record and 
        /// max records.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="command">The command.</param>
        /// <param name="mapping">The mapping.</param>
        /// <param name="startRecord">The start record.</param>
        /// <param name="maxRecords">The max records.</param>
        /// <param name="transactionID">The transaction ID.</param>
        private void FillList(IList list, IDbCommand command, DataTableMapping mapping, int startRecord, int maxRecords, string transactionID = null)
        {
            IDataReader reader = BaseDAL.ExecuteReaderCommand(command, this.TransactionManager.GetTransaction(transactionID));
            try
            {
                IObjectProxy proxy = this.commandBuilder.Proxy;
                if (mapping == null)
                {
                    mapping = this.commandBuilder.DefaultTableMapping;
                }
                IConversionManager conversions = IoCDefault.Get<IConversionManager>();
                List<string> columnNames = new List<string>();
                List<string> propertyNames = new List<string>();
                List<DbType> dataTypes = new List<DbType>();
                List<Type> propertyTypes = new List<Type>();
                List<ConversionDelegate> conversionDelegates = new List<ConversionDelegate>();
                foreach (DataColumnMapping columnMapping in mapping.ColumnMappings)
                {
                    columnNames.Add(columnMapping.DataSetColumn);
                    propertyNames.Add(columnMapping.SourceColumn);
                    AttributeDefinition columnDefinition = this.commandBuilder.Definition.GetAttribute(columnMapping.SourceColumn);
                    dataTypes.Add(columnDefinition.DataType);
                    propertyTypes.Add(proxy.GetPropertyType(columnMapping.SourceColumn));
                    conversionDelegates.Add(conversions.GetConversion(proxy.GetPropertyType(columnMapping.SourceColumn), columnDefinition.DataType));
                }
                int actualPosition = 0;
                int numRecords = 0;
                while ((reader.Read()) && ((maxRecords == -1) || (numRecords < maxRecords)))
                {
                    if (actualPosition >= startRecord)
                    {
                        object item = proxy.CreateObject();
                        for (int i = 0; i < columnNames.Count; i++)
                        {
                            object value = this.GetReaderValue(reader, columnNames[i], dataTypes[i]);
                            if (value == DBNull.Value)
                            {
                                proxy.SetValue(item, propertyNames[i], null);
                            }
                            else
                            {
                                if (conversionDelegates[i] != null)
                                {
                                    value = conversionDelegates[i](value);
                                }
                                proxy.SetValue(item, propertyNames[i], value);
                            }
                        }
                        list.Add(item);
                        numRecords++;
                    }
                    actualPosition++;
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        /// <summary>
        /// Fills an object selecting its information from the select using its primary key stored in the item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="transactionID">The transaction ID.</param>
        public void Fill(object item, string transactionID = null)
        {
            DataTableMapping mapping;
            IDbCommand command = this.commandBuilder.GetConditionSelectCommand(out mapping);
            this.FillParameters(command, this.GetPrimaryKeys(item));
            this.FillObject(item, command, mapping, transactionID);
        }

        #endregion

        #region - Select -

        /// <summary>
        /// Executes a select using the primary key of the item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public object Select(object item, string transactionID = null)
        {
            return this.Select(this.GetPrimaryKeys(item), transactionID);
        }

        /// <summary>
        /// Executes a select by primary key.
        /// </summary>
        /// <param name="primaryKey">The primary key.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public object Select(object[] primaryKey, string transactionID = null)
        {
            DataTableMapping mapping;
            IDbCommand command = this.commandBuilder.GetConditionSelectCommand(out mapping);
            this.FillParameters(command, primaryKey);
            object item = this.commandBuilder.Proxy.CreateObject();
            if (this.FillObject(item, command, mapping, transactionID))
            {
                return item;
            }
            return null;
        }

        /// <summary>
        /// Executes a select using the primary key of the item and mapping column names.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="columnNames">The column names.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public object Select(object item, string[] columnNames, string transactionID = null)
        {
            return this.Select(this.GetPrimaryKeys(item), columnNames, transactionID);
        }

        /// <summary>
        /// Executes a select using the primary key and mapping column names.
        /// </summary>
        /// <param name="primaryKey">The primary key.</param>
        /// <param name="columnNames">The column names.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public object Select(object[] primaryKey, string[] columnNames, string transactionID = null)
        {
            DataTableMapping mapping;
            IDbCommand command = this.commandBuilder.GetConditionSelectCommand(columnNames, out mapping);
            this.FillParameters(command, primaryKey);
            object item = this.commandBuilder.Proxy.CreateObject();
            if (this.FillObject(item, command, mapping, transactionID))
            {
                return item;
            }
            return null;
        }

        /// <summary>
        /// Executes a select using the primary key of the item and mapping column names by column expression array.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="columnExpressions">The column expressions.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public object Select(object item, ColumnExpression[] columnExpressions, string transactionID = null)
        {
            return this.Select(this.GetPrimaryKeys(item), columnExpressions, transactionID);
        }

        /// <summary>
        /// Executes a select using the primary key and mapping column names by column expression array.
        /// </summary>
        /// <param name="primaryKey">The primary key.</param>
        /// <param name="columnExpressions">The column expressions.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public object Select(object[] primaryKey, ColumnExpression[] columnExpressions, string transactionID = null)
        {
            return this.Select(primaryKey, this.GetColumnNames(columnExpressions), transactionID);
        }

        /// <summary>
        /// Executes a select with a condition specified by a command.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public object Select(BaseCommand condition, string transactionID = null)
        {
            IList list = this.SelectList(condition, 0, 1, transactionID);
            return list.Count > 0 ? list[0] : null;
        }

        /// <summary>
        /// Executes a select with a condition specified by a command and mapping column names.
        /// </summary>
        /// <param name="columnNames">The column names.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public object Select(string[] columnNames, BaseCommand condition, string transactionID = null)
        {
            IList list = this.SelectList(columnNames, condition, 0, 1, transactionID);
            return list.Count > 0 ? list[0] : null;
        }

        /// <summary>
        /// Executes a select with a condition specified by a command and mapping column names of the column expression.
        /// </summary>
        /// <param name="columnExpressions">The column expressions.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public object Select(ColumnExpression[] columnExpressions, BaseCommand condition, string transactionID = null)
        {
            IList list = this.SelectList(columnExpressions, condition, 0, 1, transactionID);
            return list.Count > 0 ? list[0] : null;
        }

        /// <summary>
        /// Executes a select with a condition specified by a condition expression.
        /// </summary>
        /// <param name="conditionExpression">The condition expression.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public object Select(ConditionExpression conditionExpression, string transactionID = null)
        {
            IList list = this.SelectList(conditionExpression, 0, 1, transactionID);
            return list.Count > 0 ? list[0] : null;
        }

        /// <summary>
        /// Executes a select with a condition specified by a condition expression and mapping column names.
        /// </summary>
        /// <param name="columnNames">The column names.</param>
        /// <param name="conditionExpression">The condition expression.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public object Select(string[] columnNames, ConditionExpression conditionExpression, string transactionID = null)
        {
            IList list = this.SelectList(columnNames, conditionExpression, 0, 1, transactionID);
            return list.Count > 0 ? list[0] : null;
        }

        /// <summary>
        /// Executes a select with a condition specified by a condition expression and mapping column names
        /// from a column expression array.
        /// </summary>
        /// <param name="columnExpressions">The column expressions.</param>
        /// <param name="conditionExpression">The condition expression.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public object Select(ColumnExpression[] columnExpressions, ConditionExpression conditionExpression, string transactionID = null)
        {
            IList list = this.SelectList(columnExpressions, conditionExpression, 0, 1, transactionID);
            return list.Count > 0 ? list[0] : null;
        }

        /// <summary>
        /// Executes a query by a one field primary key.
        /// </summary>
        /// <param name="primaryKey">The primary key.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public object SelectByPrimaryKey(object primaryKey, string transactionID = null)
        {
            return this.Select(new object[] { primaryKey }, transactionID);
        }

        /// <summary>
        /// Executes a query by a one field primary key.
        /// </summary>
        /// <param name="primaryKey">The primary key.</param>
        /// <param name="columnNames">The column names.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public object SelectByPrimaryKey(object primaryKey, string[] columnNames, string transactionID = null)
        {
            return this.Select(new object[] { primaryKey }, columnNames, transactionID);
        }

        /// <summary>
        /// Executes a query by a one field primary key.
        /// </summary>
        /// <param name="primaryKey">The primary key.</param>
        /// <param name="columnExpressions">The column expressions.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public object SelectByPrimaryKey(object primaryKey, ColumnExpression[] columnExpressions, string transactionID = null)
        {
            return this.Select(new object[] { primaryKey }, columnExpressions, transactionID);
        }

        #endregion

        #region - Select List -

        /// <summary>
        /// Executes a query to get all elements of the list.
        /// </summary>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public IList SelectList(string transactionID = null)
        {
            return this.SelectList(-1, -1, transactionID);
        }

        /// <summary>
        /// Executes a query to get all elements of the list paginated.
        /// </summary>
        /// <param name="startRecord">The start record.</param>
        /// <param name="maxRecords">The max records.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public IList SelectList(int startRecord, int maxRecords, string transactionID = null)
        {
            DataTableMapping mapping;
            IDbCommand command = this.commandBuilder.GetSelectCommand(out mapping);
            IList result = this.commandBuilder.Proxy.CreateList();
            this.FillList(result, command, mapping, startRecord, maxRecords, transactionID);
            return result;
        }

        /// <summary>
        /// Executes a query to get all elements of the list.
        /// </summary>
        /// <param name="columnNames">The column names.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public IList SelectList(string[] columnNames, string transactionID = null)
        {
            return this.SelectList(columnNames, -1, -1, transactionID);
        }


        /// <summary>
        /// Executes a query to get all elements of the list paginated.
        /// </summary>
        /// <param name="columnNames">The column names.</param>
        /// <param name="startRecord">The start record.</param>
        /// <param name="maxRecords">The max records.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public IList SelectList(string[] columnNames, int startRecord, int maxRecords, string transactionID = null)
        {
            DataTableMapping mapping;
            IDbCommand command = this.commandBuilder.GetSelectCommand(columnNames, out mapping);
            IList result = this.commandBuilder.Proxy.CreateList();
            this.FillList(result, command, mapping, startRecord, maxRecords, transactionID);
            return result;
        }

        /// <summary>
        /// Executes a query to get all elements of the list.
        /// </summary>
        /// <param name="columnExpressions">The column expressions.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public IList SelectList(ColumnExpression[] columnExpressions, string transactionID = null)
        {
            return this.SelectList(columnExpressions, -1, -1, transactionID);
        }

        /// <summary>
        /// Executes a query to get all elements of the list paginated.
        /// </summary>
        /// <param name="columnExpressions">The column expressions.</param>
        /// <param name="startRecord">The start record.</param>
        /// <param name="maxRecords">The max records.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public IList SelectList(ColumnExpression[] columnExpressions, int startRecord, int maxRecords, string transactionID = null)
        {
            return this.SelectList(this.GetColumnNames(columnExpressions), startRecord, maxRecords, transactionID);
        }

        /// <summary>
        /// Executed a query to get elements by a condition.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public IList SelectList(BaseCommand condition, string transactionID = null)
        {
            return this.SelectList(condition, -1, -1, transactionID);
        }

        /// <summary>
        /// Executed a query to get elements by a condition paginated.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <param name="startRecord">The start record.</param>
        /// <param name="maxRecords">The max records.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public IList SelectList(BaseCommand condition, int startRecord, int maxRecords, string transactionID = null)
        {
            DataTableMapping mapping;
            IDbCommand command = this.commandBuilder.GetSelectCommand(condition, out mapping);
            IList result = this.commandBuilder.Proxy.CreateList();
            this.FillList(result, command, mapping, startRecord, maxRecords, transactionID);
            return result;
        }

        /// <summary>
        /// Executed a query to get elements by a condition.
        /// </summary>
        /// <param name="columnNames">The column names.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public IList SelectList(string[] columnNames, BaseCommand condition, string transactionID = null)
        {
            return this.SelectList(columnNames, condition, -1, -1, transactionID);
        }

        /// <summary>
        /// Executed a query to get elements by a condition paginated.
        /// </summary>
        /// <param name="columnNames">The column names.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="startRecord">The start record.</param>
        /// <param name="maxRecords">The max records.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public IList SelectList(string[] columnNames, BaseCommand condition, int startRecord, int maxRecords, string transactionID = null)
        {
            DataTableMapping mapping;
            IDbCommand command = this.commandBuilder.GetSelectCommand(columnNames, condition, out mapping);
            IList result = this.commandBuilder.Proxy.CreateList();
            this.FillList(result, command, mapping, startRecord, maxRecords, transactionID);
            return result;
        }

        /// <summary>
        /// Executed a query to get elements by a condition.
        /// </summary>
        /// <param name="columnExpressions">The column expressions.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public IList SelectList(ColumnExpression[] columnExpressions, BaseCommand condition, string transactionID = null)
        {
            return this.SelectList(columnExpressions, condition, -1, -1, transactionID);
        }

        /// <summary>
        /// Executed a query to get elements by a condition paginated.
        /// </summary>
        /// <param name="columnExpressions">The column expressions.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="startRecord">The start record.</param>
        /// <param name="maxRecords">The max records.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public IList SelectList(ColumnExpression[] columnExpressions, BaseCommand condition, int startRecord, int maxRecords, string transactionID = null)
        {
            return this.SelectList(this.GetColumnNames(columnExpressions), condition, startRecord, maxRecords, transactionID);
        }

        /// <summary>
        /// Executed a query to get elements by a condition.
        /// </summary>
        /// <param name="conditionExpression">The condition expression.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public IList SelectList(ConditionExpression conditionExpression, string transactionID = null)
        {
            return this.SelectList(conditionExpression, -1, -1, transactionID);
        }

        public IList SelectList(Expression condition, string transactionID = null)
        {
            BaseCommand command = ExpressionCommandBuilder.GetCommand(condition, this.Definition);
            return this.SelectList(command, -1, -1, transactionID);
        }

        /// <summary>
        /// Executed a query to get elements by a condition paginated.
        /// </summary>
        /// <param name="conditionExpression">The condition expression.</param>
        /// <param name="startRecord">The start record.</param>
        /// <param name="maxRecords">The max records.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public IList SelectList(ConditionExpression conditionExpression, int startRecord, int maxRecords, string transactionID = null)
        {
            BaseCommand command = ConditionCommandBuilder.GetCommand(conditionExpression, this.commandBuilder.Definition);
            return this.SelectList(command, startRecord, maxRecords, transactionID);
        }

        /// <summary>
        /// Executed a query to get elements by a condition.
        /// </summary>
        /// <param name="columnNames">The column names.</param>
        /// <param name="conditionExpression">The condition expression.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public IList SelectList(string[] columnNames, ConditionExpression conditionExpression, string transactionID = null)
        {
            return this.SelectList(columnNames, conditionExpression, -1, -1, transactionID);
        }

        /// <summary>
        /// Executed a query to get elements by a condition paginated.
        /// </summary>
        /// <param name="columnNames">The column names.</param>
        /// <param name="conditionExpression">The condition expression.</param>
        /// <param name="startRecord">The start record.</param>
        /// <param name="maxRecords">The max records.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public IList SelectList(string[] columnNames, ConditionExpression conditionExpression, int startRecord, int maxRecords, string transactionID = null)
        {
            BaseCommand command = ConditionCommandBuilder.GetCommand(conditionExpression, this.commandBuilder.Definition);
            return this.SelectList(columnNames, command, startRecord, maxRecords, transactionID);
        }

        /// <summary>
        /// Executed a query to get elements by a condition.
        /// </summary>
        /// <param name="columnExpressions">The column expressions.</param>
        /// <param name="conditionExpression">The condition expression.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public IList SelectList(ColumnExpression[] columnExpressions, ConditionExpression conditionExpression, string transactionID = null)
        {
            return this.SelectList(columnExpressions, conditionExpression, -1, -1, transactionID);
        }

        /// <summary>
        /// Executed a query to get elements by a condition paginated.
        /// </summary>
        /// <param name="columnExpressions">The column expressions.</param>
        /// <param name="conditionExpression">The condition expression.</param>
        /// <param name="startRecord">The start record.</param>
        /// <param name="maxRecords">The max records.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public IList SelectList(ColumnExpression[] columnExpressions, ConditionExpression conditionExpression, int startRecord, int maxRecords, string transactionID = null)
        {
            return this.SelectList(this.GetColumnNames(columnExpressions), conditionExpression, startRecord, maxRecords, transactionID);
        }

        #endregion

        #region - GetEnumerator -

        /// <summary>
        /// Executes a query and returns an enumerator for the result items.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="mapping">The mapping.</param>
        /// <param name="startRecord">The start record.</param>
        /// <param name="maxRecords">The max records.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        private IEnumerable GetEnumerator(IDbCommand command, DataTableMapping mapping, int startRecord, int maxRecords, string transactionID = null)
        {
            IDataReader reader = BaseDAL.ExecuteReaderCommand(command, this.TransactionManager.GetTransaction(transactionID));
            try
            {
                IObjectProxy proxy = this.commandBuilder.Proxy;
                if (mapping == null)
                {
                    mapping = this.commandBuilder.DefaultTableMapping;
                }
                IConversionManager conversions = IoCDefault.Get<IConversionManager>();
                List<string> columnNames = new List<string>();
                List<string> propertyNames = new List<string>();
                List<DbType> dataTypes = new List<DbType>();
                List<Type> propertyTypes = new List<Type>();
                List<ConversionDelegate> conversionDelegates = new List<ConversionDelegate>();
                foreach (DataColumnMapping columnMapping in mapping.ColumnMappings)
                {
                    columnNames.Add(columnMapping.DataSetColumn);
                    propertyNames.Add(columnMapping.SourceColumn);
                    AttributeDefinition columnDefinition = this.commandBuilder.Definition.GetAttribute(columnMapping.SourceColumn);
                    dataTypes.Add(columnDefinition.DataType);
                    propertyTypes.Add(proxy.GetPropertyType(columnMapping.SourceColumn));
                    conversionDelegates.Add(conversions.GetConversion(proxy.GetPropertyType(columnMapping.SourceColumn), columnDefinition.DataType));
                }
                int actualPosition = 0;
                int numRecords = 0;
                while ((reader.Read()) && ((maxRecords == -1) || (numRecords < maxRecords)))
                {
                    if (actualPosition >= startRecord)
                    {
                        object item = proxy.CreateObject();
                        for (int i = 0; i < columnNames.Count; i++)
                        {
                            object value = this.GetReaderValue(reader, columnNames[i], dataTypes[i]);
                            if (value == DBNull.Value)
                            {
                                proxy.SetValue(item, propertyNames[i], null);
                            }
                            else
                            {
                                if (conversionDelegates[i] != null)
                                {
                                    value = conversionDelegates[i](value);
                                }
                                proxy.SetValue(item, propertyNames[i], value);
                            }
                        }
                        yield return item;
                        numRecords++;
                    }
                    actualPosition++;
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        /// <summary>
        /// Executes a query and returns an enumerator for the result items.
        /// </summary>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public IEnumerable GetEnumerator(string transactionID = null)
        {
            return this.GetEnumerator(-1, -1, transactionID);
        }

        public IEnumerable<T> GetEnumerator<T>(string transactionID = null)
        {
            foreach (object item in this.GetEnumerator(transactionID))
            {
                yield return (T)item;
            }
        }


        /// <summary>
        /// Executes a query and returns an enumerator for the result items.
        /// </summary>
        /// <param name="startRecord">The start record.</param>
        /// <param name="maxRecords">The max records.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public IEnumerable GetEnumerator(int startRecord, int maxRecords, string transactionID = null)
        {
            DataTableMapping mapping;
            IDbCommand command = this.commandBuilder.GetSelectCommand(out mapping);
            return this.GetEnumerator(command, mapping, startRecord, maxRecords, transactionID);
        }

        /// <summary>
        /// Executes a query and returns an enumerator for the result items.
        /// </summary>
        /// <param name="columnNames">The column names.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public IEnumerable GetEnumerator(string[] columnNames, string transactionID = null)
        {
            return this.GetEnumerator(columnNames, -1, -1, transactionID);
        }

        /// <summary>
        /// Executes a query and returns an enumerator for the result items.
        /// </summary>
        /// <param name="columnNames">The column names.</param>
        /// <param name="startRecord">The start record.</param>
        /// <param name="maxRecords">The max records.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public IEnumerable GetEnumerator(string[] columnNames, int startRecord, int maxRecords, string transactionID = null)
        {
            DataTableMapping mapping;
            IDbCommand command = this.commandBuilder.GetSelectCommand(columnNames, out mapping);
            return this.GetEnumerator(command, mapping, startRecord, maxRecords, transactionID);
        }

        /// <summary>
        /// Executes a query and returns an enumerator for the result items.
        /// </summary>
        /// <param name="columnNames">The column names.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public IEnumerable GetEnumerator(string[] columnNames, BaseCommand condition, string transactionID = null)
        {
            return this.GetEnumerator(columnNames, condition, -1, -1, transactionID);
        }

        /// <summary>
        /// Executes a query and returns an enumerator for the result items.
        /// </summary>
        /// <param name="columnNames">The column names.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="startRecord">The start record.</param>
        /// <param name="maxRecords">The max records.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public IEnumerable GetEnumerator(string[] columnNames, BaseCommand condition, int startRecord, int maxRecords, string transactionID = null)
        {
            DataTableMapping mapping;
            IDbCommand command = this.commandBuilder.GetSelectCommand(columnNames, condition, out mapping);
            return this.GetEnumerator(command, mapping, startRecord, maxRecords, transactionID);
        }

        /// <summary>
        /// Executes a query and returns an enumerator for the result items.
        /// </summary>
        /// <param name="columnNames">The column names.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public IEnumerable GetEnumerator(string[] columnNames, ConditionExpression condition, string transactionID = null)
        {
            return this.GetEnumerator(columnNames, condition, -1, -1, transactionID);
        }

        /// <summary>
        /// Executes a query and returns an enumerator for the result items.
        /// </summary>
        /// <param name="columnNames">The column names.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="startRecord">The start record.</param>
        /// <param name="maxRecords">The max records.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public IEnumerable GetEnumerator(string[] columnNames, ConditionExpression condition, int startRecord, int maxRecords, string transactionID = null)
        {
            BaseCommand conditionCommand = ConditionCommandBuilder.GetCommand(condition, this.commandBuilder.Definition);
            return this.GetEnumerator(columnNames, conditionCommand, startRecord, maxRecords, transactionID);
        }

        /// <summary>
        /// Executes a query and returns an enumerator for the result items.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public IEnumerable GetEnumerator(BaseCommand condition, string transactionID = null)
        {
            return this.GetEnumerator(condition, -1, -1, transactionID);
        }

        public IEnumerable<T> GetEnumerator<T>(BaseCommand condition, string transactionID = null)
        {
            foreach (object item in this.GetEnumerator(condition, transactionID))
            {
                yield return (T)item;
            }
        }

        /// <summary>
        /// Executes a query and returns an enumerator for the result items.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <param name="startRecord">The start record.</param>
        /// <param name="maxRecords">The max records.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public IEnumerable GetEnumerator(BaseCommand condition, int startRecord, int maxRecords, string transactionID = null)
        {
            DataTableMapping mapping;
            IDbCommand command = this.commandBuilder.GetSelectCommand(condition, out mapping);
            return this.GetEnumerator(command, mapping, startRecord, maxRecords, transactionID);
        }

        /// <summary>
        /// Executes a query and returns an enumerator for the result items.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public IEnumerable GetEnumerator(ConditionExpression condition, string transactionID = null)
        {
            return this.GetEnumerator(condition, -1, -1, transactionID);
        }

        /// <summary>
        /// Executes a query and returns an enumerator for the result items.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <param name="startRecord">The start record.</param>
        /// <param name="maxRecords">The max records.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public IEnumerable GetEnumerator(ConditionExpression condition, int startRecord, int maxRecords, string transactionID = null)
        {
            BaseCommand conditionCommand = ConditionCommandBuilder.GetCommand(condition, this.commandBuilder.Definition);
            return this.GetEnumerator(conditionCommand, startRecord, maxRecords, transactionID);
        }

        /// <summary>
        /// Executes a query and returns an enumerator for the result items.
        /// </summary>
        /// <param name="columns">The columns.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public IEnumerable GetEnumerator(ColumnExpression[] columns, string transactionID = null)
        {
            return this.GetEnumerator(this.GetColumnNames(columns), transactionID);
        }

        /// <summary>
        /// Executes a query and returns an enumerator for the result items.
        /// </summary>
        /// <param name="columns">The columns.</param>
        /// <param name="startRecord">The start record.</param>
        /// <param name="maxRecords">The max records.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public IEnumerable GetEnumerator(ColumnExpression[] columns, int startRecord, int maxRecords, string transactionID = null)
        {
            return this.GetEnumerator(this.GetColumnNames(columns), startRecord, maxRecords, transactionID);
        }

        /// <summary>
        /// Executes a query and returns an enumerator for the result items.
        /// </summary>
        /// <param name="columns">The columns.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public IEnumerable GetEnumerator(ColumnExpression[] columns, BaseCommand condition, string transactionID = null)
        {
            return this.GetEnumerator(this.GetColumnNames(columns), condition, transactionID);
        }

        /// <summary>
        /// Executes a query and returns an enumerator for the result items.
        /// </summary>
        /// <param name="columns">The columns.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="startRecord">The start record.</param>
        /// <param name="maxRecords">The max records.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public IEnumerable GetEnumerator(ColumnExpression[] columns, BaseCommand condition, int startRecord, int maxRecords, string transactionID = null)
        {
            return this.GetEnumerator(this.GetColumnNames(columns), condition, startRecord, maxRecords, transactionID);
        }

        /// <summary>
        /// Executes a query and returns an enumerator for the result items.
        /// </summary>
        /// <param name="columns">The columns.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public IEnumerable GetEnumerator(ColumnExpression[] columns, ConditionExpression condition, string transactionID = null)
        {
            return this.GetEnumerator(this.GetColumnNames(columns), condition, transactionID);
        }

        /// <summary>
        /// Executes a query and returns an enumerator for the result items.
        /// </summary>
        /// <param name="columns">The columns.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="startRecord">The start record.</param>
        /// <param name="maxRecords">The max records.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public IEnumerable GetEnumerator(ColumnExpression[] columns, ConditionExpression condition, int startRecord, int maxRecords, string transactionID = null)
        {
            return this.GetEnumerator(this.GetColumnNames(columns), condition, startRecord, maxRecords, transactionID);
        }

        #endregion

        #region - Base -

        /// <summary>
        /// Executes a query and return the number of affected rows.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public int ExecuteNonQueryCommand(BaseCommand command, string transactionID = null)
        {
            IDbCommand dbCommand = this.commandBuilder.ConvertCommand(command);
            this.LogCommand(dbCommand);
            int result = BaseDAL.ExecuteNonQueryCommand(dbCommand, this.TransactionManager.GetTransaction(transactionID));
            CommandConverter.FeedbackParameters(command, dbCommand);
            return result;
        }

        /// <summary>
        /// Executes a query and returns the scalar value.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public object ExecuteScalarCommand(BaseCommand command, string transactionID = null)
        {
            IDbCommand dbCommand = this.commandBuilder.ConvertCommand(command);
            this.LogCommand(dbCommand);
            object result = BaseDAL.ExecuteScalarCommand(dbCommand, this.TransactionManager.GetTransaction(transactionID));
            CommandConverter.FeedbackParameters(command, dbCommand);
            return result;
        }

        /// <summary>
        /// Executes a query and returns the data reader interface.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public IDataReader ExecuteReaderCommand(BaseCommand command, string transactionID = null)
        {
            IDbCommand dbCommand = this.commandBuilder.ConvertCommand(command);
            this.LogCommand(dbCommand);
            IDataReader result = BaseDAL.ExecuteReaderCommand(dbCommand, this.TransactionManager.GetTransaction(transactionID));
            CommandConverter.FeedbackParameters(command, dbCommand);
            return result;
        }

        #endregion

        #endregion
    }
}
