using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using LorealOptimiseData;

namespace LorealOptimiseBusiness.Lists
{
    public class AnimationProductDetailManager : BaseManager, IModify<AnimationProductDetail>
    {
        public event EntityChangedEventHandler<AnimationProductDetail> EntityChanged;

        private static AnimationProductDetailManager instance = null;
        public static AnimationProductDetailManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AnimationProductDetailManager();
                }
                return instance;
            }
        }

        public void InsertOrUpdate(AnimationProductDetail entity)
        {
            bool isEntityChanged = Db.AnimationProductDetails.GetModifiedMembers(entity).Count() > 0;


            if (entity.ID == Guid.Empty)
            {
                Db.AnimationProductDetails.InsertOnSubmit(entity);
            }

            try
            {
                Db.SubmitChanges();
            }
            catch (SqlException exc)
            {
                // trigger
                if (exc.Number == 50000 && exc.Class == 16)
                {
                    AnimationProductDetail originalEntity = new DbDataContext().AnimationProductDetails.SingleOrDefault(apd => apd.ID == entity.ID);
                    if (originalEntity != null)
                    {
                        entity.AllocationQuantity = originalEntity.AllocationQuantity;
                        Db.SubmitChanges();
                    }
                    else
                    {
                        Delete(entity);
                        AnimationManager.GetInstance().Animation.ObservableProductDetails.Remove(entity);
                    }
                }
                throw;
            }

            if(EntityChanged != null && isEntityChanged)
                EntityChanged(this, entity);
        }

        public void Delete(AnimationProductDetail entity)
        {
            Db.AnimationProductDetails.DeleteOnSubmit(entity);
            Db.SubmitChanges();

        }

    }
}
