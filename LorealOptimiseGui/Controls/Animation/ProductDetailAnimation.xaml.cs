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
using LorealOptimiseBusiness.Lists;
using LorealOptimiseData;
using LorealOptimiseData.Enums;
using LorealOptimiseShared;
using DevExpress.Xpf.Grid;
using DevExpress.Data.Filtering;

namespace LorealOptimiseGui.Controls
{
    /// <summary>
    /// Interaction logic for ProductDetailAnimation.xaml
    /// </summary>
    public partial class ProductDetailAnimation : BaseUserControl
    {
        public event EventHandler OpenAllocations;
        private AnimationManager animationManager = AnimationManager.GetInstance();

        public ProductDetailAnimation()
        {
            InitializeComponent();
            if (DesignerProperties.GetIsInDesignMode(this) == false)
            {
                Loaded += new RoutedEventHandler(ProductDetailAnimation_Loaded);
                grdProductDetails.View.ShowFilterPopup += new FilterPopupEventHandler(View_ShowFilterPopup);
                (grdProductDetails.View as TableView).ShowingEditor += new ShowingEditorEventHandler(View_ShowingEditor);
                grdProductDetails.GroupRowExpanding += new RowAllowEventHandler(grdProductDetails_GroupRowExpanding);
                grdProductDetails.GroupRowExpanded += new RowEventHandler(grdProductDetails_GroupRowExpanded);

                TableViewEventHandlers<AnimationProductDetail> eventHandler = new TableViewEventHandlers<AnimationProductDetail>(grdProductDetails, AnimationProductDetailManager.Instance, true);
                eventHandler.AssignEvents();
                (grdProductDetails.View as TableView).AutoWidth = true;

                MakeColumnsReadOnly();

                animationManager.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(animationManager_PropertyChanged);

                //(colAllocate.FindName("btnOpenAllocation") as Button).DataContext = AnimationManager.GetInstance();
            }
        }

        void grdProductDetails_GroupRowExpanding(object sender, RowAllowEventArgs e)
        {
            LongTaskExecutor.RaiseLongTaskEvent(sender, new LongTaskEventArgs("Expanding product details", TaskStatus.Started));
        }

        void grdProductDetails_GroupRowExpanded(object sender, RowEventArgs e)
        {
            LongTaskExecutor.RaiseLongTaskEvent(sender, new LongTaskEventArgs(string.Empty, TaskStatus.Finished));
        }

        void animationManager_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "AnimationEntity")
            {
                if (animationManager.Animation != null)
                {
                    animationManager.Animation.PropertyChanged -= Animation_PropertyChanged;
                    animationManager.Animation.PropertyChanged += Animation_PropertyChanged;
                }

                bFirstTime = true;
            }

        }

        void Animation_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == "Status")
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
                    this.grdProductDetails.View.AllowEditing = true;
                    break;
                case AnimationStatus.Locked:
                case AnimationStatus.Draft:
                case AnimationStatus.Published:
                case AnimationStatus.Closed:
                case AnimationStatus.Cleared:
                    this.grdProductDetails.View.AllowEditing = false;
                    break;
            }
        }

        void View_ShowingEditor(object sender, ShowingEditorEventArgs e)
        {
            if (e.Row != null && e.Column.FieldName == "RRP")
            {
                AnimationProductDetail apd = e.Row as AnimationProductDetail;

                if (apd.AnimationProduct == null)
                {
                    return;
                }

                if (apd.AnimationProduct.ItemType == null)
                {
                    e.Cancel = true;
                }
                else if (apd.AnimationProduct.ItemType != null)
                {
                    e.Cancel = apd.AnimationProduct.ItemType.RRPAvailable == false;
                }
            }
        }

        void View_ShowFilterPopup(object sender, FilterPopupEventArgs e)
        {
            if (e.Column.FieldName == "AllocationRemainder")
            {
                CustomComboBoxItem item = new CustomComboBoxItem();
                item.DisplayValue = "Products with Allocation Remainder > 0";
                item.EditValue = CriteriaOperator.Parse("[AllocationRemainder] > 0");
                List<object> itemssSource = e.ComboBoxEdit.ItemsSource.Cast<object>().ToList();
                itemssSource.Add(item);

                e.ComboBoxEdit.ItemsSource = itemssSource;
            }
        }

        private bool bFirstTime = false;
        void ProductDetailAnimation_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsVisible && bFirstTime)
            {
                double bestFitWidth = (grdProductDetails.View as TableView).CalcColumnBestFitWidth(clmProductDescription);
                if (bestFitWidth > 0)
                {
                    clmProductDescription.Width = (grdProductDetails.View as TableView).CalcColumnBestFitWidth(clmProductDescription);
                }

                DataContext = AnimationManager.GetInstance();

                grdProductDetails.Visibility = System.Windows.Visibility.Hidden;
                LongTaskExecutor.RaiseLongTaskEvent(this, new LongTaskEventArgs("Loading product details", TaskStatus.Started));

                cboSalesArea.ItemsSource = SalesAreaManager.Instance.GetAll();
                // cboProducts.ItemsSource = AnimationManager.GetInstance().Products;


                // grouping and collapsing
                grdProductDetails.BeginDataUpdate();
                try
                {
                    // sorting by ItemGroup, then by SortOrder
                    grdProductDetails.ClearSorting();

                    GridSortInfo sort1 = new GridSortInfo(clmItemGroup.FieldName);
                    GridSortInfo sort2 = new GridSortInfo(clmSortOrder.FieldName);
                    grdProductDetails.SortInfo.Add(sort1);
                    grdProductDetails.SortInfo.Add(sort2);
                    grdProductDetails.ClearGrouping();
                    grdProductDetails.GroupBy(clmSalesArea);
                }
                finally
                {
                    grdProductDetails.EndDataUpdate();
                    grdProductDetails.CollapseAllGroups();
                    LongTaskExecutor.RaiseLongTaskEvent(this, new LongTaskEventArgs("Loading product details", TaskStatus.Finished));
                    grdProductDetails.Visibility = System.Windows.Visibility.Visible;
                }

                if (animationManager.Animation != null && animationManager.Animation.Status != null)
                    ChangeDataEntryLocking(animationManager.Animation.Status.Value);

                bFirstTime = false;
            }
        }

        void MakeColumnsReadOnly()
        {
            LoggedUser loggedUser = LoggedUser.GetInstance();

            // marketing
            if (loggedUser.IsInRole(RoleEnum.Marketing) || loggedUser.IsInRole(RoleEnum.DivisionAdmin))
            {
                clmRRP.ReadOnly = false;
                clmBDCQuantity.ReadOnly = false;
                clmAllocationQuantity.ReadOnly = false;
                clmForecastProcQuantity.ReadOnly = false;
                clmMarketingComments.ReadOnly = false;
            }
            else
            {
                clmRRP.ReadOnly = true;
                clmBDCQuantity.ReadOnly = true;
                clmAllocationQuantity.ReadOnly = true;
                clmForecastProcQuantity.ReadOnly = true;
                clmMarketingComments.ReadOnly = true;
            }

            // fix button show/hide
            if (loggedUser.IsInRole(RoleEnum.Marketing) || loggedUser.IsInRole(RoleEnum.DivisionAdmin) || loggedUser.IsInRole(RoleEnum.NAMs) || loggedUser.IsInRole(RoleEnum.ReadOnly))
            {
                colAllocate.Visible = true;
            }
            else
            {
                colAllocate.Visible = false;
            }

        }

        private void btnOpenAllocation_Click(object sender, RoutedEventArgs e)
        {
            AnimationManager.GetInstance().OpenAllocation();
            
            if (OpenAllocations != null)
            {
                OpenAllocations(this, new EventArgs());
            }
        }

        private void ForecastProcQuantity_Validate(object sender, GridCellValidationEventArgs e)
        {
            if (e.Row != null && e.Value != null)
            {
                int value;
                if (int.TryParse(e.Value.ToString(), out value) == false)
                    value = 0;

                AnimationProductDetail apd = e.Row as AnimationProductDetail;
                if (apd.AllocationQuantity != null && apd.AllocationQuantity.Value > value)
                {
                    e.SetError("Allocation Quantity cannot exceed Forecast Procurement Quantity.");
                }
            }
        }

        private void AllocationQuantity_Validate(object sender, GridCellValidationEventArgs e)
        {
            if (e.Row != null && e.Value != null)
            {
                int value;
                if (int.TryParse(e.Value.ToString(), out value) == false)
                    value = 0;

                if (value > 0)
                {
                    AnimationProductDetail apd = e.Row as AnimationProductDetail;
                    if (apd.ForecastProcQuantity == null || apd.ForecastProcQuantity.Value < value)
                    {
                        e.SetError("Allocation Quantity cannot exceed Forecast Procurement Quantity.");
                    }
                }
            }
        }
    }
}
