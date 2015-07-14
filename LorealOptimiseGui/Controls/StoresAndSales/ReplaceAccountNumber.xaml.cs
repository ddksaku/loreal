using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Data;
using LorealOptimiseBusiness.Lists;
using LorealOptimiseData;
using LorealOptimiseBusiness;
using LorealOptimiseGui.Lists;

namespace LorealOptimiseGui.Controls
{
    /// <summary>
    /// Interaction logic for ReplaceAccountNumber.xaml
    /// </summary>
    public partial class ReplaceAccountNumber : BaseUserControl
    {
        // customer store, for which we do replace account number
        private Customer dummyCustomer = null;
        private Customer SAPCustomer = null;

        // EventHandler for Closing Parent Window
        public event EventHandler Close = null;

        public ReplaceAccountNumber(Customer customer)
        {
            InitializeComponent();

            dummyCustomer = customer;

            txtAccountNumber.Text = dummyCustomer.AccountNumber;
            txtName.Text = dummyCustomer.Name;

            this.DataContext = CustomerManager.Instance.GetAll().Where(c=>c.Manual == false);
          
        }

        // result of Replacing
        private Customer removedSAPCustomer = null;
        public Customer RemovedSAPCustomer
        {
            get
            {
                return removedSAPCustomer;
            }
        }

        private void btnReplace_Click(object sender, RoutedEventArgs e)
        {
            if (SAPCustomer == null)
            {
               // MessageBox.Show("Select a store to replace from");
                MessageBox.Show(SystemMessagesManager.Instance.GetMessage("ReplaceAccounNumberSelectStore"));
                return;
            }

            try
            {
                dummyCustomer.Name = SAPCustomer.Name;
                dummyCustomer.AccountNumber = SAPCustomer.AccountNumber;
                dummyCustomer.Manual = false;

                dummyCustomer.CleanEntityRef("IDCustomerGroup");
                dummyCustomer.CustomerGroup = SAPCustomer.CustomerGroup;

                dummyCustomer.CleanEntityRef("IDSalesArea_AllocationSalesArea");
                dummyCustomer.SalesArea = SAPCustomer.SalesArea;

                dummyCustomer.CleanEntityRef("IDSalesEmployee");
                dummyCustomer.SalesEmployee = SAPCustomer.SalesEmployee;

                // insert & delete
                CustomerManager manager = CustomerManager.Instance;

                // insert
                manager.InsertOrUpdate(dummyCustomer);

                // delete
                removedSAPCustomer = SAPCustomer;
                SAPCustomer.Deleted = true;
                manager.Delete(SAPCustomer);

                //MessageBox.Show("Successfully replaced the dummy store with a SAP store", "Replace Account");
                MessageBox.Show(SystemMessagesManager.Instance.GetMessage("ReplaceAccountNumberReplaced"), "Replace Account");

                btnClose.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
            catch (Exception exc)
            {
                removedSAPCustomer = null;
                //MessageBox.Show("An error occured when replacing the dummy store with a SAP store: " + LorealOptimiseShared.Utility.GetExceptionsMessages(exc));
                MessageBox.Show(SystemMessagesManager.Instance.GetMessage("ReplaceAccountNumberException", LorealOptimiseShared.Utility.GetExceptionsMessages(exc)));
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            if (Close != null)
            {
                Close(this, new EventArgs());
            }
        }

        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            if (grdCustomers.View.FocusedRow != null)
            {
                SAPCustomer = grdCustomers.GetFocusedRow() as Customer;
            }
        }

    }
}
