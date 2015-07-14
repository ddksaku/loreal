using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LorealOptimiseData;
using System.Collections;

namespace LorealOptimiseBusiness.Lists
{
    public class SystemMessageManager : BaseManager, IListManager<SystemMessage>
    {
        public event EntityChangedEventHandler<SystemMessage> EntityChanged;

        public System.Data.Linq.DataLoadOptions LoadOptions
        {
            get
            {
                return null;
            }
        }

        private static SystemMessageManager instance = null;
        public static SystemMessageManager Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new SystemMessageManager();
                }
                return instance;
            }
        }

        public IEnumerable<SystemMessage> GetFiltered(Hashtable conditions)
        {
            return GetAll();
        }

        public IEnumerable<SystemMessage> GetAll()
        {
            return SystemMessagesManager.Instance.GetAll();
        }

        public void InsertOrUpdate(SystemMessage entity)
        {
            Db.SubmitChanges();

            Refresh();
        }
        public void Delete(SystemMessage entity)
        {
            // do nothing
        }

        public void Refresh()
        {
            SystemMessagesManager.Instance.Refresh();
        }
    }
}
