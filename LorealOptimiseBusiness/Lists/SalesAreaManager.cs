using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using LorealOptimiseData;

namespace LorealOptimiseBusiness.Lists
{
    public class SalesAreaManager : BaseManager, IListManager<SalesArea>
    {
        public event EntityChangedEventHandler<SalesArea> EntityChanged;

        public System.Data.Linq.DataLoadOptions LoadOptions
        {
            get
            {
                return null;
            }
        }

        private static SalesAreaManager instance = null;
        public static SalesAreaManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SalesAreaManager();
                }

                return instance;
            }
        }

        private static IEnumerable<SalesArea> all = null;

        public IEnumerable<SalesArea> GetAll()
        {
            if (all == null)
            {
                all = Db.SalesAreas.Where(DivisionFilter<SalesArea>()).Where(s => s.Deleted == false).ToList();
            }

            return all;
        }

        public IEnumerable<SalesArea> GetFiltered(Hashtable conditions)
        {
            return GetAll();
        }

        public void InsertOrUpdate(SalesArea entity)
        {
            if (entity.ID == Guid.Empty)
            {
                Db.SalesAreas.InsertOnSubmit(entity);
            }

            Db.SubmitChanges();

            all = null;
        }

        public void Delete(SalesArea entity)
        {
            Db.SalesAreas.DeleteOnSubmit(entity);
            try
            {
                Db.SubmitChanges();
            }
            catch (SqlException e)
            {
                // conflict with foreign key
                if (e.Number == 547)
                {
                    Db.SalesAreas.InsertOnSubmit(entity);
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
