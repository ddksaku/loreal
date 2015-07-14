using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using LorealOptimiseData;

namespace LorealOptimiseBusiness.Lists
{
    public class SalesOrganizationManager : BaseManager, IListManager<SalesOrganization>
    {
        public event EntityChangedEventHandler<SalesOrganization> EntityChanged;

        public System.Data.Linq.DataLoadOptions LoadOptions
        {
            get
            {
                return null;
            }
        }

        private static SalesOrganizationManager instance = null;
        public static SalesOrganizationManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SalesOrganizationManager();
                }
                return instance;
            }
        }

        private static IEnumerable<SalesOrganization> all = null;
        public IEnumerable<SalesOrganization> GetAll()
        {
            if (all == null)
            {
                all = Db.SalesOrganizations.Where(s => s.Deleted == false).ToList();
            }

            return all;
        }

        public IEnumerable<SalesOrganization> GetFiltered(Hashtable conditions)
        {
            return GetAll();
        }

        public void InsertOrUpdate(SalesOrganization entity)
        {
            if (entity.ID == Guid.Empty)
            {
                Db.SalesOrganizations.InsertOnSubmit(entity);
            }

            Db.SubmitChanges();

            all = null;
        }

        public void Delete(SalesOrganization entity)
        {
            for (int i = entity.SalesAreas.Count - 1; i >= 0; i--)
            {
                SalesAreaManager.Instance.Delete(entity.SalesAreas[i]);
            }

            Db.SalesOrganizations.DeleteOnSubmit(entity);
            try
            {
                Db.SubmitChanges();
            }
            catch (SqlException e)
            {
                // conflict with foreign key
                if (e.Number == 547)
                {
                    Db.SalesOrganizations.InsertOnSubmit(entity);
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
    }
}
