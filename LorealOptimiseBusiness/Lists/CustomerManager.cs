using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Text;
using LorealOptimiseData;
using LorealOptimiseBusiness.Exceptions;
using System.Data.Linq.SqlClient;
using LorealOptimiseShared;
using LorealOptimiseData.Enums;
using System.Data.Linq;
using System.ComponentModel;


namespace LorealOptimiseBusiness.Lists
{
    public class CustomerManager : BaseManager, IListManager<Customer>, INotifyPropertyChanged
    {
        public event EntityChangedEventHandler<Customer> EntityChanged;

        public System.Data.Linq.DataLoadOptions LoadOptions
        {
            get
            {
                DataLoadOptions loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<Customer>(c => c.SalesArea);
                loadOptions.LoadWith<Customer>(c => c.SalesEmployee);
                loadOptions.LoadWith<Customer>(c => c.CustomerGroup);

                loadOptions.LoadWith<Customer>(c => c.CustomerCategories);
                loadOptions.LoadWith<CustomerCategory>(cc => cc.Category);

                return loadOptions;
            }
        }

        public const string CustomerName = "CustomerName";
        public const string AccountNumber = "AccountNumber";
        public const string IDSalesEmployee = "IDSalesEmployee";
        public const string IDCustomerGroup = "IDCustomerGroup";

        private static CustomerManager instance = null;
        public static CustomerManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CustomerManager();
                }
                return instance;
            }
        }

        public CustomerManager()
        {
            instance = this;
        }

        protected Expression<Func<Customer, bool>> defaultFilter()
        {
            return c => (c.CustomerGroup.SalesArea.IDDivision == LoggedUser.Division.ID || c.SalesArea.IDDivision == LoggedUser.Division.ID) 
                && c.Deleted == false
                && c.CustomerGroup.SalesArea.Deleted == false 
                && c.CustomerGroup.SalesArea.DistributionChannel.Deleted == false 
                && c.CustomerGroup.SalesArea.SalesOrganization.Deleted == false;
        }

        private static IEnumerable<Customer> all = null;

        private void getAll()
        {
            all = Db.Customers.Where(defaultFilter()).OrderBy(c=>c.Name).ToList();
        }

        public IEnumerable<Customer> GetAll()
        {
            if (all == null)
            {
                //LongTaskExecutor removeAllExecutor = new LongTaskExecutor(new Action(getAll), null);
                //removeAllExecutor.TaskStarted += new EventHandler(removeAllExecutor_TaskStarted);
                //removeAllExecutor.TaskFinished += new EventHandler(removeAllExecutor_TaskFinished);
                //removeAllExecutor.Run();

                getAll();
            }

            return all;
        }

        public IEnumerable<Customer> GetFiltered(Hashtable conditions)
        {
            var result = Db.Customers.AsQueryable();

            if (conditions != null)
            {
                if (conditions.ContainsKey(CustomerName) && !String.IsNullOrEmpty(conditions[CustomerName].ToString()))
                {
                    string condition = "%" + conditions[CustomerName] + "%";

                    result = result.Where(c => SqlMethods.Like(c.Name, condition));
                }
                if (conditions.Contains(AccountNumber) && !String.IsNullOrEmpty(conditions[AccountNumber].ToString()))
                {
                    string condition = "%" + conditions[AccountNumber] + "%";

                    result = result.Where(c => SqlMethods.Like(c.AccountNumber, condition));
                }

                Guid salesEmployee = Guid.Empty;
                Guid customerGroup = Guid.Empty;

                if (conditions.ContainsKey(IDCustomerGroup) && conditions[IDCustomerGroup].ToString() != Guid.Empty.ToString() && conditions[IDCustomerGroup] != null && conditions[IDCustomerGroup].ToString().IsValidGuid())
                {
                    customerGroup = (Guid)conditions[IDCustomerGroup];
                    result = result.Where(c => c.IDCustomerGroup == customerGroup);
                }

                if (conditions.ContainsKey(IDSalesEmployee) && conditions[IDSalesEmployee].ToString() != Guid.Empty.ToString() && conditions[IDSalesEmployee] != null && conditions[IDSalesEmployee].ToString().IsValidGuid())
                {
                    salesEmployee = (Guid)conditions[IDSalesEmployee];
                    result = result.Where(c => c.IDSalesEmployee == salesEmployee);
                }

            }

            result = result.Where(defaultFilter());

            IEnumerable<Customer> resultList = result.ToList().OrderByDescending(c => c.RetailSales);
            int rank = 0;
            double prevRetailSales = -1;
            foreach (Customer c in resultList)
            {
                if (prevRetailSales != c.RetailSales)
                {
                    prevRetailSales = c.RetailSales;
                    rank++;
                }
                c.Rank = rank;
            }

            return resultList;
        }

        public void InsertOrUpdate(Customer entity)
        {
            bool isUpdate = true;

            if (entity.ID == Guid.Empty)
            {
                Db.Customers.InsertOnSubmit(entity);
                isUpdate = false;
            }

            try
            {
                if (Db.Customers.GetModifiedMembers(entity).Any(m => m.Member.Name == "IDSalesArea_AllocationSalesArea") == false)
                {
                    isUpdate = false;
                }

                Db.SubmitChanges();

                if (isUpdate)
                {
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("NewCustomerGroup"));
                    }
                    
                    Db.Refresh(RefreshMode.OverwriteCurrentValues, entity);
                }
            }
            catch (SqlException sqlexc)
            {
                if (sqlexc.Number == 50000 && sqlexc.Class == 16 && sqlexc.State == 37)
                {
                    Customer originalEntity = Db.Customers.GetOriginalEntityState(entity);
                    entity.IncludeInSystem = originalEntity.IncludeInSystem;
                }
                throw;
            }

            all = null;
        }

        public void Delete(Customer entity)
        {
            try
            {
                Db.CustomerCapacities.DeleteAllOnSubmit(entity.CustomerCapacities);
                Db.Sales.DeleteAllOnSubmit(entity.Sales);
                Db.Sales.DeleteAllOnSubmit(entity.Sales1);

                entity.Deleted = true;
                Db.Customers.DeleteOnSubmit(entity);
                Db.SubmitChanges();
            }
            catch (SqlException e)
            {
                // conflict with foreign key
                if (e.Number == 547)
                {
                    Db.Customers.InsertOnSubmit(entity);
                    entity.Deleted = true;
                    Db.SubmitChanges();
                }
            }
            all = null;
        }

        public void Refresh()
        {
            all = null;
        }

        public bool CanChangeSalesArea(Guid idCustomer, ref string errorMessage)
        {
            DbDataContext simpleContext = new DbDataContext(false);

            List<Animation> animations = simpleContext.CustomerAllocations.Where(ca => ca.IDCustomer == idCustomer
                && ca.AnimationProductDetail.AnimationProduct.Animation.Status != (byte)AnimationStatus.Cleared
                && ca.AnimationProductDetail.AnimationProduct.Animation.Status != (byte)AnimationStatus.Closed).Select(ca => ca.AnimationProductDetail.AnimationProduct.Animation).Distinct().ToList();

            if (animations.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (Animation a in animations)
                {
                    sb.AppendLine(a.Name);
                }

                errorMessage += SystemMessagesManager.Instance.GetMessage("CannotChangeStoreSalesArea", sb.ToString());

                return false;
            }
            else
            {
                return true;
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
