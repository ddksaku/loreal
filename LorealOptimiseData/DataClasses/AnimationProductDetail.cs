using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Data.Linq;
using System.Text;
using LorealOptimiseShared;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace LorealOptimiseData
{
    /// <summary>
    /// 
    /// </summary>
    public partial class AnimationProductDetail : IPrimaryKey, ITrackChanges, ICleanEntityRef, IDeletionLimit
    {
        private int? maximumCapacity;

        private ObservableCollection<CustomerGroupAllocation> observableCustomerGroupAllocations = null;
        public ObservableCollection<CustomerGroupAllocation> ObservableCustomerGroupAllocations
        {
            get
            {
                if (observableCustomerGroupAllocations == null)
                {
                    observableCustomerGroupAllocations = new ObservableCollection<CustomerGroupAllocation>(CustomerGroupAllocations.OrderBy(cg=>cg.CustomerGroup.Name));
                }

                return observableCustomerGroupAllocations;
            }
        }

        private void calculate()
        {
            maximumCapacity = DbDataContext.GetInstance().calculate_ProductDetailTotals(ID);
        }

        partial void OnLoaded()
        {
            // ObservableCustomerGroupAllocations.CollectionChanged += new NotifyCollectionChangedEventHandler(observableCustomerGroupAllocations_CollectionChanged);   
        }

        void observableCustomerGroupAllocations_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //PropertyChanged(this, new PropertyChangedEventArgs("TotalFixedAllocation"));
            //totalFixedAllocation = null;

            //if (e.Action == NotifyCollectionChangedAction.Add)
            //{
            //    (e.NewItems[0] as AnimationCustomerGroup).PropertyChanged += new PropertyChangedEventHandler(FixedAllocation_PropertyChanged);
            //}
        }

        void FixedAllocation_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "FixedAllocation" || e.PropertyName == "ManualFixedAllocation")
            {
                totalFixedAllocation = null;
            }
        }

        public Guid IDItemGroup
        {
            get;
            set;
        }

        public Guid IDProduct
        {
            get;
            set;
        }

        public string ProductCountryIdentifier
        {
            get
            {
                return AnimationProduct.ProductIdentifier + "/" + SalesArea.Name;
            }
        }


        public int MaximumCapacity
        {
            get
            {
                if (maximumCapacity == null)
                {
                    calculate();
                }

                return maximumCapacity.Value;
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

        public double ListPrice
        {
            get
            {
                if (SalesArea != null && RRP.HasValue && SalesArea.RRPToListPriceRate.HasValue)
                {
                    return SalesArea.RRPToListPriceRate.Value * (double)RRP.Value;
                }

                return 0;
            }
        }

        public double CostValue
        {
            get
            {
                if (AnimationProduct != null && ForecastProcQuantity.HasValue)
                {
                    return ForecastProcQuantity.Value * (double)AnimationProduct.Product.CIV;
                }

                return 0;
            }
        }

        public string GroupColumn
        {
            get
            {
                if (AnimationProduct != null)
                {
                    return AnimationProduct.ItemGroup.Name + " " + AnimationProduct.Product.MaterialCode + " " +
                           AnimationProduct.Product.Description;
                }

                return String.Empty;
            }
        }

        private int? allocationRemainder = null;
        public int AllocationRemainder
        {
            get
            {
                if (allocationRemainder == null)
                {
                    allocationRemainder = DbDataContext.GetInstance().calculate_AllocationRemainder(this.ID);
                }

                if (allocationRemainder.HasValue)
                    return allocationRemainder.Value;
                return 0;
            }
        }

        private int? totalFixedAllocation = null;
        public int TotalFixedAllocation
        {
            get
            {
                if (totalFixedAllocation == null)
                {
                    totalFixedAllocation = 0;

                    DbDataContext Db = DbDataContext.GetInstance();
                    IEnumerable<CustomerGroupAllocation> cgAllocations = Db.CustomerGroupAllocations.Where(cga => cga.IDAnimationProductDetail == this.ID).ToList();

                    foreach (CustomerGroupAllocation cga in cgAllocations)
                    {
                        if (cga.ManualFixedAllocation.HasValue)
                        {
                            totalFixedAllocation += cga.ManualFixedAllocation.Value;
                        }
                        else
                        {
                            IEnumerable<CustomerAllocation> customerAllocations = cga.CustomerAllocations.ToList();

                            foreach (CustomerAllocation ca in customerAllocations)
                            {
                                if (ca.FixedAllocation.HasValue)
                                {
                                    totalFixedAllocation += ca.FixedAllocation.Value;
                                }
                            }
                        }
                    }
                }

                return totalFixedAllocation.Value;
            }
            set
            {
                if (value == -1)
                {
                    totalFixedAllocation = null;
                    SendPropertyChanged("TotalFixedAllocation");
                }
            }
        }

        public bool IsValid(out string errorMessage)
        {
            return Utility.IsValid(this, out errorMessage);
        }

        public void CleanEntityRef(string fieldName)
        {
            if (fieldName == "IDAnimationProduct")
            {
                this._AnimationProduct = default(EntityRef<AnimationProduct>);
            }
            else if (fieldName == "IDSalesArea")
            {
                this._SalesArea = default(EntityRef<SalesArea>);
            }
        }

        public bool CanBeDeleted(out string reasonMsg, out string warning)
        {
            reasonMsg = string.Empty;
            warning = string.Empty;
            //if (this.CustomerGroupAllocations.Count != 0 || this.CustomerAllocations.Count != 0)
            //{
            //    reasonMsg = string.Format("This animation product detail has one or more fixed allocations.");
            //    return false;
            //}

            return false;
        }
    }
}
