using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LorealOptimiseData;

namespace LorealOptimiseBusiness.Lists
{
    public class ItemGroupManager : BaseManager, IListManager<ItemGroup>
    {
        public event EntityChangedEventHandler<ItemGroup> EntityChanged;

        public System.Data.Linq.DataLoadOptions LoadOptions
        {
            get
            {
                return null;
            }
        }

        private static ItemGroupManager instance = null;
        public static ItemGroupManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ItemGroupManager();
                }
                return instance;
            }
        }

        private static IEnumerable<ItemGroup> all = null;
        public IEnumerable<ItemGroup> GetAll()
        {
            if (all == null)
            {
                all = Db.ItemGroups.Where(DivisionFilter<ItemGroup>()).OrderBy(ig=>ig.Name).ToList();
            }
            return all;
        }

        public IEnumerable<ItemGroup> GetFiltered(Hashtable conditions)
        {
            return GetAll();
        }

        public void InsertOrUpdate(ItemGroup entity)
        {
            if (entity.ID == Guid.Empty)
            {
                Db.ItemGroups.InsertOnSubmit(entity);
            }

            Db.SubmitChanges();
            all = null;
        }

        public void Delete(ItemGroup entity)
        {
            Db.ItemGroups.DeleteOnSubmit(entity);
            Db.SubmitChanges();
            all = null;
        }

        public void Refresh()
        {
            all = null;
        }
    }
}
