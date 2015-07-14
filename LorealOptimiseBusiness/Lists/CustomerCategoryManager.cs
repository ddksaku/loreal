using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LorealOptimiseData;
using System.Data.SqlClient;
using LorealOptimiseBusiness.Exceptions;

namespace LorealOptimiseBusiness.Lists
{
    public class CustomerCategoryManager : BaseManager, IListManager<CustomerCategory>
    {
        public event EntityChangedEventHandler<CustomerCategory> EntityChanged;

        public System.Data.Linq.DataLoadOptions LoadOptions
        {
            get
            {
                return null;
            }
        }

        private static CustomerCategoryManager instance = null;
        public static CustomerCategoryManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CustomerCategoryManager();
                }
                return instance;
            }
        }

        private static IEnumerable<CustomerCategory> all = null;
        public IEnumerable<CustomerCategory> GetAll()
        {
            if (all == null)
            {
                all = Db.CustomerCategories.Where(cc => cc.Category.IDDivision == LoggedUser.Division.ID
                    && cc.Customer.SalesArea.IDDivision == LoggedUser.Division.ID).ToList();
            }
            return all;
        }

        public IEnumerable<CustomerCategory> GetFiltered(Hashtable conditions)
        {
            return GetAll();
        }

        public void InsertOrUpdate(CustomerCategory entity)
        {
            if (entity.ID == Guid.Empty)
            {
                Db.CustomerCategories.InsertOnSubmit(entity);
            }
            Db.SubmitChanges();
            all = null;
        }

        public void Delete(CustomerCategory entity)
        {
            try
            {
                Db.CustomerCategories.DeleteOnSubmit(entity);
                Db.SubmitChanges();
            }
            catch (SqlException sqlExc)
            {
                if (sqlExc.Number == 50000 && sqlExc.Class == 16 && sqlExc.State == 36)
                {
                    Db.CustomerCategories.InsertOnSubmit(entity);
                    Db.SubmitChanges();
                    throw;
                }
            }

            all = null;
        }

        public void Refresh()
        {
            all = null;
        }
    }
}
