using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Text;
using LorealOptimiseShared;
using LorealOptimiseShared.Logging;

namespace LorealOptimiseData
{
    public partial class CustomerAllocation : IPrimaryKey, ITrackChanges, ICleanEntityRef
    {
        public bool IsValid(out string errorMessage)
        {
            if (FixedAllocation != null && RetailUplift != null)
            {
                //errorMessage = "Please fill either fixed allocation or retail uplift. Both values can not be filled";
                errorMessage = SystemMessagesManager.Instance.GetMessage("CustomerAllocationFixedOrRetail");
                return false;
            }

            return Utility.IsValid(this, out errorMessage);
        }

        public Guid IDAnimation
        {
            get
            {
                return AnimationProductDetail.AnimationProduct.Animation.ID;
            }
        }

        private int? maximumCapacity = -1;
        public int MaximumCapacity
        {
            get
            {
                if (CachedCapacity.HasValue && maximumCapacity == -1)
                {
                    //CachedCapacity is calculated (this will have value in most cases (values are relcalulated every day in job))
                    //and we did not change any value which should affect capacity (Priority, ItemType, Category, etc..)
                    return CachedCapacity.Value;
                }

                if (maximumCapacity == null)
                {
                    //we change some value, which should affect capacity (Prioritu, ItemType, ...)
                    if (AnimationProductDetail != null)
                    {
                        maximumCapacity = DbDataContext.GetInstance().calculate_TotalCapacityCustomerAllocation(ID, true);
                    }
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

        private DbDataContext db = DbDataContext.GetInstance();


        public void CleanEntityRef(string fieldName)
        {
            if(fieldName == "IDAnimationProductDetail")
            {
                this._AnimationProductDetail = default(EntityRef<AnimationProductDetail>);
            }
            else if (fieldName == "IDCustomer")
            {
                this._Customer = default(EntityRef<Customer>);
            }
        }
    }
}
