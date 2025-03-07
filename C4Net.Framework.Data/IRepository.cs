using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using C4Net.Framework.Data.Base;
using C4Net.Framework.Data.DAO;
using C4Net.Framework.Expressions;

namespace C4Net.Framework.Data
{
    /// <summary>
    /// Class for a repository for one entity type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T> where T : IDAO
    {
        #region - Properties -

        /// <summary>
        /// Gets the Data context for the repository.
        /// </summary>
        /// <value>
        /// The data context.
        /// </value>
        IDataContext Context { get; set; }

        /// <summary>
        /// Gets or sets the transaction id.
        /// </summary>
        /// <value>
        /// The transaction id.
        /// </value>
        string TransactionId { get; set; }

        #endregion

        #region - Methods -

        /// <summary>
        /// Selects a IQueryable to access an IEnumerator from a given linq expression.
        /// </summary>
        /// <param name="expression">The linq expression.</param>
        /// <returns></returns>
        IQueryable<T> SelectQuery(Expression expression);

        /// <summary>
        /// Selects all the entities from database.
        /// </summary>
        /// <returns></returns>
        IList<T> SelectList();

        /// <summary>
        /// Selects the list of entities selected by a condition expression.
        /// </summary>
        /// <param name="conditionExpression">The condition expression.</param>
        /// <returns></returns>
        IList<T> SelectList(ConditionExpression conditionExpression);

        /// <summary>
        /// Selects the list of entities selected by a condition command.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <returns></returns>
        IList<T> SelectList(BaseCommand condition);

        /// <summary>
        /// Selects the list.
        /// </summary>
        /// <param name="startRecord">The start record.</param>
        /// <param name="maxRecords">The max records.</param>
        /// <returns></returns>
        IList<T> SelectList(int startRecord, int maxRecords);

        /// <summary>
        /// Selects the list.
        /// </summary>
        /// <param name="conditionExpression">The condition expression.</param>
        /// <param name="startRecord">The start record.</param>
        /// <param name="maxRecords">The max records.</param>
        /// <returns></returns>
        IList<T> SelectList(ConditionExpression conditionExpression, int startRecord, int maxRecords);

        /// <summary>
        /// Selects the list.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <param name="startRecord">The start record.</param>
        /// <param name="maxRecords">The max records.</param>
        /// <returns></returns>
        IList<T> SelectList(BaseCommand condition, int startRecord, int maxRecords);

        /// <summary>
        /// Selects the first entity from database.
        /// </summary>
        /// <returns></returns>
        T SelectFirst();

        /// <summary>
        /// Selects the first entity from database by a condition expression.
        /// </summary>
        /// <param name="conditionExpression">The condition expression.</param>
        /// <returns></returns>
        T SelectFirst(ConditionExpression conditionExpression);

        /// <summary>
        /// Selects the first entity from database by a condition command.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <returns></returns>
        T SelectFirst(BaseCommand condition);

        /// <summary>
        /// Determines if the entity exists at database.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        bool Exists(T entity);

        /// <summary>
        /// Inserts the specified entity into database.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Insert(T entity);

        /// <summary>
        /// Updates the specified entity to dabase.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Update(T entity);

        /// <summary>
        /// Saves the specified entity to database.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Save(T entity);

        /// <summary>
        /// Deletes the specified entity from database.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Delete(T entity);

        #endregion
    }
}
