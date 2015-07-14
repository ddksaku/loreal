using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LorealOptimiseData;

namespace LorealOptimiseBusiness.Lists
{
    public class UserRoleManager : BaseManager, IListManager<UserRole>
    {
        public event EntityChangedEventHandler<UserRole> EntityChanged;

        public System.Data.Linq.DataLoadOptions LoadOptions
        {
            get
            {
                return null;
            }
        }

        private static UserRoleManager instance = null;
        public static UserRoleManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UserRoleManager();
                }
                return instance;
            }
        }

        private static IEnumerable<UserRole> all = null;
        public IEnumerable<UserRole> GetAll()
        {
            if (all == null)
            {
                all = Db.UserRoles.Where(DivisionFilter<UserRole>());
            }
            return all;
        }

        public IEnumerable<UserRole> GetFiltered(Hashtable conditions)
        {
            return GetAll();
        }

        public void InsertOrUpdate(UserRole entity)
        {
            if (entity.ID == Guid.Empty)
            {
                Db.UserRoles.InsertOnSubmit(entity);
            }
            Db.SubmitChanges();
            all = null;
        }

        public void Delete(UserRole entity)
        {
            Db.UserRoles.DeleteOnSubmit(entity);
            Db.SubmitChanges();
            all = null;
        }

        public void Refresh()
        {
            all = null;
        }
    }
}
