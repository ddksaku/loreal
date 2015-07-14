using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using LorealOptimiseData;

namespace LorealOptimiseBusiness.Lists
{
    public class OrderTypeManager : BaseManager, IListManager<OrderType>
    {
        public event EntityChangedEventHandler<OrderType> EntityChanged;

        public System.Data.Linq.DataLoadOptions LoadOptions
        {
            get
            {
                return null;
            }
        }

        private static OrderTypeManager instance = null;
        public static OrderTypeManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new OrderTypeManager();
                }
                return instance;
            }
        }

        private static IEnumerable<OrderType> all = null;
        public IEnumerable<OrderType> GetAll()
        {
            if (all == null)
            {
                all = Db.OrderTypes.Where(DivisionFilter<OrderType>()).Where(o => o.Deleted == false).OrderBy(o=>o.Name).ToList();
            }

            return all;
        }

        public IEnumerable<OrderType> GetFiltered(Hashtable conditions)
        {
            return GetAll();
        }

        public void InsertOrUpdate(OrderType entity)
        {
            if (entity.ID == Guid.Empty)
            {
                Db.OrderTypes.InsertOnSubmit(entity);
            }

            Db.SubmitChanges();

            all = null;
        }

        public void Delete(OrderType entity)
        {
            Db.OrderTypes.DeleteOnSubmit(entity);
            try
            {
                Db.SubmitChanges();
            }
            catch (SqlException e)
            {
                // conflict with foreign key
                if (e.Number == 547)
                {
                    Db.OrderTypes.InsertOnSubmit(entity);
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
