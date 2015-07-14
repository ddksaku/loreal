using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LorealOptimiseData
{
    /// <summary>
    /// Interface used to save/track last changes made to the table
    /// If table needs to be tracked. Changes are track in table HistoryLog
    /// </summary>
    interface ITrackChanges : IPrimaryKey
    {
        /// <summary>
        /// Identification of the user who changed the table 
        /// </summary>
        string ModifiedBy { get; set; }

        /// <summary>
        /// Date of the change
        /// </summary>
        DateTime? ModifiedDate { get; set; }

    }
}
