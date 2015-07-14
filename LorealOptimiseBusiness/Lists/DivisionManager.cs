using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using LorealOptimiseData;
using LorealOptimiseShared;


namespace LorealOptimiseBusiness.Lists
{
    public class DivisionManager : BaseManager, IListManager<Division>
    {
        public event EntityChangedEventHandler<Division> EntityChanged;

        public System.Data.Linq.DataLoadOptions LoadOptions
        {
            get
            {
                return null;
            }
        }

        public const string IDDivision = "IDDivision";

        /// <summary>
        /// Returns all non-deleted divisions for logged user
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Division> GetAllToLogged()
        {
            return Db.UserRoles.Where(u => u.User.LoginName == LoggedUserName && u.Division.Deleted == false).Select(u => u.Division).Distinct().OrderBy(u=>u.Name).ToList();
        }

        private static DivisionManager instance = null;
        public static DivisionManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DivisionManager();
                }
                return instance;
            }
        }

        private static IEnumerable<Division> all = null;
        public IEnumerable<Division> GetAll()
        {
            if (all == null)
            {
                all = Db.Divisions.Where(d => d.Deleted == false).OrderBy(d=>d.Name).ToList();
            }
            return all;
        }

        public IEnumerable<Division> GetFiltered(Hashtable conditions)
        {
            var result = Db.Divisions.AsQueryable();

            if (conditions != null)
            {
                Guid divisionID = Guid.Empty;

                if (conditions.ContainsKey(IDDivision) && conditions[IDDivision].ToString() != Guid.Empty.ToString() && conditions[IDDivision] != null && conditions[IDDivision].ToString().IsValidGuid())
                {
                    divisionID = (Guid)conditions[IDDivision];
                    result = result.Where(c => c.ID == divisionID);
                }
            }            

            return result.ToList();

        }

        public void InsertOrUpdate(Division division)
        {
            if (division.ID == Guid.Empty)
            {
                Db.Divisions.InsertOnSubmit(division);
            }

            Db.SubmitChanges();
            all = null;
        }

        public void Delete(Division entity)
        {
            Db.Divisions.DeleteOnSubmit(entity);
            try
            {
                Db.SubmitChanges();
            }
            catch (SqlException e)
            {
                // conflict with foreign key
                if (e.Number == 547)
                {
                    Db.Divisions.InsertOnSubmit(entity);
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
