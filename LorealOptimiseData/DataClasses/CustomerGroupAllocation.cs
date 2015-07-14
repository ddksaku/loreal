using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Text;
using LorealOptimiseShared;

namespace LorealOptimiseData
{
    public partial class CustomerGroupAllocation : IPrimaryKey, ITrackChanges, ICleanEntityRef
    {
        public bool IsValid(out string errorMessage)
        {
            if (ManualFixedAllocation != null && RetailUplift != null)
            {
                //errorMessage = "Please fill either fixed allocation or retail uplift. Both values can not be filled";
                errorMessage = SystemMessagesManager.Instance.GetMessage("CustomerGroupAllocationFixedOrRetail");
                return false;
            }

            return Utility.IsValid(this, out errorMessage);
        }

        public void CleanEntityRef(string fieldName)
        {
            if (fieldName == "IDAnimationProductDetail")
            {
                this._AnimationProductDetail = default(EntityRef<AnimationProductDetail>);
            }
            else if (fieldName == "IDCustomerGroup")
            {
                this._CustomerGroup = default(EntityRef<CustomerGroup>);
            }
        }

        private int? maximumCapacity;
        public int MaximumCapacity
        {
            get
            {
                if (maximumCapacity == null && AnimationProductDetail != null)
                {
                    maximumCapacity =
                        DbDataContext.GetInstance().calculate_TotalCapacityCustomerGroup(ID, true);
                }

                if (maximumCapacity.HasValue)
                {
                    return maximumCapacity.Value;
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                if (value == -1)
                {
                    maximumCapacity = null;
                    SendPropertyChanged("MaximumCapacity");
                }
            }
        }

        public bool IsStillQuantityAvailable
        {
            get
            {
                int fixedAllocation = ManualFixedAllocation != null ? ManualFixedAllocation.Value : 0;
                int systemAllocation = SystemFixedAllocation != null ? SystemFixedAllocation.Value : 0;
                return fixedAllocation > systemAllocation;
            }
        }
    }
}
