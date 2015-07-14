using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using LorealOptimiseBusiness.Lists;
using LorealOptimiseBusiness.ViewMode;
using LorealOptimiseData;
using System.Diagnostics;
using System.Collections.Generic;

namespace LorealOptimiseBusiness
{
    public class AnimationAllocations : BaseManager, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private static DbDataContext newDB;
        private static DbDataContext NewDB
        {
            get
            {
                if (newDB == null)
                {
                    newDB = new DbDataContext();
                }
                return newDB;
            }
            set
            {
                newDB = value;
            }
        }

        public static DbDataContext GetDataContext()
        {
            return NewDB;
        }

        private ObservableCollection<CustomerGroupAllocation> customerGroupsAllocation;
        public  ObservableCollection<CustomerGroupAllocation> CustomerGroupsAllocation
        {
            get
            {
                if (customerGroupsAllocation == null)
                {
                    if (productDetail != null)
                    {
                        customerGroupsAllocation = new ObservableCollection<CustomerGroupAllocation>(NewDB.CustomerGroupAllocations.Where(cga => cga.IDAnimationProductDetail == productDetail.ID).OrderBy(c => c.CustomerGroup.Name));
                    }
                }

                return customerGroupsAllocation;
            }
            set
            {
                customerGroupsAllocation = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("CustomerGroupsAllocation"));
                }
            }
        }

        private ExtendedObservableCollection<CustomerGroup> customerGroup;
        public ExtendedObservableCollection<CustomerGroup> CustomerGroups
        {
            get
            {
                if (customerGroup == null)
                {
                    customerGroup = new ExtendedObservableCollection<CustomerGroup>(ProductDetail.AnimationProduct.Animation.AnimationCustomerGroups.Select(acg => acg).Select(cg => cg.CustomerGroup).Distinct().Except(customerGroupsAllocation.Where(cga=>cga.ID!=Guid.Empty).Select(cga => cga.CustomerGroup)).OrderBy(cg => cg.Name));
                }
                return customerGroup;
            }
        }

        private CustomerGroupAllocation selectedAllocationCustomerGroup;
        public CustomerGroupAllocation SelectedAllocationCustomerGroup
        {
            get
            {
                return selectedAllocationCustomerGroup;
            }
            set
            {
                if (selectedAllocationCustomerGroup != value)
                {
                    selectedAllocationCustomerGroup = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("SelectedAllocationCustomerGroup"));
                    }
                }
            }
        }

        public CustomerGroup SelectedCustomerGroup
        {
            get
            {
                if (SelectedAllocationCustomerGroup == null)
                {
                    return null;
                }

                if (SelectedAllocationCustomerGroup.CustomerGroup == null)
                {
                    SelectedAllocationCustomerGroup.CustomerGroup = CustomerGroupManager.Instance.GetById(SelectedAllocationCustomerGroup.IDCustomerGroup);
                }

                return SelectedAllocationCustomerGroup.CustomerGroup;
            }
        }

        private ObservableCollection<CustomerAllocation> customersAllocation;
        public ObservableCollection<CustomerAllocation> CustomersAllocation
        {
            get
            {
                return customersAllocation;
            }
            set
            {
                customersAllocation = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("CustomersAllocation"));
                }
            }

        }

        private ExtendedObservableCollection<Customer> customers;
        public ExtendedObservableCollection<Customer> Customers
        {
            get
            {
                if (customers == null)
                {
                    if (SelectedCustomerGroup != null)
                    {
                        customers = new ExtendedObservableCollection<Customer>(SelectedCustomerGroup.Customers.Where(c => c.Deleted == false && c.IncludeInSystem == true).Except(customersAllocation.Where(ca=>ca.ID!=Guid.Empty).Select(ca => ca.Customer)).OrderBy(c => c.Name));
                    }
                    else
                    {
                        customers = new ExtendedObservableCollection<Customer>();
                    }
                }

                return customers;
            }
            set
            {
                customers = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Customers"));
                }
            }
        }

        private AnimationProductDetail productDetail;
        public AnimationProductDetail ProductDetail
        {
            get
            {
                return productDetail;
            }
        }

        public void RefreshAllocations()
        {
            NewDB = null;
            CustomerGroupsAllocation = null;
        }

        public AnimationAllocations(AnimationProductDetail apd)
        {
            RefreshAllocations();

            PropertyChanged -= AnimationAllocations_PropertyChanged;
            PropertyChanged +=AnimationAllocations_PropertyChanged;

            productDetail = apd;

            CustomerGroupsAllocation.CollectionChanged -= new NotifyCollectionChangedEventHandler(CustomerGroupsAllocation_CollectionChanged);
            CustomerGroupsAllocation.CollectionChanged +=new NotifyCollectionChangedEventHandler(CustomerGroupsAllocation_CollectionChanged);

            apd.AnimationProduct.Animation.ObservableAnimationCustomerGroups.CollectionChanged += new NotifyCollectionChangedEventHandler(ObservableAnimationCustomerGroups_CollectionChanged);
        }

        public void UpdateTotalFixedAllocation()
        {
            productDetail.TotalFixedAllocation = -1;
        }

        void ObservableAnimationCustomerGroups_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            customerGroup = null;
        }

        void ca_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ID")
            {
                customers = null;

            }
        }

        void CustomerGroupsAllocation_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                CustomerGroupAllocation acg = e.NewItems[0] as CustomerGroupAllocation;
                acg.PropertyChanged +=new PropertyChangedEventHandler(acg_PropertyChanged);
                // acg.AnimationProductDetail = productDetail;
                acg.IDAnimationProductDetail = productDetail.ID;
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                CustomerGroupAllocation acg = e.OldItems[0] as CustomerGroupAllocation;

                if (acg.CustomerGroup != null)
                {
                    customerGroup = null;
                    acg.CustomerGroup.CustomerGroupAllocations.Remove(acg);
                }
                productDetail.CustomerGroupAllocations.Remove(acg);
            }
        }

        void acg_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ID")
            {
                customerGroup = null;
            }
        }

        void AnimationAllocations_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedAllocationCustomerGroup")
            {
                if (SelectedAllocationCustomerGroup != null)
                {
                    CustomersAllocation =
                        new ObservableCollection<CustomerAllocation>(SelectedAllocationCustomerGroup.CustomerAllocations);
                }
                else
                {
                    CustomersAllocation = new ObservableCollection<CustomerAllocation>();
                }
                customers = null;
            }
        }

        public ICommand CopyToFixedAllocationCommand
        {
            get
            {
                return new RelayCommand(param => true, param => CopyToFixedAllocation());
            }
        }

        public void CopyToFixedAllocation()
        {
            foreach (CustomerGroupAllocation cg in CustomerGroupsAllocation)
            {
                cg.ManualFixedAllocation = cg.SystemFixedAllocation;
            }

            try
            {
                NewDB.SubmitChanges();
            }
            catch(Exception exc)
            {
                System.Windows.MessageBox.Show(exc.Message);
            }

        }
    }
}
