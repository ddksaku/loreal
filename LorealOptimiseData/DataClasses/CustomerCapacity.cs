using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Text;

namespace LorealOptimiseData
{
    public partial class CustomerCapacity : IPrimaryKey, ITrackChanges, IDeletionLimit, ICleanEntityRef
    {
        public bool CanBeDeleted(out string reasonMsg, out string warning)
        {
            warning = string.Empty;
            //reasonMsg = "Any Customer Capacity data cannot be deleted. The only way to exclude the records is be to update the capacity value manually to 0.";
            reasonMsg = SystemMessagesManager.Instance.GetMessage("CustomerCapacityDelete");
            return false;
        }

        public void CleanEntityRef(string fieldName)
        {
            if (fieldName == "IDCustomer")
            {
                this._Customer = default(EntityRef<Customer>);
            }
        }
    }
}
