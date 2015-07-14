using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Text;
using LorealOptimiseData;

namespace LorealOptimiseBusiness.Lists
{
    public class SalesEmployeeManager : BaseManager, IListManager<SalesEmployee>
    {
        public event EntityChangedEventHandler<SalesEmployee> EntityChanged;

        public System.Data.Linq.DataLoadOptions LoadOptions
        {
            get
            {
                return null;
            }
        }

        private static SalesEmployeeManager instance = null;
        public static SalesEmployeeManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SalesEmployeeManager();
                }
                return instance;
            }
        }

        private static IEnumerable<SalesEmployee> all = null;
        public IEnumerable<SalesEmployee> GetAll()
        {
            if (all == null)
            {
                 all = Db.SalesEmployees.Where(DivisionFilter<SalesEmployee>()).OrderBy(cg => cg.Name).ToList();
            }
            return all;
        }

        public IEnumerable<SalesEmployee> GetFiltered(Hashtable conditions)
        {
            
            return GetAll();
        }

        public void InsertOrUpdate(SalesEmployee entity)
        {
            if (entity.ID == Guid.Empty)
            {
                Db.SalesEmployees.InsertOnSubmit(entity);
            }
            Db.SubmitChanges();
            all = null;
        }

        public void Delete(SalesEmployee entity)
        {
            Db.SalesEmployees.DeleteOnSubmit(entity);
            Db.SubmitChanges();
            all = null;
        }

        public void Refresh()
        {
            all = null;
        }
    }
}
