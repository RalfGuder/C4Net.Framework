using System;
using System.Data;
using C4Net.Framework.Core.Conversions;
using C4Net.Framework.Core.IoC;
using C4Net.Framework.Data.Configuration;

namespace C4Net.Framework.Data.Base
{
    public static class CommandConverter
    {
        #region - Methods -

        /// <summary>
        /// Converts the specified BaseCommand to an IDbCommand converting also parameters.
        /// </summary>
        /// <param name="commandBase">The command base.</param>
        /// <param name="connection">The connection.</param>
        /// <param name="providerName">Name of the provider.</param>
        /// <returns></returns>
        public static IDbCommand Convert(BaseCommand commandBase, IDbConnection connection, string providerName)
        {
            string commandText = commandBase.CommandText;
            IDbCommand result = connection.CreateCommand();
            result.CommandTimeout = commandBase.CommandTimeout;
            result.CommandType = commandBase.CommandType;
            foreach (BaseParameter baseParameter in commandBase.Parameters)
            {
                IDbDataParameter parameter = result.CreateParameter();
                parameter.ParameterName = ReplaceParameterName(commandBase.CommandType, baseParameter.ParameterName, commandBase.ParameterPrefix, providerName, ref commandText);
                if (baseParameter.DbType != DbType.String)
                {
                    parameter.DbType = baseParameter.DbType;
                    parameter.Size = baseParameter.Size;
                }
                parameter.Direction = baseParameter.Direction;
                parameter.SourceColumn = baseParameter.SourceColumn;
                parameter.SourceVersion = baseParameter.SourceVersion;
                object value = baseParameter.Value;
                parameter.Value = value == null ? DBNull.Value : ConvertDataValue(value.GetType(), parameter.DbType, value);
                result.Parameters.Add(parameter);
            }
            result.CommandText = commandText;
            return result;
        }

        /// <summary>
        /// Remove a text from a string given its position and length, and insert a substring in this position.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="subs">The substring.</param>
        /// <param name="i">The index.</param>
        /// <param name="l">The length.</param>
        /// <returns></returns>
        private static string ReplaceTextPos(string source, string subs, int i, int l)
        {
            string result = source.Remove(i, l);
            result = result.Insert(i, subs);
            return result;
        }

        /// <summary>
        /// Replaces the name of the parameter.
        /// </summary>
        /// <param name="commonCommandText">The common command text.</param>
        /// <param name="commonParameterName">Name of the common parameter.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <returns></returns>
        private static string ReplaceParameterName(string commonCommandText, string commonParameterName, string parameterName)
        {
            string commandText = commonCommandText;
            int i = commandText.IndexOf(commonParameterName);
            while (i >= 0)
            {
                int paramLength = 0;
                if ((i + commonParameterName.Length < commandText.Length) && (char.IsLetterOrDigit(commandText[i + commonParameterName.Length])))
                {
                    paramLength = commonParameterName.Length;
                }
                else
                {
                    commandText = ReplaceTextPos(commandText, parameterName, i, commonParameterName.Length);
                    paramLength = parameterName.Length;
                }
                i = commandText.IndexOf(commonParameterName, i + paramLength);
            }
            return commandText;
        }

        /// <summary>
        /// Replaces the name of the parameter.
        /// </summary>
        /// <param name="commandType">Type of the command.</param>
        /// <param name="baseParamName">Name of the base param.</param>
        /// <param name="baseParamPrefix">The base param prefix.</param>
        /// <param name="providerName">Name of the provider.</param>
        /// <param name="commandText">The command text.</param>
        /// <returns></returns>
        public static string ReplaceParameterName(CommandType commandType, string baseParamName, string baseParamPrefix, string providerName, ref string commandText)
        {
            string commonParamName;
            string paramName = IoCDefault.Get<IDbProviderManager>().GetParameterName(providerName, baseParamName, baseParamPrefix, out commonParamName);
            if (commandType == CommandType.Text)
            {
                commandText = ReplaceParameterName(commandText, baseParamName, paramName);
            }
            return paramName;
        }

        /// <summary>
        /// Converts the data value.
        /// </summary>
        /// <param name="propertyType">Type of the property.</param>
        /// <param name="dbType">Type of the db.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        private static object ConvertDataValue(Type propertyType, DbType dbType, object value)
        {
            return IoCDefault.Get<IConversionManager>().Convert(propertyType, dbType, value);
        }

        /// <summary>
        /// Feedbacks the parameters.
        /// </summary> 
        /// <param name="baseCommand">The base command.</param>
        /// <param name="command">The command.</param>
        public static void FeedbackParameters(BaseCommand baseCommand, IDbCommand command)
        {
            for (int i = 0; i < command.Parameters.Count; i++)
            {
                IDbDataParameter parameter = command.Parameters[i] as IDbDataParameter;
                if (parameter.Direction != ParameterDirection.Input)
                {
                    baseCommand.Parameters[i].Value = parameter.Value;
                }
            }
        }

        #endregion
    }
}
