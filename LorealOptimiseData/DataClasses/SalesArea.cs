using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Text;

namespace LorealOptimiseData
{
    public partial class SalesArea : IPrimaryKey, IDivision, ITrackChanges, ICleanEntityRef, IDeletionLimit
    {
        public void CleanEntityRef(string FieldName)
        {
            if (FieldName == "IDDistributionChannel")
            {
                this._DistributionChannel = default(EntityRef<DistributionChannel>);
            }
            else if(FieldName == "IDSalesOrganization")
            {
                this._SalesOrganization = default(EntityRef<SalesOrganization>);
            }
        }

        public bool CanBeDeleted(out string reasonMsg, out string warning)
        {
            reasonMsg = string.Empty;
            warning = string.Empty;
            if (this.AnimationProductDetails.Any(apd => apd.AnimationProduct != null && apd.AnimationProduct.Animation != null && apd.AnimationProduct.Animation.IsActive))
            {
                // reasonMsg = "This Sales Area cannot be deleted because it has active animations.";
                reasonMsg = SystemMessagesManager.Instance.GetMessage("SalesAreaDelete");
                return false;
            }
            return true;
        }
    }
}
