using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LorealOptimiseData;
using System.Data.Linq.SqlClient;
using LorealOptimiseShared;

namespace LorealOptimiseBusiness.Lists
{
    public class AuditAlertManager : BaseManager, IListManager<AuditAlert>
    {
        public event EntityChangedEventHandler<AuditAlert> EntityChanged;

        public System.Data.Linq.DataLoadOptions LoadOptions
        {
            get
            {
                return null;
            }
        }

        public const string DateCreated = "DateCreated";
        public const string AlertType = "AlertType";

        private static AuditAlertManager instance = null;
        public static AuditAlertManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AuditAlertManager();
                }
                return instance;
            }
        }

        private static IEnumerable<AuditAlert> all = null;
        public IEnumerable<AuditAlert> GetAll()
        {
            if (all == null)
            {
                all = Db.AuditAlerts.Where(DivisionFilter<AuditAlert>()).OrderByDescending(aa=>aa.DateCreated).ToList();
            }
            return all;
        }

       

        public IEnumerable<AuditAlert> GetFiltered(Hashtable conditions)
        {
            var result = Db.AuditAlerts.AsQueryable();

            if (conditions != null)
            {
                if (conditions.ContainsKey(AlertType) && !String.IsNullOrEmpty(conditions[AlertType].ToString()))
                {
                    string condition = "%" + conditions[AlertType] + "%";

                    result = result.Where(c => SqlMethods.Like(c.AlertType, condition));
                }

                

                DateTime dateFrom = DateTime.MinValue;
                if (conditions.ContainsKey(DateCreated) && conditions[DateCreated] != null && conditions[DateCreated].ToString().IsValidDate())
                {
                    dateFrom = (DateTime)conditions[DateCreated];
                    result = result.Where(c => c.DateCreated >= dateFrom);
                }

            }

            result = result.Where(DivisionFilter<AuditAlert>()).OrderByDescending(c => c.DateCreated);
            return result.ToList();
        }

        public void InsertOrUpdate(AuditAlert entity)
        {
            if (entity.ID == Guid.Empty)
            {
                Db.AuditAlerts.InsertOnSubmit(entity);
            }

            Db.SubmitChanges();

            all = null;
        }

        public void Delete(AuditAlert entity)
        {
            Db.AuditAlerts.DeleteOnSubmit(entity);
            Db.SubmitChanges();

            all = null;
        }

        public void Refresh()
        {
            all = null;
        }
    }
}
