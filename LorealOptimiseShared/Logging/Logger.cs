using System;
using System.IO;
using System.Net;
using System.Web;
using log4net;
using System.Reflection;
using System.Diagnostics;
using log4net.Appender;

namespace LorealOptimiseShared.Logging
{
    /// <summary>
    /// Class that encapsulates log4net logginng engine.
    /// </summary>
    public static class Logger
    {
        #region Fields

        /// <summary>
        /// Executing assembly.
        /// </summary>
        public static Assembly assembly;

        /// <summary>
        /// ILog instance.
        /// </summary>
        private static ILog log;

        #endregion

        #region Log
        /// <summary>
        /// Logs messages into the log output
        /// </summary>
        /// <param name="message">Message that is sent to the log ouput</param>
        /// <param name="level">Level of the message</param>
        public static void Log(string message, LogLevel level)
        {
            Log(message, level, LogFilePrefix.Error);
        }

        public static void Log(string message, LogLevel level, LogFilePrefix prefix)
        {
            Log(message, level, prefix, false);
        }

        /// <summary>
        /// Logs messages into the log output
        /// </summary>
        /// <param name="message">Message that is sent to the log ouput</param>
        /// <param name="level">Level of the message</param>
        /// <param name="prefix">Prefix of the log file. It is valid on for PackRollingFileAppender</param>
        public static void Log(string message, LogLevel level, LogFilePrefix prefix, bool throwException)
        {
            try
            {
                if (assembly == null)
                {
                    return;
                }

                log = LogManager.GetLogger(assembly, assembly.GetTypes()[0]);

                if (log == null)
                {
                    return;
                }

                   

                //Save additional information
                string logMessage = message + Environment.NewLine;
                ThreadContext.Properties[PrefixFileAppender.LOG_PREFIX] = prefix;

                switch (level)
                {
                    case LogLevel.Info:
                        log.Info(logMessage);
                        break;
                    case LogLevel.Debug:
                        log.Debug(logMessage);
                        break;
                    case LogLevel.Warn:
                        log.Warn(logMessage);
                        break;
                    case LogLevel.Error:
                        log.Error(logMessage);
                        break;
                    case LogLevel.Fatal:
                        log.Fatal(logMessage);
                        break;
                }
            }
            catch (Exception exc)
            {
                if (throwException)
                {
                    throw exc;
                }
            }
        }
        #endregion
    }
}