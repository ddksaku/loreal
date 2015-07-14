using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Data.Linq.SqlClient;
using LorealOptimiseData;
using LorealOptimiseShared;
using LorealOptimiseBusiness.Exceptions;
using System.Data.Linq;


namespace LorealOptimiseBusiness.Lists
{
    public class SaleManager : BaseManager, IListManager<Sale>
    {
        public event EntityChangedEventHandler<Sale> EntityChanged;

        public System.Data.Linq.DataLoadOptions LoadOptions
        {
            get
            {
                DataLoadOptions loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<Sale>(s => s.BrandAxe);
                loadOptions.LoadWith<Sale>(s => s.Customer);
                loadOptions.LoadWith<Customer>(c => c.CustomerGroup);
                loadOptions.LoadWith<Customer>(c => c.SalesEmployee);
                loadOptions.LoadWith<Customer>(c => c.SalesArea);
                loadOptions.LoadWith<SalesArea>(sa=>sa.DistributionChannel);
                loadOptions.LoadWith<SalesArea>(sa => sa.SalesOrganization);

                return loadOptions;
            }
        }

        public const string CustomerName = "CustomerName";
        public const string IDBrandAxe = "IDBrandAxe";
        public const string IDSignature = "IDSignature";
        public const string DateFrom = "DateFrom";
        public const string DateTo = "DateTo";
       
        private static SaleManager instance = null;
        public static SaleManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SaleManager();
                }
                return instance;
            }
        }

        protected Expression<Func<Sale, bool>> defaultFilter()
        {
            return s => s.BrandAxe.Signature.IDDivision == LoggedUser.Division.ID
                   && s.Customer.SalesArea.IDDivision == LoggedUser.Division.ID && s.Customer.Deleted == false && s.BrandAxe.Deleted == false;
        }

        private static IEnumerable<Sale> all = null;
        public IEnumerable<Sale> GetAll()
        {           
            if (all == null)
            {
                all = Db.Sales.Where(defaultFilter()).ToList();
            }

            return all;
        }

        public IEnumerable<Sale> GetFiltered(Hashtable conditions)
        {
            var result = Db.Sales.AsQueryable();
            var brandAxeResult = Db.BrandAxes.AsQueryable();

            if (conditions != null)
            {
                Guid customer = Guid.Empty;
                Guid brandAxe = Guid.Empty;
                Guid signature = Guid.Empty;
                DateTime dateFrom = DateTime.MinValue;
                DateTime dateTo = DateTime.MaxValue;

                if (conditions.ContainsKey(CustomerName) && !String.IsNullOrEmpty(conditions[CustomerName].ToString()))
                {
                    string condition = "%" + conditions[CustomerName] + "%";

                    result = result.Where(c => SqlMethods.Like(c.Customer.Name, condition));
                }               

                if (conditions.ContainsKey(IDBrandAxe) && conditions[IDBrandAxe] != null && conditions[IDBrandAxe].ToString() != Guid.Empty.ToString() && conditions[IDBrandAxe].ToString().IsValidGuid())
                {
                    brandAxe = (Guid)conditions[IDBrandAxe];
                    result = result.Where(c => c.IDBrandAxe == brandAxe);
                }

                if (conditions.ContainsKey(IDSignature) && conditions[IDSignature] != null && conditions[IDSignature].ToString() != Guid.Empty.ToString() && conditions[IDSignature].ToString().IsValidGuid())
                {
                    signature = (Guid)conditions[IDSignature];
                    result = result.Where(c => c.BrandAxe.IDSignature == signature);
                }

                if (conditions.ContainsKey(DateFrom) && conditions[DateFrom] != null && conditions[DateFrom].ToString().IsValidDate())
                {
                    dateFrom = (DateTime)conditions[DateFrom];
                    result = result.Where(c => c.Date >= dateFrom);
                }

                if (conditions.ContainsKey(DateTo) && conditions[DateTo] != null && conditions[DateTo].ToString().IsValidDate())
                {
                    dateTo = (DateTime)conditions[DateTo];
                    result = result.Where(c => c.Date <= dateTo);
                }
            }


            result = result.Where(defaultFilter());

            return result.ToList();
        }

        public void InsertOrUpdate(Sale entity)
        {
            if (entity.ID == Guid.Empty)
            {
                Db.Sales.InsertOnSubmit(entity);
            }
            Db.SubmitChanges();
            all = null;
        }

        public void Delete(Sale entity)
        {
            Db.Sales.DeleteOnSubmit(entity);
            Db.SubmitChanges();
            all = null;
        }

        public void Refresh()
        {
            all = null;
        }
    }

}
