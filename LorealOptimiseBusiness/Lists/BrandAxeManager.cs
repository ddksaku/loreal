using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using LorealOptimiseData;
using LorealOptimiseBusiness.Exceptions;

namespace LorealOptimiseBusiness.Lists
{
    public class BrandAxeManager : BaseManager, IListManager<BrandAxe>
    {
        public event EntityChangedEventHandler<BrandAxe> EntityChanged;

        public System.Data.Linq.DataLoadOptions LoadOptions
        {
            get
            {
                return null;
            }
        }

        private static BrandAxeManager instance = null;
        public static BrandAxeManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BrandAxeManager();
                }
                return instance;
            }
            
        }

        private Dictionary<Guid, IEnumerable<BrandAxe>> brandsWithSales = new Dictionary<Guid, IEnumerable<BrandAxe>>();

        private static IEnumerable<BrandAxe> all = null;
        public IEnumerable<BrandAxe> GetAll()
        {
            if (all == null)
            {
                all = Db.BrandAxes.Where(b => b.Deleted == false && b.Signature.IDDivision == LoggedUser.Division.ID).OrderBy(b=>b.Brand).ThenBy(b=>b.Name).ToList();
            }

            return all;
        }

        private static IEnumerable<BrandAxe> allForAllocation = null;
        public IEnumerable<BrandAxe> GetAllForAllocation()
        {
            if (allForAllocation == null)
            {
                Division d = LoggedUser.LoggedDivision;
                if (d != null)
                {
                    if (d.AllocateByBrand && d.AllocateByAxe)
                    {
                        allForAllocation = Db.BrandAxes.Where(b => b.Deleted == false && b.Signature.IDDivision == LoggedUser.Division.ID && b.Sales.Any(s=>b.ID == s.IDBrandAxe)).OrderBy(b => b.Brand).ThenBy(b => b.Name).ToList();
                    }
                    else if (d.AllocateByBrand)
                    {
                        allForAllocation = Db.BrandAxes.Where(b => b.Deleted == false && 
                                            b.Signature.IDDivision == LoggedUser.Division.ID && 
                                            b.Sales.Any(s => b.ID == s.IDBrandAxe) && b.Brand).OrderBy(b => b.Name).ToList();
                    }
                    else if (d.AllocateByAxe)
                    {
                        allForAllocation = Db.BrandAxes.Where(b => b.Deleted == false && 
                                            b.Signature.IDDivision == LoggedUser.Division.ID && 
                                            b.Sales.Any(s => b.ID == s.IDBrandAxe) && !b.Brand).OrderBy(b => b.Name).ToList();
                    }
                }
            }
            return allForAllocation;
        }

        public IEnumerable<BrandAxe> GetAllForAllocation(Guid idSignature)
        {
            return GetAllForAllocation().Where(b => b.IDSignature == idSignature).ToList();

            if (brandsWithSales.ContainsKey(idSignature))
            {
                return brandsWithSales[idSignature];
            }

            IEnumerable<BrandAxe> brands = GetAllForAllocation().Where(b => b.IDSignature == idSignature).Where(b => b.Sales.Any(s => s.IDBrandAxe == b.ID && b.Deleted == false)).ToList();
            brandsWithSales.Add(idSignature, brands);

            return brands;
        }

        public IEnumerable<BrandAxe> GetFiltered(Hashtable conditions)
        {
            return GetAll();
        }

        public void InsertOrUpdate(BrandAxe entity)
        {
            if (entity.ID != Guid.Empty && entity.Manual == false)
            {
                //Do not allow to update brand axe from SAP (not manual)
                return;
            }

            Validate(entity);

            if (entity.ID == Guid.Empty)
            {
                Db.BrandAxes.InsertOnSubmit(entity);
            }

            entity.Manual = true;

            Db.SubmitChanges();

            all = null;
        }

        public void Validate(BrandAxe entity)
        {
            BrandAxe duplicateRecord = Db.BrandAxes.Where(b => b.IDSignature == entity.IDSignature && b.Code == entity.Code && b.Brand == entity.Brand && b.ID != entity.ID).FirstOrDefault();

            if (duplicateRecord != null)
            {
                throw new LorealValidationException("BrandAxe with specified value already exists in the system.");
            }
        }

        public void Delete(BrandAxe entity)
        {
            try
            {
                // before we delete a brandaxe, we should delete all sales data related to it.
                Db.Sales.DeleteAllOnSubmit(entity.Sales);

                Db.BrandAxes.DeleteOnSubmit(entity);
                Db.SubmitChanges();
            }
            catch (SqlException e)
            {
                // conflict with foreign key
                if (e.Number == 547)
                {
                    Db.BrandAxes.InsertOnSubmit(entity);
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
