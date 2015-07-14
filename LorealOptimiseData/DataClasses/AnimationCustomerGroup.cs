using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Data.Linq;
using System.Text;
using LorealOptimiseShared;
using LorealOptimiseData.Enums;

namespace LorealOptimiseData
{
    /// <summary>
    /// 
    /// </summary>
    public partial class AnimationCustomerGroup : IPrimaryKey, ITrackChanges, ICleanEntityRef, IDeletionLimit
    {
        partial void OnCreated()
        {
            //ID = Guid.NewGuid();
        }

        public bool IsOnCounterDateOverriden
        {
            get
            {
                if (OverridenFlags == null)
                {
                    return false;
                }

                return ((AnimationCustomerGroupOverrides)OverridenFlags & AnimationCustomerGroupOverrides.OnCounterDate) == AnimationCustomerGroupOverrides.OnCounterDate;
            }
        }

        public bool IsPLVComponentDateOverriden
        {
            get
            {
                if (OverridenFlags == null)
                {
                    return false;
                }

                return ((AnimationCustomerGroupOverrides)OverridenFlags & AnimationCustomerGroupOverrides.PLVComponentDate) == AnimationCustomerGroupOverrides.PLVComponentDate;
            }
        }

        public bool IsPLVDeliveryDateOverriden
        {
            get
            {
                if (OverridenFlags == null)
                {
                    return false;
                }

                return ((AnimationCustomerGroupOverrides)OverridenFlags & AnimationCustomerGroupOverrides.PLVDeliveryDate) == AnimationCustomerGroupOverrides.PLVDeliveryDate;
            }
        }

        public bool IsSapDespatchCodeOverriden
        {
            get
            {
                if (OverridenFlags == null)
                {
                    return false;
                }

                return ((AnimationCustomerGroupOverrides)OverridenFlags & AnimationCustomerGroupOverrides.SapDespatchCode) == AnimationCustomerGroupOverrides.SapDespatchCode;
            }
        }

        public bool IsStockDateOverriden
        {
            get
            {
                if (OverridenFlags == null)
                {
                    return false;
                }

                return ((AnimationCustomerGroupOverrides)OverridenFlags & AnimationCustomerGroupOverrides.StockDate) == AnimationCustomerGroupOverrides.StockDate;
            }
        }

        public bool IsValid(out string errorMessage)
        {
            return Utility.IsValid(this, out errorMessage);
        }

        public bool IsValidProperty(string propertyName, object propertyValue, out string errorMessage)
        {
            return Utility.IsValid(this, propertyName, propertyValue, out errorMessage);
        }

        public void CleanEntityRef(string fieldName)
        {
            if (fieldName == "IDRetailerType")
            {
                this._RetailerType = default(EntityRef<RetailerType>);
            }
        }

        public bool CanBeDeleted(out string reasonMsg, out string warning)
        {
            reasonMsg = string.Empty;
            warning = string.Empty;

            Guid idAnimationType = Animation.IDAnimationType.HasValue ? Animation.IDAnimationType.Value : Guid.Empty;
            Guid idPriority = Animation.IDPriority.HasValue ? Animation.IDPriority.Value : Guid.Empty;
            //foreach (AnimationProduct ap in Animation.AnimationProducts)
            //{
            //    Guid idItemType = ap.IDItemType.HasValue? ap.IDItemType.Value : Guid.Empty;
            //    foreach (AnimationProductDetail apd in ap.AnimationProductDetails)
            //    {
            //        if (apd.AllocationQuantity != null && apd.AllocationQuantity.Value > 0)
            //        {
            //            int? capacity = Animation.ObservableAnimationCustomerGroups.Where(acg => acg.ID != this.ID).Sum(acg => acg.CustomerGroup.Customers.Sum(c => c.CustomerCapacities.Where(cc => cc.IDAnimationType == idAnimationType && cc.IDItemType == idItemType && cc.IDPriority == idPriority).Sum(cc => cc.Capacity)));
            //            if (capacity == null || apd.AllocationQuantity.Value > capacity.Value)
            //            {
            //                //reasonMsg = "This Customer Group cannot be removed because the allocation quantity of some animation products will exceed the available capacity.";
            //                reasonMsg = SystemMessagesManager.Instance.GetMessage("AnimationCustomerGroupGroupRemove");
            //                return false;
            //            }
            //        }
                    
            //    }
            //}

            AnimationProduct ap = null;
            Guid idItemType = Guid.Empty;
            int? capacityToMiss = null;
            foreach (AnimationProductDetail apd in Animation.ObservableProductDetails)
            {
                if (ap != apd.AnimationProduct)
                {
                    ap = apd.AnimationProduct;
                    idItemType = ap.IDItemType.HasValue ? ap.IDItemType.Value : Guid.Empty;
                    capacityToMiss = this.CustomerGroup.Customers.Sum(c => c.CustomerCapacities.Where(cc => cc.IDAnimationType == idAnimationType && cc.IDItemType == idItemType && cc.IDPriority == idPriority).Sum(cc => cc.Capacity));
                    if(capacityToMiss == null || capacityToMiss.HasValue == false)
                        capacityToMiss = 0;
                }

                if (apd.AllocationQuantity != null && apd.AllocationQuantity.Value > 0)
                {
                    // int? capacity = Animation.ObservableAnimationCustomerGroups.Where(acg => acg.ID != this.ID).Sum(acg => acg.CustomerGroup.Customers.Sum(c => c.CustomerCapacities.Where(cc => cc.IDAnimationType == idAnimationType && cc.IDItemType == idItemType && cc.IDPriority == idPriority).Sum(cc => cc.Capacity)));
                    int capacity = apd.MaximumCapacity - capacityToMiss.Value;
                    if (apd.AllocationQuantity.Value > capacity)
                    {
                        //reasonMsg = "This Customer Group cannot be removed because the allocation quantity of some animation products will exceed the available capacity.";
                        reasonMsg = SystemMessagesManager.Instance.GetMessage("AnimationCustomerGroupGroupRemove");
                        return false;
                    }
                }
            }
            
            return true;
        }
    }
}
