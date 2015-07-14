namespace LorealOptimiseShared.Logging
{
    /// <summary>
    /// Log file prefixes.
    /// </summary>
    public enum LogFilePrefix
    {
        /// <summary>
        /// Error prefix.
        /// </summary>
        Error,
        /// <summary>
        /// Db prefix.
        /// </summary>
        Db,
        /// <summary>
        /// Debug prefix.
        /// </summary>
        Debug,
        /// <summary>
        /// Restart prefix.
        /// </summary>
        Restart,
        /// <summary>
        /// Sql prefix is used for records created during sql communication
        /// </summary>
        Sql,
        /// <summary>
        /// Info prefix used for information messags
        /// </summary>
        Info
    }
}