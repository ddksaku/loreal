using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using LorealOptimiseData;

namespace LorealOptimiseBusiness.Lists
{
    public class RetailerTypeManager : BaseManager, IListManager<RetailerType>
    {
        public event EntityChangedEventHandler<RetailerType> EntityChanged;

        public System.Data.Linq.DataLoadOptions LoadOptions
        {
            get
            {
                return null;
            }
        }

        private static RetailerTypeManager instance = null;
        public static RetailerTypeManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new RetailerTypeManager();
                }
                return instance;
            }
        }

        private static IEnumerable<RetailerType> all = null;
        public IEnumerable<RetailerType> GetAll(bool includeDeleted)
        {
            if (all == null)
            {
                all = Db.RetailerTypes.Where(DivisionFilter<RetailerType>()).OrderBy(r=>r.Name).ToList();
            }

            if (!includeDeleted)
            {
                return all.Where(rt => rt.Deleted == false);
            }

            return all;
        }

        public IEnumerable<RetailerType> GetAll()
        {
            return GetAll(false);
        }

        public IEnumerable<RetailerType> GetFiltered(Hashtable conditions)
        {
            return GetAll(false);
        }

        public void InsertOrUpdate(RetailerType entity)
        {
            if (entity.ID == Guid.Empty)
            {
                Db.RetailerTypes.InsertOnSubmit(entity);
            }

            if (entity.Default == true)
            {
                List<RetailerType> defaultRetailerTypes = LoggedUser.Division.RetailerTypes.Where(rt => rt.Default == true && rt.ID != entity.ID).ToList();
                if (defaultRetailerTypes != null)
                {
                    defaultRetailerTypes.ForEach(rt => rt.Default = false);
                }
            }

            Db.SubmitChanges();

            all = null;
        }

        public void Delete(RetailerType entity)
        {
            try
            {
                Db.RetailerTypes.DeleteOnSubmit(entity);
                Db.SubmitChanges();
            }
            catch (SqlException e)
            {
                // conflict with foreign key
                if (e.Number == 547)
                {
                    Db.RetailerTypes.InsertOnSubmit(entity);
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
