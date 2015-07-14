using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LorealOptimiseData;

namespace LorealOptimiseBusiness.Lists
{
    public class AnimationManager : BaseManager, IListManager<Animation>
    {
        public IEnumerable<Animation> GetAll()
        {
            return Db.Animations;
        }

        public void InsertOrUpdate(Animation entity)
        {
            if (entity.ID == Guid.Empty)
            {
                Db.Animations.InsertOnSubmit(entity);
            }

            Db.SubmitChanges();
        }

        public void Delete(Animation entity)
        {
            Db.Animations.DeleteOnSubmit(entity);
            Db.SubmitChanges();
        }
    }
}
