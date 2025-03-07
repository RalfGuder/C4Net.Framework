using C4Net.Framework.Core.Log;
using NLog;

namespace C4Net.Framework.NLoggable
{
    /// <summary>
    /// Implementation of the NLog logger.
    /// </summary>
    public class NLoggable : LoggableBase
    {
        #region - Static Fields -

        /// <summary>
        /// Field for the default class logger for NLog.
        /// </summary>
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region - Methods -

        /// <summary>
        /// Converts a LogSeverity to a NLog LogLevel.
        /// </summary>
        /// <param name="severity">The severity.</param>
        /// <returns></returns>
        private LogLevel GetLogLevel(LogSeverity severity)
        {
            switch (severity)
            {
                case LogSeverity.Debug:
                    return LogLevel.Debug;
                case LogSeverity.Info:
                    return LogLevel.Info;
                case LogSeverity.Warning:
                    return LogLevel.Warn;
                case LogSeverity.Error:
                    return LogLevel.Error;
                case LogSeverity.Fatal:
                    return LogLevel.Fatal;
                default:
                    return LogLevel.Trace;
            }
        }

        /// <summary>
        /// Writes a message into the log given the logger name, the severity and the message.
        /// </summary>
        /// <param name="loggerName">Name of the logger.</param>
        /// <param name="severity">The severity.</param>
        /// <param name="message">The message.</param>
        protected override void InnerWriteLog(string loggerName, LogSeverity severity, string message)
        {
            LogEventInfo logEvent = new LogEventInfo(GetLogLevel(severity), "default", message);
            logger.Log(logEvent);
        }

        #endregion
    }
}
