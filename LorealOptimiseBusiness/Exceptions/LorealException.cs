using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LorealOptimiseBusiness.Exceptions
{
    /// <summary>
    /// Base class for all exceptions throw by the application
    /// </summary>
    public class LorealException : ApplicationException
    {
        /// <summary>
        /// Creates new instance of LorealException with specified message
        /// </summary>
        /// <param name="message"></param>
        public LorealException(string message):base(message)
        {
            
        }

        /// <summary>
        /// Creates new instance of LorealException with specified message and parameters
        /// </summary>
        /// <param name="message"></param>
        /// <param name="parameters"></param>
        public LorealException(string message, params object[] parameters):this(string.Format(message, parameters))
        {
            
        }

        /// <summary>
        /// Creates new instance of LorealException with specified message
        /// </summary>
        /// <param name="message"></param>
        public LorealException(string message, Exception exc)
            : base(message, exc)
        {

        }

        /// Creates new instance of LorealException with specified message and parameters
        /// </summary>
        /// <param name="message"></param>
        /// <param name="parameters"></param>
        public LorealException(string message, Exception exc, params object[] parameters)
            : this(string.Format(message, parameters), exc)
        {

        }
    }
}
