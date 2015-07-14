using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using LorealOptimiseData;

namespace LorealOptimiseBusiness.Lists
{
    public class SalesDriveManager : BaseManager, IListManager<SalesDrive>
    {
        public event EntityChangedEventHandler<SalesDrive> EntityChanged;

        public System.Data.Linq.DataLoadOptions LoadOptions
        {
            get
            {
                return null;
            }
        }

        private static SalesDriveManager instance = null;
        public static SalesDriveManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SalesDriveManager();
                }
                return instance;
            }
        }

        private static IEnumerable<SalesDrive> all = null;
        public IEnumerable<SalesDrive> GetAll()
        {
            if (all == null)
            {
                all = Db.SalesDrives.Where(DivisionFilter<SalesDrive>()).Where(s => s.Deleted == false).OrderBy(sd=>sd.Name).ToList();
            }
            return all;
        }

        public IEnumerable<Guid> GetAnimationsBySalesDrive(SalesDrive salesDrive)
        {
            return Db.Animations.Where(c => c.SalesDrive == salesDrive).Select(a => a.ID);
        }
       

        public IEnumerable<SalesDrive> GetFiltered(Hashtable conditions)
        {
            return GetAll();
        }

        public void InsertOrUpdate(SalesDrive entity)
        {
            if (entity.ID == Guid.Empty)
            {
                Db.SalesDrives.InsertOnSubmit(entity);
            }

            Db.SubmitChanges();

            all = null;
        }

        public void Delete(SalesDrive entity)
        {
            Db.SalesDrives.DeleteOnSubmit(entity);
            try
            {
                Db.SubmitChanges();
            }
            catch (SqlException e)
            {
                // conflict with foreign key
                if (e.Number == 547)
                {
                    Db.SalesDrives.InsertOnSubmit(entity);
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
