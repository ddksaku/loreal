using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;

namespace LorealOptimiseData
{
    public partial class CustomerGroupItemType : IPrimaryKey, ITrackChanges, IDeletionLimit, ICleanEntityRef
    {
        public bool CanBeDeleted(out string reasonMsg, out string warning)
        {
            reasonMsg = string.Empty;
            warning = string.Empty;            
            
            if (IDCustomerGroup == Guid.Empty)
            {
                return true;
            }

            CustomerGroup cg = this.CustomerGroup;

            if (cg == null)
            {
                cg = DbDataContext.GetInstance().CustomerGroups.Where(cgr => cgr.ID == IDCustomerGroup).FirstOrDefault();
            }

            if (cg == null)
            {
                return true;
            }


            if (cg.CustomerGroupAllocations.Any(cga=>cga.AnimationProductDetail.AnimationProduct.Animation.IsActive))
            {
                //reasonMsg = "This Customer Group Item Type cannot be deleted because it is attached to some animations.";
                reasonMsg = SystemMessagesManager.Instance.GetMessage("CustomerGroupItemTypeDelete");
                return false;
            }
            return true;
        }



        #region ICleanEntityRef Members

        public void CleanEntityRef(string fieldName)
        {
            if (fieldName == "IDCustomer")
            {
                this._Customer = default(EntityRef<Customer>);
            }
            else if (fieldName == "IDCustomerGroup")
            {
                this._CustomerGroup = default(EntityRef<CustomerGroup>);
            }
            else if (fieldName == "IDItemType")
            {
                this._ItemType = default(EntityRef<ItemType>);
            }
        }

        #endregion
    }
}
