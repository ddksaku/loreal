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
using LorealOptimiseGui.Controls;
using DevExpress.Xpf.Grid;
using System.Collections;
using DevExpress.Xpf.Data;

namespace LorealOptimiseGui.Lists
{
    /// <summary>
    /// Interaction logic for CustomerGroups.xaml
    /// </summary>
    public partial class CustomerGroups : BaseListUserControl<CustomerGroupManager, CustomerGroup>
    {
        private CustomerGroup NewRowItem = null;

        public CustomerGroups()
            : base()
        {
            InitializeComponent();

            if (DesignerProperties.GetIsInDesignMode(this) == false)
            {
                Loaded += new RoutedEventHandler(CustomerGroups_Loaded);
                AssignEvents(grdCustomerGroups, true);

                if (grdCustomerGroups.View.AllowEditing == false)
                {
                    colOperation.Visible = false;
                }
            }
        }

       protected override Hashtable Filters
        {
            get 
            {
                return new Hashtable();
            }
        }

        void CustomerGroups_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsVisible)
            {
                cboSalesAreas.ItemsSource = SalesAreaManager.Instance.GetAll();
            }
        }

        private void TableView_ShowingEditor(object sender, ShowingEditorEventArgs e)
        {
            if (e.Column.FieldName == "IDSalesArea" || e.Column.FieldName == "Code" || e.Column.FieldName == "Name" )
            {
                if (e.RowHandle != GridControl.NewItemRowHandle)
                    e.Cancel = true;
            }
        }

        private void TableView_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            try
            {
                int order = 0;
                foreach (CustomerGroup cg in Data)
                {
                    if(cg.SortOrder.HasValue)
                        order = Math.Max(order, cg.SortOrder.Value);
                }

                CustomerGroup newCG = this.Data.Single(cg => cg.ID == Guid.Empty);
                newCG.SortOrder = order + 1;
                newCG.Manual = true;
                newCG.IncludeInSystem = true;
            }
            catch (Exception exc)
            {
                //MessageBox.Show("An error occured when setting 'SortOrder' for a new row:" + LorealOptimiseShared.Utility.GetExceptionsMessages(exc));
                MessageBox.Show(SystemMessagesManager.Instance.GetMessage("CustomerGroupExceptionNewRow", LorealOptimiseShared.Utility.GetExceptionsMessages(exc)));
            }
        }

        private void TableView_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName == "IDSalesArea")
            {
                SalesArea sa = DbDataContext.GetInstance().SalesAreas.Single(s => s.ID == (Guid)e.Value);
                if (sa != null)
                {
                    (e.Row as CustomerGroup).SalesArea = sa;
                }
            }

            if (e.RowHandle == GridControl.NewItemRowHandle)
                NewRowItem = e.Row as CustomerGroup;
        }

        private void TableView_RowCanceled(object sender, RowEventArgs e)
        {
            if (NewRowItem != null)
            {
                if (NewRowItem.SalesArea != null)
                {
                    NewRowItem.SalesArea.CustomerGroups.Remove(NewRowItem);
                }
                NewRowItem = null;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        private void TableView_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TableView tableView = sender as TableView;
            if (e.Key == System.Windows.Input.Key.Delete && tableView.IsEditing == false)
            {
                e.Handled = true;
            }
        }

        private void btnReplace_Click(object sender, RoutedEventArgs e)
        {
            if (grdCustomerGroups.View.FocusedRow != null)
            {
                int rowHandle = grdCustomerGroups.View.FocusedRowHandle;

                MergeCustomerGroup mergeControl = new MergeCustomerGroup(grdCustomerGroups.View.FocusedRow as CustomerGroup);
                PopupWindow mergetDialog = new PopupWindow("Merge Customer Group");
                mergeControl.Close += new EventHandler(mergetDialog.CloseWindowEvent);
                mergetDialog.AddControl(mergeControl);
                mergetDialog.ShowDialog();

                if (mergeControl.RemovedSAPCustomerGroup != null)
                {
                    int pos = this.Data.IndexOf(mergeControl.RemovedSAPCustomerGroup);
                    if (pos >= 0)
                    {
                        (grdCustomerGroups.View as TableView).DeleteRow(grdCustomerGroups.GetRowHandleByListIndex(pos));
                    }
                }
            }
        }

        private void btnReplace_Initialized(object sender, EventArgs e)
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
                else
                    (sender as FrameworkElement).Visibility = Visibility.Collapsed;
            }
        }

    }
}
