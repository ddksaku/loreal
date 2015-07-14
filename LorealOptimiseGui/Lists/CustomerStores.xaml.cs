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
using DevExpress.Xpf.Data;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Editors;
using LorealOptimiseGui.Controls;
using System.Collections;
using System.Linq.Expressions;
using System.Data.Linq.SqlClient;
using System.Data.SqlClient;
using DevExpress.Data.Filtering;
using LorealOptimiseGui.Controls.StoresAndSales;
using LorealOptimiseData.Enums;
using LorealOptimiseShared;
using System.Threading;

namespace LorealOptimiseGui.Lists
{
    /// <summary>
    /// Interaction logic for CustomerStores.xaml
    /// </summary>
    public partial class CustomerStores : BaseListUserControl<CustomerManager, Customer>
    {
        private IEnumerable<Category> AllCategories = null;
        private Customer NewRowItem = null;
        private CheckComboBox chkComboForInsert = null;

        private CustomerGroup OldCustomerGroup = null;
        private SalesArea OldSalesArea = null;
        private Customer OldRowItem = null;

        public CustomerStores()
            : base()
        {
            InitializeComponent();
            if (DesignerProperties.GetIsInDesignMode(this) == false)
            {
                AllowRefreshing = false;

                (grdCustomerStores.View as TableView).CellValueChanged += new CellValueChangedEventHandler(View_CellValueChangedBeforeSave);

                Loaded += new RoutedEventHandler(CustomerStores_Loaded);

                AssignEvents(grdCustomerStores, true);

                grdCustomerStores.View.ShowFilterPopup += new FilterPopupEventHandler(View_ShowFilterPopup);
                grdCustomerStores.CustomRowFilter += new RowFilterEventHandler(grdCustomerStores_CustomRowFilter);
                (grdCustomerStores.View as TableView).RowUpdated += new RowEventHandler(View_RowUpdated);

                (grdCustomerStores.View as TableView).CellValueChanged += new CellValueChangedEventHandler(TableView_CellValueChanged);

                CustomerManager.Instance.PropertyChanged += new PropertyChangedEventHandler(Instance_PropertyChanged);

                // Account Number Validation
                grdCustomerStores.Columns["AccountNumber"].Validate +=
                    new GridCellValidationEventHandler(CustomerStores_Validate);
                this.colSalesArea.Validate += new GridCellValidationEventHandler(SalesAreaColumn_Validate);

                // Security stuff
                if (LoggedUser.GetInstance().IsInRole(RoleEnum.DivisionAdmin) || LoggedUser.GetInstance().IsInRole(RoleEnum.SystemAdmin))
                {
                    btnGenerate.IsEnabled = true;
                }
                else
                {
                    btnGenerate.IsEnabled = false;
                }

                if (LoggedUser.GetInstance().IsInRole(LorealOptimiseData.Enums.RoleEnum.DivisionAdmin) == false)
                {
                    colCustomerGroup.ReadOnly = true;
                    colAccountNumber.ReadOnly = true;
                    colStoreName.ReadOnly = true;
                    colRetailSales.ReadOnly = true;
                    colSalesArea.ReadOnly = true;
                    colSalesEmployee.ReadOnly = true;
                    colSystemInclude.ReadOnly = true;
                    colStoreCategory.ReadOnly = true;

                    colOperation.Visible = false;
                }
            }
        }

        void View_CellValueChangedBeforeSave(object sender, CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName == "IncludeInSystem")
            {
                LongTaskExecutor.DoEvents();    
            }
        }

        void Instance_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "NewCustomerGroup")
            {
                CustomerGroupManager.Instance.Refresh();
                cboCustomerGroupFilter.ItemsSource = new CustomerGroup[] { new CustomerGroup() }.Union(CustomerGroupManager.Instance.GetAll());
                cboCustomerGroup.ItemsSource = new CustomerGroup[] { new CustomerGroup() }.Union(CustomerGroupManager.Instance.GetAll());
            }
        }

        void Instance_EntityChanged(object sender, Customer entity)
        {
            if (entity != null)
            {
                
            }
        }

        void View_RowUpdated(object sender, RowEventArgs e)
        {
            chkComboForInsert.IsEnabled = false;
        }

        void grdCustomerStores_CustomRowFilter(object sender, RowFilterEventArgs e)
        {

        }

        private ComboBoxEdit cboCatFilter = null;
        void View_ShowFilterPopup(object sender, FilterPopupEventArgs e)
        {
            if (e.Column.FieldName == "StoreCategory")
            {
                if (cboCatFilter == null)
                {
                    cboCatFilter = e.ComboBoxEdit;
                    cboCatFilter.PopupClosed += new ClosePopupEventHandler(ComboBoxEdit_PopupClosed);
                    cboCatFilter.PopupFooterButtons = new PopupFooterButtons();
                }

                cboCatFilter.StyleSettings = new CheckedComboBoxStyleSettings();
                cboCatFilter.ItemsSource = AllCategories;
                cboCatFilter.ValueMember = "Name";
                cboCatFilter.DisplayMember = "Name";
                cboCatFilter.ShowEditorButtons = true;

                string filterString = grdCustomerStores.FilterString;
                cboCatFilter.SelectedItems.Clear();
                foreach (Category cat in AllCategories)
                {
                    if (filterString.Contains(cat.Name))
                    {
                        cboCatFilter.SelectedItems.Add(cat);
                    }
                }
            }
        }

        void ComboBoxEdit_PopupClosed(object sender, ClosePopupEventArgs e)
        {
            if (e.EditValue != null)
            {
                CriteriaOperatorCollection criteriaCollection = new CriteriaOperatorCollection();
                List<object> items = e.EditValue as List<object>;
                
                foreach (object item in items)
                {
                    criteriaCollection.Add(CriteriaOperator.Parse(string.Format("[StoreCategories] like '%;{0};%'", item.ToString())));
                }


                if (criteriaCollection.Count > 0)
                {
                    grdCustomerStores.MergeColumnFilters(CriteriaOperator.And(criteriaCollection));
                }
                else
                {
                    grdCustomerStores.ClearColumnFilter("StoreCategories");
                }

                e.Handled = true;
            }
        }

        protected override Hashtable Filters
        {
            get
            {
                Hashtable conditions = new Hashtable();

                if (txtCustomer.Text != String.Empty)
                {
                    conditions.Add(CustomerManager.CustomerName, txtCustomer.Text);
                }
                if (txtAccount.Text != String.Empty)
                {
                    conditions.Add(CustomerManager.AccountNumber, txtAccount.Text);
                }
                if (cboSalesEmployeeFilter.SelectedIndex >= 0)
                {
                    conditions.Add(CustomerManager.IDSalesEmployee, ((SalesEmployee)cboSalesEmployeeFilter.SelectedItem).ID);
                }
                if (cboCustomerGroupFilter.SelectedIndex >= 0)
                {
                    conditions.Add(CustomerManager.IDCustomerGroup, ((CustomerGroup)cboCustomerGroupFilter.SelectedItem).ID);
                }

                return conditions;
            }
        }

        void CustomerStores_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsVisible)
            {
                // filter              
                cboCustomerGroupFilter.ItemsSource = new CustomerGroup[] { new CustomerGroup() }.Union(CustomerGroupManager.Instance.GetAll());
                cboSalesEmployeeFilter.ItemsSource = new SalesEmployee[] { new SalesEmployee() }.Union(SalesEmployeeManager.Instance.GetAll());

                // Categories for 'Store Category' Column
                AllCategories = CategoryManager.Instance.GetAll();

                cboSalesArea.ItemsSource = SalesAreaManager.Instance.GetAll();
                cboCustomerGroup.ItemsSource = new CustomerGroup[] { new CustomerGroup() }.Union(CustomerGroupManager.Instance.GetAll());
                cboSalesEmployee.ItemsSource = SalesEmployeeManager.Instance.GetAll();
            }
        }

        void SalesAreaColumn_Validate(object sender, GridCellValidationEventArgs e)
        {
            Customer customer = e.Row as Customer;

            if (customer != null)
            {
                string errorMessage = String.Empty;

                bool canChange = CustomerManager.Instance.CanChangeSalesArea(customer.ID, ref errorMessage);
                if (canChange == false)
                {
                    e.SetError(errorMessage);
                    e.IsValid = false;

                    if (grdCustomerStores.View.ValidationError != null && grdCustomerStores.View.ValidationError.ErrorContent != null)
                    {
                        if (grdCustomerStores.View.ValidationError.ErrorContent.ToString() == errorMessage)
                        {
                            return;
                        }
                    }

                    MessageBox.Show(errorMessage);
                }

            }
        }

        void CustomerStores_Validate(object sender, GridCellValidationEventArgs e)
        {
            //if(this.Data.Any(c=>c.AccountNumber == e.Value.ToString()))
            //{
            //    e.SetError("New account number must not match existing account numbers.");
            //}
        }

        private void btnAddSales_Initialized(object sender, EventArgs e)
        {
            EditGridCellData gridCellData = (sender as Button).DataContext as EditGridCellData;
            if (gridCellData != null)
            {
                RowTypeDescriptor rowType = gridCellData.Data as RowTypeDescriptor;
                if (rowType != null)
                {
                    if (rowType.RowHandle.Value == GridControl.NewItemRowHandle)
                    {
                        // new row 
                        (sender as FrameworkElement).Visibility = Visibility.Collapsed;
                    }
                }
            }


        }

        private void btnAddSales_Click(object sender, RoutedEventArgs e)
        {
            if (grdCustomerStores.View.FocusedRow != null)
            {
                AddSales addSalesControl = new AddSales(grdCustomerStores.View.FocusedRow as Customer);
                PopupWindow addSalesDialog = new PopupWindow("Add Sales");
                addSalesControl.Close += new EventHandler(addSalesDialog.CloseWindowEvent);
                addSalesDialog.AddControl(addSalesControl);
                addSalesDialog.ShowDialog();
            }
        }

        private void btnReplaceAccNumber_Click(object sender, RoutedEventArgs e)
        {
            if (grdCustomerStores.View.FocusedRow != null)
            {
                int rowHandle = grdCustomerStores.View.FocusedRowHandle;

                ReplaceAccountNumber replaceAccountControl = new ReplaceAccountNumber(grdCustomerStores.View.FocusedRow as Customer);

                PopupWindow replaceAccountDialog = new PopupWindow("Replace Account");
                replaceAccountControl.Close += new EventHandler(replaceAccountDialog.CloseWindowEvent);
                replaceAccountDialog.AddControl(replaceAccountControl);
                replaceAccountDialog.ShowDialog();

                if (replaceAccountControl.RemovedSAPCustomer != null)
                {
                    int pos = this.Data.IndexOf(replaceAccountControl.RemovedSAPCustomer);
                    if (pos >= 0)
                    {
                        (grdCustomerStores.View as TableView).DeleteRow(grdCustomerStores.GetRowHandleByListIndex(pos));
                    }
                }
            }
        }

        private void btnSetCapacities_Click(object sender, RoutedEventArgs e)
        {
            if (grdCustomerStores.View.FocusedRow != null)
            {
                Customer selectedCustomer = grdCustomerStores.View.FocusedRow as Customer;
                CustomerCapacities capacityControl = new CustomerCapacities(selectedCustomer);
                PopupWindow resetCapacityDialog = new PopupWindow("Set Capacity");
                resetCapacityDialog.AddControl(capacityControl);
                resetCapacityDialog.ShowDialog();
            }
        }

        private void chkCboStoreCategory_Loaded(object sender, RoutedEventArgs e)
        {
            CheckComboBox chkCombo = sender as CheckComboBox;

            chkCombo.IsReadOnly = colStoreCategory.ReadOnly;

            if (chkCombo != null && chkCombo.ParentDataID == Guid.Empty)
            {
                chkComboForInsert = chkCombo;
                chkComboForInsert.IsEnabled = false;
            }

            if (AllCategories != null)
            {
                chkCombo.StyleSettings = new CheckedComboBoxStyleSettings();
                chkCombo.ItemsSource = AllCategories.OrderBy(cat => cat.Name);
                chkCombo.SeparatorString = ((char)13).ToString() + ((char)10).ToString();
                chkCboStoreCategory_UpdateSelectedItems(chkCombo, new DependencyPropertyChangedEventArgs());
            }
        }

        private void chkCboStoreCategory_UpdateSelectedItems(object sender, DependencyPropertyChangedEventArgs e)
        {
            CheckComboBox chkCombo = sender as CheckComboBox;
            if ((Data != null) && (this.Data.Any(c => c.ID == chkCombo.ParentDataID) == true))
            {
                Customer selectedCustomer = this.Data.Single(c => c.ID == chkCombo.ParentDataID);

                try
                {
                    List<Category> items = new List<Category>();
                    foreach (CustomerCategory cc in selectedCustomer.ActiveCustomerCategories)
                    {
                        //we need to get category from chbox ItemSource (AllCategories property), otherwise it may happen sometime (when we go to animation detail and back to CustomerStore), that selectedCustomer.ActiveCustomerCategories is loaded in different datacontext, and items in checkbox will not be checked
                        Category cat = AllCategories.Where(c => c.ID == cc.IDCategory).FirstOrDefault();

                        if (items.Contains(cat) == false)
                        {
                            items.Add(cat);
                        }
                    }

                    chkCombo.Text = items.ToString();
                    chkCombo.EditValue = items;

                    // update SelectedItems to show checked categories, it should be after seeting EditValue
                    chkCombo.SelectedItems.Clear();
                    foreach (Category c in items)
                    {
                        chkCombo.SelectedItems.Add(c);
                    }
                    
                }
                catch (Exception exc)
                {
                    //MessageBox.Show("An error occured when updating Store Categories:" + LorealOptimiseShared.Utility.GetExceptionsMessages(exc));
                    MessageBox.Show(SystemMessagesManager.Instance.GetMessage("CustomerStoreExceptionUpdate", LorealOptimiseShared.Utility.GetExceptionsMessages(exc)));
                }

            }
        }

        void chkCboStoreCategory_PopupOpening(object sender, RoutedEventArgs e)
        {
            CheckComboBox chkCombo = (sender as CheckComboBox);
            if (chkCombo != null)
            {
                Customer selectedCustomer = this.Data.Single(c => c.ID == chkCombo.ParentDataID);

                chkCboStoreCategory_UpdateSelectedItems(chkCombo, new DependencyPropertyChangedEventArgs());
            }
        }

        void chkCombo_PopupOpened(object sender, RoutedEventArgs e)
        {
           
        }

        private void chkCboStoreCategory_PopupClosed(object sender, ClosePopupEventArgs e)
        {
            if (e.CloseMode == PopupCloseMode.Normal)
            {
                CheckComboBox chkCombo = (sender as CheckComboBox);
                if (this.Data.Any(c => c.ID == chkCombo.ParentDataID) == true)
                {
                    Customer selectedCustomer = this.Data.Single(c => c.ID == chkCombo.ParentDataID);
                    
                    if (e.EditValue != null)
                    {
                        try
                        {
                            for (int i = selectedCustomer.ActiveCustomerCategories.Count - 1; i >= 0; i--)
                            {
                                CustomerCategory cc = selectedCustomer.ActiveCustomerCategories[i];
                                if (chkCombo.SelectedItems.Contains(cc.Category))
                                    continue;

                                // first delete from CustomerCategory and remove it from selected Customer
                                if (chkCombo.ParentDataID != Guid.Empty)
                                    CustomerCategoryManager.Instance.Delete(cc);
                                selectedCustomer.CustomerCategories.Remove(cc);
                            }
                            if (chkCombo.ParentDataID != Guid.Empty)
                                this.Manager.InsertOrUpdate(selectedCustomer);

                            foreach (Category cat in chkCombo.SelectedItems)
                            {
                                if (selectedCustomer.CustomerCategories.Count(cc => cc.IDCategory == cat.ID) > 0)
                                    continue;

                                // if new selected category is not in  add new CustomerCategory
                                CustomerCategory newCC = new CustomerCategory();
                                newCC.IDCategory = cat.ID;
                                selectedCustomer.CustomerCategories.Add(newCC);
                            }
                            if (chkCombo.ParentDataID != Guid.Empty)
                                this.Manager.InsertOrUpdate(selectedCustomer);
                        }
                        catch (SqlException sqlExc)
                        {
                            if (sqlExc.Number == 50000 && sqlExc.Class == 16 && sqlExc.State == 36)
                            {
                                if (sqlExc.Errors.Count > 0)
                                {
                                    MessageBox.Show(sqlExc.Errors[0].Message);
                                }
                                else
                                {
                                    MessageBox.Show(sqlExc.Message);
                                }

                                chkCboStoreCategory_UpdateSelectedItems(chkCombo, new DependencyPropertyChangedEventArgs());
                            }
                            else
                            {
                                MessageBox.Show(sqlExc.Message);
                            }
                        }
                        catch (Exception exc)
                        {
                            throw exc;
                        }

                        selectedCustomer.ActiveCustomerCategories = null;
                    }
                }
            }
        }

        private void TableView_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            try
            {
                Customer newCustomer = this.Data.Single(c => c.ID == Guid.Empty);
                newCustomer.Manual = true;
                newCustomer.IncludeInSystem = true;
                chkComboForInsert.IsEnabled = true;

                Category allCategory = CategoryManager.Instance.GetAll().FirstOrDefault(c => c.Name == "All");
                if (allCategory != null)
                {
                    chkComboForInsert.SelectedItems.Add(allCategory);
                }

                this.NewRowItem = newCustomer;
            }
            catch (Exception exc)
            {
                //MessageBox.Show("An error occured when initialzing values for a new row:" + LorealOptimiseShared.Utility.GetExceptionsMessages(exc));
                MessageBox.Show(SystemMessagesManager.Instance.GetMessage("CustomerStoreExceptionNewRow", LorealOptimiseShared.Utility.GetExceptionsMessages(exc)));
            }
        }

        private void TableView_ShowingEditor(object sender, ShowingEditorEventArgs e)
        {
            string columnName = e.Column.FieldName;
            if (columnName == "IDSalesEmployee" || columnName == "AccountNumber" || columnName == "Name")
            {
                if (e.RowHandle >= 0)
                    e.Cancel = true;
            }
            else if (columnName == "StoreCategory")
            {
                
            }
        }

        private void TableView_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            DbDataContext Db = DbDataContext.GetInstance();
            if (e.RowHandle == GridControl.NewItemRowHandle)
            {
                if (e.Column.FieldName == "IDSalesEmployee")
                {
                    SalesEmployee se = Db.SalesEmployees.Single(s => s.ID == (Guid)e.Value);
                    (e.Row as Customer).SalesEmployee = se;
                }
            }

            if (e.Column.FieldName == "IDSalesArea_AllocationSalesArea")
            {
                SalesArea sa = Db.SalesAreas.Single(s => s.ID == (Guid)e.Value);
                (e.Row as Customer).SalesArea = sa;
            }
            else if (e.Column.FieldName == "IDCustomerGroup")
            {
                CustomerGroup cg = Db.CustomerGroups.Single(c => c.ID == (Guid)e.Value);
                (e.Row as Customer).CustomerGroup = cg;
            }

            if (e.Column.FieldName == "IncludeInSystem")
            {
                Thread.Sleep(1000);
                string taskName = (e.Row as Customer).IncludeInSystem == true ? "Including customer" : "Excluding customer";
                LongTaskExecutor.RaiseLongTaskEvent(this, new LongTaskEventArgs(taskName, TaskStatus.Finished));
            }
        }

        private void TableView_RowCanceled(object sender, RowEventArgs e)
        {
            if (NewRowItem != null)
            {
                if (NewRowItem.CustomerGroup != null)
                    NewRowItem.CustomerGroup.Customers.Remove(NewRowItem);
                if (NewRowItem.SalesArea != null)
                    NewRowItem.SalesArea.Customers.Remove(NewRowItem);
                if (NewRowItem.SalesEmployee != null)
                    NewRowItem.SalesEmployee.Customers.Remove(NewRowItem);
                NewRowItem = null;
            }

            if (OldRowItem != null)
            {
                OldRowItem.CleanEntityRef("IDCustomerGroup");
                OldRowItem.CleanEntityRef("IDSalesArea_AllocationSalesArea");

                OldRowItem.CustomerGroup = OldCustomerGroup;
                OldRowItem.SalesArea = OldSalesArea;

                OldRowItem = null;
            }

            if (chkComboForInsert != null)
            {
                chkComboForInsert.SelectedItems.Clear();
                chkComboForInsert.Text = "";
                chkComboForInsert.IsEnabled = false;
            }
        }

        private void TableView_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            NewRowItem = null;
            OldRowItem = null;
            if (chkComboForInsert != null && (sender as TableView).FocusedRowHandle != GridControl.NewItemRowHandle)
            {
                chkComboForInsert.SelectedItems.Clear();
                chkComboForInsert.Text = "";
                chkComboForInsert.IsEnabled = false;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
            (grdCustomerStores.View as TableView).NewItemRowPosition = NewItemRowPosition.Top;
        }

        private void TableView_ShownEditor(object sender, EditorEventArgs e)
        {
            //if (e.Column.FieldName == "IDCustomerGroup")
            //{
            //    ComboBoxEdit edit = e.Editor as ComboBoxEdit;
            //    if (e.Row != null)
            //    {
            //        Customer customer = e.Row as Customer;
            //        edit.ItemsSource = CustomerGroupManager.Instance.GetAll().Where(cg=>cg.IDSalesArea == customer.IDSalesArea_AllocationSalesArea);
            //    }
            //}
        }

        private void TableView_CellValueChanging(object sender, CellValueChangedEventArgs e)
        {
            //if (e.Column.FieldName == "IDSalesArea_AllocationSalesArea" && e.Row != null)
            //{
            //    Customer customer = e.Row as Customer;

            //    OldCustomerGroup = customer.CustomerGroup;
            //    OldSalesArea = customer.SalesArea;
            //    OldRowItem = customer;

            //    customer.CleanEntityRef("IDCustomerGroup");
            //    customer.IDCustomerGroup = Guid.Empty;
            //}

            if (e.Column.FieldName == "IncludeInSystem")
            {
                string taskName = (e.Row as Customer).IncludeInSystem == false ? "Including customer" : "Excluding customer";
                LongTaskExecutor.RaiseLongTaskEvent(this, new LongTaskEventArgs(taskName, TaskStatus.Started));
            }
        }

        private void txtCustomer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Refresh();
            }
        }

        private void btnGenerate_Click(object sender, RoutedEventArgs r)
        {
            AddCapacities generateCapacitiesCtrl = new AddCapacities();
            PopupWindow createProductDialog = new PopupWindow("Generating capacities");
            createProductDialog.Width = 950;
            createProductDialog.Height = 600;
            createProductDialog.AddControl(generateCapacitiesCtrl);
            createProductDialog.Show();
        }
    }

}
