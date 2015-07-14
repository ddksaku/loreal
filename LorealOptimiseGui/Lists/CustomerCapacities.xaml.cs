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
using System.Collections.ObjectModel;
using System.Windows.Markup;
using LorealOptimiseBusiness.Lists;
using LorealOptimiseBusiness.ViewMode;
using LorealOptimiseData;
using LorealOptimiseBusiness;
using DevExpress.Xpf.Grid;
using System.Collections;
using System.Linq.Expressions;
using System.Data.Linq.SqlClient;
using Microsoft.Reporting.WinForms;
using LorealOptimiseGui.Enums;
using LorealOptimiseGui.Controls.StoresAndSales;
using LorealOptimiseShared;
using LorealOptimiseData.Enums;

namespace LorealOptimiseGui.Lists
{
    /// <summary>
    /// Interaction logic for CustomerCapacities.xaml
    /// </summary>
    public partial class CustomerCapacities : BaseListUserControl<CustomerCapacityManager, CustomerCapacity>
    {
        private CustomerCapacity NewRowItem = null;
        private Customer selectedCustomer = null;

        PopupWindow createProductDialog;

        protected override Hashtable Filters
        {
            get
            {
                Hashtable conditions = new Hashtable();

                if (txtCustomer.Text != String.Empty)
                {
                    conditions.Add(CustomerCapacityManager.CustomerName, txtCustomer.Text);
                }
                if (cboAnimationTypeFilter.SelectedIndex >= 0)
                {
                    conditions.Add(CustomerCapacityManager.IDAnimationType, ((AnimationType)cboAnimationTypeFilter.SelectedItem).ID);
                }
                if (cboPriorityFilter.SelectedIndex >= 0)
                {
                    conditions.Add(CustomerCapacityManager.IDPriority, ((Priority)cboPriorityFilter.SelectedItem).ID);
                }
                if (cboItemTypeFilter.SelectedIndex >= 0)
                {
                    conditions.Add(CustomerCapacityManager.IDItemType, ((ItemType)cboItemTypeFilter.SelectedItem).ID);
                }

                return conditions;
            }
        }
           
       
        public CustomerCapacities()
            : base()
        {
            InitializeComponent();
            if (DesignerProperties.GetIsInDesignMode(this) == false)
            {
                AllowRefreshing = false;
                Loaded += new RoutedEventHandler(CustomerCapacities_Loaded);
            }

            if (LoggedUser.GetInstance().IsInRole(RoleEnum.DivisionAdmin) || LoggedUser.GetInstance().IsInRole(RoleEnum.SystemAdmin))
            {
                btnGenerate.IsEnabled = true;
            }
            else
            {
                btnGenerate.IsEnabled = false;
            }
        }

        public CustomerCapacities(Customer customer)
            : this()
        {
            if (customer != null)
            {
                selectedCustomer = customer;
                Data = new ExtendedObservableCollection<CustomerCapacity>(CustomerCapacityManager.Instance.GetAll(selectedCustomer));
                DataContext = Data;
                this.AllowRefreshing = false;
            }
        }

        void CustomerCapacities_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsVisible)
            {
                AssignEvents(grdCustomerCapacities, true);

                grdCustomerCapacities.View.AllowEditing = true;

                if (LoggedUser.GetInstance().IsInRole(RoleEnum.DivisionAdmin) || LoggedUser.GetInstance().IsInRole(LorealOptimiseData.Enums.RoleEnum.NAMs))
                {
                    colCapacity.ReadOnly = false;
                }
                else
                {
                    colCapacity.ReadOnly = true;
                }

                cboAnimationType.ItemsSource = AnimationTypeManager.Instance.GetAll();
                cboItemType.ItemsSource = ItemTypeManager.Instance.GetAll();
                cboPriority.ItemsSource = PriorityManager.Instance.GetAll();

                if (selectedCustomer == null)
                {
                    cboCustomer.ItemsSource = CustomerManager.Instance.GetAll();
                }
                else
                {
                    cboCustomer.ItemsSource = new Customer[] { selectedCustomer };
                }

                // filter
                cboAnimationTypeFilter.ItemsSource = new AnimationType[] {new AnimationType()}.Union(AnimationTypeManager.Instance.GetAll());
                cboPriorityFilter.ItemsSource = new Priority[] {new Priority()}.Union(PriorityManager.Instance.GetAll());
                cboItemTypeFilter.ItemsSource = new ItemType[] {new ItemType() }.Union(ItemTypeManager.Instance.GetAll());

                grdCustomerCapacities.BeginDataUpdate();
                grdCustomerCapacities.SortInfo.Clear();
                grdCustomerCapacities.SortInfo.Add(new GridSortInfo("IDAnimationType"));
                grdCustomerCapacities.SortInfo.Add(new GridSortInfo("IDPriority"));

                grdCustomerCapacities.GroupBy(clmCustomerGroupName);
                grdCustomerCapacities.GroupBy(clmAccountNumber);
                grdCustomerCapacities.GroupBy(clmItemType);
                grdCustomerCapacities.EndDataUpdate();
            }
        }

        private void TableView_ShowingEditor(object sender, ShowingEditorEventArgs e)
        {
            string fieldName = e.Column.FieldName;
            if (fieldName == "IDCustomer" || fieldName == "IDAnimationType" || fieldName == "IDPriority" || fieldName == "IDItemType")
            {
                if (e.RowHandle != GridControl.NewItemRowHandle && e.RowHandle != GridControl.AutoFilterRowHandle)
                {
                    e.Cancel = true;
                }
            }

        }

        private void TableView_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            if (e.RowHandle == GridControl.NewItemRowHandle)
            {
                if (e.Column.FieldName == "IDCustomer")
                {
                    Customer cus = DbDataContext.GetInstance().Customers.SingleOrDefault(c => c.ID == (Guid)e.Value);
                    (e.Row as CustomerCapacity).Customer = cus;
                }
            }
        }

        private void TableView_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            NewRowItem = this.Data.SingleOrDefault(c => c.ID == Guid.Empty);
        }

        private void TableView_RowCanceled(object sender, RowEventArgs e)
        {
            if (NewRowItem != null)
            {
                if (NewRowItem.Customer != null)
                {
                    NewRowItem.Customer.CustomerCapacities.Remove(NewRowItem);
                }

                NewRowItem = null;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            ReportParameter[] p = new ReportParameter[5];

            // Priority
            if (cboPriorityFilter.SelectedIndex >= 0)
            {
                ReportParameter rp3 = new ReportParameter("Priority", ((Priority)cboPriorityFilter.SelectedItem).ID.ToString());
                p[2] = rp3;
            }
            else
            {
                string[] priorityAll = (from prio in DbDataContext.GetInstance().Priorities
                                        where prio.Deleted == false
                                        select prio.ID.ToString().ToLower()).ToArray();
                
                ReportParameter rp3 = new ReportParameter("Priority", priorityAll);
                p[2] = rp3;
            }

            // Animation Type
            if (cboAnimationTypeFilter.SelectedIndex >= 0)
            {             
                ReportParameter rp = new ReportParameter("AnimationType", ((AnimationType)cboAnimationTypeFilter.SelectedItem).ID.ToString());
                p[0] = rp;
            }
            else
            {
                string[] animationTypeAll = (from aniT in DbDataContext.GetInstance().AnimationTypes
                                             where aniT.Deleted == false
                                             select aniT.ID.ToString().ToLower()).ToArray();

                ReportParameter rp = new ReportParameter("AnimationType", animationTypeAll);
                p[0] = rp;
            }

            // Item Type
            if (cboItemTypeFilter.SelectedIndex >= 0)
            {
                ReportParameter rp2 = new ReportParameter("ItemType", ((ItemType)cboItemTypeFilter.SelectedItem).ID.ToString());
                p[1] = rp2;
            }
            else
            {
                string[] itemTypeAll = (from iteT in DbDataContext.GetInstance().ItemTypes
                                        where iteT.Deleted == false
                                        select iteT.ID.ToString().ToLower()).ToArray();               

                ReportParameter rp2 = new ReportParameter("ItemType", itemTypeAll);
                p[1] = rp2;
            }

            p[3] = new ReportParameter("DivisionID", LoggedUser.LoggedDivision.ID.ToString());


            // Customer
            string[] customerSelected = (from c in DbDataContext.GetInstance().Customers
                                         join cg in DbDataContext.GetInstance().CustomerGroups on c.IDCustomerGroup equals cg.ID 
                                         join sa in DbDataContext.GetInstance().SalesAreas on cg.IDSalesArea equals sa.ID 
                                         join d in DbDataContext.GetInstance().Divisions on sa.IDDivision equals d.ID 
                                         where c.Deleted == false 
                                                && d.ID == LoggedUser.LoggedDivision.ID && SqlMethods.Like(c.Name, "%" + txtCustomer.Text + "%")
                                                && cg.IncludeInSystem == true
                                                && sa.Deleted == false
                                                && d.Deleted == false
                                         select c.ID.ToString().ToLower()).ToArray();
            
            p[4] = new ReportParameter("Customers", customerSelected);

            Report reportCtrl = new Report(ReportType.CapacityReport, p);
            reportCtrl.Width = 800;
            reportCtrl.Height = 600;
            reportCtrl.Show();

        }

        private void txtCustomer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Refresh();
            }
        }

        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            AddCapacities generateCapacitiesCtrl = new AddCapacities();
            generateCapacitiesCtrl.AfterGenerateOrDeleteCapacities += new EventHandler(generateCapacitiesCtrl_AfterGenerateOrDeleteCapacities);

            createProductDialog = new PopupWindow("Generating capacities");
            createProductDialog.Width = 950;
            createProductDialog.Height = 600;
            createProductDialog.AddControl(generateCapacitiesCtrl);
            createProductDialog.Show();
        }

        void generateCapacitiesCtrl_AfterGenerateOrDeleteCapacities(object sender, EventArgs e)
        {
            // createProductDialog.Close();
            //Refresh();
        }

    }

}
