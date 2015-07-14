using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LorealOptimiseData;
using System.Data.SqlClient;
using System.Data.Linq;

namespace LorealOptimiseBusiness.Lists
{
    public class CustomerBrandExclusionManager : BaseManager, IListManager<CustomerBrandExclusion>
    {
        public event EntityChangedEventHandler<CustomerBrandExclusion> EntityChanged;

        public System.Data.Linq.DataLoadOptions LoadOptions
        {
            get
            {
                DataLoadOptions loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<CustomerBrandExclusion>(cge => cge.BrandAxe);
                loadOptions.LoadWith<CustomerBrandExclusion>(cge => cge.Customer);
                loadOptions.LoadWith<Customer>(c=>c.CustomerGroup);
                loadOptions.LoadWith<Customer>(c => c.SalesArea);

                return loadOptions;
            }
        }

        private static CustomerBrandExclusionManager instance = null;
        public static CustomerBrandExclusionManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CustomerBrandExclusionManager();
                }
                return instance;
            }
        }

        private static IEnumerable<CustomerBrandExclusion> all = null;
        public IEnumerable<CustomerBrandExclusion> GetAll()
        {
            if (all == null)
            {
                all = Db.CustomerBrandExclusions.Where(c => c.BrandAxe.Signature.IDDivision == LoggedUser.Division.ID
                    && c.Customer.SalesArea.IDDivision == LoggedUser.Division.ID).OrderBy(c=>c.BrandAxe.Name).ThenBy(c=>c.Customer.Name).ToList();
            }

            return all;
        }

        public IEnumerable<CustomerBrandExclusion> GetFiltered(Hashtable conditions)
        {
            return GetAll();
        }

        public void InsertOrUpdate(CustomerBrandExclusion entity)
        {
            if (entity.ID == Guid.Empty)
            {
                Db.CustomerBrandExclusions.InsertOnSubmit(entity);
            }

            try
            {
                Db.SubmitChanges();
            }
            catch (SqlException sqlexc)
            {
                if (sqlexc.Class == 16 && sqlexc.Number == 50000 && sqlexc.State==38)
                {
                    CustomerBrandExclusion originalEntity = Db.CustomerBrandExclusions.GetOriginalEntityState(entity);
                    entity.Excluded = originalEntity.Excluded;
                }
                throw;
            }

            all = null;
        }

        public void Delete(CustomerBrandExclusion entity)
        {
            Db.CustomerBrandExclusions.DeleteOnSubmit(entity);
            Db.SubmitChanges();
            all = null;
        }

        public void Refresh()
        {
            all = null;
        }

    }
}
