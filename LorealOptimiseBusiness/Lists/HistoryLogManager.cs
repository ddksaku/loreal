using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.SqlClient;
using System.Data.SqlTypes;
using LorealOptimiseData;

namespace LorealOptimiseBusiness.Lists
{
    public class HistoryLogManager : BaseManager, IListManager<HistoryLog>
    {
        public event EntityChangedEventHandler<HistoryLog> EntityChanged;

        public System.Data.Linq.DataLoadOptions LoadOptions
        {
            get
            {
                return null;
            }
        }

        private static HistoryLogManager instance = null;
        public static HistoryLogManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new HistoryLogManager();
                }
                return instance;
            }
        }

        // private static IEnumerable<HistoryLog> all = null;
        public IEnumerable<HistoryLog> GetAll()
        {
            //if (all == null)
            //{
            //    all = Db.HistoryLogs;
            //}
            //return all;
            return new HistoryLog[] { };
        }

        public IEnumerable<HistoryLog> GetAll(string tablename, DateTime? dt1, DateTime? dt2)
        {
            if (tablename == null || tablename.Trim() == "")
                tablename = "%%";
            if (dt1 == null)
                dt1 = SqlDateTime.MinValue.Value; 
            if (dt2 == null)
                dt2 = SqlDateTime.MaxValue.Value;

            return Db.HistoryLogs.Where(h=>SqlMethods.Like(h.TableName,tablename) && h.ModifiedDate >= dt1 && h.ModifiedDate <=dt2).OrderByDescending(hl=>hl.ModifiedDate);
        }

        public IEnumerable<HistoryLog> GetFiltered(Hashtable conditions)
        {
            return GetAll();
        }

        public void InsertOrUpdate(HistoryLog entity)
        {}

        public void Delete(HistoryLog entity)
        {}

        public void Refresh()
        {
            // all = null;
        }

        public void Refresh2()
        {
            // all = null;
        }
    }
}
