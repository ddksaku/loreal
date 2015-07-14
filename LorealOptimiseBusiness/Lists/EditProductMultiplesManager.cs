using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LorealOptimiseBusiness.Lists;
using LorealOptimiseData;
using LorealOptimiseBusiness.ViewMode;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace LorealOptimiseBusiness
{
    public class EditProductMultiplesManager : BaseManager, IModify<Multiple>
    {
        private static EditProductMultiplesManager instance = null;
        public static EditProductMultiplesManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EditProductMultiplesManager();
                }
                return instance;
            }
        }

        private Product editEntity;
        public Product EditEntity
        {
            get
            {
                return editEntity;
            }
            set
            {
                editEntity = value;
                data = null;
            }
        }

        private ExtendedObservableCollection<Multiple> data;
        public ExtendedObservableCollection<Multiple> Data
        {
            get
            {
                if (data == null && EditEntity != null)
                {
                    data = new ExtendedObservableCollection<Multiple>(Db.Multiples.Where(m => m.IDProduct == EditEntity.ID));
                }
                return data;
            }
            set
            {
                data = value;
            }
        }

        //private static IEnumerable<Multiple> all = null;
        //public IEnumerable<Multiple> GetAll()
        //{
        //    if(all == null)
        //    {
        //        MultipleManager.Instance.GetAll().Where(m => m.IDProduct == EditEntity.ID).ToList();
        //    }
        //    return all;
        //}

        #region IModify<Multiple> Members

        public void InsertOrUpdate(Multiple entity)
        {
            if (entity.ID == Guid.Empty)
            {
                entity.IDProduct = EditEntity.ID;
                entity.Manual = true;
            }

            MultipleManager.Instance.InsertOrUpdate(entity);
        }

        public void Delete(Multiple entity)
        {
            MultipleManager.Instance.Delete(entity);
        }

        public event EntityChangedEventHandler<Multiple> EntityChanged;

        #endregion
    }
}
