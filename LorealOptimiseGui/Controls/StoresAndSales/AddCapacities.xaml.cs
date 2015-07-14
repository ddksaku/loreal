using LorealOptimiseBusiness.Lists;
using LorealOptimiseData;
using LorealOptimiseBusiness;
using LorealOptimiseGui;
using System;
using System.Linq;
using System.Windows;
using System.Text;
using System.ComponentModel;
using DevExpress.Xpf.Grid;
using System.Collections;
using LorealOptimiseShared;
using LorealOptimiseShared.Logging;
using System.Collections.Generic;

namespace LorealOptimiseGui.Controls.StoresAndSales
{
    /// <summary>
    /// Interaction logic for AddCapacities.xaml
    /// </summary>
    public partial class AddCapacities : BaseUserControl
    {
        TableView view;

        private IEnumerable<Customer> customers;

        public event EventHandler AfterGenerateOrDeleteCapacities;

        public AddCapacities()
        {
            InitializeComponent();
            view = (grdCustomers.View as TableView);

            if (DesignerProperties.GetIsInDesignMode(this) == false)
            {
                Loaded += new RoutedEventHandler(AddCapacities_Loaded);
            }
        }

        void AddCapacities_Loaded(object sender, RoutedEventArgs e)
        {
            cboPriority.ItemsSource = LorealOptimiseBusiness.Lists.PriorityManager.Instance.GetAll().OrderBy(c => c.Name);
            cboItemType.ItemsSource = ItemTypeManager.Instance.GetAll().OrderBy(c => c.Name);
            cboAnimationType.ItemsSource = AnimationTypeManager.Instance.GetAll().OrderBy(c => c.Name);
            customers = CustomerManager.Instance.GetAll().Where(c => c.IncludeInSystem == true);

            foreach (Customer c in customers)
            {
                c.IsSelected = false;
            }

            grdCustomers.DataSource = customers;
        }

        // generating capacities
        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            int capacityValue = 0;
            int.TryParse(txtCapacityValue.Text, out capacityValue);

            IEnumerable<Customer> selectedCustomers = customers.Where(c => c.IsSelected);

            #region Validation warning
            if (capacityValue == 0)
            {
                MessageBox.Show("Please set positive capacity value");
                return;
            }

            if (cboAnimationType.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select animation type");
                return;
            }

            if (cboItemType.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select item type");
                return;
            }

            if (cboPriority.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select priority");
                return;
            }

            if (selectedCustomers.Count() == 0)
            {
                MessageBox.Show("Please select at least one customer");
                return;
            }
            #endregion

            // warning before ovewriting
            DbDataContext simpleContext = new DbDataContext(false);
            LongTaskExecutor createCapacity = new LongTaskExecutor("");
  

            IEnumerable<Customer> customersWithCapacitiesAlreadyInDB = simpleContext.CustomerCapacities.Where(ca => ca.IDAnimationType == ((AnimationType)cboAnimationType.SelectedItem).ID
                && ca.IDItemType == ((ItemType)cboItemType.SelectedItem).ID               
                && ca.IDPriority == ((Priority)cboPriority.SelectedItem).ID
                && selectedCustomers.Contains((Customer)ca.Customer)).Select(ca => ca.Customer).Distinct().ToList();

            if (customersWithCapacitiesAlreadyInDB.Count() > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (Customer a in customersWithCapacitiesAlreadyInDB)
                {
                    sb.AppendLine(String.Format("{0} {1}", a.AccountNumber, a.Name));
                }

                String warningMessage = String.Format("{0} \r\n{1}", SystemMessagesManager.Instance.GetMessage("OverwriteCustomerCapacity"), sb.ToString());

                if (MessageBox.Show(warningMessage, "Warning", MessageBoxButton.YesNo) == MessageBoxResult.No)
                {
                    return;
                }                    
                else
                {                    
                    createCapacity.DoWork += createCapacity_DoWork;
                    createCapacity.Run(selectedCustomers);
                }  
            }
           

            createCapacity.DoWork += createCapacity_DoWork;
            createCapacity.Run(selectedCustomers);

           
        }


       

        void createCapacity_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                IEnumerable<Customer> selectedCustomers = e.Argument as IEnumerable<Customer>;

                int generatedItemsCount = cboAnimationType.SelectedItems.Count * cboItemType.SelectedItems.Count * cboPriority.SelectedItems.Count * selectedCustomers.Count();
                int increaseCount = selectedCustomers.Count() * cboItemType.SelectedItems.Count;
                int currentState = 0;

                int? capacityValue = Convert.ToInt32(txtCapacityValue.Text);
                string[] strArray = selectedCustomers.Select(c => c.ID.ToString()).ToArray();

                // comma separated string for DB procedure
                string customerCommaSeparatedList = String.Join(",", strArray);
                string itemTypesCommaSeparatedList = String.Empty;

                foreach (ItemType i in cboItemType.SelectedItems)
                {
                    if (itemTypesCommaSeparatedList != String.Empty)
                    {
                        itemTypesCommaSeparatedList += "," + i.ID.ToString();
                    }
                    else
                    {
                        itemTypesCommaSeparatedList = i.ID.ToString();
                    }

                }

                // foreach cycle
                foreach (AnimationType animationType in cboAnimationType.SelectedItems)
                {
                    foreach (Priority priority in cboPriority.SelectedItems)
                    {
                        // run DB procedure 
                        DbDataContext.GetInstance().up_generateCapacities(animationType.ID, itemTypesCommaSeparatedList, priority.ID, customerCommaSeparatedList, capacityValue, false /* generate */);
                        currentState += increaseCount;
                        LongTaskExecutor.RaiseLongTaskEvent(this, new LongTaskEventArgs("task", TaskStatus.InProgress, String.Format("Generating capacities {0}/{1}", currentState, generatedItemsCount)));
                    }
                }

                if (AfterGenerateOrDeleteCapacities != null)
                {
                    AfterGenerateOrDeleteCapacities(this, null);
                }

            }
            catch (Exception exc)
            {
                Logger.Log(exc.ToString(), LogLevel.Error);
                MessageBox.Show(SystemMessagesManager.Instance.GetMessage("GenerateCapacityException", Utility.GetExceptionsMessages(exc)));
            }
            finally
            {
                DbDataContext.GetInstance().CommandTimeout = Utility.SqlCommandTimeOut;
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            IEnumerable<Customer> selectedCustomers = customers.Where(c => c.IsSelected);

            #region Validation warning

            if (cboAnimationType.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select animation type");
                return;
            }

            if (cboItemType.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select item type");
                return;
            }

            if (cboPriority.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select priority");
                return;
            }

            if (selectedCustomers.Count() == 0)
            {
                MessageBox.Show("Please select at least one customer");
                return;
            }
            #endregion

            LongTaskExecutor deleteCapacity = new LongTaskExecutor("");
            deleteCapacity.DoWork += deleteCapacity_DoWork;
            deleteCapacity.Run(selectedCustomers);

        }

        void deleteCapacity_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                IEnumerable<Customer> selectedCustomers = e.Argument as IEnumerable<Customer>;

                int generatedItemsCount = cboAnimationType.SelectedItems.Count * cboItemType.SelectedItems.Count * cboPriority.SelectedItems.Count * selectedCustomers.Count();
                int increaseCount = selectedCustomers.Count() * cboItemType.SelectedItems.Count;
                int currentState = 0;

                string[] strArray = selectedCustomers.Select(c => c.ID.ToString()).ToArray();

                // comma separated string for DB procedure
                string customerCommaSeparatedList = String.Join(",", strArray);
                string itemTypesCommaSeparatedList = "";

                foreach (ItemType i in cboItemType.SelectedItems)
                {
                    if (itemTypesCommaSeparatedList != String.Empty)
                    {
                        itemTypesCommaSeparatedList += "," + i.ID.ToString();
                    }
                    else
                    {
                        itemTypesCommaSeparatedList = i.ID.ToString();
                    }
                }

                // foreach cycle
                foreach (AnimationType animationType in cboAnimationType.SelectedItems)
                {
                    foreach (Priority priority in cboPriority.SelectedItems)
                    {
                        // run DB procedure 
                        DbDataContext.GetInstance().up_generateCapacities(animationType.ID, itemTypesCommaSeparatedList, priority.ID, customerCommaSeparatedList, null, true /* delete */);
                        currentState += increaseCount;
                        LongTaskExecutor.RaiseLongTaskEvent(this, new LongTaskEventArgs("task", TaskStatus.InProgress, String.Format("Deleting capacities {0}/{1}", currentState, generatedItemsCount)));
                    }
                }

                if (AfterGenerateOrDeleteCapacities != null)
                {
                    AfterGenerateOrDeleteCapacities(this, null);
                }
            }
            catch (Exception exc)
            {
                Logger.Log(exc.ToString(), LogLevel.Error);
                MessageBox.Show(SystemMessagesManager.Instance.GetMessage("GenerateCapacityException", Utility.GetExceptionsMessages(exc)));
            }
            finally
            {
                DbDataContext.GetInstance().CommandTimeout = Utility.SqlCommandTimeOut;
            }
        }

        private void chkSelectAll_Checked(object sender, RoutedEventArgs e)
        {
            IEnumerable<Customer> visibleCustomers = getVisibleCustomers();

            customers.Where(c => visibleCustomers.Contains(c)).ToList().ForEach(c => c.IsSelected = true);

            grdCustomers.RefreshData();
        }

        private void chkSelectAll_Unchecked(object sender, RoutedEventArgs e)
        {
            IEnumerable<Customer> visibleCustomers = getVisibleCustomers();

            customers.Where(c => visibleCustomers.Contains(c)).ToList().ForEach(c => c.IsSelected = false);
            grdCustomers.RefreshData();
        }

        private IEnumerable<Customer> getVisibleCustomers()
        {
            List<Customer> customers = new List<Customer>();

            for (int i = 0; i < grdCustomers.VisibleRowCount; i++)
            {
                int rowHandle = grdCustomers.GetRowHandleByVisibleIndex(i);
                Customer c = grdCustomers.GetRow(rowHandle) as Customer;

                customers.Add(c);
            }

            return customers;
        }
    }
}
