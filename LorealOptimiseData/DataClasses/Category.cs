using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LorealOptimiseData
{
    public partial class Category : IPrimaryKey, IDivision, ITrackChanges, IDeletionLimit
    {
        public bool CanBeDeleted(out string reasonMsg, out string warning)
        {
            reasonMsg = string.Empty;
            warning = string.Empty;
            if (this.AnimationProducts.Any(ap => ap.Animation.IsActive))
            {
                //reasonMsg = "This Category cannot be deleted because it is attached to some animations.";
                reasonMsg = SystemMessagesManager.Instance.GetMessage("CategoryDelete");
                return false;
            }
            return true;
        }
    }
}
