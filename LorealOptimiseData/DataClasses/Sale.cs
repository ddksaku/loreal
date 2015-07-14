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
    public partial class Sale : IPrimaryKey, ITrackChanges, ICleanEntityRef, IDeletionLimit
    {
        public bool AddRetailSales
        {
            get;
            set;
        }

        public decimal RetailSalesFromBenchmark
        {
            get;
            set;
        }

        public double RetailMultiplier
        {
            get
            {
                if (Customer == null || Customer.SalesArea == null)
                {
                    return 1;
                }

                return this.Customer.SalesArea.RetailMultiplier ?? 1;
            }
        }

        public void CleanEntityRef(string FieldName)
        {
            if (FieldName == "IDCustomer")
            {
                this._Customer = default(EntityRef<Customer>);
            }
            else if (FieldName == "IDBrandAxe")
            {
                this._BrandAxe = default(EntityRef<BrandAxe>);
            }
        }

        public bool CanBeDeleted(out string reasonMsg, out string warning)
        {
            warning = string.Empty;
            //reasonMsg = "Any sale data cannot be deleted. The only way to exclude sales is to update the sales value manually to 0 or exclude sales data for certain Brands/Axes using the Store Brand Exclusion window";
            reasonMsg = SystemMessagesManager.Instance.GetMessage("SaleDelete");
            return false;
        }
    }
}
