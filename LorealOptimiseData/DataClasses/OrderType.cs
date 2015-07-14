using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LorealOptimiseData
{
    /// <summary>
    /// 
    /// </summary>
    public partial class OrderType : IPrimaryKey, IDivision, ITrackChanges, IDeletionLimit
    {
        public bool CanBeDeleted(out string reasonMsg, out string warning)
        {
            reasonMsg = string.Empty;
            warning = string.Empty;
            if (this.Animations.Any(ap => ap.IsActive))
            {
                //reasonMsg = "This Order Type cannot be deleted because it is attached to some animations.";
                reasonMsg = SystemMessagesManager.Instance.GetMessage("OrderTypeDelete");
                return false;
            }
            return true;
        }
    }
}
