using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using LorealOptimiseData;

namespace LorealOptimiseBusiness.Lists
{
    public class DistributionChannelManager : BaseManager, IListManager<DistributionChannel>
    {
        public event EntityChangedEventHandler<DistributionChannel> EntityChanged;

        public System.Data.Linq.DataLoadOptions LoadOptions
        {
            get
            {
                return null;
            }
        }

        private static DistributionChannelManager instance = null;
        public static DistributionChannelManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DistributionChannelManager();
                }
                return instance;
            }
        }

        private static IEnumerable<DistributionChannel> all = null;
        public IEnumerable<DistributionChannel> GetAll()
        {
            if (all == null)
            {
                all = Db.DistributionChannels.Where(d => d.Deleted == false).OrderBy(dc=>dc.Name).ToList();
            }
            return all;
        }

        public IEnumerable<DistributionChannel> GetFiltered(Hashtable conditions)
        {
            return GetAll();
        }

        public void InsertOrUpdate(DistributionChannel entity)
        {
            if (entity.ID == Guid.Empty)
            {
                Db.DistributionChannels.InsertOnSubmit(entity);
            }

            Db.SubmitChanges();
            all = null;
        }

        public void Delete(DistributionChannel entity)
        {
            Db.DistributionChannels.DeleteOnSubmit(entity);
            try
            {
                Db.SubmitChanges();
            }
            catch (SqlException e)
            {
                // conflict with foreign key
                if (e.Number == 547)
                {
                    Db.DistributionChannels.InsertOnSubmit(entity);
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
