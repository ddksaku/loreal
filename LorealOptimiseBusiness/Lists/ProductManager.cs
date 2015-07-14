using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LorealOptimiseData;
using System.Data.Linq.SqlClient;
using System.Linq.Expressions;
using System.Reflection;
using LorealOptimiseData.Enums;
using System.ComponentModel;
using System.Threading;
using LorealOptimiseShared.Logging;

namespace LorealOptimiseBusiness.Lists
{
    public class ProductManager : BaseManager, IListManager<Product>
    {
        public event EntityChangedEventHandler<Product> EntityChanged;

        public System.Data.Linq.DataLoadOptions LoadOptions
        {
            get
            {
                return null;
            }
        }

        public const string Description = "Description";
        public const string MaterialCode = "MaterialCode";
        public const string ProcurementType = "ProcurementType";

        private static ProductManager instance = null;
        public static ProductManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ProductManager();
                }
                return instance;
            }
        }

        public ProductManager()
        {
            instance = this;
        }

        private static IEnumerable<Product> all = null;
        public IEnumerable<Product> GetAll()
        {
            // lock (padlock)
            {
                if (all == null)
                {
                    materialCodesToProduct = new Hashtable();
                    descriptionToProduct = new Hashtable();

                    all = Db.Products.Where(defaultFilter()).ToList();
                }
            }
            return all;
        }

        protected Expression<Func<Product, bool>> defaultFilter()
        {
            return d => d.Division.ID == LoggedUser.Division.ID && d.Division.Deleted == false;
        }

        public IEnumerable<Product> GetFiltered(Hashtable conditions)
        {
            var result = Db.Products.AsQueryable();

            if (conditions != null)
            {
                if (conditions.ContainsKey(MaterialCode) && !String.IsNullOrEmpty(conditions[MaterialCode].ToString()))
                {
                    string conditionMaterialCode = "%" + conditions[MaterialCode] + "%";
                    result = result.Where(c => SqlMethods.Like(c.MaterialCode, conditionMaterialCode));
                }

                if (conditions.ContainsKey(Description) && !String.IsNullOrEmpty(conditions[Description].ToString()))
                {
                    string conditionDescription = "%" + conditions[Description] + "%";
                    result = result.Where(c => SqlMethods.Like(c.Description, conditionDescription));
                }

                if (conditions.ContainsKey(ProcurementType) && !String.IsNullOrEmpty(conditions[ProcurementType].ToString()))
                {
                    string conditionProcurement = "%" + conditions[ProcurementType] + "%";
                    result = result.Where(c => SqlMethods.Like(c.ProcurementType, conditionProcurement));
                }
            }

            result = result.Where(defaultFilter());

            return result.ToList();
        }

        private Hashtable materialCodesToProduct;
        public Product GetByMaterialCode(string materialCode)
        {
            try
            {
                //object product = materialCodesToProduct[materialCode];
                //if (product == null)
                //{
                //    product = GetAll().Where(p => p.MaterialCode == materialCode && p.Status != ProductSalesStatus.DeadProduct).FirstOrDefault();
                //    if (product != null)
                //    {
                //        materialCodesToProduct[materialCode] = product;
                //    }
                //}
                //return product as Product;

                return Db.Products.Where(DivisionFilter<Product>()).Where(p => p.MaterialCode == materialCode && p.Status != ProductSalesStatus.DeadProduct).FirstOrDefault();
            }
            catch(Exception exc)
            {
                Logger.Log(exc.Message, LogLevel.Error);
                return null;
            }
        }

        public Product GetByID(Guid guid)
        {
            try
            {
                return GetAll().Where(p => p.ID == guid && p.Status != ProductSalesStatus.DeadProduct).FirstOrDefault();
            }
            catch (Exception exc)
            {
                Logger.Log(exc.Message, LogLevel.Error);
                return null;
            }
        }

        private Hashtable descriptionToProduct;
        public Product GetByDescription(string description)
        {
            try
            {
                //object product = descriptionToProduct[description];
                //if (product == null)
                //{
                //    product = GetAll().Where(p => p.Description == description && p.Status != ProductSalesStatus.DeadProduct).FirstOrDefault();
                //    if (product != null)
                //    {
                //        descriptionToProduct[description] = product;
                //    }
                //}
                //return product as Product;

                return Db.Products.Where(DivisionFilter<Product>()).Where(p => p.Description == description && p.Status != ProductSalesStatus.DeadProduct).FirstOrDefault();
            }
            catch(Exception exc)
            {
                Logger.Log(exc.Message, LogLevel.Error);
                return null;
            }
        }

        public IEnumerable<string> GetMaterialCodes()
        {
            return GetAll().Where(p => p.Status != ProductSalesStatus.DeadProduct).OrderBy(p => p.MaterialCode).Select(p => p.MaterialCode).ToList();
        }

        public IEnumerable<string> GetMaterialCodes(string code)
        {
            return GetAll().Where(p => p.MaterialCode.ToLower().StartsWith(code.ToLower()) && p.Status != ProductSalesStatus.DeadProduct).OrderBy(p => p.MaterialCode).Select(p => p.MaterialCode).ToList();
        }

        public IEnumerable<string> GetDescriptions()
        {
            return GetAll().Where(p => p.Status != ProductSalesStatus.DeadProduct).OrderBy(p => p.Description).Select(p => p.Description).ToList();
        }

        public IEnumerable<string> GetDescriptions(string desc)
        {
            //TODO: Description can be null and calling ToLower throws an exception
            return GetAll().Where(p => p.Description.ToLower().Contains(desc.ToLower()) && p.Status != ProductSalesStatus.DeadProduct).OrderBy(p => p.Description).Select(p => p.Description).ToList();
        }

        public void InsertOrUpdate(Product entity)
        {
            if (entity.ID == Guid.Empty)
            {
                Db.Products.InsertOnSubmit(entity);
            }

            if (entity.Status == String.Empty)
            {
                entity.Status = "0";
            }

            if (entity.Description == null)
            {
                //Force to save not null value. Application may crashes for null value
                entity.Description = String.Empty;
            }

            Db.SubmitChanges();

            all = null;
        }

        public void Delete(Product entity)
        {
            Db.Multiples.DeleteAllOnSubmit(entity.Multiples);
            Db.ProductConfirmeds.DeleteAllOnSubmit(entity.ProductConfirmeds);
            Db.ProductReceiveds.DeleteAllOnSubmit(entity.ProductReceiveds);

            Db.Products.DeleteOnSubmit(entity);
            Db.SubmitChanges();

            all = null;
        }

        public void Refresh()
        {
            all = null;
        }
    }
}
