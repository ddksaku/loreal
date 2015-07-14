
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using LorealOptimiseData;

namespace LorealOptimiseBusiness.Lists
{
    public class PriorityManager : BaseManager, IListManager<Priority>
    {
        public event EntityChangedEventHandler<Priority> EntityChanged;

        public System.Data.Linq.DataLoadOptions LoadOptions
        {
            get
            {
                return null;
            }
        }

        private static PriorityManager instance = null;
        public static PriorityManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PriorityManager();
                }
                return instance;
            }
        }

        private static IEnumerable<Priority> all = null;
        public IEnumerable<Priority> GetAll()
        {
            if (all == null)
            {
                all = Db.Priorities.Where(DivisionFilter<Priority>()).Where(p => p.Deleted == false).OrderBy(it=>it.Name).ToList();
            }

            return all;
        }

        public IEnumerable<Priority> GetFiltered(Hashtable conditions)
        {
            return GetAll();
        }

        public void InsertOrUpdate(Priority entity)
        {
            if (entity.ID == Guid.Empty)
            {
                Db.Priorities.InsertOnSubmit(entity);
            }

            Db.SubmitChanges();

            all = null;
        }

        public void Delete(Priority entity)
        {
            try
            {
                Db.CustomerCapacities.DeleteAllOnSubmit(entity.CustomerCapacities);
                Db.Priorities.DeleteOnSubmit(entity);
                Db.SubmitChanges();
            }
            catch (SqlException e)
            {
                // conflict with foreign key
                if (e.Number == 547)
                {
                    Db.Priorities.InsertOnSubmit(entity);
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
