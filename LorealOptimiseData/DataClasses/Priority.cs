using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LorealOptimiseData
{
    public partial class Priority : IPrimaryKey, IDivision, ITrackChanges, IDeletionLimit
    {
        public bool CanBeDeleted(out string reasonMsg, out string warning)
        {
            reasonMsg = string.Empty;
            warning = string.Empty;
            if (this.Animations.Any(a => a.IsActive))
            {
                //reasonMsg = "This Animation Priority cannot be deleted because it is attached to some animations.";
                reasonMsg = SystemMessagesManager.Instance.GetMessage("PriorityDelete");
                return false;
            }

            //warning = "If you delete this priority, then all capacities for this priority will also be deleted. Do you want to proceed?";
            warning = SystemMessagesManager.Instance.GetMessage("PriorityDeleteWarning");
            return true;
        }
    }
}
