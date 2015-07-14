using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LorealOptimiseData
{
    public partial class ItemType : IPrimaryKey, IDivision, ITrackChanges, IDeletionLimit
    {
        public bool CanBeDeleted(out string reasonMsg, out string warning)
        {
            reasonMsg = string.Empty;
            warning = string.Empty;
            if (this.AnimationProducts.Any(ap => ap.Animation.IsActive))
            {
                //reasonMsg = "This Item Type cannot be deleted because it is attached to some animations.";
                reasonMsg = SystemMessagesManager.Instance.GetMessage("ItemTypeDelete");
                return false;
            }
            //warning = "If you delete this item type, then all capacities for this item type will also be deleted. Do you want to proceed?";
            warning = SystemMessagesManager.Instance.GetMessage("ItemTypeDeleteWarning");
            return true;
        }
    }
}
