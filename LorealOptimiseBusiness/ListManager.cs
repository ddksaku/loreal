using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LorealOptimiseBusiness.Exceptions;
using LorealOptimiseData;
using System.Linq.Expressions;
using System.Security.Principal;

namespace LorealOptimiseBusiness
{
    /// <summary>
    /// Class which handles operations with lists
    /// </summary>
    public class ListManager : BaseManager
    {
        /// <summary>
        /// Returns all non-deleted sales drives filtered by non-deleted division of logged user
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SalesDrive> SalesDriveGetAll()
        {
            return Db.SalesDrives.Where(DivisionFilter<SalesDrive>()).Where(s=>s.Deleted == false);
        }

        /// <summary>
        /// Returns all non-deleted priorities filtered by non-deleted division of logged user
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Priority> PrioritiesGetAll()
        {
            return Db.Priorities.Where(DivisionFilter<Priority>()).Where(p=>p.Deleted==false);
        }

        /// <summary>
        /// Returns all non-deleted item types filtered by non-deleted division of logged user
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ItemType> ItemTypesGetAll()
        {
            return Db.ItemTypes.Where(DivisionFilter<ItemType>()).Where(it=>it.Deleted==false);
        }

        /// <summary>
        /// Returns all non-deleted item groups filtered by non-deleted division of logged user
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ItemGroup> ItemGroupsGetAll()
        {
            return Db.ItemGroups.Where(DivisionFilter<ItemGroup>());
        }

        public IEnumerable<Role> RolesGetAll()
        {
            return Db.Roles;
        }
    }
}
