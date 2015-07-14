using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LorealOptimiseData;

namespace LorealOptimiseBusiness.Lists
{
    public class EventLogManager : BaseManager, IListManager<EventLog>
    {
        public event EntityChangedEventHandler<EventLog> EntityChanged;

        public System.Data.Linq.DataLoadOptions LoadOptions
        {
            get
            {
                return null;
            }
        }


        private static EventLogManager instance = null;
        public static EventLogManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EventLogManager();
                }
                return instance;
            }
        }

        private static IEnumerable<EventLog> all = null;
        public IEnumerable<EventLog> GetAll()
        {
            if (all == null)
            {
                all = Db.EventLogs.OrderByDescending(el=>el.DateCreated);
            }
            return all;
        }

        public IEnumerable<EventLog> GetFiltered(Hashtable conditions)
        {
            return GetAll();
        }

        public void InsertOrUpdate(EventLog entity)
        {
            if (entity.ID == Guid.Empty)
            {
                Db.EventLogs.InsertOnSubmit(entity);
            }

            Db.SubmitChanges();
            all = null;
        }

        public void Delete(EventLog entity)
        {
            Db.EventLogs.DeleteOnSubmit(entity);
            Db.SubmitChanges();
            all = null;
        }

        public void Refresh()
        {
            all = null;
        }
    }
}
