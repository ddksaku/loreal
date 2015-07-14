using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LorealOptimiseBusiness.Exceptions
{
    /// <summary>
    /// The exception which is thrown when non-valid data are being sent to the database
    /// </summary>
    public class LorealValidationException : LorealException
    {
        /// <summary>
        /// Creates new instance of LorealValidationException
        /// </summary>
        /// <param name="message">Message which is displayed to the user</param>
        public LorealValidationException(string message):base(message)
        {
            
        }

        /// <summary>
        /// Creates new instance of LorealValidationException
        /// </summary>
        /// <param name="message">Message which is displayed to the user</param>
        /// <param name="parameters">Parameters for string.format method </param>
        public LorealValidationException(string message, params object[] parameters):base(message, parameters)
        {
            
        }
    }
}
