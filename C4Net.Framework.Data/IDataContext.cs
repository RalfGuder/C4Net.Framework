using System;
using System.Collections;
using System.Collections.Generic;
using C4Net.Framework.Data.Adapters;
using C4Net.Framework.Data.Base;
using C4Net.Framework.Data.Transactions;
using C4Net.Framework.Expressions;

namespace C4Net.Framework.Data
{
    /// <summary>
    /// Interface for a Data Context.
    /// </summary>
    public interface IDataContext : ITransactionManager
    {
        #region - Methods -

        #region - Aux -

        /// <summary>
        /// Gets the data object adapter.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        DataObjectAdapter GetDataObjectAdapter(Type type);

        /// <summary>
        /// Get a string representing a primary key that can be composed.
        /// </summary>
        /// <param name="keys">The key values.</param>
        /// <returns></returns>
        string GetKeyStr(object[] keys);

        /// <summary>
        /// Gets a string representing a primary key of one field only.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        string GetOneKeyStr(object key);

        /// <summary>
        /// Gets the primary key values of an item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        object[] GetPrimaryKeys<T>(T item);

        /// <summary>
        /// Gets the primary key string of an item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        string GetPrimaryKeyStr<T>(T item);

        #endregion

        #region - Aggregate -

        /// <summary>
        /// Execute the aggregate function AVG.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        object GetAvg<T>(string columnName, string transactionID = null);

        /// <summary>
        /// Execute the aggregate function AVG.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        object GetAvg<T>(string columnName, BaseCommand condition, string transactionID = null);

        /// <summary>
        /// Execute the aggregate function COUNT.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command">The command.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        long GetCount<T>(BaseCommand command, string transactionID = null);

        /// <summary>
        /// Execute the aggregate function COUNT.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        long GetCount<T>(string transactionID = null);

        /// <summary>
        /// Execute the aggregate function MAX.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        object GetMax<T>(string columnName, BaseCommand condition, string transactionID = null);

        /// <summary>
        /// Execute the aggregate function MAX.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        object GetMax<T>(string columnName, string transactionID = null);

        /// <summary>
        /// Execute the aggregate function MIN.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        object GetMin<T>(string columnName, BaseCommand condition, string transactionID = null);

        /// <summary>
        /// Execute the aggregate function MIN.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        object GetMin<T>(string columnName, string transactionID = null);

        /// <summary>
        /// Execute the aggregate function SUM.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="command">The command.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        object GetSum<T>(string columnName, BaseCommand command, string transactionID = null);

        /// <summary>
        /// Execute the aggregate function SUM.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        object GetSum<T>(string columnName, string transactionID = null);

        #endregion

        #region - Exist -

        /// <summary>
        /// Indicates if a given item exist at database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item">The item.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        bool Exists<T>(object item, string transactionID = null);

        /// <summary>
        /// Indicates if a given item exist at database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="primaryKeys">The primary keys.</param>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns></returns>
        bool Exists<T>(object[] primaryKeys, string transactionID = null);

        #endregion

        #region - Insert -

        int Insert<T>(object item, ColumnExpression[] columnExpressions, string transactionID = null);

        int Insert<T>(object item, string transactionID = null);

        int Insert<T>(object item, string[] columnNames, string transactionID = null);

        int InsertList<T>(ICollection items, ColumnExpression[] columnExpressions, string transactionID = null);

        int InsertList<T>(ICollection items, string transactionID = null);

        int InsertList<T>(ICollection items, string[] columnNames, string transactionID = null);

        #endregion

        #region - Update -

        int Update<T>(object item, ColumnExpression[] columnExpressions, BaseCommand condition, string transactionID = null);

        int Update<T>(object item, ColumnExpression[] columnExpressions, string transactionID = null);

        int Update<T>(object item, BaseCommand condition, string transactionID = null);

        int Update<T>(object item, string transactionID = null);

        int Update<T>(object item, string[] columnNames, BaseCommand condition, string transactionID = null);

        int Update<T>(object item, string[] columnNames, string transactionID = null);

        int UpdateList<T>(ICollection items, ColumnExpression[] columnExpressions, string transactionID = null);

        int UpdateList<T>(ICollection items, string transactionID = null);

        int UpdateList<T>(ICollection items, string[] columnNames, string transactionID = null);

        #endregion

        #region - Save -

        int Save<T>(object item, ColumnExpression[] columnExpressions, string transactionID = null);

        int Save<T>(object item, string transactionID = null);

        int Save<T>(object item, string[] columnNames, string transactionID = null);

        int SaveList<T>(ICollection items, ColumnExpression[] columnExpressions, string transactionID = null);

        int SaveList<T>(ICollection items, string transactionID = null);

        int SaveList<T>(ICollection items, string[] columnNames, string transactionID = null);

        #endregion

        #region - Delete -

        int Delete<T>(ConditionExpression condition, string transactionID = null);

        int Delete<T>(BaseCommand condition, string transactionID = null);

        int Delete<T>(object item, string transactionID = null);

        int Delete<T>(object[] primaryKeys, string transactionID = null);

        int DeleteAll<T>(string transactionID = null);

        #endregion

        #region - Select -

        T Select<T>(ColumnExpression[] columnExpressions, ConditionExpression conditionExpression, string transactionID = null);

        T Select<T>(ColumnExpression[] columnExpressions, BaseCommand condition, string transactionID = null);

        T Select<T>(ConditionExpression conditionExpression, string transactionID = null);

        T Select<T>(BaseCommand condition, string transactionID = null);

        T Select<T>(object item, ColumnExpression[] columnExpressions, string transactionID = null);

        T Select<T>(object item, string transactionID = null);

        T Select<T>(object item, string[] columnNames, string transactionID = null);

        T Select<T>(object[] primaryKey, ColumnExpression[] columnExpressions, string transactionID = null);

        T Select<T>(object[] primaryKey, string transactionID = null);

        T Select<T>(object[] primaryKey, string[] columnNames, string transactionID = null);

        T Select<T>(string[] columnNames, ConditionExpression conditionExpression, string transactionID = null);

        T Select<T>(string[] columnNames, BaseCommand condition, string transactionID = null);

        T SelectByPrimaryKey<T>(object primaryKey, ColumnExpression[] columnExpressions, string transactionID = null);

        T SelectByPrimaryKey<T>(object primaryKey, string transactionID = null);

        T SelectByPrimaryKey<T>(object primaryKey, string[] columnNames, string transactionID = null);

        #endregion

        #region - Select list -

        IList<T> SelectList<T>(ColumnExpression[] columnExpressions, ConditionExpression conditionExpression, int startRecord, int maxRecords, string transactionID = null);

        IList<T> SelectList<T>(ColumnExpression[] columnExpressions, ConditionExpression conditionExpression, string transactionID = null);

        IList<T> SelectList<T>(ColumnExpression[] columnExpressions, BaseCommand condition, int startRecord, int maxRecords, string transactionID = null);

        IList<T> SelectList<T>(ColumnExpression[] columnExpressions, BaseCommand condition, string transactionID = null);

        IList<T> SelectList<T>(ColumnExpression[] columnExpressions, int startRecord, int maxRecords, string transactionID = null);

        IList<T> SelectList<T>(ColumnExpression[] columnExpressions, string transactionID = null);

        IList<T> SelectList<T>(ConditionExpression conditionExpression, int startRecord, int maxRecords, string transactionID = null);

        IList<T> SelectList<T>(ConditionExpression conditionExpression, string transactionID = null);

        IList<T> SelectList<T>(BaseCommand condition, int startRecord, int maxRecords, string transactionID = null);

        IList<T> SelectList<T>(BaseCommand condition, string transactionID = null);

        IList<T> SelectList<T>(int startRecord, int maxRecords, string transactionID = null);

        IList<T> SelectList<T>(string transactionID = null);

        IList<T> SelectList<T>(string[] columnNames, ConditionExpression conditionExpression, int startRecord, int maxRecords, string transactionID = null);

        IList<T> SelectList<T>(string[] columnNames, ConditionExpression conditionExpression, string transactionID = null);

        IList<T> SelectList<T>(string[] columnNames, BaseCommand condition, int startRecord, int maxRecords, string transactionID = null);

        IList<T> SelectList<T>(string[] columnNames, BaseCommand condition, string transactionID = null);

        IList<T> SelectList<T>(string[] columnNames, int startRecord, int maxRecords, string transactionID = null);

        IList<T> SelectList<T>(string[] columnNames, string transactionID = null);

        #endregion

        #endregion
    }
}
