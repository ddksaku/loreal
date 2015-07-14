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
using System.Data.SqlClient;
using DevExpress.Xpf.Grid;
using LorealOptimiseBusiness;
using LorealOptimiseBusiness.Lists;
using LorealOptimiseShared;
using LorealOptimiseData;
using LorealOptimiseGui.Lists;
using LorealOptimiseData.Enums;
using System.ComponentModel;

namespace LorealOptimiseGui.Controls
{
    /// <summary>
    /// Interaction logic for CustomerGroupAllocation.xaml
    /// </summary>
    public partial class CustomerGroupAllocation : BaseUserControl
    {
        AnimationManager animationManager = AnimationManager.GetInstance();
        public CustomerGroupAllocation()
        {
            InitializeComponent();

            if (DesignerProperties.GetIsInDesignMode(this) == false)
            {
                Loaded += new RoutedEventHandler(CustomerGroupAllocation_Loaded);
            }
        }

        void MakeColumnsReadOnly()
        {
            if (LoggedUser.GetInstance().IsInRole(RoleEnum.Marketing) || LoggedUser.GetInstance().IsInRole(RoleEnum.DivisionAdmin) || LoggedUser.GetInstance().IsInRole(RoleEnum.NAMs))
            {
                clmFixedAllocation.ReadOnly = false;
                clmRetailUplift.ReadOnly = false;
            }
            else
            {
                clmFixedAllocation.ReadOnly = true;
                clmRetailUplift.ReadOnly = true;
            }
        }

        void CustomerGroupAllocation_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "AnimationEntity")
            {
                if (animationManager.Animation != null)
                {
                    animationManager.Animation.PropertyChanged -= new System.ComponentModel.PropertyChangedEventHandler(Animation_PropertyChanged);
                    animationManager.Animation.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(Animation_PropertyChanged);
                }
            }
            else if (e.PropertyName == "Allocations")
            {
                if (animationManager.Allocations != null)
                {
                    ;
                }
            }
        }

        void Animation_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Status")
            {
                if (animationManager.Animation != null && animationManager.Animation.Status != null)
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
                    this.grdCustomerGroupAllocations.View.AllowEditing = true;
                    break;
                case AnimationStatus.Published:
                case AnimationStatus.Closed:
                case AnimationStatus.Cleared:
                    this.grdCustomerGroupAllocations.View.AllowEditing = false;
                    break;
            }
        }

        void CustomerGroupAllocation_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsVisible)
            {
                DataContext = animationManager;

                TableViewEventHandlers<LorealOptimiseData.CustomerGroupAllocation> eventHandler = new TableViewEventHandlers<LorealOptimiseData.CustomerGroupAllocation>(grdCustomerGroupAllocations, CustomerGroupAllocationManager.Instance);
                eventHandler.AssignEvents();
                (grdCustomerGroupAllocations.View as TableView).AutoWidth = true;

                animationManager.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(CustomerGroupAllocation_PropertyChanged);

                MakeColumnsReadOnly();

                // setting cell style
                foreach (GridColumn column in this.grdCustomerGroupAllocations.Columns)
                {
                    if (column.ReadOnly)
                    {
                        column.CellStyle = FindResource("CellNormalStyle") as Style;
                    }
                }

                // sorting            
                grdCustomerGroupAllocations.BeginDataUpdate();
                grdCustomerGroupAllocations.SortInfo.Clear();
                grdCustomerGroupAllocations.SortInfo.Add(new GridSortInfo("CustomerGroup.Name"));
                grdCustomerGroupAllocations.EndDataUpdate();

                (grdCustomerGroupAllocations.View as TableView).FocusedRowChanged += new FocusedRowChangedEventHandler(CustomerGroupAllocation_FocusedRowChanged);

                if (animationManager.Animation != null && animationManager.Animation.Status != null)
                {
                    ChangeDataEntryLocking(animationManager.Animation.Status.Value);
                }
            }
  
        }

        void CustomerGroupAllocation_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            if ((grdCustomerGroupAllocations.View.FocusedRowHandle < 0)
                && animationManager.Allocations != null)
            {
                animationManager.Allocations.SelectedAllocationCustomerGroup = null;
            }
        }

        private void TableView_ShowingEditor(object sender, ShowingEditorEventArgs e)
        {
            if(e.Column.FieldName == "IDCustomerGroup" && e.RowHandle != GridControl.NewItemRowHandle)
            {
                e.Cancel = true;
            }
        }

        private void TableView_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            if(e.Column.FieldName == "IDCustomerGroup")
            {
                LorealOptimiseData.CustomerGroupAllocation cga = e.Row as LorealOptimiseData.CustomerGroupAllocation;
                if (cga.IDCustomerGroup != Guid.Empty && cga.CustomerGroup == null)
                {
                    cga.CustomerGroup = DbDataContext.GetInstance().CustomerGroups.Single(cg=>cg.ID == cga.IDCustomerGroup);
                }
            }
        }
    }

}
