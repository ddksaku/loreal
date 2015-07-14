using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using LorealOptimiseData;
using LorealOptimiseBusiness.Lists;

namespace LorealOptimiseGui.Controls
{
    /// <summary>
    /// Interaction logic for MergeCustomerGroup.xaml
    /// </summary>
    public partial class MergeCustomerGroup : BaseUserControl
    {
        private CustomerGroup dummyCustomerGroup = null;
        private CustomerGroup SAPCustomerGroup = null;

        public event EventHandler Close = null;

        public MergeCustomerGroup(CustomerGroup cg)
        {
            InitializeComponent();

            dummyCustomerGroup = cg;

            txtCustomerGroupName.Text = dummyCustomerGroup.Name;
            txtCustomerGroupCode.Text = dummyCustomerGroup.Code;                      

            this.DataContext = CustomerGroupManager.Instance.GetAll().Where(c => c.Manual == false);

            cboSalesAreas.ItemsSource = SalesAreaManager.Instance.GetAll();
        }

        // result of Merging
        private CustomerGroup removedSAPCustomerGroup = null;
        public CustomerGroup RemovedSAPCustomerGroup
        {
            get
            {
                return removedSAPCustomerGroup;
            }
        }

        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            if (grdCustomerGroups.View.FocusedRow != null)
            {
                SAPCustomerGroup = grdCustomerGroups.GetFocusedRow() as CustomerGroup;
            }
        }

        private void btnMerge_Click(object sender, RoutedEventArgs e)
        {
            if (SAPCustomerGroup == null)
            {
                MessageBox.Show(SystemMessagesManager.Instance.GetMessage("MergeCustomerGroupSelectCustomerGroup"));
                return;
            }

            if (SAPCustomerGroup.AnimationCustomerGroups.Any(acg => acg.Animation != null && acg.Animation.IsActive) == true)
            {
                MessageBox.Show(SystemMessagesManager.Instance.GetMessage("MergeCustomerGroupAttachedToLiveAnimation"));
                return;
            }


            // because of (Code,SalesArea)-unique property 
            string code = SAPCustomerGroup.Code;
            SAPCustomerGroup.Code += "merged";

            CustomerGroupManager manager = CustomerGroupManager.Instance;

            try
            {
                // first, upate the SAP customer group
                manager.InsertOrUpdate(SAPCustomerGroup);

                dummyCustomerGroup.Name = SAPCustomerGroup.Name;
                dummyCustomerGroup.Code = code; 
                dummyCustomerGroup.Manual = false;

                dummyCustomerGroup.CleanEntityRef("IDSalesArea");
                dummyCustomerGroup.SalesArea = SAPCustomerGroup.SalesArea;

                dummyCustomerGroup.IncludeInSystem = SAPCustomerGroup.IncludeInSystem;
                dummyCustomerGroup.ShowRBMInReporting = SAPCustomerGroup.ShowRBMInReporting;
                dummyCustomerGroup.IncludeInSAPOrders = SAPCustomerGroup.IncludeInSAPOrders;
                dummyCustomerGroup.SortOrder = SAPCustomerGroup.SortOrder;

                // move SAP Cg's customers to dummy
                for (int i = SAPCustomerGroup.Customers.Count - 1; i>=0; i--)
                    SAPCustomerGroup.Customers[i].CustomerGroup = dummyCustomerGroup;

                // change SAP cg's CustomerGroupItemTypes to dummy
                for (int i = SAPCustomerGroup.CustomerGroupItemTypes.Count - 1; i >= 0; i-- )
                    SAPCustomerGroup.CustomerGroupItemTypes[i].CustomerGroup = dummyCustomerGroup;
                
                // insert
                manager.InsertOrUpdate(dummyCustomerGroup);

                // delete
                removedSAPCustomerGroup = SAPCustomerGroup;
                manager.Delete(SAPCustomerGroup);

                MessageBox.Show(SystemMessagesManager.Instance.GetMessage("MergeCustomerGroupMerged"), "Merge Customer Group");

                btnClose.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
            catch (Exception exc)
            {
                removedSAPCustomerGroup = null;
                //MessageBox.Show("An error occured when replacing the dummy store with a SAP store: " + LorealOptimiseShared.Utility.GetExceptionsMessages(exc));
                MessageBox.Show(SystemMessagesManager.Instance.GetMessage("MergeCustomerGroupException", LorealOptimiseShared.Utility.GetExceptionsMessages(exc)));

                SAPCustomerGroup.Code = code;
                manager.InsertOrUpdate(SAPCustomerGroup);
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            if (Close != null)
            {
                Close(this, new EventArgs());
            }
        }
    }
}
