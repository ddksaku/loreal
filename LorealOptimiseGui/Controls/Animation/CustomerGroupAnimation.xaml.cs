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
using System.ComponentModel;
using LorealOptimiseGui.Lists;
using LorealOptimiseBusiness;
using LorealOptimiseData;
using LorealOptimiseData.Enums;
using LorealOptimiseBusiness.Lists;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Editors.Settings;
using LorealOptimiseShared;

namespace LorealOptimiseGui.Controls
{
    /// <summary>
    /// Interaction logic for CustomerGroupAnimation.xaml
    /// </summary>
    public partial class CustomerGroupAnimation : BaseUserControl
    {
        AnimationManager animationManager = AnimationManager.GetInstance();

        public CustomerGroupAnimation()
        {
            InitializeComponent();

            if (DesignerProperties.GetIsInDesignMode(this) == false)
            {
                animationManager.PropertyChanged += new PropertyChangedEventHandler(animationManager_PropertyChanged);
                animationManager.RemoveFinished += new Action<string>(animationManager_RemoveFinished);

                Loaded += new RoutedEventHandler(CustomerGroupAnimation_Loaded);

                TableViewEventHandlers<AnimationCustomerGroup> eventHandler =
                    new TableViewEventHandlers<AnimationCustomerGroup>(grdAnimationCustomerGroups,
                                                                       new AnimationCustomerGroupManager());
                eventHandler.AssignEvents();

                MakeColumnsReadOnly();

                grdAnimationCustomerGroups.AutoExpandAllGroups = true;
            }
        }

        void animationManager_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "AnimationEntity")
            {
                if (animationManager.Animation != null)
                {
                    animationManager.Animation.PropertyChanged -= new PropertyChangedEventHandler(Animation_PropertyChanged);
                    animationManager.Animation.PropertyChanged += new PropertyChangedEventHandler(Animation_PropertyChanged);

                    bFirstTime = true;
                }
            }
        }
        void Animation_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Status")
            {
                if (animationManager.Animation.Status != null)
                {
                    ChangeDataEntryLocking(animationManager.Animation.Status.Value);
                }
            }
        }

        void ChangeDataEntryLocking(byte status)
        {
            switch ((AnimationStatus)status)
            {
                case AnimationStatus.Open:
                case AnimationStatus.Locked:
                case AnimationStatus.Draft:
                    this.grdAnimationCustomerGroups.View.AllowEditing = true;
                    this.btnInclude.IsEnabled = true;
                    this.btnIncludeAll.IsEnabled = true;
                    this.btnRemove.IsEnabled = true;
                    this.btnRemoveAll.IsEnabled = true;
                    break;
                case AnimationStatus.Published:
                case AnimationStatus.Closed:
                case AnimationStatus.Cleared:
                    this.grdAnimationCustomerGroups.View.AllowEditing = false;
                    this.btnInclude.IsEnabled = false;
                    this.btnIncludeAll.IsEnabled = false;
                    this.btnRemove.IsEnabled = false;
                    this.btnRemoveAll.IsEnabled = false;
                    break;
            }
        }

        bool bFirstTime = false;
        void CustomerGroupAnimation_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.IsVisible && bFirstTime)
            {
                DataContext = AnimationManager.GetInstance();

                cboRetailerType.ItemsSource = RetailerTypeManager.Instance.GetAll();

                grdAnimationCustomerGroups.BeginDataUpdate();
                grdAnimationCustomerGroups.GroupBy("IDRetailerType");
                grdAnimationCustomerGroups.SortInfo.Add(new GridSortInfo("CustomerGroupName"));
                grdAnimationCustomerGroups.SortInfo.Add(new GridSortInfo("CustomerGroupSalesAreaName"));
                grdAnimationCustomerGroups.EndDataUpdate();

                grdCustomerGroups.BeginDataUpdate();
                grdCustomerGroups.GroupBy("SalesArea.Name");
                grdCustomerGroups.SortInfo.Add(new GridSortInfo("Name"));
                grdCustomerGroups.EndDataUpdate();

                if (animationManager.Animation != null && animationManager.Animation.Status != null)
                    ChangeDataEntryLocking(animationManager.Animation.Status.Value);

                bFirstTime = false;

            }
        }

        void MakeColumnsReadOnly()
        {
            LoggedUser loggedUser = LoggedUser.GetInstance();

            // marketing
            if (loggedUser.IsInRole(RoleEnum.Marketing) || LoggedUser.GetInstance().IsInRole(RoleEnum.DivisionAdmin))
            {
                colRetailerType.ReadOnly = false;
                colCounterDate.ReadOnly = false;
                colIncludeInAllocation.ReadOnly = false;

                btnInclude.Visibility = System.Windows.Visibility.Visible;
                btnIncludeAll.Visibility = System.Windows.Visibility.Visible;
                btnRemove.Visibility = System.Windows.Visibility.Visible;
                btnRemoveAll.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                colRetailerType.ReadOnly = true;
                colCounterDate.ReadOnly = true;

                btnInclude.Visibility = System.Windows.Visibility.Hidden;
                btnIncludeAll.Visibility = System.Windows.Visibility.Hidden;
                btnRemove.Visibility = System.Windows.Visibility.Hidden;
                btnRemoveAll.Visibility = System.Windows.Visibility.Hidden;
            }

            // logistics
            if (loggedUser.IsInRole(RoleEnum.Logistics))
            {
                colPLVDeliveryDate.ReadOnly = false;
                colPLVComponentDate.ReadOnly = false;
                colStockDate.ReadOnly = false;
            }
            else
            {
                colPLVDeliveryDate.ReadOnly = true;
                colPLVComponentDate.ReadOnly = true;
                colStockDate.ReadOnly = true;
            }

            // client care
            if (loggedUser.IsInRole(RoleEnum.ClientCare) || LoggedUser.GetInstance().IsInRole(RoleEnum.DivisionAdmin))
            {
                colSAPDespCode.ReadOnly = false;
            }
            else
            {
                colSAPDespCode.ReadOnly = true;
            }
        }

        private void btnInclude_Click(object sender, RoutedEventArgs e)
        {
            animationManager.SelectedCustomerGroups =
                (grdCustomerGroups.View as TableView).SelectedRows.Cast<CustomerGroup>();
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            animationManager.SelectedAnimationCustomerGroups =
                (grdCustomerGroups.View as TableView).SelectedRows.Cast<AnimationCustomerGroup>();
        }

        private void grdAnimationCustomerGroups_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            //Guid IDCustomerGroup = (Guid)e.GetListSourceFieldValue("IDCustomerGroup");
            //CustomerGroup cg = AnimationManager.GetInstance().GetCustomerGroupByID(IDCustomerGroup);
            CustomerGroup cg = e.GetListSourceFieldValue("CustomerGroup") as CustomerGroup;

            if (cg != null)
            {
                string fieldName = e.Column.FieldName;
                if (fieldName == "CustomerGroupName")
                {
                    if (e.IsGetData)
                    {
                        e.Value = cg.Name;
                    }
                }
                else if (fieldName == "CustomerGroupCode")
                {
                    if (e.IsGetData)
                    {
                        e.Value = cg.Code;
                    }
                }
                else if (fieldName == "CustomerGroupSalesAreaName")
                {
                    if (e.IsGetData)
                    {
                        e.Value = cg.SalesArea.Name;
                    }
                }
            }
        }

        private void TableView_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete && (sender as TableView).IsEditing == false)
            {
                e.Handled = true;
            }
        }

        private void Column_Validate(object sender, GridCellValidationEventArgs e)
        {
            if (e.Row != null)
            {
                string errorMessage;
                if ((e.Row as AnimationCustomerGroup).IsValidProperty(e.Column.FieldName, e.Value, out errorMessage) == false)
                    e.SetError(errorMessage);
            }
        }

        void animationManager_RemoveFinished(string obj)
        {
            MessageBox.Show(obj);
        }

        private void grdAnimationCustomerGroups_SelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {
            animationManager.IsSelectedAnimationCustomerGroup = (grdAnimationCustomerGroups.View as TableView).SelectedRows.Count > 0;
        }

        private void grdCustomerGroups_SelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {
            animationManager.IsSelectedCustomerGroup = (grdCustomerGroups.View as TableView).SelectedRows.Count > 0;
        }

    }
}
