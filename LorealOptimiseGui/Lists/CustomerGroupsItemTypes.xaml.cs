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
using System.Collections;
using System.Threading;
using LorealOptimiseShared;

namespace LorealOptimiseGui.Lists
{
    /// <summary>
    /// Interaction logic for CustomerGroupsItemTypes.xaml
    /// </summary>
    public partial class CustomerGroupsItemTypes : BaseListUserControl<CustomerGroupsItemTypeManager, CustomerGroupItemType>
    {
        public CustomerGroupsItemTypes()
            : base()
        {
            InitializeComponent();

            if (DesignerProperties.GetIsInDesignMode(this) == false)
            {
                Loaded += new RoutedEventHandler(CustomerGroupsItemTypes_Loaded);

                (grdCustomerGroupsItemTypes.View as TableView).CellValueChanging += new CellValueChangedEventHandler(View_CellValueChanging);
                (grdCustomerGroupsItemTypes.View as TableView).CellValueChanged += new CellValueChangedEventHandler(View_CellValueChangedBeforeSave);

                AssignEvents(grdCustomerGroupsItemTypes);

                (grdCustomerGroupsItemTypes.View as TableView).CellValueChanged += new CellValueChangedEventHandler(View_CellValueChangedAfterSave);
            }
        }

        void View_CellValueChangedAfterSave(object sender, CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName == "IncludeInSAPOrders")
            {
                Thread.Sleep(1000);
                string taskName = (e.Row as CustomerGroupItemType).IncludeInSAPOrders == true ? "Including" : "Excluding";
                LongTaskExecutor.RaiseLongTaskEvent(this, new LongTaskEventArgs(taskName, TaskStatus.Finished));
            }
        }

        void View_CellValueChangedBeforeSave(object sender, CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName == "IncludeInSAPOrders")
            {
                // cellvaluechanging is still work with UI, so we should call DoEvents after cellvaluechaing, before saving entity
                LongTaskExecutor.DoEvents();
            }
        }

        void View_CellValueChanging(object sender, CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName == "IncludeInSAPOrders")
            {
                string taskName = (e.Row as CustomerGroupItemType).IncludeInSAPOrders == false ? "Including" : "Excluding";
                LongTaskExecutor.RaiseLongTaskEvent(this, new LongTaskEventArgs(taskName, TaskStatus.Started));
            }
        }

        protected override Hashtable Filters
        {
            get 
            {
                return new Hashtable();
            }
        }

        void CustomerGroupsItemTypes_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsVisible)
            {
                cboCustomerGroups.ItemsSource = CustomerGroupManager.Instance.GetAll();
                cboItemTypes.ItemsSource = ItemTypeManager.Instance.GetAll();
                cboCustomers.ItemsSource = CustomerManager.Instance.GetAll().OrderBy(c=>c.FullName);

                grdCustomerGroupsItemTypes.BeginDataUpdate();
                grdCustomerGroupsItemTypes.SortBy("IDCustomer");
                grdCustomerGroupsItemTypes.EndDataUpdate();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
        }
    }
}
