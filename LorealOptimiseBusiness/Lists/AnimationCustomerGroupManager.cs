using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LorealOptimiseData;

namespace LorealOptimiseBusiness.Lists
{
    public class AnimationCustomerGroupManager : IModify<AnimationCustomerGroup>
    {
        public void InsertOrUpdate(AnimationCustomerGroup entity)
        {
            AnimationManager.GetInstance().CustomerGroupInsertUpdate(entity);
        }

        public void Delete(AnimationCustomerGroup entity)
        {
            ;
        }

        public event EntityChangedEventHandler<AnimationCustomerGroup> EntityChanged;
    }
}