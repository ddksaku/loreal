using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LorealOptimiseData
{
    /// <summary>
    /// Interface used to identify entities which has relation with Division entity
    /// </summary>
    public interface IDivision
    {
        /// <summary>
        /// Division
        /// </summary>
        Division Division
        {
            get;
        }
    }
}
