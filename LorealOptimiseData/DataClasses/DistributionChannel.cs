using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LorealOptimiseData
{
    /// <summary>
    /// 
    /// </summary>
    public partial class DistributionChannel : IPrimaryKey, ITrackChanges, IDeletionLimit
    {
        public bool CanBeDeleted(out string reasonMsg, out string warning)
        {
            reasonMsg = string.Empty;
            warning = string.Empty;
            if (this.Animations.Any(ap => ap.IsActive))
            {
                reasonMsg = SystemMessagesManager.Instance.GetMessage("DistributionChannelDelete");
                return false;
            }
            return true;
        }
    }
}
