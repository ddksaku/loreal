using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using LorealOptimiseData;
using System.Collections;
using System.Data.Linq.SqlClient;
using LorealOptimiseShared;
using System.Data.SqlClient;
using LorealOptimiseBusiness.Exceptions;
using System.Data.Linq;

namespace LorealOptimiseBusiness.Lists
{
    public class CustomerCapacityManager : BaseManager, IListManager<CustomerCapacity>
    {
        public event EntityChangedEventHandler<CustomerCapacity> EntityChanged;

        public System.Data.Linq.DataLoadOptions LoadOptions
        {
            get
            {
                DataLoadOptions loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<CustomerCapacity>(c => c.Customer);
                loadOptions.LoadWith<CustomerCapacity>(c => c.AnimationType);
                loadOptions.LoadWith<CustomerCapacity>(c => c.ItemType);
                loadOptions.LoadWith<CustomerCapacity>(c => c.Priority);
                loadOptions.LoadWith<CustomerGroup>(cg => cg.SalesArea);
                loadOptions.LoadWith<SalesArea>(sa => sa.SalesOrganization);

                loadOptions.LoadWith<Customer>(c=>c.CustomerGroup);

                return loadOptions;
            }
        }

        public const string IDAnimationType = "IDAnimationType";
        public const string IDPriority = "IDPriority";
        public const string IDItemType = "IDItemType";
        public const string CustomerName = "CustomerName";

        private static CustomerCapacityManager instance = null;
        public static CustomerCapacityManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CustomerCapacityManager();
                }
                return instance;
            }
        }


        protected Expression<Func<CustomerCapacity, bool>> defaultFilter()
        {
            return c => c.AnimationType.IDDivision == LoggedUser.Division.ID
                        && c.ItemType.IDDivision == LoggedUser.Division.ID
                        && c.Priority.IDDivision == LoggedUser.Division.ID
                        && c.Customer.SalesArea.IDDivision == LoggedUser.Division.ID
                        && c.Priority.Deleted == false
                        && c.ItemType.Deleted == false
                        && c.AnimationType.Deleted == false
                        && c.Customer.Deleted == false;
        }

        private static IEnumerable<CustomerCapacity> all = null;
        public IEnumerable<CustomerCapacity> GetAll()
        {
            if (all == null)
            {
                all = Db.CustomerCapacities.Where(defaultFilter()).ToList();
            }
            return all;
        }

        public IEnumerable<CustomerCapacity> GetFiltered(Hashtable conditions)
        {
            var result = Db.CustomerCapacities.AsQueryable();

            if (conditions != null)
            {
                if (conditions.ContainsKey(CustomerName) && !String.IsNullOrEmpty(conditions[CustomerName].ToString()))
                {
                    string condition = "%" + conditions[CustomerName] + "%";

                    result = result.Where(c => SqlMethods.Like(c.Customer.Name, condition));
                }

                Guid animationType = Guid.Empty;
                Guid itemType = Guid.Empty;
                Guid priority = Guid.Empty;

                if (conditions.ContainsKey(IDAnimationType) && conditions[IDAnimationType].ToString() != Guid.Empty.ToString() && conditions[IDAnimationType] != null && conditions[IDAnimationType].ToString().IsValidGuid())
                {
                    animationType = (Guid)conditions[IDAnimationType];
                    result = result.Where(c => c.IDAnimationType == animationType);
                }

                if (conditions.ContainsKey(IDPriority) && conditions[IDPriority].ToString() != Guid.Empty.ToString() &&  conditions[IDPriority] != null && conditions[IDPriority].ToString().IsValidGuid())
                {
                    priority = (Guid)conditions[IDPriority];
                    result = result.Where(c => c.IDPriority == priority);
                }

                if (conditions.ContainsKey(IDItemType) && conditions[IDItemType].ToString() != Guid.Empty.ToString() && conditions[IDItemType] != null && conditions[IDItemType].ToString().IsValidGuid())
                {
                    itemType = (Guid)conditions[IDItemType];
                    result = result.Where(c => c.IDItemType == itemType);
                }
            }

            result = result.Where(defaultFilter());

            return result.ToList();
        }

        public IEnumerable<CustomerCapacity> GetAll(Customer customer)
        {
            return Db.CustomerCapacities.Where(c => c.AnimationType.IDDivision == LoggedUser.Division.ID
                    && c.ItemType.IDDivision == LoggedUser.Division.ID
                    && c.Priority.IDDivision == LoggedUser.Division.ID
                    && c.Customer.SalesArea.IDDivision == LoggedUser.Division.ID
                    && c.IDCustomer == customer.ID).ToList();
        }

        public void InsertOrUpdate(CustomerCapacity entity)
        {
            if (entity.ID == Guid.Empty)
            {
                Db.CustomerCapacities.InsertOnSubmit(entity);
            }


            try
            {
                Db.SubmitChanges();
            }
            catch (SqlException exc)
            {
                if (exc.Number == 50000 && exc.Class == 16)
                {
                    CustomerCapacity originalEntity = new DbDataContext().CustomerCapacities.SingleOrDefault(cc => cc.ID == entity.ID);
                    
                    if (originalEntity != null)
                    {
                        entity.Capacity = originalEntity.Capacity;
                        Db.SubmitChanges();
                    }
                    else
                    {
                        Delete(entity);
                    }
                }

                throw;
            }

            all = null;
        }

        public void Delete(CustomerCapacity entity)
        {
            Db.CustomerCapacities.DeleteOnSubmit(entity);
            Db.SubmitChanges();
            all = null;
        }

        public void Refresh()
        {
            all = null;
        }
    }
}
