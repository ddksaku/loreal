using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Text;
using LorealOptimiseData;
using LorealOptimiseData.Enums;

namespace LorealOptimiseBusiness.Lists
{
    public class CustomerAllocationManager : BaseManager, IModify<CustomerAllocation>
    {
        public event EntityChangedEventHandler<CustomerAllocation> EntityChanged;

        private static CustomerAllocationManager instance;
        public static CustomerAllocationManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CustomerAllocationManager();
                }
                return instance;
            }
        }

        public void InsertOrUpdate(CustomerAllocation entity)
        {
            DbDataContext db = AnimationAllocations.GetDataContext();

            if (entity.ID == Guid.Empty)
            {
                db.CustomerAllocations.InsertOnSubmit(entity);
            }

            try
            {
                db.SubmitChanges();
            }
            catch (SqlException exc)
            {
                if (exc.Number == 50000 && exc.Class == 16)
                {
                    CustomerAllocation originalEntity = new DbDataContext().CustomerAllocations.SingleOrDefault(ca => ca.ID == entity.ID);
                    if (originalEntity != null)
                    {
                        entity.FixedAllocation = originalEntity.FixedAllocation;
                        entity.RetailUplift = originalEntity.RetailUplift;
                        db.SubmitChanges();
                    }
                    else
                    {
                        Delete(entity);
                        AnimationManager.GetInstance().Allocations.CustomersAllocation.Remove(entity);
                    }
                }

                throw;
            }

            if (EntityChanged != null)
            {
                EntityChanged(this, entity);
            }
        }

        public void Delete(CustomerAllocation entity)
        {
            DbDataContext db = AnimationAllocations.GetDataContext();
            db.CustomerAllocations.DeleteOnSubmit(entity);
            db.SubmitChanges();

            if (EntityChanged != null)
            {
                EntityChanged(this, entity);
            }
        }
    }
}
