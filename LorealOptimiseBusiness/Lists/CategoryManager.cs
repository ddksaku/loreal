using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using LorealOptimiseData;
using System.Collections.Specialized;

namespace LorealOptimiseBusiness.Lists
{
    public class CategoryManager : BaseManager, IListManager<Category>, INotifyCollectionChanged
    {
        public event EntityChangedEventHandler<Category> EntityChanged;

        public System.Data.Linq.DataLoadOptions LoadOptions
        {
            get
            {
                return null;
            }
        }

        private static CategoryManager instance = null;
        public static CategoryManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CategoryManager();
                }
                return instance;
            }
        }

        private static IEnumerable<Category> all = null;
        public IEnumerable<Category> GetAll()
        {
            if (all == null)
            {
                all = Db.Categories.Where(DivisionFilter<Category>()).Where(c => c.Deleted == false).OrderBy(c=>c.Name).ToList();
            }

            return all;
        }

        public IEnumerable<Category> GetFiltered(Hashtable conditions)
        {
            return GetAll();
        }

        public void InsertOrUpdate(Category entity)
        {
            if (entity.ID == Guid.Empty)
            {
                Db.Categories.InsertOnSubmit(entity);
            }

            Db.SubmitChanges();

            all = null;

            RaiseCollectionChanged(NotifyCollectionChangedAction.Add);
        }

        public void Delete(Category entity)
        {
            Db.Categories.DeleteOnSubmit(entity);
            try
            {
                Db.SubmitChanges();
            }
            catch (SqlException e)
            {
                // conflict with foreign key
                if (e.Number == 547)
                {
                    Db.Categories.InsertOnSubmit(entity);
                    entity.Deleted = true;
                    Db.SubmitChanges();
                }
            }

            all = null;

            RaiseCollectionChanged(NotifyCollectionChangedAction.Remove);
        }

        public void Refresh()
        {
            all = null;
        }

        private void RaiseCollectionChanged(NotifyCollectionChangedAction action)
        {
            if (CollectionChanged != null)
            {
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove));
            }
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;
    }
}
