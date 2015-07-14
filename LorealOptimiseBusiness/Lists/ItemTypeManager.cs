using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Text;
using LorealOptimiseData;

namespace LorealOptimiseBusiness.Lists
{
    public class ItemTypeManager : BaseManager, IListManager<ItemType>
    {
        public event EntityChangedEventHandler<ItemType> EntityChanged;

        public System.Data.Linq.DataLoadOptions LoadOptions
        {
            get
            {
                return null;
            }
        }

        private static ItemTypeManager instance = null;
        public static ItemTypeManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ItemTypeManager();
                }
                return instance;
            }
        }

        private static IEnumerable<ItemType> all = null;
        public IEnumerable<ItemType> GetAll()
        {
            if (all == null)
            {
                all = Db.ItemTypes.Where(DivisionFilter<ItemType>()).Where(it => it.Deleted == false).OrderBy(i=>i.Name).ToList();
            }
            return all;
        }

        public IEnumerable<ItemType> GetFiltered(Hashtable conditions)
        {
            return GetAll();
        }

        public void InsertOrUpdate(ItemType entity)
        {
            if (entity.ID == Guid.Empty)
            {
                Db.ItemTypes.InsertOnSubmit(entity);
            }

            Db.SubmitChanges();
            all = null;
        }

        public void Delete(ItemType entity)
        {
            try
            {
                Db.CustomerCapacities.DeleteAllOnSubmit(entity.CustomerCapacities);
                Db.ItemTypes.DeleteOnSubmit(entity);
                Db.SubmitChanges();
            }
            catch(SqlException exc)
            {
                if (exc.Number == 547)
                {
                    Db.ItemTypes.InsertOnSubmit(entity);
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
