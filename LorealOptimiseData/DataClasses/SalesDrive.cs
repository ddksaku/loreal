using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LorealOptimiseData
{
    /// <summary>
    /// 
    /// </summary>
    public partial class SalesDrive : IPrimaryKey, IDivision, ITrackChanges, IDeletionLimit
    {
        public string YearAndName
        {
            get
            {
                return this.Year + " " + this.Name;
            }
        }

        public bool CanBeDeleted(out string reasonMsg, out string warning)
        {
            reasonMsg = string.Empty;
            warning = string.Empty;
            if (this.Animations.Any(ap => ap.IsActive))
            {
                //reasonMsg = "This Sales Drive cannot be deleted because it is attached to some animations.";
                reasonMsg = SystemMessagesManager.Instance.GetMessage("SaleDriveDelete");
                return false;
            }
            return true;
        }
    }
}
