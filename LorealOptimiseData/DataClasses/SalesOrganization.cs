using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LorealOptimiseData
{
    /// <summary>
    /// 
    /// </summary>
    public partial class SalesOrganization : IPrimaryKey, ITrackChanges, IDeletionLimit
    {
        public bool CanBeDeleted(out string reasonMsg, out string warning)
        {
            reasonMsg = string.Empty;
            warning = string.Empty;
            if (this.SalesAreas.Any(sa=>sa.AnimationProductDetails.Any(apd=>apd.AnimationProduct!= null && apd.AnimationProduct.Animation!=null && apd.AnimationProduct.Animation.IsActive)))
            {
               // reasonMsg = "This Sales Organization cannot be deleted it has active animations.";
                reasonMsg = SystemMessagesManager.Instance.GetMessage("SalesOrganizationDelete");
                return false;
            }
            return true;
        }
    }
}
