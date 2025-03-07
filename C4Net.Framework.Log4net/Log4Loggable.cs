using C4Net.Framework.Core.Log;
using log4net;
using log4net.Config;

namespace C4Net.Framework.Log4net
{
    /// <summary>
    /// Class for logging using Apache Log4Net
    /// </summary>
    public class Log4Loggable : LoggableBase
    {
        #region - Static fields -

        /// <summary>
        /// The default Log4Net logger.
        /// </summary>
        private static readonly ILog logger = LogManager.GetLogger("default");

        /// <summary>
        /// Indicates if is already configured.
        /// </summary>
        private static bool configured = false;

        #endregion

        /// <summary>
        /// Configures this instance from the Log4net config xml.
        /// </summary>
        private void Configure()
        {
            configured = true;
            XmlConfigurator.Configure();
        }

        /// <summary>
        /// Writes a message into the log given the logger name, the severity and the message.
        /// </summary>
        /// <param name="loggerName">Name of the logger.</param>
        /// <param name="severity">The severity.</param>
        /// <param name="message">The message.</param>
        protected override void InnerWriteLog(string loggerName, LogSeverity severity, string message)
        {
            if (!configured)
            {
                Configure();
            }

            switch (severity)
            {
                case LogSeverity.Debug:
                    logger.Debug(message);
                    break;
                case LogSeverity.Info:
                    logger.Info(message);
                    break;
                case LogSeverity.Warning:
                    logger.Warn(message);
                    break;
                case LogSeverity.Error:
                    logger.Error(message);
                    break;
                case LogSeverity.Fatal:
                    logger.Fatal(message);
                    break;
                default:
                    logger.Debug(message);
                    break;
            }
        }
    }
}
