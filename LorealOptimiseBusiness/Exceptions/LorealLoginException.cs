using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LorealOptimiseBusiness.Exceptions
{
    /// <summary>
    /// Class for exception occured during login process
    /// </summary>
    public class LorealLoginException : LorealException
    {
        /// <summary>
        /// Creates new instance of LorealLoginException with specified message
        /// </summary>
        /// <param name="message"></param>
        public LorealLoginException(string message):base(message)
        {
            
        }

        /// <summary>
        /// Creates new instance of LorealLoginException with specified message and parameters
        /// </summary>
        /// <param name="message"></param>
        /// <param name="parameters"></param>
        public LorealLoginException(string message, params object[] parameters)
            : this(string.Format(message, parameters))
        {
            
        }
    }
}
