using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using LorealOptimiseBusiness.Lists;
using LorealOptimiseData;
using LorealOptimiseBusiness;
using DevExpress.Xpf.Grid;
using DevExpress.Xpo.XtraData;
using System.Collections;
using DevExpress.Utils;

namespace LorealOptimiseGui.Lists
{
    /// <summary>
    /// Interaction logic for Sales.xaml
    /// </summary>
    public partial class Sales : BaseListUserControl<SaleManager, Sale>
    {
        private Sale NewRowItem = null;

        public Sales()
            : base()
        {
            InitializeComponent();
            if (DesignerProperties.GetIsInDesignMode(this) == false)
            {
                AllowRefreshing = false;

                Loaded +=new RoutedEventHandler(Sales_Loaded);
                AssignEvents(grdSales, true);
            }

           
        }

        protected override Hashtable Filters
        {
            get 
            {
                Hashtable conditions = new Hashtable();

                if (txtCustomerFilter.Text != String.Empty)
                {
                    conditions.Add(SaleManager.CustomerName, txtCustomerFilter.Text);
                }               

                if (cboBrandAxeFilter.SelectedIndex >= 0)
                {
                    conditions.Add(SaleManager.IDBrandAxe, ((BrandAxe)cboBrandAxeFilter.SelectedItem).ID);
                }
                if (cboSignatureFilter.SelectedIndex >= 0)
                {
                    conditions.Add(SaleManager.IDSignature, ((Signature)cboSignatureFilter.SelectedItem).ID);
                }
                if (startDateFilter.EditValue != null)
                {
                    conditions.Add(SaleManager.DateFrom, startDateFilter.EditValue);
                }
                if (endDateFilter.EditValue != null)
                {
                    conditions.Add(SaleManager.DateTo, endDateFilter.EditValue);
                }

                return conditions;
            }
        }

        void Sales_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsVisible)
            {
               
                // filter                
                cboBrandAxeFilter.ItemsSource = new BrandAxe[] { new BrandAxe() }.Union(BrandAxeManager.Instance.GetAllForAllocation());
                cboSignatureFilter.ItemsSource = new Signature[] { new Signature() }.Union(SignatureManager.Instance.GetAll().OrderBy(c=>c.Name));
                cboCustomerGroup.ItemsSource = CustomerGroupManager.Instance.GetAll().Union(new CustomerGroup[] { new CustomerGroup() });
                
                this.Cursor = Cursors.Wait;

                cboCustomers.ItemsSource = CustomerManager.Instance.GetAll();
                cboBrandAxes.ItemsSource = BrandAxeManager.Instance.GetAll();
                cboBenchmarkCustomers.ItemsSource = CustomerManager.Instance.GetAll();

                this.Cursor = Cursors.Arrow;
            }
        }

        private void TableView_ShowingEditor(object sender, ShowingEditorEventArgs e)
        {
            if (e.Column.FieldName == "IDCustomer" || e.Column.FieldName == "IDBrandAxe" || e.Column.FieldName == "IDCustomer_Benchmark")
            {
                if (e.RowHandle != GridControl.NewItemRowHandle)
                {
                    e.Cancel = true;
                }
            }
        }

        private void TableView_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName == "IDCustomer")
            {
                Customer customer = CustomerManager.Instance.GetAll().SingleOrDefault(c => c.ID == (Guid)e.Value);
                if (customer != null)
                {
                    (e.Row as Sale).Customer = customer;

                    cboBenchmarkCustomers.ItemsSource = CustomerManager.Instance.GetAll().Except(new Customer[]{customer});
                }

            }
            else if (e.Column.FieldName == "IDBrandAxe")
            {
                BrandAxe brandaxe = BrandAxeManager.Instance.GetAll().Single(b => b.ID == (Guid)e.Value);
                if (brandaxe != null)
                {
                    (e.Row as Sale).BrandAxe = brandaxe;
                }
            }

            if (e.RowHandle == GridControl.NewItemRowHandle)
            {
                NewRowItem = e.Row as Sale;
            }
        }

        private void TableView_RowCanceled(object sender, RowEventArgs e)
        {
            if (NewRowItem != null)
            {
                if (NewRowItem.Customer != null)
                    NewRowItem.Customer.Sales.Remove(NewRowItem);
                if (NewRowItem.BrandAxe != null)
                    NewRowItem.BrandAxe.Sales.Remove(NewRowItem);
                NewRowItem = null;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        private void cboCustomerFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Refresh();
            }
        }

        private void TableView_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            Sale newSale = Data.SingleOrDefault(s => s.ID == Guid.Empty);
            if (newSale != null)
            {
                newSale.Date = DateTime.Now;
            }
        }

    }

}
