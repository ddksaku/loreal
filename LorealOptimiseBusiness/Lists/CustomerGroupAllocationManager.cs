using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using LorealOptimiseData;

namespace LorealOptimiseBusiness.Lists
{
    public class CustomerGroupAllocationManager : BaseManager, IModify<CustomerGroupAllocation>
    {
        public event EntityChangedEventHandler<CustomerGroupAllocation> EntityChanged;

        private static CustomerGroupAllocationManager instance;
        public static CustomerGroupAllocationManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CustomerGroupAllocationManager();
                }
                return instance;
            }
        }

        public void InsertOrUpdate(CustomerGroupAllocation entity)
        {
            DbDataContext db = AnimationAllocations.GetDataContext();

            if (entity.ID == Guid.Empty)
            {
                db.CustomerGroupAllocations.InsertOnSubmit(entity);
            }

            try
            {
                db.SubmitChanges();
            }
            catch (SqlException exc)
            {
                // trigger
                if (exc.Number == 50000 && exc.Class == 16)
                {
                    CustomerGroupAllocation originalEntity = new DbDataContext().CustomerGroupAllocations.SingleOrDefault(cga => cga.ID == entity.ID);
                    if (originalEntity != null)
                    {
                        entity.IDCustomerGroup = originalEntity.IDCustomerGroup;
                        entity.IDAnimationProductDetail = originalEntity.IDAnimationProductDetail;
                        entity.ManualFixedAllocation = originalEntity.ManualFixedAllocation;
                        entity.RetailUplift = originalEntity.RetailUplift;
                        db.SubmitChanges();
                    }
                    else
                    {
                        Delete(entity);
                        AnimationManager.GetInstance().Allocations.CustomerGroupsAllocation.Remove(entity);
                        //AnimationManager.GetInstance().Animation.ObservableCustomerGroupAllocations.Remove(entity);
                    }
                }

                throw;
            }

            if (EntityChanged != null)
            {
                EntityChanged(this, entity);
            }
        }

        public void Delete(CustomerGroupAllocation entity)
        {
            DbDataContext db = AnimationAllocations.GetDataContext();
            db.CustomerGroupAllocations.DeleteOnSubmit(entity);
            db.SubmitChanges();

            if (EntityChanged != null)
            {
                EntityChanged(this, entity);
            }
        }
    }
}
