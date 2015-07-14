using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Text;
using LorealOptimiseData;
using LorealOptimiseBusiness.Exceptions;
using System.ComponentModel;

namespace LorealOptimiseBusiness.Lists
{
    public class SignatureManager : BaseManager, IListManager<Signature>
    {
        public event EntityChangedEventHandler<Signature> EntityChanged;
        
        public System.Data.Linq.DataLoadOptions LoadOptions
        {
            get
            {
                return null;
            }
        }

        private static SignatureManager instance = null;
        public static SignatureManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SignatureManager();
                }
                return instance;
            }
        }

        private static IEnumerable<Signature> all = null;
        public IEnumerable<Signature> GetAll()
        {
            if (all == null)
            {
                all = Db.Signatures.Where(DivisionFilter<Signature>()).Where(s => s.Deleted == false).OrderBy(s => s.Name).ToList();
            }

            return all;
        }

        private IEnumerable<Signature> allWithSales = null;
        public IEnumerable<Signature> GetAllWithSales()
        {
            if (allWithSales == null)
            {
                //allWithSales = GetAll().Where(s => s.BrandAxes.Any(b => SaleManager.Instance.GetAll().Any(sale => sale.IDBrandAxe == b.ID))).ToList();
                allWithSales = Db.Signatures.Where(DivisionFilter<Signature>()).Where(s => s.Deleted == false && s.BrandAxes.Any(b=>b.Sales.Any(sale=>sale.IDBrandAxe == b.ID))).OrderBy(c => c.Name).ToList();
            }

            return allWithSales;
        }

        public IEnumerable<Signature> GetFiltered(Hashtable conditions)
        {
            return GetAll();
        }

        public void InsertOrUpdate(Signature entity)
        {
            if (entity.ID == Guid.Empty)
            {
                Db.Signatures.InsertOnSubmit(entity);
            }

            Db.SubmitChanges();
            all = null;
            allWithSales = null;
        }

        public void Delete(Signature entity)
        {
            Db.Signatures.DeleteOnSubmit(entity);
            try
            {
                Db.SubmitChanges();
            }
            catch (SqlException e)
            {
                // conflict with foreign key
                if (e.Number == 547)
                {
                    Db.Signatures.InsertOnSubmit(entity);
                    entity.Deleted = true;
                    Db.SubmitChanges();
                }
            }
            all = null;
            allWithSales = null;
        }

        public void Refresh()
        {
            all = null;
            allWithSales = null;
        }
    }
}
