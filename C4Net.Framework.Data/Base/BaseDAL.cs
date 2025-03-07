using System.Data;

namespace C4Net.Framework.Data.Base
{
    /// <summary>
    /// Base data access to database.
    /// </summary>
    public static class BaseDAL
    {
        #region - Methods -

        /// <summary>
        /// Opens the connection of a command (if closed) and return the previous state.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        private static ConnectionState CommandOpen(IDbCommand command)
        {
            ConnectionState state = command.Connection.State;
            if ((state & ConnectionState.Open) != ConnectionState.Open)
            {
                command.Connection.Open();
            }
            return state;
        }

        /// <summary>
        /// Closes the connection of a command if the previous state was closed.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="state">The state.</param>
        private static void CommandClose(IDbCommand command, ConnectionState state)
        {
            if (state == ConnectionState.Closed)
            {
                command.Connection.Close();
            }
        }

        /// <summary>
        /// If the transaction is not null then assign it to the command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="transaction">The transaction.</param>
        private static void AssignTransaction(IDbCommand command, IDbTransaction transaction)
        {
            if (transaction != null)
            {
                command.Transaction = transaction;
            }
        }

        /// <summary>
        /// Executes a query and return the number of affected rows.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="transaction">The transaction.</param>
        /// <returns></returns>
        public static int ExecuteNonQueryCommand(IDbCommand command, IDbTransaction transaction = null)
        {
            AssignTransaction(command, transaction);
            ConnectionState oldState = CommandOpen(command);
            try
            {
                return command.ExecuteNonQuery();
            }
            finally
            {
                CommandClose(command, oldState);
            }
        }

        /// <summary>
        /// Executes a query and returns the scalar value.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="transaction">The transaction.</param>
        /// <returns></returns>
        public static object ExecuteScalarCommand(IDbCommand command, IDbTransaction transaction = null)
        {
            AssignTransaction(command, transaction);
            ConnectionState oldState = CommandOpen(command);
            try
            {
                return command.ExecuteScalar();
            }
            finally
            {
                CommandClose(command, oldState);
            }
        }

        /// <summary>
        /// Executes a query and returns the data reader interface.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="transaction">The transaction.</param>
        /// <returns></returns>
        public static IDataReader ExecuteReaderCommand(IDbCommand command, IDbTransaction transaction = null)
        {
            AssignTransaction(command, transaction);
            ConnectionState oldState = CommandOpen(command);
            try
            {
                return oldState == ConnectionState.Closed ? command.ExecuteReader(CommandBehavior.CloseConnection) : command.ExecuteReader();
            }
            catch
            {
                CommandClose(command, oldState);
                throw;
            }
        }

        #endregion
    }
}
