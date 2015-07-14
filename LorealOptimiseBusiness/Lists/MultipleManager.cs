using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Text;
using LorealOptimiseData;

namespace LorealOptimiseBusiness.Lists
{
    public class MultipleManager : BaseManager, IListManager<Multiple>
    {
        public event EntityChangedEventHandler<Multiple> EntityChanged;

        public System.Data.Linq.DataLoadOptions LoadOptions
        {
            get
            {
                return null;
            }
        }

        private static MultipleManager instance = null;
        public static MultipleManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MultipleManager();
                }
                return instance;
            }
        }

        public MultipleManager()
        {
            instance = this;
        }

        private static IEnumerable<Multiple> all = null;
        public IEnumerable<Multiple> GetAll()
        {
            if (all == null)
            {
                all = Db.Multiples.Where(m => m.Product.IDDivision == LoggedUser.Division.ID && m.Deleted == false).OrderBy(m=>m.Value).ToList();
            }
            return all;
        }

        public IEnumerable<Multiple> GetFiltered(Hashtable conditions)
        {
            return GetAll();
        }

        public IEnumerable<Multiple> MultipleGetToProduct(Guid idProduct)
        {
            // return Db.Multiples.Where(m => m.IDProduct == idProduct && m.Deleted == false).OrderBy(m=>m.Value);
            return GetAll().Where(m => m.IDProduct == idProduct).GroupBy(m => m.Value).Select(g => g.First()).OrderBy(m => m.Value);
        }

        public void InsertOrUpdate(Multiple entity)
        {
            List<AnimationProduct> animationProducts = entity.AnimationProducts.ToList();
            List<AnimationProduct> animationProducts1 = entity.AnimationProducts1.ToList();

            if (entity.ID == Guid.Empty)
            {
                Db.Multiples.InsertOnSubmit(entity);
            }
            else
            {
                //entity.AnimationProducts.Clear();
                //entity.AnimationProducts1.Clear();
            }

            Db.SubmitChanges();

            Db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, animationProducts);
            Db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, animationProducts1);

            all = null;
        }

        public void Delete(Multiple entity)
        {
            entity.AnimationProducts.Clear();
            entity.AnimationProducts1.Clear();
            
            try
            {
                Db.Multiples.DeleteOnSubmit(entity);
                Db.SubmitChanges();
            }
            catch (SqlException exc)
            {
                // conflict with foreign key
                if (exc.Number == 547)
                {
                    Db.Multiples.InsertOnSubmit(entity);
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
