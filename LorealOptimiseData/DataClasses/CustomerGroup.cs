using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Text;

namespace LorealOptimiseData
{
    /// <summary>
    /// 
    /// </summary>
    public partial class CustomerGroup : IPrimaryKey, ITrackChanges, ICleanEntityRef, IDeletionLimit
    {
        double? retailSales = null;
        public double RetailSales
        {
            get
            {
                if (retailSales == null)
                {
                    if (this.ID == Guid.Empty)
                        retailSales = 0;
                    else
                    {
                        retailSales = Customers.Where(c=>!c.Deleted && c.IncludeInSystem).Sum(c => c.TotalSales);

                        //retailSales = DbDataContext.GetInstance().countSale(null, this.ID, null, null, null, null, null);
                    }
                }
                if (retailSales.HasValue)
                    return Math.Round(retailSales.Value, 2);
                else
                    return 0;
            }
        }

        public string FullName
        {
            get
            {
                if (SalesArea != null)
                    return string.Format("{0} ({1})", Name, SalesArea.Name);
                else if (ID == Guid.Empty)
                    return string.Empty;
                else
                    return Name;  

            }
        }

        public void CleanEntityRef(string FieldName)
        {
            if (FieldName == "IDSalesArea")
            {
                this._SalesArea = default(EntityRef<SalesArea>);
            }
        }

        public bool CanBeDeleted(out string reasonMsg, out string warning)
        {
            reasonMsg = string.Empty;
            warning = string.Empty;
            if (this.Manual == false)
            {
                //reasonMsg = "This customer group cannot be deleted because it is not manual.";
                reasonMsg = SystemMessagesManager.Instance.GetMessage("CustomerGroupDelete");
                return false;
            }

            if (this.AnimationCustomerGroups.Count != 0 || this.CustomerGroupAllocations.Count != 0)
            {
                //reasonMsg = "This customer group cannot be deleted because it is attached to some animations.";
                reasonMsg = SystemMessagesManager.Instance.GetMessage("CustomerGroupDeleteAttached");
                return false;
            }

            string rMsg;
            string wMsg;
            foreach (Customer cus in this.Customers)
            {
                if (cus.CanBeDeleted(out rMsg, out wMsg) == false)
                {
                    //reasonMsg = "This customer group cannot be deleted because it has some customer stores which cannot be deleted.";
                    reasonMsg = SystemMessagesManager.Instance.GetMessage("CustomerGroupDeleteWithCustomers");
                    return false;
                }
            }

            return true;
        }
        
    }
}
