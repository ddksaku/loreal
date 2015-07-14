using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LorealOptimiseData;
using System.Collections;

namespace LorealOptimiseBusiness.Lists
{
    public class LockoutManager : BaseManager, IListManager<Lockout>
    {
        public event EntityChangedEventHandler<Lockout> EntityChanged;

        private static LockoutManager instance = null;
        public static LockoutManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new LockoutManager();
                }
                return instance;
            }
        }

        private static IEnumerable<Lockout> all = null;
        public IEnumerable<Lockout> GetAll()
        {
            if (all == null)
            {
                all = Db.Lockouts.ToList();
            }
            return all;
        }

        public IEnumerable<Lockout> GetFiltered(Hashtable conditions)
        {
            return GetAll();
        }

        public System.Data.Linq.DataLoadOptions LoadOptions
        {
            get
            {
                return null;
            }
        }

        public void InsertOrUpdate(Lockout entity)
        {
            if (entity.ID == 0)
            {
                Db.Lockouts.InsertOnSubmit(entity);
            }

            Db.SubmitChanges();
            all = null;
        }

        public void Delete(Lockout entity)
        {
            try
            {
                Db.Lockouts.DeleteOnSubmit(entity);
                Db.SubmitChanges();
            }
            catch (Exception ex)
            {

            }
            all = null;
        }

        public void Refresh()
        {
            all = null;
        }

        public void UnlockCurrentLock()
        {
            // unlock the current lock
            Db.unlockCurrentLock();
        }

        public Lockout GetCurrentLockout()
        {
            // Update Lockout table first
            Db.updateLockouts();

            Lockout result = null;
            if (GetAll() != null)
            {
                result = GetAll().Where(lc=>lc.IsActive==true).FirstOrDefault();
            }
            return result;
        }

        public List<Lockout> GetExistingLockouts()
        {
            // Update Lockout table first
            Db.updateLockouts();

            List<Lockout> result = null;
            if (GetAll() != null)
            {
                result = GetAll().OrderBy(lc => lc.Start).ToList();
            }
            return result;
        }

        public bool CanAddLockout(Lockout entity, out string errMsg)
        {
            bool result = true;
            errMsg = string.Empty;

            if (entity != null)
            {
                // check start time < endtime
                if (!(entity.Start < entity.End))
                {
                    errMsg = "Start time should be earlier than end time. Do you want to close without saving?";
                    return false;
                }

                // overlapping check
                if (GetAll().Count(lc => DoOverlap(lc,entity)) > 0)
                {
                    errMsg = string.Format("There already exist lockout schedule(s) overlapping with {0}. Do you want to close without saving?", entity.ScheduleRange);
                    return false;
                }
            }

            return result;
        }

        private bool DoOverlap(Lockout lc1, Lockout lc2)
        {
            if (lc1.Start > lc2.Start && lc1.Start < lc2.End)
                return true;

            if (lc1.End > lc2.Start && lc1.End < lc2.End)
                return true;

            if (lc1.Start < lc2.Start && lc1.End > lc2.End)
                return true;

            return false;
        }

    }
}
