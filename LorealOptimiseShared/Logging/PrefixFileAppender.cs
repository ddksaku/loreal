using System;
using System.IO;
using System.Web;
using log4net.Appender;
using log4net.Core;
using log4net;

namespace LorealOptimiseShared.Logging
{
    /// <summary>
    /// Pack rolling file appender.
    /// </summary>
    public class PrefixFileAppender : RollingFileAppender
    {
        #region Fields
        
        /// <summary>
        /// Log prefix.
        /// </summary>
        public const string LOG_PREFIX = "LOG_PREFIX";

        /// <summary>
        /// HTTP context.
        /// </summary>
        public const string HTTP_CONTEXT = "HTTP_CONTEXT";

        /// <summary>
        /// Session vars.
        /// </summary>
        public const string SESSION_VARS = "SESSION_VARS"; 
        
        #endregion

        #region Append
        /// <summary>
        /// 
        /// </summary>
        /// <param name="loggingEvent"></param>
        protected override void Append(LoggingEvent loggingEvent)
        {
            string filename = Path.GetDirectoryName(File) + "\\";

            OpenFile(filename, true);

            base.Append(loggingEvent);
        } 
        #endregion

        #region OpenFile
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="append"></param>
        protected override void OpenFile(string fileName, bool append)
        {
            LogFilePrefix prefix = LogFilePrefix.Debug;

            if (ThreadContext.Properties[LOG_PREFIX] != null)
            {
                prefix = (LogFilePrefix)ThreadContext.Properties[LOG_PREFIX];
            }

            fileName += prefix;

            base.OpenFile(fileName, append);
        }
        #endregion
    }
}