using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using LorealOptimiseBusiness.ViewMode;
using LorealOptimiseData;

namespace LorealOptimiseBusiness.Lists
{
    public class AnimationTypeManager : BaseManager, IListManager<AnimationType>
    {
        public event EntityChangedEventHandler<AnimationType> EntityChanged;

        public System.Data.Linq.DataLoadOptions LoadOptions
        {
            get
            {
                return null;
            }
        }

        private static AnimationTypeManager instance = null;
        public static AnimationTypeManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AnimationTypeManager();    
                }

                return instance;
            }
        }

        private static IEnumerable<AnimationType> all = null;
        public IEnumerable<AnimationType> GetAll()
        {
            if (all == null)
            {
                all = new ExtendedObservableCollection<AnimationType>(Db.AnimationTypes.Where(DivisionFilter<AnimationType>()).Where(t=>t.Deleted==false).OrderBy(at=>at.Name));
            }
        
            return all;
        }

        public IEnumerable<AnimationType> GetFiltered(Hashtable conditions)
        {
            return GetAll();
        }

        public void InsertOrUpdate(AnimationType entity)
        {
            if (entity.ID == Guid.Empty)
            {
                Db.AnimationTypes.InsertOnSubmit(entity);
            }
            Db.SubmitChanges();

            all = null;
        }

        public void Delete(AnimationType entity)
        {
            try
            {
                Db.CustomerCapacities.DeleteAllOnSubmit(entity.CustomerCapacities);
                Db.AnimationTypes.DeleteOnSubmit(entity);
                Db.SubmitChanges();
            }
            catch (SqlException e)
            {
                // conflict with foreign key
                if (e.Number == 547)
                {
                    Db.AnimationTypes.InsertOnSubmit(entity);
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
