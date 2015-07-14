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
using DevExpress.Xpf.Grid;
using LorealOptimiseBusiness;
using LorealOptimiseBusiness.Lists;
using LorealOptimiseShared;
using LorealOptimiseData.Enums;
using LorealOptimiseGui.Lists;
using LorealOptimiseData;

namespace LorealOptimiseGui.Controls
{
    /// <summary>
    /// Interaction logic for CustomerAllocation.xaml
    /// </summary>
    public partial class CustomerAllocation : BaseUserControl
    {
        AnimationManager animationManager = AnimationManager.GetInstance();
        public CustomerAllocation()
        {
            InitializeComponent();


            Loaded += new RoutedEventHandler(CustomerAllocation_Loaded);


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

        void animationManger_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "AnimationEntity")
            {
                if (animationManager.Animation != null)
                {
                    animationManager.Animation.PropertyChanged -= Animation_PropertyChanged;
                    animationManager.Animation.PropertyChanged += Animation_PropertyChanged;
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
                    this.grdAllocation.View.AllowEditing = true;
                    break;
                case AnimationStatus.Published:
                    if (LorealOptimiseData.LoggedUser.GetInstance().IsInRole(RoleEnum.DivisionAdmin) || LorealOptimiseData.LoggedUser.GetInstance().IsInRole(RoleEnum.NAMs))
                        this.grdAllocation.View.AllowEditing = true;
                    else
                        this.grdAllocation.View.AllowEditing = false;
                    break;
                case AnimationStatus.Closed:
                case AnimationStatus.Cleared:
                    this.grdAllocation.View.AllowEditing = false;
                    break;
            }
        }

        void CustomerAllocation_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsVisible)
            {
                DataContext = animationManager;

                if (DesignerProperties.GetIsInDesignMode(this) == false)
                {
                    TableViewEventHandlers<LorealOptimiseData.CustomerAllocation> eventHandler = new TableViewEventHandlers<LorealOptimiseData.CustomerAllocation>(grdAllocation, CustomerAllocationManager.Instance);
                    eventHandler.AssignEvents();
                    (grdAllocation.View as TableView).AutoWidth = true;

                    MakeColumnsReadOnly();
                }

                animationManager.PropertyChanged += animationManger_PropertyChanged;

                // sorting            
                grdAllocation.BeginDataUpdate();
                grdAllocation.SortInfo.Clear();
                grdAllocation.SortInfo.Add(new GridSortInfo("Customer.Name"));
                grdAllocation.EndDataUpdate();

                if (animationManager.Animation != null && animationManager.Animation.Status != null)
                {
                    ChangeDataEntryLocking(animationManager.Animation.Status.Value);
                }
            }

        }

        private void TableView_ShowingEditor(object sender, ShowingEditorEventArgs e)
        {
            if (e.Column.FieldName == "IDCustomer" && e.RowHandle != GridControl.NewItemRowHandle)
            {
                e.Cancel = true;
            }
        }

        private void TableView_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            if(e.Column.FieldName == "IDCustomer")
            {
                LorealOptimiseData.CustomerAllocation ca = e.Row as LorealOptimiseData.CustomerAllocation;
                if (ca.IDCustomer != Guid.Empty && ca.Customer == null)
                {
                    ca.Customer = DbDataContext.GetInstance().Customers.Single(c=>c.ID == ca.IDCustomer);
                }
            }
        }
    }
}
