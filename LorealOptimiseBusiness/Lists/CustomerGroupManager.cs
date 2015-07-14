using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LorealOptimiseData;
using LorealOptimiseBusiness.Exceptions;
using System.Data.Linq;

namespace LorealOptimiseBusiness.Lists
{
    public class CustomerGroupManager : BaseManager, IListManager<CustomerGroup>
    {
        public event EntityChangedEventHandler<CustomerGroup> EntityChanged;

        public System.Data.Linq.DataLoadOptions LoadOptions
        {
            get
            {
                DataLoadOptions loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<CustomerGroup>(cg => cg.SalesArea);
                //loadOptions.LoadWith<CustomerGroup>(cg => cg.Customers);
                loadOptions.LoadWith<SalesArea>(sa => sa.DistributionChannel);
                loadOptions.LoadWith<SalesArea>(sa => sa.SalesOrganization);
                loadOptions.LoadWith<SalesArea>(sa => sa.Division);

                return loadOptions;
            }
        }

        private static CustomerGroupManager instance = null;
        public static CustomerGroupManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CustomerGroupManager();
                }
                return instance;
            }
        }

        private static IEnumerable<CustomerGroup> all = null;
        public IEnumerable<CustomerGroup> GetAll()
        {
            if (all == null)
            {
                all = Db.CustomerGroups.Where(cg =>cg.SalesArea.Deleted == false && cg.SalesArea.IDDivision == LoggedUser.Division.ID && cg.SalesArea.DistributionChannel.Deleted == false && cg.SalesArea.SalesOrganization.Deleted == false).OrderBy(cg => cg.Name).ToList();
            }
            return all;
        }

        public IEnumerable<CustomerGroup> GetFiltered(Hashtable conditions)
        {
            return GetAll();
        }

        public CustomerGroup GetById(Guid id)
        {
            return GetAll().Where(cg => cg.ID == id).FirstOrDefault();
        }

        public void InsertOrUpdate(CustomerGroup entity)
        {
            if (entity.ID == Guid.Empty)
            {
                Db.CustomerGroups.InsertOnSubmit(entity);
            }

            Db.SubmitChanges();
            all = null;
        }

        public void Delete(CustomerGroup entity)
        {
            try
            {
                CustomerManager customerManager = CustomerManager.Instance;
                int count = entity.Customers.Count;
                for (int i = count - 1; i >= 0; i--)
                {
                    customerManager.Delete(entity.Customers[i]);
                }

                Db.CustomerGroups.DeleteOnSubmit(entity);
                Db.SubmitChanges();
            }
            catch (Exception exc)
            {
                //System.Windows.MessageBox.Show("An error occured when deleting CustomerGroup:" + LorealOptimiseShared.Utility.GetExceptionsMessages(exc));

                System.Windows.MessageBox.Show(SystemMessagesManager.Instance.GetMessage("CustomerGroupManagerErrorDeleting", LorealOptimiseShared.Utility.GetExceptionsMessages(exc)));
            }

            all = null;
        }

        public void Refresh()
        {
            all = null;
        }
    }
}
