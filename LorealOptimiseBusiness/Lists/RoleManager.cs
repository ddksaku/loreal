using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LorealOptimiseData;
using LorealOptimiseData.Enums;

namespace LorealOptimiseBusiness.Lists
{
    public class RoleManager : BaseManager, IListManager<Role>
    {
        public event EntityChangedEventHandler<Role> EntityChanged;

        public System.Data.Linq.DataLoadOptions LoadOptions
        {
            get
            {
                return null;
            }
        }

        private const string SYSTEM_ADMIN = "System Administrator";

        private static RoleManager instance = null;
        public static RoleManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new RoleManager();
                }
                return instance;
            }
        }

        private static IEnumerable<Role> all = null;
        public IEnumerable<Role> GetAll()
        {
            if (all == null)
            {
                if (LoggedUser.IsInRole(RoleEnum.SystemAdmin))
                {
                    return Db.Roles.OrderBy(r=>r.Name);
                }
                else
                {
                    return Db.Roles.Where(r => r.Name != SYSTEM_ADMIN).OrderBy(r=>r.Name).ToList();
                }
            }
            return all;
        }

        public IEnumerable<Role> GetFiltered(Hashtable conditions)
        {
            return GetAll();
        }

        public void InsertOrUpdate(Role entity)
        {
            if (entity.ID == Guid.Empty)
            {
                Db.Roles.InsertOnSubmit(entity);
            }
            Db.SubmitChanges();
            all = null;
        }

        public void Delete(Role entity)
        {
            Db.Roles.DeleteOnSubmit(entity);
            Db.SubmitChanges();
            all = null;
        }

        public void Refresh()
        {
            all = null;
        }
    }
}
