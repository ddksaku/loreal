using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LorealOptimiseData
{
    /// <summary>
    /// Interfaces used to identify primary key of the table
    /// </summary>
    public interface IPrimaryKey
    {
        /// <summary>
        /// Primary key of the table
        /// </summary>
        Guid ID
        {
            get; set;
        }
    }
}
