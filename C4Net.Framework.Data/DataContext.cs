using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using C4Net.Framework.Data.Adapters;
using C4Net.Framework.Data.Base;
using C4Net.Framework.Expressions;

namespace C4Net.Framework.Data
{
    /// <summary>
    /// Class for the master adapter, that work as a hub for adapters.
    /// </summary>
    public class DataContext : DataAdapterBase, IDataContext
    {
        #region - Fields -

        /// <summary>
        /// The connection name.
        /// </summary>
        private string connectionName;

        /// <summary>
        /// The object adapters dictionary.
        /// </summary>
        private Dictionary<Type, DataObjectAdapter> objectAdapters = new Dictionary<Type, DataObjectAdapter>();

        #endregion

        #region - Properties -

        public string ConnectionName
        {
            get { return this.connectionName; }
        }

        #endregion

        #region - Constructor -

        /// <summary>
        /// Initializes a new instance of the <see cref="DataContext"/> class.
        /// </summary>
        /// <param name="connectionName">Name of the connection.</param>
        public DataContext(string connectionName)
            : base(connectionName)
        {
            this.connectionName = connectionName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataContext"/> class.
        /// </summary>
        public DataContext()
            : base()
        {
            this.connectionName = string.Empty;
        }

        #endregion

        #region - Methods -

        #region - Aux Methods -

        /// <summary>
        /// Gets the data object adapter.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        private DataObjectAdapter GetDataObjectAdapter<T>()
        {
            if (this.objectAdapters.ContainsKey(typeof(T)))
            {
                return this.objectAdapters[typeof(T)];
            }
            DataObjectAdapter adapter = new DataObjectAdapter(this.connectionName, typeof(T), this.TransactionManager);
            this.objectAdapters.Add(typeof(T), adapter);
            return adapter;
        }

        public DataObjectAdapter GetDataObjectAdapter(Type type)
        {
            if (this.objectAdapters.ContainsKey(type))
            {
                return this.objectAdapters[type];
            }
            DataObjectAdapter adapter = new DataObjectAdapter(this.connectionName, type, this.TransactionManager);
            this.objectAdapters.Add(type, adapter);
            return adapter;
        }

        public object[] GetPrimaryKeys<T>(T item)
        {
            return this.GetDataObjectAdapter<T>().GetPrimaryKeys(item);
        }

        public string GetKeyStr(object[] keys)
        {
            StringBuilder result = new StringBuilder();
            foreach (object obj in keys)
            {
                result.Append(obj.ToString());
                result.Append("|");
            }
            return result.ToString();
        }

        public string GetOneKeyStr(object key)
        {
            return this.GetKeyStr(new object[] { key });
        }

        public string GetPrimaryKeyStr<T>(T item)
        {
            return this.GetKeyStr(this.GetPrimaryKeys<T>(item));
        }

        #endregion

        #region - Exists -

        /// <summary>
        /// Returns if the item exists at database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item">The item.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public bool Exists<T>(object item, string transactionID = null)
        {
            return this.GetDataObjectAdapter<T>().Exists(item, transactionID);
        }

        /// <summary>
        /// Returns if the item exists at database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="primaryKeys">The primary keys.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public bool Exists<T>(object[] primaryKeys, string transactionID = null)
        {
            return this.GetDataObjectAdapter<T>().Exists(primaryKeys, transactionID);
        }

        #endregion

        #region - Aggregate functions -

        /// <summary>
        /// Execute the aggregate function COUNT.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public long GetCount<T>(string transactionID = null)
        {
            return this.GetDataObjectAdapter<T>().GetCount(null, transactionID);
        }

        /// <summary>
        /// Execute the aggregate function COUNT.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command">The command.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public long GetCount<T>(BaseCommand command, string transactionID = null)
        {
            return this.GetDataObjectAdapter<T>().GetCount(command, transactionID);
        }

        /// <summary>
        /// Execute the aggregate function SUM.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public object GetSum<T>(string columnName, string transactionID = null)
        {
            return this.GetDataObjectAdapter<T>().GetSum(columnName, null, transactionID);
        }

        /// <summary>
        /// Execute the aggregate function SUM.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="command">The command.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public object GetSum<T>(string columnName, BaseCommand command, string transactionID = null)
        {
            return this.GetDataObjectAdapter<T>().GetSum(columnName, command, transactionID);
        }

        /// <summary>
        /// Execute the aggregate function MAX.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public object GetMax<T>(string columnName, string transactionID = null)
        {
            return this.GetDataObjectAdapter<T>().GetMax(columnName, null, transactionID);
        }

        /// <summary>
        /// Execute the aggregate function MAX.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public object GetMax<T>(string columnName, BaseCommand condition, string transactionID = null)
        {
            return this.GetDataObjectAdapter<T>().GetMax(columnName, condition, transactionID);
        }

        /// <summary>
        /// Execute the aggregate function MIN.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public object GetMin<T>(string columnName, string transactionID = null)
        {
            return this.GetDataObjectAdapter<T>().GetMin(columnName, null, transactionID);
        }

        /// <summary>
        /// Execute the aggregate function MIN.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public object GetMin<T>(string columnName, BaseCommand condition, string transactionID = null)
        {
            return this.GetDataObjectAdapter<T>().GetMin(columnName, condition, transactionID);
        }

        /// <summary>
        /// Execute the aggregate function AVG.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public object GetAvg<T>(string columnName, string transactionID = null)
        {
            return this.GetDataObjectAdapter<T>().GetAvg(columnName, null, transactionID);
        }

        /// <summary>
        /// Execute the aggregate function AVG.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public object GetAvg<T>(string columnName, BaseCommand condition, string transactionID = null)
        {
            return this.GetDataObjectAdapter<T>().GetAvg(columnName, condition, transactionID);
        }

        #endregion

        #region - Insert -

        /// <summary>
        /// Inserts the item into database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item">The item.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public int Insert<T>(object item, string transactionID = null)
        {
            return this.GetDataObjectAdapter<T>().Insert(item, transactionID);
        }

        /// <summary>
        /// Inserts the item into database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item">The item.</param>
        /// <param name="columnNames">The column names.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public int Insert<T>(object item, string[] columnNames, string transactionID = null)
        {
            return this.GetDataObjectAdapter<T>().Insert(item, columnNames, transactionID);
        }

        /// <summary>
        /// Inserts the item into database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item">The item.</param>
        /// <param name="columnExpressions">The column expressions.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public int Insert<T>(object item, ColumnExpression[] columnExpressions, string transactionID = null)
        {
            return this.GetDataObjectAdapter<T>().Insert(item, columnExpressions, transactionID);
        }

        /// <summary>
        /// Inserts a list of items into database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">The items.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public int InsertList<T>(ICollection items, string transactionID = null)
        {
            return this.GetDataObjectAdapter<T>().InsertList(items, transactionID);
        }

        /// <summary>
        /// Inserts a list of items into database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">The items.</param>
        /// <param name="columnNames">The column names.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public int InsertList<T>(ICollection items, string[] columnNames, string transactionID = null)
        {
            return this.GetDataObjectAdapter<T>().InsertList(items, columnNames, transactionID);
        }

        /// <summary>
        /// Inserts a list of items into database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">The items.</param>
        /// <param name="columnExpressions">The column expressions.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public int InsertList<T>(ICollection items, ColumnExpression[] columnExpressions, string transactionID = null)
        {
            return this.GetDataObjectAdapter<T>().InsertList(items, columnExpressions, transactionID);
        }

        #endregion

        #region - Update -

        /// <summary>
        /// Updates the item at database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item">The item.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public int Update<T>(object item, string transactionID = null)
        {
            return this.GetDataObjectAdapter<T>().Update(item, transactionID);
        }

        /// <summary>
        /// Updates the item at database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item">The item.</param>
        /// <param name="columnNames">The column names.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public int Update<T>(object item, string[] columnNames, string transactionID = null)
        {
            return this.GetDataObjectAdapter<T>().Update(item, columnNames, transactionID);
        }

        /// <summary>
        /// Updates the item at database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item">The item.</param>
        /// <param name="columnExpressions">The column expressions.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public int Update<T>(object item, ColumnExpression[] columnExpressions, string transactionID = null)
        {
            return this.GetDataObjectAdapter<T>().Update(item, columnExpressions, transactionID);
        }

        /// <summary>
        /// Updates the item at database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item">The item.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public int Update<T>(object item, BaseCommand condition, string transactionID = null)
        {
            return this.GetDataObjectAdapter<T>().Update(item, condition, transactionID);
        }

        /// <summary>
        /// Updates the item at database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item">The item.</param>
        /// <param name="columnNames">The column names.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public int Update<T>(object item, string[] columnNames, BaseCommand condition, string transactionID = null)
        {
            return this.GetDataObjectAdapter<T>().Update(item, columnNames, condition, transactionID);
        }

        /// <summary>
        /// Updates the item at database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item">The item.</param>
        /// <param name="columnExpressions">The column expressions.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public int Update<T>(object item, ColumnExpression[] columnExpressions, BaseCommand condition, string transactionID = null)
        {
            return this.GetDataObjectAdapter<T>().Update(item, columnExpressions, condition, transactionID);
        }

        /// <summary>
        /// Updates a list of items into database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">The items.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public int UpdateList<T>(ICollection items, string transactionID = null)
        {
            return this.GetDataObjectAdapter<T>().UpdateList(items, transactionID);
        }

        /// <summary>
        /// Updates a list of items into database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">The items.</param>
        /// <param name="columnNames">The column names.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public int UpdateList<T>(ICollection items, string[] columnNames, string transactionID = null)
        {
            return this.GetDataObjectAdapter<T>().UpdateList(items, columnNames, transactionID);
        }

        /// <summary>
        /// Updates a list of items into database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">The items.</param>
        /// <param name="columnExpressions">The column expressions.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public int UpdateList<T>(ICollection items, ColumnExpression[] columnExpressions, string transactionID = null)
        {
            return this.GetDataObjectAdapter<T>().UpdateList(items, columnExpressions, transactionID);
        }

        #endregion

        #region - Delete -

        /// <summary>
        /// Deletes the item from database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item">The item.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public int Delete<T>(object item, string transactionID = null)
        {
            return this.GetDataObjectAdapter<T>().Delete(item, transactionID);
        }

        /// <summary>
        /// Deletes the item from database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="primaryKeys">The primary keys.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public int Delete<T>(object[] primaryKeys, string transactionID = null)
        {
            return this.GetDataObjectAdapter<T>().Delete(primaryKeys, transactionID);
        }

        /// <summary>
        /// Delete items from database specified by a condition.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="condition">The condition.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public int Delete<T>(BaseCommand condition, string transactionID = null)
        {
            return this.GetDataObjectAdapter<T>().Delete(condition, transactionID);
        }

        public int Delete<T>(ConditionExpression condition, string transactionID = null)
        {
            return this.GetDataObjectAdapter<T>().Delete(condition, transactionID);
        }

        /// <summary>
        /// Deletes all records from the table.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public int DeleteAll<T>(string transactionID = null)
        {
            return this.GetDataObjectAdapter<T>().DeleteAll(transactionID);
        }

        #endregion

        #region - Save -

        /// <summary>
        /// Saves an item into database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item">The item.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public int Save<T>(object item, string transactionID = null)
        {
            return this.GetDataObjectAdapter<T>().Save(item, transactionID);
        }

        /// <summary>
        /// Saves an item into database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item">The item.</param>
        /// <param name="columnNames">The column names.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public int Save<T>(object item, string[] columnNames, string transactionID = null)
        {
            return this.GetDataObjectAdapter<T>().Save(item, columnNames, transactionID);
        }

        /// <summary>
        /// Saves an item into database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item">The item.</param>
        /// <param name="columnExpressions">The column expressions.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public int Save<T>(object item, ColumnExpression[] columnExpressions, string transactionID = null)
        {
            return this.GetDataObjectAdapter<T>().Save(item, columnExpressions, transactionID);
        }

        /// <summary>
        /// Saves a list of items into database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">The items.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public int SaveList<T>(ICollection items, string transactionID = null)
        {
            return this.GetDataObjectAdapter<T>().SaveList(items, transactionID);
        }

        /// <summary>
        /// Saves a list of items into database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">The items.</param>
        /// <param name="columnNames">The column names.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public int SaveList<T>(ICollection items, string[] columnNames, string transactionID = null)
        {
            return this.GetDataObjectAdapter<T>().SaveList(items, columnNames, transactionID);
        }

        /// <summary>
        /// Saves a list of items into database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">The items.</param>
        /// <param name="columnExpressions">The column expressions.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public int SaveList<T>(ICollection items, ColumnExpression[] columnExpressions, string transactionID = null)
        {
            return this.GetDataObjectAdapter<T>().SaveList(items, columnExpressions, transactionID);
        }

        #endregion

        #region - Select -

        /// <summary>
        /// Selects an item from database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item">The item.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public T Select<T>(object item, string transactionID = null)
        {
            return (T)this.GetDataObjectAdapter<T>().Select(item, transactionID);
        }

        /// <summary>
        /// Selects an item from database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="primaryKey">The primary key.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public T Select<T>(object[] primaryKey, string transactionID = null)
        {
            return (T)this.GetDataObjectAdapter<T>().Select(primaryKey, transactionID);
        }

        /// <summary>
        /// Selects an item from database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item">The item.</param>
        /// <param name="columnNames">The column names.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public T Select<T>(object item, string[] columnNames, string transactionID = null)
        {
            return (T)this.GetDataObjectAdapter<T>().Select(item, columnNames, transactionID);
        }

        /// <summary>
        /// Selects an item from database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="primaryKey">The primary key.</param>
        /// <param name="columnNames">The column names.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public T Select<T>(object[] primaryKey, string[] columnNames, string transactionID = null)
        {
            return (T)this.GetDataObjectAdapter<T>().Select(primaryKey, columnNames, transactionID);
        }

        /// <summary>
        /// Selects an item from database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item">The item.</param>
        /// <param name="columnExpressions">The column expressions.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public T Select<T>(object item, ColumnExpression[] columnExpressions, string transactionID = null)
        {
            return (T)this.GetDataObjectAdapter<T>().Select(item, columnExpressions, transactionID);
        }

        /// <summary>
        /// Selects an item from database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="primaryKey">The primary key.</param>
        /// <param name="columnExpressions">The column expressions.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public T Select<T>(object[] primaryKey, ColumnExpression[] columnExpressions, string transactionID = null)
        {
            return (T)this.GetDataObjectAdapter<T>().Select(primaryKey, columnExpressions, transactionID);
        }

        /// <summary>
        /// Selects an item from database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="condition">The condition.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public T Select<T>(BaseCommand condition, string transactionID = null)
        {
            return (T)this.GetDataObjectAdapter<T>().Select(condition, transactionID);
        }

        /// <summary>
        /// Selects an item from database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="columnNames">The column names.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public T Select<T>(string[] columnNames, BaseCommand condition, string transactionID = null)
        {
            return (T)this.GetDataObjectAdapter<T>().Select(columnNames, condition, transactionID);
        }

        /// <summary>
        /// Selects an item from database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="columnExpressions">The column expressions.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public T Select<T>(ColumnExpression[] columnExpressions, BaseCommand condition, string transactionID = null)
        {
            return (T)this.GetDataObjectAdapter<T>().Select(columnExpressions, condition, transactionID);
        }

        /// <summary>
        /// Selects an item from database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conditionExpression">The condition expression.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public T Select<T>(ConditionExpression conditionExpression, string transactionID = null)
        {
            return (T)this.GetDataObjectAdapter<T>().Select(conditionExpression, transactionID);
        }

        /// <summary>
        /// Selects an item from database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="columnNames">The column names.</param>
        /// <param name="conditionExpression">The condition expression.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public T Select<T>(string[] columnNames, ConditionExpression conditionExpression, string transactionID = null)
        {
            return (T)this.GetDataObjectAdapter<T>().Select(columnNames, conditionExpression, transactionID);
        }

        /// <summary>
        /// Selects an item from database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="columnExpressions">The column expressions.</param>
        /// <param name="conditionExpression">The condition expression.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public T Select<T>(ColumnExpression[] columnExpressions, ConditionExpression conditionExpression, string transactionID = null)
        {
            return (T)this.GetDataObjectAdapter<T>().Select(columnExpressions, conditionExpression, transactionID);
        }

        /// <summary>
        /// Selects an item from database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="primaryKey">The primary key.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public T SelectByPrimaryKey<T>(object primaryKey, string transactionID = null)
        {
            return (T)this.GetDataObjectAdapter<T>().SelectByPrimaryKey(primaryKey, transactionID);
        }

        /// <summary>
        /// Selects an item from database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="primaryKey">The primary key.</param>
        /// <param name="columnNames">The column names.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public T SelectByPrimaryKey<T>(object primaryKey, string[] columnNames, string transactionID = null)
        {
            return (T)this.GetDataObjectAdapter<T>().SelectByPrimaryKey(primaryKey, columnNames, transactionID);
        }

        /// <summary>
        /// Selects an item from database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="primaryKey">The primary key.</param>
        /// <param name="columnExpressions">The column expressions.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public T SelectByPrimaryKey<T>(object primaryKey, ColumnExpression[] columnExpressions, string transactionID = null)
        {
            return (T)this.GetDataObjectAdapter<T>().SelectByPrimaryKey(primaryKey, columnExpressions, transactionID);
        }

        #endregion

        #region - Select list -

        /// <summary>
        /// Selects a list of objects from database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public IList<T> SelectList<T>(string transactionID = null)
        {
            return (IList<T>)this.GetDataObjectAdapter<T>().SelectList(transactionID);
        }

        /// <summary>
        /// Selects a list of objects from database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="startRecord">The start record.</param>
        /// <param name="maxRecords">The max records.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public IList<T> SelectList<T>(int startRecord, int maxRecords, string transactionID = null)
        {
            return (IList<T>)this.GetDataObjectAdapter<T>().SelectList(startRecord, maxRecords, transactionID);
        }

        /// <summary>
        /// Selects a list of objects from database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="columnNames">The column names.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public IList<T> SelectList<T>(string[] columnNames, string transactionID = null)
        {
            return (IList<T>)this.GetDataObjectAdapter<T>().SelectList(columnNames, transactionID);
        }

        /// <summary>
        /// Selects a list of objects from database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="columnNames">The column names.</param>
        /// <param name="startRecord">The start record.</param>
        /// <param name="maxRecords">The max records.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public IList<T> SelectList<T>(string[] columnNames, int startRecord, int maxRecords, string transactionID = null)
        {
            return (IList<T>)this.GetDataObjectAdapter<T>().SelectList(columnNames, startRecord, maxRecords, transactionID);
        }

        /// <summary>
        /// Selects a list of objects from database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="columnExpressions">The column expressions.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public IList<T> SelectList<T>(ColumnExpression[] columnExpressions, string transactionID = null)
        {
            return (IList<T>)this.GetDataObjectAdapter<T>().SelectList(columnExpressions, transactionID);
        }

        /// <summary>
        /// Selects a list of objects from database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="columnExpressions">The column expressions.</param>
        /// <param name="startRecord">The start record.</param>
        /// <param name="maxRecords">The max records.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public IList<T> SelectList<T>(ColumnExpression[] columnExpressions, int startRecord, int maxRecords, string transactionID = null)
        {
            return (IList<T>)this.GetDataObjectAdapter<T>().SelectList(columnExpressions, startRecord, maxRecords, transactionID);
        }

        /// <summary>
        /// Selects a list of objects from database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="condition">The condition.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public IList<T> SelectList<T>(BaseCommand condition, string transactionID = null)
        {
            return (IList<T>)this.GetDataObjectAdapter<T>().SelectList(condition, transactionID);
        }

        /// <summary>
        /// Selects a list of objects from database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="condition">The condition.</param>
        /// <param name="startRecord">The start record.</param>
        /// <param name="maxRecords">The max records.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public IList<T> SelectList<T>(BaseCommand condition, int startRecord, int maxRecords, string transactionID = null)
        {
            return (IList<T>)this.GetDataObjectAdapter<T>().SelectList(condition, startRecord, maxRecords, transactionID);
        }

        /// <summary>
        /// Selects a list of objects from database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="columnNames">The column names.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public IList<T> SelectList<T>(string[] columnNames, BaseCommand condition, string transactionID = null)
        {
            return (IList<T>)this.GetDataObjectAdapter<T>().SelectList(columnNames, condition, transactionID);
        }

        /// <summary>
        /// Selects a list of objects from database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="columnNames">The column names.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="startRecord">The start record.</param>
        /// <param name="maxRecords">The max records.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public IList<T> SelectList<T>(string[] columnNames, BaseCommand condition, int startRecord, int maxRecords, string transactionID = null)
        {
            return (IList<T>)this.GetDataObjectAdapter<T>().SelectList(columnNames, condition, startRecord, maxRecords, transactionID);
        }

        /// <summary>
        /// Selects a list of objects from database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="columnExpressions">The column expressions.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public IList<T> SelectList<T>(ColumnExpression[] columnExpressions, BaseCommand condition, string transactionID = null)
        {
            return (IList<T>)this.GetDataObjectAdapter<T>().SelectList(columnExpressions, condition, transactionID);
        }

        /// <summary>
        /// Selects a list of objects from database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="columnExpressions">The column expressions.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="startRecord">The start record.</param>
        /// <param name="maxRecords">The max records.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public IList<T> SelectList<T>(ColumnExpression[] columnExpressions, BaseCommand condition, int startRecord, int maxRecords, string transactionID = null)
        {
            return (IList<T>)this.GetDataObjectAdapter<T>().SelectList(columnExpressions, condition, startRecord, maxRecords, transactionID);
        }

        /// <summary>
        /// Selects a list of objects from database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conditionExpression">The condition expression.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public IList<T> SelectList<T>(ConditionExpression conditionExpression, string transactionID = null)
        {
            return (IList<T>)this.GetDataObjectAdapter<T>().SelectList(conditionExpression, transactionID);
        }

        /// <summary>
        /// Selects a list of objects from database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conditionExpression">The condition expression.</param>
        /// <param name="startRecord">The start record.</param>
        /// <param name="maxRecords">The max records.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public IList<T> SelectList<T>(ConditionExpression conditionExpression, int startRecord, int maxRecords, string transactionID = null)
        {
            return (IList<T>)this.GetDataObjectAdapter<T>().SelectList(conditionExpression, startRecord, maxRecords, transactionID);
        }

        /// <summary>
        /// Selects a list of objects from database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="columnNames">The column names.</param>
        /// <param name="conditionExpression">The condition expression.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public IList<T> SelectList<T>(string[] columnNames, ConditionExpression conditionExpression, string transactionID = null)
        {
            return (IList<T>)this.GetDataObjectAdapter<T>().SelectList(columnNames, conditionExpression, transactionID);
        }

        /// <summary>
        /// Selects a list of objects from database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="columnNames">The column names.</param>
        /// <param name="conditionExpression">The condition expression.</param>
        /// <param name="startRecord">The start record.</param>
        /// <param name="maxRecords">The max records.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public IList<T> SelectList<T>(string[] columnNames, ConditionExpression conditionExpression, int startRecord, int maxRecords, string transactionID = null)
        {
            return (IList<T>)this.GetDataObjectAdapter<T>().SelectList(columnNames, conditionExpression, startRecord, maxRecords, transactionID);
        }

        /// <summary>
        /// Selects a list of objects from database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="columnExpressions">The column expressions.</param>
        /// <param name="conditionExpression">The condition expression.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public IList<T> SelectList<T>(ColumnExpression[] columnExpressions, ConditionExpression conditionExpression, string transactionID = null)
        {
            return (IList<T>)this.GetDataObjectAdapter<T>().SelectList(columnExpressions, conditionExpression, transactionID);
        }

        /// <summary>
        /// Selects a list of objects from database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="columnExpressions">The column expressions.</param>
        /// <param name="conditionExpression">The condition expression.</param>
        /// <param name="startRecord">The start record.</param>
        /// <param name="maxRecords">The max records.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        public IList<T> SelectList<T>(ColumnExpression[] columnExpressions, ConditionExpression conditionExpression, int startRecord, int maxRecords, string transactionID = null)
        {
            return (IList<T>)this.GetDataObjectAdapter<T>().SelectList(columnExpressions, conditionExpression, startRecord, maxRecords, transactionID);
        }

        #endregion

        #endregion
    }
}
