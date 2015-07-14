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
    /// Interaction logic for AddSales.xaml
    /// </summary>
    public partial class AddSales : BaseUserControl
    {
        // data source for grdBrandAxe
        protected ObservableCollection<BrandAxe> BrandAxeData;

        // data source for grdRetailSales
        private List<Sale> retailSalesToAdd = new List<Sale>();

        // for selecting BrandAxes
        private Dictionary<Guid, bool> selectedBrands = new Dictionary<Guid, bool>();
        private CheckBox chkSelectAll = null;

        // current Customer Store, for which user wants to add sales
        private Customer customerStore = null;

        // Benchmark Store, from which user gets retail sales information
        private Customer benchmarkCustomer = null;

        private DbDataContext db = DbDataContext.GetInstance();

        public event EventHandler Close = null;

        public AddSales(Customer customer)
        {
            InitializeComponent();

            if (DesignerProperties.GetIsInDesignMode(this) == false)
            {
                grdBrandAxes.BeginDataUpdate();
                grdBrandAxes.ClearSorting();
                grdBrandAxes.SortInfo.Add(new GridSortInfo("Signature.Name"));
                grdBrandAxes.SortInfo.Add(new GridSortInfo("FullName"));
                grdBrandAxes.EndDataUpdate();

                // Data for BrandAxes
                BrandAxeData = new ObservableCollection<BrandAxe>(BrandAxeManager.Instance.GetAll());
                DataContext = BrandAxeData;

                // Data for RetailSales to add
                grdRetailSales.DataSource = retailSalesToAdd;

                // Data for Benchmark Customer Store
                customerStore = customer;
                cboBenchmarkStore.ItemsSource = CustomerManager.Instance.GetAll().Where(c => c.ID != customerStore.ID).OrderBy(c => c.AccountNumber);

                txtStoreCode.Text = customerStore.AccountNumber;
                txtStoreName.Text = customerStore.Name;
            }
        }

        private void chkBenchmark_Checked(object sender, RoutedEventArgs e)
        {
            cboBenchmarkStore.IsEnabled = chkBenchmark.IsChecked.HasValue ? chkBenchmark.IsChecked.Value : false;
        }

        private void chkBenchmark_Unchecked(object sender, RoutedEventArgs e)
        {
            cboBenchmarkStore.IsEnabled = chkBenchmark.IsChecked.HasValue ? chkBenchmark.IsChecked.Value : false;
            cboBenchmarkStore.SelectedItem = null;
        }

        private void cboBenchmarkStore_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            benchmarkCustomer = cboBenchmarkStore.SelectedItem as Customer;

            selectedBrands.Clear();
            grdBrandAxes.RefreshData();

            retailSalesToAdd.Clear();
            grdRetailSales.RefreshData();
        }

        #region UnboundedColumn

        // CustomUnboundColumnData Event for BrandAxe GridControl
        private void grdBrandAxes_CustomUnboundColumnData(object sender, GridColumnDataEventArgs e)
        {
            Guid id = (Guid)e.GetListSourceFieldValue("ID");
            if (e.Column.FieldName == "AddBrand")
            {
                if (e.IsGetData)
                {
                    e.Value = GetIsSelected(id);
                }
                if (e.IsSetData)
                {
                    SetIsSelected(id, (bool)e.Value);

                    if (chkSelectAll.IsChecked == true && selectedBrands.Count != BrandAxeData.Count)
                        chkSelectAll.IsChecked = null;
                    if (chkSelectAll.IsChecked == false && selectedBrands.Count != BrandAxeData.Count)
                        chkSelectAll.IsChecked = null;
                }
            }
            else if (e.Column.FieldName == "RetailSales")
            {
                if (e.IsGetData)
                {
                    e.Value = Math.Round(GetRetailSales(id));
                }
            }

        }

        double GetRetailSales(Guid brandAxeId)
        {
            if (benchmarkCustomer != null)
            {
                double? retailSale = db.countSale(benchmarkCustomer.ID, null, brandAxeId, null, null,null, null, false);
                if (retailSale.HasValue)
                    return retailSale.Value;
            }
            return 0;
        }

        bool GetIsSelected(Guid id)
        {
            bool isSelected;
            if (selectedBrands.TryGetValue(id, out isSelected))
                return isSelected;
            return false;
        }

        void SetIsSelected(Guid id, bool value)
        {
            if (value)
            {
                selectedBrands[id] = value;

                for (int m = -12; m <= 0; m++)
                {
                    Sale newSale = new Sale();

                    newSale.IDBrandAxe = id;
                    newSale.IDCustomer = customerStore.ID;
                    if (benchmarkCustomer != null)
                        newSale.IDCustomer_Benchmark = benchmarkCustomer.ID;
                    newSale.Date = DateTime.Now.AddMonths(m);

                    retailSalesToAdd.Add(newSale);
                }
            }
            else
            {
                selectedBrands.Remove(id);
                retailSalesToAdd.RemoveAll(s => s.IDBrandAxe == id);
            }

            grdRetailSales.RefreshData();
        }

        private void grdRetailSales_CustomUnboundColumnData(object sender, GridColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "BrandAxe.Name")
            {
                if (e.IsGetData)
                {
                    Guid idBrandAxe = (Guid)e.GetListSourceFieldValue("IDBrandAxe");
                    e.Value = BrandAxeData.Single(b => b.ID == idBrandAxe).Name;
                }
            }
            else if (e.Column.FieldName == "RetailSales")
            {
                if (e.IsGetData)
                {
                    Guid idBrandAxe = (Guid)e.GetListSourceFieldValue("IDBrandAxe");
                    DateTime monthDate = (DateTime)e.GetListSourceFieldValue("Date");
                    retailSalesToAdd[e.ListSourceRowIndex].RetailSalesFromBenchmark = Math.Round(GetRetailSaleForBrandAxeFromBenchmark(monthDate, idBrandAxe));
                    e.Value = Math.Round(retailSalesToAdd[e.ListSourceRowIndex].RetailSalesFromBenchmark);
                }
            }
        }

        #endregion

        decimal GetRetailSaleForBrandAxeFromBenchmark(DateTime month, Guid idBrandAxe)
        {
            decimal value = 0;
            // need to modify 'First', which is now used because there is more rows for a month
            if (benchmarkCustomer != null && benchmarkCustomer.Sales != null && benchmarkCustomer.Sales.Any(s => s.IDBrandAxe == idBrandAxe && s.Date.Year == month.Year && s.Date.Month == month.Month))
            {
                Sale saleForMonth = benchmarkCustomer.Sales.FirstOrDefault(s => s.IDBrandAxe == idBrandAxe && s.Date.Year == month.Year && s.Date.Month == month.Month);
                if (saleForMonth == null)
                    value = 0;
                else
                {
                    if (saleForMonth.ManualValue.HasValue)
                        value = saleForMonth.ManualValue.Value;
                    else
                    {
                        if (saleForMonth.EPOSValue.HasValue)
                            value = saleForMonth.EPOSValue.Value;
                        else if (saleForMonth.CaCatValue.HasValue)
                        {
                            if (benchmarkCustomer.SalesArea.RetailMultiplier.HasValue)
                                value = saleForMonth.CaCatValue.Value * (decimal)benchmarkCustomer.SalesArea.RetailMultiplier.Value;
                            else
                                value = 0;
                        }
                    }
                }
            }
            else
                value = 0;
            
            return value;
        }

        private void btnAddBrandAxes_Click(object sender, RoutedEventArgs e)
        {
            BrandAxes brandAxes = new BrandAxes();
            
            PopupWindow addBrandAxeDialog = new PopupWindow("Add new Brand/Axe");
            addBrandAxeDialog.AddControl(brandAxes);
            addBrandAxeDialog.ShowDialog();

            // update grdBrandAxe DataSource
            BrandAxeData = new ObservableCollection<BrandAxe>(BrandAxeManager.Instance.GetAll());
            grdBrandAxes.DataContext = BrandAxeData;
            grdBrandAxes.RefreshData();

            // remove sales, which have a reference to one of deleted brandaxes
            retailSalesToAdd.RemoveAll(s=> BrandAxeData.Where(b=>b.ID == s.IDBrandAxe).Count() == 0);
            grdRetailSales.RefreshData();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            if (Close != null)
                Close(this, new EventArgs());
        }

        private void btnSaveSales_Click(object sender, RoutedEventArgs e)
        {
            SaleManager salesManager = SaleManager.Instance;
            foreach (Sale s in retailSalesToAdd)
            {
                if (s.AddRetailSales == true)
                {
                    if (s.ManualValue.HasValue == false)
                        s.ManualValue = s.RetailSalesFromBenchmark;
                    salesManager.InsertOrUpdate(s);

                    // unselect
                    s.AddRetailSales = false;
                }
            }

            grdRetailSales.RefreshData();
        }

        private void btnSetCapacity_Click(object sender, RoutedEventArgs e)
        {
            CustomerCapacities capacityControl = new CustomerCapacities();
            PopupWindow resetCapacityDialog = new PopupWindow("Set Capacity");
            resetCapacityDialog.AddControl(capacityControl);
            resetCapacityDialog.ShowDialog();
        }

        private void chkSelectAll_Checked(object sender, RoutedEventArgs e)
        {
            Cursor = System.Windows.Input.Cursors.Wait;
            foreach (BrandAxe ba in BrandAxeData)
                SetIsSelected(ba.ID, true);
            Cursor = System.Windows.Input.Cursors.Arrow;
            grdBrandAxes.RefreshData();
        }

        private void chkSelectAll_Unchecked(object sender, RoutedEventArgs e)
        {
            Cursor = System.Windows.Input.Cursors.Wait;
            foreach (BrandAxe ba in BrandAxeData)
            {
                SetIsSelected(ba.ID, false);
            }
            Cursor = System.Windows.Input.Cursors.Arrow;
            grdBrandAxes.RefreshData();
        }

        private void chkSelectAll_Initialized(object sender, EventArgs e)
        {
            chkSelectAll = sender as CheckBox;
            chkSelectAll.IsChecked = false;
        }
    }

}