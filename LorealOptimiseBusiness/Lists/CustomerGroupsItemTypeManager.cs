using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using LorealOptimiseData;

namespace LorealOptimiseBusiness.Lists
{
    public class CustomerGroupsItemTypeManager : BaseManager, IListManager<CustomerGroupItemType>
    {
        public event EntityChangedEventHandler<CustomerGroupItemType> EntityChanged;

        public System.Data.Linq.DataLoadOptions LoadOptions
        {
            get
            {
                return null;
            }
        }

        private static CustomerGroupsItemTypeManager instance = null;
        public static CustomerGroupsItemTypeManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CustomerGroupsItemTypeManager();
                }
                return instance;
            }
        }

        private static IEnumerable<CustomerGroupItemType> all = null;
        public IEnumerable<CustomerGroupItemType> GetAll()
        {
            if (all == null)
            {
                all = Db.CustomerGroupItemTypes.Where(c => c.CustomerGroup.SalesArea.IDDivision == LoggedUser.Division.ID && c.ItemType.IDDivision == LoggedUser.Division.ID).ToList();
            }
            return all;
        }

        public IEnumerable<CustomerGroupItemType> GetFiltered(Hashtable conditions)
        {
            return GetAll();
        }

        public void InsertOrUpdate(CustomerGroupItemType entity)
        {
            if (entity.ID == Guid.Empty)
            {
                Db.CustomerGroupItemTypes.InsertOnSubmit(entity);
            }

            try
            {
                Db.SubmitChanges();
            }
            catch (SqlException sqlExc)
            {
                if (sqlExc.Number == 50000 && sqlExc.Class == 16)
                {
                    CustomerGroupItemType originalEntity = new DbDataContext().CustomerGroupItemTypes.SingleOrDefault(cgit => cgit.ID == entity.ID);
                    entity.WarehouseAllocation = originalEntity.WarehouseAllocation;
                    entity.IDItemType = originalEntity.IDItemType;
                }
                throw;
            }

            all = null;
        }

        public void Delete(CustomerGroupItemType entity)
        {
            Db.CustomerGroupItemTypes.DeleteOnSubmit(entity);
            Db.SubmitChanges();
            all = null;
        }

        public void Refresh()
        {
            all = null;
        }
    }
}
