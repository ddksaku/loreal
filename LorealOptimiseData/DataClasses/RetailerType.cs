using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LorealOptimiseData
{
    /// <summary>
    /// 
    /// </summary>
    public partial class RetailerType : IPrimaryKey, IDivision, ITrackChanges, IDeletionLimit
    {
        partial void OnCreated()
        {
            PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(RetailerType_PropertyChanged);    
        }

        void RetailerType_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //if(e.PropertyName == "Default")
            //{
            //    if (this.Default == true)
            //    {
            //        IEnumerable<RetailerType> defaultRetailerTypes = this.Division.RetailerTypes.Where(rt => rt.Default == true && rt.ID != this.ID);
            //        if (defaultRetailerTypes != null)
            //        {
            //            foreach(RetailerType rt in defaultRetailerTypes)
            //                rt.Default = false;
            //        }
            //    }
            //}
        }

        public bool CanBeDeleted(out string reasonMsg, out string warning)
        {
            reasonMsg = string.Empty;
            warning = string.Empty;

            return true;

            if (this.AnimationCustomerGroups.Any(acg=>acg.Animation.IsActive))
            {
                //reasonMsg = "This Retailer Type cannot be deleted because it is attached to some animations.";
                reasonMsg = SystemMessagesManager.Instance.GetMessage("RetailerTypeDelete");
                return false;
            }
            return true;
        }
    }
}
