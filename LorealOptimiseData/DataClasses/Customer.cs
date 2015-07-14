using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Text;
using System.Diagnostics;

namespace LorealOptimiseData
{
    public partial class Customer : IPrimaryKey, ITrackChanges, ICleanEntityRef, IDeletionLimit
    {
        private static long time = 0;

        private string storeCategories = null;
        public string StoreCategories
        {
            get
            {
                if (storeCategories != null)
                {
                    return storeCategories;
                }
                
                StringBuilder categories = new StringBuilder();
                foreach (CustomerCategory cc in this.CustomerCategories)
                {
                    if (cc.Category == null)
                    {
                        cc.Category = DbDataContext.GetInstance().Categories.SingleOrDefault(cat => cat.ID == cc.IDCategory);
                    }
                    categories.Append( ";"+cc.Category.Name + ";");
                }

                storeCategories = categories.ToString();

                return storeCategories;
            }
        }

        private double? retailSales = null;
        public double RetailSales
        {
            get
            {
                if(retailSales == null)
                {
                    if (this.ID == Guid.Empty)
                    {
                        retailSales = 0;
                    }
                    else
                    {
                        return TotalSales;
                    }
                        
                }

                if (retailSales.HasValue)
                {
                    return Math.Round(retailSales.Value, 2);
                }
                else
                {
                    return 0;
                }
            }
        }

        public int Rank
        {
            get;
            set;
        }

        public string FullName
        {
            get
            {
                string salesArea = CustomerGroup.SalesArea.Name;
                if (SalesArea != null)
                {
                    salesArea = SalesArea.Name;
                }

                return string.Format("{3} - {0} ({1} - {2})", Name, CustomerGroup.Name, salesArea, AccountNumber);
            }
        }

        public Division Division
        {
            get
            {
                if (SalesArea == null)
                {
                    return CustomerGroup.SalesArea.Division;
                }

                return SalesArea.Division;
            }
        }

        public Guid IDDivision
        {
            get
            {
                return Division.ID;
            }
        }

        private bool isSelected;
        /// <summary>
        /// This value is used on Generate capacity screen and indicates if the customer is selected
        /// </summary>
        public bool IsSelected
        {
            get
            {
                return isSelected;
            }
            set
            {
                isSelected = value;
                SendPropertyChanged("IsSelected");
            }
        }

        private List<CustomerCategory> activeCustomerCategories = null;
        public List<CustomerCategory> ActiveCustomerCategories
        {
            get
            {
                if(activeCustomerCategories == null)
                {
                    activeCustomerCategories = this.CustomerCategories.Where(cc => cc.Category.Deleted == false).OrderBy(cc=>cc.Category.Name).ToList();
                }
                return activeCustomerCategories ;
            }
            set
            {
                activeCustomerCategories = value;
            }
        }

        public void CleanEntityRef(string FieldName)
        {
            if (FieldName == "IDCustomerGroup")
            {
                this._CustomerGroup = default(EntityRef<CustomerGroup>);
            }
            else if (FieldName == "IDSalesArea_AllocationSalesArea")
            {
                this._SalesArea = default(EntityRef<SalesArea>);
            }
            else if (FieldName == "IDSalesEmployee")
            {
                this._SalesEmployee = default(EntityRef<SalesEmployee>);
            }
        }

        public bool CanBeDeleted(out string reasonMsg, out string warning)
        {
            reasonMsg = string.Empty;
            warning = string.Empty;
            if (this.Manual == false)
            {
                //reasonMsg = "This customer store cannot be deleted because it is not a dummy store.";
                reasonMsg = SystemMessagesManager.Instance.GetMessage("CustomerDelete");
                return false;
            }

            if (this.CustomerAllocations.Count != 0)
            {
                //reasonMsg = "This customer store cannot be deleted because it is attached to some animations.";
                reasonMsg = SystemMessagesManager.Instance.GetMessage("CustomerDeleteAttached");
                return false;
            }

            //warning = "If you delete this Customer Sotre, then all its sales & capacities data will also be deleted. Do you want to proceed?";
            warning = SystemMessagesManager.Instance.GetMessage("CustomerDeleteWarning");
            return true;
        }
    }
}
