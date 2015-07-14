using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Editors.Settings;
using LorealOptimiseData.Enums;
using LorealOptimiseGui.Lists;
using LorealOptimiseBusiness;
using LorealOptimiseData;
using LorealOptimiseBusiness.Lists;
using LorealOptimiseShared;
using LorealOptimiseBusiness.DAO;
using LorealOptimiseShared.Logging;
using DevExpress.Data;
using DevExpress.Xpf.Grid;
using DevExpress.Data.Filtering;

namespace LorealOptimiseGui.Controls
{
    /// <summary>
    /// Interaction logic for ProductAnimation.xaml
    /// </summary>
    public partial class ProductAnimation : BaseUserControl
    {
        private ListManager manager = new ListManager();
        private AnimationManager animationManager = AnimationManager.GetInstance();
        private AnimationProduct NewItemRow = null;
        private bool isMarketingUser = LoggedUser.GetInstance().IsInRole(RoleEnum.Marketing);
        private bool isDivisionAdmin = LoggedUser.GetInstance().IsInRole(RoleEnum.DivisionAdmin);
        private bool isFirstLoading;
        private RecalculationType calcType = RecalculationType.None;
        private List<string> madMonths = new List<string>();

        public ProductAnimation()
        {
            InitializeComponent();
            if (DesignerProperties.GetIsInDesignMode(this) == false)
            {
                animationManager.PropertyChanged += (AnimationManager_PropertyChanged);

                cboView.SelectedIndexChanged += new RoutedEventHandler(cboView_SelectedIndexChanged);
                grdProduct.LayoutUpdated += new EventHandler(grdProduct_LayoutUpdated);
                grdProduct.View.FocusedRowChanged += new FocusedRowChangedEventHandler(View_FocusedRowChanged);
                grdProduct.View.PreviewKeyDown += new KeyEventHandler(TableView_PreviewKeyDown);
                grdProduct.CustomUnboundColumnData += new GridColumnDataEventHandler(grdProduct_CustomUnboundColumnData);

                (grdProduct.View as TableView).HiddenEditor += new EditorEventHandler(View_HiddenEditor);

                grdProduct.EndSorting += new RoutedEventHandler(grdProduct_EndSorting);
                
                Loaded += new RoutedEventHandler(ProductAnimation_Loaded);
                LayoutUpdated += new EventHandler(ProductAnimation_LayoutUpdated);

                MakeColumnsReadOnly();

                SetColumnsProperties();

                SetUserRights();
            }
        }

        void grdProduct_EndSorting(object sender, RoutedEventArgs e)
        {
            SetItemsSourceForFiltering("IDMultipleNormal");
            SetItemsSourceForFiltering("IDMultipleWarehouse");
            SetItemsSourceForFiltering("IDBrandAxe");
        }

        void View_HiddenEditor(object sender, EditorEventArgs e)
        {
            SetItemsSourceForFiltering(e.Column.FieldName);
        }

        void SetItemsSourceForFiltering(string fieldName)
        {
            switch (fieldName)
            {
                case "IDMultipleNormal":
                    {
                        List<Guid?> multiples = animationManager.Products.Select(ap => ap.IDMultipleNormal).Where(m => m.HasValue).Distinct().ToList();
                        var source = MultipleManager.Instance.GetAll().Where(m => multiples.Any(it => it.Value == m.ID)).OrderBy(m => m.Value).ToList();
                        if (source.Count > 0)
                        {
                            cboNormalMultiple.ItemsSource = source;
                        }
                        else
                        {
                            cboNormalMultiple.ItemsSource = null;
                        }
                    }
                    break;
                case "IDMultipleWarehouse":
                    {
                        List<Guid?> multiples = animationManager.Products.Select(ap => ap.IDMultipleWarehouse).Where(m => m.HasValue).Distinct().ToList();
                        var source = MultipleManager.Instance.GetAll().Where(m => multiples.Any(it => it.Value == m.ID)).ToList();
                        if (source.Count > 0)
                        {
                            cboWarehouseMultiple.ItemsSource = source;
                        }
                        else
                        {
                            cboWarehouseMultiple = null;
                        }
                    }
                    break;
                case "IDBrandAxe":
                    {
                        List<Guid?> brandaxes = animationManager.Products.Select(ap => ap.IDBrandAxe).Where(b => b.HasValue).Distinct().ToList();
                        var source = BrandAxeManager.Instance.GetAll().Where(b => brandaxes.Any(it => it.Value == b.ID)).ToList();
                        if (source.Count > 0)
                        {
                            cboBrandAxes.ItemsSource = source;
                        }
                        else
                        {
                            cboBrandAxes.ItemsSource = null;
                        }
                    }
                    break;
            }
        }

        void grdProduct_CustomUnboundColumnData(object sender, GridColumnDataEventArgs e)
        {
            if (e.Column != null && !string.IsNullOrEmpty(e.Column.Name))
            {
                if (e.Column.Name.StartsWith("RetailerType"))
                {
                    if (e.IsGetData)
                    {
                        AnimationProduct ap = grdProduct.GetRowByListIndex(e.ListSourceRowIndex) as AnimationProduct;

                        if (ap != null)
                        {
                            if (ap.RetailerTypeAllocation.ContainsKey(e.Column.Header.ToString()))
                            {
                                e.Value = ap.RetailerTypeAllocation[e.Column.Header.ToString()].ToString();
                            }
                            else
                            {
                                e.Value = "0";
                            }
                        }
                    }
                }
            }

        }

        void ProductAnimation_LayoutUpdated(object sender, EventArgs e)
        {
            
        }

        void ProductAnimation_Loaded(object sender, RoutedEventArgs e)
        {
            if (isFirstLoading == true)
            {
                DataContext = AnimationManager.GetInstance();

                isFirstLoading = false;

                // Mad months stuff
                cboMadMonth.Items.BeginUpdate();
                cboMadMonth.Items.Clear();

                madMonths.Clear();
                for (int i = -12; i < 12; i++)
                {
                    DateTime date = DateTime.Now.AddMonths(i);

                    string name = date.ToString("MM.yyyy");
                    DateTime value = new DateTime(date.Year, date.Month, 1);

                    madMonths.Add(name);
                }
                cboMadMonth.ItemsSource = madMonths;
                cboMadMonth.Items.EndUpdate();

                // best fit width 
                double bestFitWidth = (grdProduct.View as TableView).CalcColumnBestFitWidth(clmProductDescription);
                if(bestFitWidth > 0)
                {
                    clmProductDescription.Width = bestFitWidth;
                }

                // security according to animatino's status
                if (animationManager.Animation != null && animationManager.Animation.Status != null)
                    ChangeDataEntryLocking(animationManager.Animation.Status.Value);

                // Normal multiples, warehouse multiples, brandaxes itemssource for filtering
                clmNormalMultiple.ColumnFilterMode = ColumnFilterMode.Value;
                clmWarehouseMultiple.ColumnFilterMode = ColumnFilterMode.Value;
                clmBrandAxe.ColumnFilterMode = ColumnFilterMode.Value;

                grdProduct.FilterString = null;
                grdProduct.FilterCriteria = null;

                SetItemsSourceForFiltering("IDMultipleNormal");
                SetItemsSourceForFiltering("IDMultipleWarehouse");
                SetItemsSourceForFiltering("IDBrandAxe");

                clmNormalMultiple.ColumnFilterMode = ColumnFilterMode.DisplayText;
                clmWarehouseMultiple.ColumnFilterMode = ColumnFilterMode.DisplayText;
                clmBrandAxe.ColumnFilterMode = ColumnFilterMode.DisplayText;

            }

            if (IsVisible)
            {
                animationManager.RecalculateRetailerTypeAllocations();
            }
        }

        void MakeColumnsReadOnly()
        {
            LoggedUser loggedUser = LoggedUser.GetInstance();

            // marketing
            if (loggedUser.IsInRole(RoleEnum.Marketing) || LoggedUser.GetInstance().IsInRole(RoleEnum.DivisionAdmin))
            {
                clmItemGroup.ReadOnly = false;
                clmMaterialCode.ReadOnly = false;
                clmProductDescription.ReadOnly = false;
                clmSortOrder.ReadOnly = false;
                clmBDC.ReadOnly = false;
                clmItemType.ReadOnly = false;
                clmSignature.ReadOnly = false;
                clmBrandAxe.ReadOnly = false;
                clmCategory.ReadOnly = false;
                clmOnCas.ReadOnly = false;
                clmNormalMultiple.ReadOnly = false;
                clmWarehouseMultiple.ReadOnly = false;
                clmMarketingComments.ReadOnly = false;

                btnCreateProduct.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                clmItemGroup.ReadOnly = true;
                clmMaterialCode.ReadOnly = true;
                clmProductDescription.ReadOnly = true;
                clmSortOrder.ReadOnly = true;
                clmBDC.ReadOnly = true;
                clmItemType.ReadOnly = true;
                clmSignature.ReadOnly = true;
                clmBrandAxe.ReadOnly = true;
                clmCategory.ReadOnly = true;
                clmOnCas.ReadOnly = true;
                clmNormalMultiple.ReadOnly = true;
                clmWarehouseMultiple.ReadOnly = true;
                clmMarketingComments.ReadOnly = true;

                btnCreateProduct.Visibility = System.Windows.Visibility.Collapsed;
            }

            // logistics
            if (loggedUser.IsInRole(RoleEnum.Logistics))
            {
                clmMadMonth.ReadOnly = false;
                clmStockRisk.ReadOnly = false;
                clmDeliveryRisk.ReadOnly = false;
                clmLogisticsComments.ReadOnly = false;
            }
            else
            {
                clmMadMonth.ReadOnly = true;
                clmStockRisk.ReadOnly = true;
                clmDeliveryRisk.ReadOnly = true;
                clmLogisticsComments.ReadOnly = true;
            }
           
        }

        private void SetColumnsProperties()
        {
            grdProduct.Columns.BeginUpdate();

            foreach (GridColumn column in grdProduct.Columns)
            {
                if (column.Name.StartsWith("clmMonth"))
                {
                    int monthNumber = int.Parse(column.Name.Replace("clmMonth", String.Empty));

                    //Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");
                    column.Header = DateTime.Now.AddMonths(monthNumber - 1).ToString("MMM yyyy");
                }

                if (column.ReadOnly)
                {
                    column.CellStyle = FindResource("CellNormalStyle") as Style;
                }
            }

            grdProduct.Columns.EndUpdate();
        }

        private void SetUserRights()
        {
            LoggedUser user = LoggedUser.GetInstance();

            if (user.IsInRole(RoleEnum.Marketing) && user.Roles.Count() == 1)
            {
                cboView.SelectedIndex = 1;
            }

            if (user.IsInRole(RoleEnum.Logistics) && user.Roles.Count() == 1)
            {
                cboView.SelectedIndex = 2;
            }

            if (user.IsInRole(RoleEnum.Logistics))
            {
                clmLogisticsComments.ReadOnly = false;
                clmMadMonth.ReadOnly = false;
                clmStockRisk.ReadOnly = false;
                clmDeliveryRisk.ReadOnly = false;
            }
            else
            {
                clmLogisticsComments.ReadOnly = true;
                clmMadMonth.ReadOnly = true;
                clmStockRisk.ReadOnly = true;
                clmDeliveryRisk.ReadOnly = true;
            }

            if (user.IsInRole(RoleEnum.Marketing) || LoggedUser.GetInstance().IsInRole(RoleEnum.DivisionAdmin))
            {
                grdProduct.Columns["MarketingComments"].ReadOnly = false;
            }
            else
            {
                grdProduct.Columns["MarketingComments"].ReadOnly = true;
            }
        }

        void View_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            ;
        }

        void grdProduct_LayoutUpdated(object sender, EventArgs e)
        {
            // Cursor = Cursors.Arrow;
        }

        void cboView_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            // Cursor = Cursors.Wait;

            grdProduct.Columns.BeginUpdate();

            foreach (GridColumn c in grdProduct.Columns)
            {
                c.Visible = false;
            }

            clmSortOrder.Visible = true;
            clmProductDescription.Visible = true;
            clmMaterialCode.Visible = true;
            clmProductDescription.Visible = true;
            clmItemGroup.Visible = true;

            switch (cboView.SelectedIndex)
            {
                case 0:
                    foreach (GridColumn c in grdProduct.Columns.Reverse())
                    {
                        c.Visible = true;
                    }

                    AddRetailerTypeColumns();
                    break;
                case 1:
                    #region Marketing
                    clmMadMonth.Visible = true;
                    clmMarketingComments.Visible = true;
                    clmROIRrp.Visible = true;
                    clmUKRrp.Visible = true;
                    clmROIList.Visible = true;
                    clmUKList.Visible = true;
                    clmTotalCostValue.Visible = true;
                    clmAllocationQuantityROI.Visible = true;
                    clmAllocationQuantityUK.Visible = true;
                    clmTotalAllocation.Visible = true;
                    clmTotalForecast.Visible = true;
                    clmTotalBDCQuantity.Visible = true;
                    clmTotalCapacity.Visible = true;
                    clmWarehouseMultiple.Visible = true;
                    clmNormalMultiple.Visible = true;
                    clmOnCas.Visible = true;
                    clmCategory.Visible = true;
                    clmBrandAxe.Visible = true;
                    clmSignature.Visible = true;
                    clmItemType.Visible = true;
                    clmSource.Visible = true;
                    clmBDC.Visible = true;
                    break;
                    #endregion
                case 2:
                    #region Logistic
                    clmAllocationQuantity.Visible = true;
                    //clmStockDeadline.Visible = true;
                    //clmPlvComponentDeadline.Visible = true;
                    //clmPlvDeliveryDeadline.Visible = true;
                    //clmOnCounterDate.Visible = true;
                    clmLogisticsComments.Visible = true;
                    clmStockLessPipe.Visible = true;
                    clmPipe.Visible = true;
                    clmStock.Visible = true;
                    clmRecievedToDate.Visible = true;
                    foreach (GridColumn c in grdProduct.Columns.Reverse())
                    {
                        if (c.Name.StartsWith("clmMonth"))
                        {
                            c.Visible = true;
                        }
                    }
                    clmReliquat.Visible = true;
                    clmConfirmedQuantity.Visible = true;
                    clmInTransit.Visible = true;
                    clmTotalAnimationQuantity.Visible = true;
                    clmDuplicateProduct.Visible = true;
                    clmActiveAnimations.Visible = true;
                    clmDeliveryRisk.Visible = true;
                    clmStockRisk.Visible = true;
                    clmMadMonth.Visible = true;
                    clmProcurementType.Visible = true;
                    clmTotalForecast.Visible = true;
                    clmTotalBDCQuantity.Visible = true;
                    clmStatus.Visible = true;
                    clmBDC.Visible = true;

                    AddRetailerTypeColumns();
                    break;
                    #endregion
                case 3:
                    #region Summary
                    clmAllocationQuantity.Visible = true;
                    //clmStockDeadline.Visible = true;
                    //clmPlvComponentDeadline.Visible = true;
                    //clmPlvDeliveryDeadline.Visible = true;
                    //clmOnCounterDate.Visible = true;
                    clmLogisticsComments.Visible = true;
                    clmStockLessPipe.Visible = true;
                    clmPipe.Visible = true;
                    clmStock.Visible = true;
                    clmRecievedToDate.Visible = true;
                    clmConfirmedQuantity.Visible = true;
                    clmInTransit.Visible = true;
                    clmTotalAnimationQuantity.Visible = true;
                    clmDeliveryRisk.Visible = true;
                    clmStockRisk.Visible = true;
                    clmMadMonth.Visible = true;
                    clmMarketingComments.Visible = true;
                    clmROIRrp.Visible = true;
                    clmUKRrp.Visible = true;
                    clmAllocationQuantityROI.Visible = true;
                    clmAllocationQuantityUK.Visible = true;
                    clmTotalAllocation.Visible = true;
                    clmTotalForecast.Visible = true;
                    clmTotalBDCQuantity.Visible = true;

                    AddRetailerTypeColumns();
                    break;
                    #endregion
            }

            grdProduct.Columns.EndUpdate();
        }

        // AnimationManager PropertyChanged
        void AnimationManager_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "AnimationEntity")
            {
                if (animationManager.Animation != null && animationManager.Animation.ID == Guid.Empty)
                {
                    return;
                }

                isFirstLoading = true;
                grdProduct.DataContext = animationManager;

                LongTaskExecutor.RaiseLongTaskEvent(this, new LongTaskEventArgs("Opening an animation", TaskStatus.InProgress, "Loading Item Groups"));
                cboItemGroups.ItemsSource = ItemGroupManager.Instance.GetAll();

                LongTaskExecutor.RaiseLongTaskEvent(this, new LongTaskEventArgs("Opening an animation", TaskStatus.InProgress, "Loading Item Types"));
                cboItemTypes.ItemsSource = new ItemType[] { new ItemType() }.Union(ItemTypeManager.Instance.GetAll());

                //LongTaskExecutor.RaiseLongTaskEvent(this, new LongTaskEventArgs("Opening an animation", TaskStatus.InProgress, "Loading Sales"));
                //SaleManager.Instance.GetAll();

                LongTaskExecutor.RaiseLongTaskEvent(this, new LongTaskEventArgs("Opening an animation", TaskStatus.InProgress, "Loading Signatures"));
                cboSignature.ItemsSource = new Signature[] { new Signature() }.Union(SignatureManager.Instance.GetAllWithSales());

                LongTaskExecutor.RaiseLongTaskEvent(this, new LongTaskEventArgs("Opening an animation", TaskStatus.InProgress, "Loading Brand/Axes"));
                BrandAxeManager.Instance.GetAllForAllocation();

                LongTaskExecutor.RaiseLongTaskEvent(this, new LongTaskEventArgs("Opening an animation", TaskStatus.InProgress, "Loading Categories"));
                cboCategories.ItemsSource = new Category[] { new Category() }.Union(CategoryManager.Instance.GetAll());

                if (animationManager.Animation != null && animationManager.Animation.AnimationProducts != null &&
                    animationManager.Animation.AnimationProducts.Count > 0)
                {
                    animationManager.ProcurementPlan.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(ProcurementPlan_CollectionChanged);
                }

                (grdProduct.View as TableView).RowUpdated -= View_RowUpdated;
                (grdProduct.View as TableView).RowUpdated += View_RowUpdated;

                if (animationManager.Animation != null)
                {
                    animationManager.Animation.PropertyChanged -= Animation_PropertyChanged;
                    animationManager.Animation.PropertyChanged += Animation_PropertyChanged;
                }

                GridSortInfo sort1 = new GridSortInfo("IDItemGroup");
                GridSortInfo sort2 = new GridSortInfo("SortOrder");

                grdProduct.BeginDataUpdate();
                
                grdProduct.SortInfo.Clear();
                grdProduct.SortInfo.Add(sort1);
                grdProduct.SortInfo.Add(sort2);

                grdProduct.ClearGrouping();

                grdProduct.EndDataUpdate();

            }
            else if (e.PropertyName == "ProcurementPlan")
            {
                AddRetailerTypeColumns();
            }
        }

        // Animation PropertyChanged
        void Animation_PropertyChanged(object sender, PropertyChangedEventArgs e)
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
                    this.grdProduct.View.AllowEditing = true;
                    break;
                case AnimationStatus.Locked:
                case AnimationStatus.Draft:
                case AnimationStatus.Published:
                case AnimationStatus.Closed:
                case AnimationStatus.Cleared:
                    this.grdProduct.View.AllowEditing = false;
                    break;
            }
        }

        void ProcurementPlan_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            AddRetailerTypeColumns();
        }

        private void AddRetailerTypeColumns()
        {
            int index = 0;

            List<GridColumn> toDelete = grdProduct.Columns.Where(c => c.Name.StartsWith("RetailerType")).ToList();
            for (int i = 0; i < toDelete.Count(); i++)
            {
                grdProduct.Columns.Remove(toDelete[i]);
            }

            if (animationManager.ProcurementPlan != null)
            {
                foreach (ProcurementPlanAnimation pp in animationManager.ProcurementPlan)
                {
                    index++;
                    GridColumn column = new GridColumn()
                                            {
                                                Header = pp.RetailerType,
                                                UnboundType = UnboundColumnType.String,
                                                FieldName = "RetailerType" + index,
                                                Name = "RetailerType" + index,
                                                ReadOnly = true
                                            };

                    grdProduct.Columns.Add(column);
                }
            }

            grdProduct.DataContext = animationManager;
            grdProduct.RefreshData();
        }

        void InsertOrUpdate(AnimationProduct entity)
        {
            try
            {
                entity.Animation = animationManager.Animation;
                animationManager.ProductInsertUpdate(entity);
            }
            catch (SqlException exc)
            {
                SqlException sqlExc = exc as SqlException;
                if (sqlExc.Number == 50000 && sqlExc.Class == 16)
                {
                    if (sqlExc.Errors.Count > 0)
                    {
                        MessageBox.Show(sqlExc.Errors[0].Message);
                    }
                    else
                    {
                        MessageBox.Show(sqlExc.Message);
                    }
                }
                else
                {
                    MessageBox.Show(exc.Message);
                    Logger.Log(exc.ToString(), LogLevel.Error);
                    throw;
                }
            }
            catch (Exception exc)
            {
                //MessageBox.Show("An error occured when inserting the product:" + Utility.GetExceptionsMessages(exc));
                Logger.Log(exc.ToString(), LogLevel.Error);
                MessageBox.Show(SystemMessagesManager.Instance.GetMessage("ProductAnimationUpdateException", Utility.GetExceptionsMessages(exc)));
                (grdProduct.View as TableView).DeleteRow(grdProduct.View.FocusedRowHandle);
            }
        }

        void View_RowUpdated(object sender, DevExpress.Xpf.Grid.RowEventArgs e)
        {
            if (e.RowHandle == GridControl.NewItemRowHandle)
            {
                AnimationProduct entity = (e.Source as TableView).FocusedRow as AnimationProduct;
                InsertOrUpdate(entity);

                entity.CalculateTotalCapacity();
                animationManager.RecalculateAnimationProduct(RecalculationType.CalculateActiveAnimations);
                animationManager.Animation.ObservableProductDetails = null;
                calcType = RecalculationType.None;
                return;
            }

            if (calcType != RecalculationType.None)
            {
                animationManager.RecalculateAnimationProduct(calcType);
                calcType = RecalculationType.None;
            }
        }

        private void TableView_CellValueChanged(object sender, DevExpress.Xpf.Grid.CellValueChangedEventArgs e)
        {
            if (e.Column == null)
            {
                return;
            }

            if (e.Row != null)
            {
                if (e.Row as ICleanEntityRef != null)
                {
                    (e.Row as ICleanEntityRef).CleanEntityRef(e.Column.FieldName);
                }
            }

            if (e.Column.Name == "clmMaterialCode")
            {
                if (e.Value != null && e.Value.ToString() != String.Empty)
                {
                    Product product = ProductManager.Instance.GetByMaterialCode(e.Value.ToString());
                    (e.Row as AnimationProduct).MaterialCode = null;

                    if (product != null)
                    {
                        try
                        {
                            AnimationProduct ap = e.Row as AnimationProduct;

                            ap.Product = product;
                            ap.Description = null;

                            IEnumerable<Multiple> multiples = MultipleManager.Instance.MultipleGetToProduct(product.ID);

                            cboNormalMultiple.ItemsSource = multiples;
                            cboWarehouseMultiple.ItemsSource = multiples;

                            // clear multiples if not valid

                            if (ap.NormalMultiple != String.Empty)
                            {
                                if (multiples.Where(m => m.Value == Convert.ToInt32(ap.NormalMultiple) && m.IDProduct == product.ID).Count() == 0)
                                    (e.Row as AnimationProduct).Multiple = null;
                            }

                            if (ap.WarehouseMultiple != String.Empty)
                            {
                                if (multiples.Where(m => m.Value == Convert.ToInt32(ap.WarehouseMultiple) && m.IDProduct == product.ID).Count() == 0)
                                    (e.Row as AnimationProduct).Multiple1 = null;                                
                            }
                        }
                        catch (Exception exc)
                        {
                            Logger.Log(exc.ToString(), LogLevel.Error);
                        }
                    }
                    else
                    {
                        IEnumerable<string> materialCodes = ProductManager.Instance.GetMaterialCodes(e.Value.ToString());
                        if (materialCodes.Count() > 0)
                        {
                            string materialCode = materialCodes.First();
                            grdProduct.SetCellValue(e.RowHandle, e.Column, materialCode);
                        }
                        else
                        {
                            if (e.OldValue != null)
                                (e.Row as AnimationProduct).MaterialCode = e.OldValue.ToString();
                            else
                                (e.Row as AnimationProduct).MaterialCode = null;

                            //MessageBox.Show(string.Format("No product exists for material code '{0}'", e.Value));
                            MessageBox.Show(SystemMessagesManager.Instance.GetMessage("ProductAnimationProductNotFound", e.Value.ToString()));
                        }
                        return;
                    }
                }
            }
            else if (e.Column.Name == "clmProductDescription")
            {
                if (e.Value != null && e.Value.ToString() != String.Empty)
                {
                    Product product = ProductManager.Instance.GetByDescription(e.Value.ToString());
                    (e.Row as AnimationProduct).Description = null;

                    if (product != null)
                    {
                        try
                        {
                            (e.Row as AnimationProduct).Product = product;
                            (e.Row as AnimationProduct).MaterialCode = null;

                            IEnumerable<Multiple> multiples = MultipleManager.Instance.MultipleGetToProduct(product.ID);

                            cboNormalMultiple.ItemsSource = multiples;
                            cboWarehouseMultiple.ItemsSource = multiples;
                        }
                        catch (Exception exc)
                        {
                            Logger.Log(exc.ToString(), LogLevel.Error);
                        }
                    }
                    else
                    {
                        IEnumerable<string> descriptions = ProductManager.Instance.GetDescriptions(e.Value.ToString());
                        if (descriptions.Count() > 0)
                        {
                            string description = descriptions.First();
                            grdProduct.SetCellValue(e.RowHandle, e.Column, description);
                        }
                        else
                        {
                            if (e.OldValue != null)
                                (e.Row as AnimationProduct).Description = e.OldValue.ToString();
                            else
                                (e.Row as AnimationProduct).Description = null;

                            //MessageBox.Show(string.Format("No product exists for description '{0}'", e.Value));
                            MessageBox.Show(SystemMessagesManager.Instance.GetMessage("ProductAnimationDescriptionNotFound", e.Value.ToString()));
                        }
                        return;
                    }
                }
            }
            else if (e.Column.Name == "clmItemGroup" && e.RowHandle != GridControl.NewItemRowHandle && e.RowHandle != GridControl.InvalidRowHandle)
            {
                (e.Row as AnimationProduct).ItemGroup = DbDataContext.GetInstance().ItemGroups.Single(ig => ig.ID == (Guid)e.Value);
            }
            else if (e.Column.Name == "clmItemType" && e.RowHandle != GridControl.NewItemRowHandle && e.Value != null)
            {
                if ((Guid)e.Value != Guid.Empty)
                {
                    (e.Row as AnimationProduct).ItemType = DbDataContext.GetInstance().ItemTypes.Single(it => it.ID == (Guid)e.Value);
                }
            }
            else if (e.Column.FieldName == "ConfirmedMadMonthString")
            {
                if (e.Value != null && e.Value.ToString() != String.Empty)
                {
                    string date = madMonths.Where(m=>m.Contains(e.Value.ToString())).FirstOrDefault();
                    cboMadMonth.ItemsSource = madMonths.Where(m => m.Contains(e.Value.ToString()));
                    if (!string.IsNullOrEmpty(date))
                    {
                        try
                        {
                            (e.Row as AnimationProduct).ConfirmedMADMonth = DateTime.Parse(date);
                            (e.Row as AnimationProduct).ConfirmedMadMonthString = string.Empty;
                        }
                        catch (Exception exc)
                        {
                            Logger.Log(exc.ToString(), LogLevel.Error);
                        }
                    }
                    else
                    {
                        (e.Row as AnimationProduct).ConfirmedMadMonthString = string.Empty;
                        return;
                    }
                }
            }

            // insert or update
            if (e.Row != null && e.Value != null && e.Value.Equals(e.OldValue) == false)
            {
                AnimationProduct entity = e.Row as AnimationProduct;

                if (e.Column.FieldName == "IDSignature")
                {
                    entity.CleanEntityRef("IDBrandAxe");
                    entity.IDBrandAxe = null;
                }

                if (entity.ID != Guid.Empty)
                {
                    InsertOrUpdate(entity);

                    if (e.Column.FieldName == "MaterialCode" || e.Column.FieldName == "Description")
                    {
                        calcType = RecalculationType.CalculateActiveAnimations;
                    }
                    else if (e.Column.FieldName == "IDItemType" || e.Column.FieldName == "IDSignature" || e.Column.FieldName == "IDBrandAxe" || e.Column.FieldName == "IDCategory")
                    {
                        entity.CalculateTotalCapacity();
                        if (animationManager.Allocations != null)
                        {
                            animationManager.Allocations.RefreshAllocations();
                        }
                    }
                    else if (e.Column.FieldName == "ConfirmedMadMonthString")
                    {
                        entity.CalculateProductRecieved();
                    }
                }
            }
           
        }

        private void TableView_InitNewRow(object sender, DevExpress.Xpf.Grid.InitNewRowEventArgs e)
        {
            AnimationProduct product = grdProduct.GetRow(e.RowHandle) as AnimationProduct;

            // Sometimes DevExpress throws an ArgumentOutOfRangeException, if there is any sort or filtering
            // now, we just block the application from crushing down

            try
            {
                product.Product = new Product();
            }
            catch { }

            try
            {
                product.Animation = animationManager.Animation;
            }
            catch { }

            try
            {
                product.ConfirmedMADMonth = DateTime.Now;
            }
            catch { }

            NewItemRow = product;
        }

        private void TableView_ValidateRow(object sender, GridRowValidationEventArgs e)
        {
            AnimationProduct ap = e.Row as AnimationProduct;
            if (ap != null)
            {
                string errorMessage;
                string firstInvalidColumn;
                if (ap.IsValid(out errorMessage, out firstInvalidColumn) == false)
                {
                    grdProduct.View.FocusedRowHandle = e.RowHandle;
                    if (firstInvalidColumn == "IDProduct")
                    {
                        (grdProduct.View as TableView).FocusedColumn = this.clmMaterialCode;
                        e.SetError("Product data is missing. Please input the Material Code or Description. ");
                    }
                    else
                    {
                        (grdProduct.View as TableView).FocusedColumn = grdProduct.Columns[firstInvalidColumn];
                        e.SetError(string.Format(errorMessage, (grdProduct.View as TableView).FocusedColumn.HeaderCaption));
                    }
                }
            }
        }

        private void btnCreateProduct_Click(object sender, RoutedEventArgs e)
        {
            CreateNewProduct createProductCtrl = new CreateNewProduct();
            PopupWindow createProductDialog = new PopupWindow("Creating New Product");
            createProductCtrl.Close += new EventHandler(createProductDialog.CloseWindowEvent);
            createProductDialog.Width = 400;
            createProductDialog.Height = 300;
            createProductDialog.AddControl(createProductCtrl);
            createProductDialog.ShowDialog();
        }

        private void btnViewProducts_Click(object sender, RoutedEventArgs e)
        {
            Products productCtrl = new Products(false);
            PopupWindow productDialog = new PopupWindow("All products");
            productDialog.Width = 1100;
            productDialog.Height = 800;
            productDialog.AddControl(productCtrl);
            productDialog.ShowDialog();

            // grdProduct.RefreshData();
        }

        private void TableView_CellValueChanging(object sender, CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName == "MaterialCode" && e.Value != null)
            {
                cboMaterialCodes.ItemsSource = ProductManager.Instance.GetMaterialCodes(e.Value.ToString());
                return;
            }
            else if (e.Column.FieldName == "Description" && e.Value != null)
            {
                cboDescriptions.ItemsSource = ProductManager.Instance.GetDescriptions(e.Value.ToString());
                return;
            }
            else if (e.Column.FieldName == "ConfirmedMadMonthString" && e.Value != null)
            {
                cboMadMonth.ItemsSource = madMonths.Where(m => m.Contains(e.Value.ToString()));
                return;
            }

            if (e.Row == null)
                return;

            AnimationProduct ap = e.Row as AnimationProduct;

            if (ap.ID == Guid.Empty)
            {
                ap.CleanEntityRef(e.Column.FieldName);
                return;
            }

            //we can not user table_CellValueChanged for some kinds of column, because table_CellValueChanged is not called right after the value is changes, but when the row looses focus
            if (e.Column.EditSettings is CheckEditSettings
                || e.Column.EditSettings is ComboBoxEditSettings)
            {
                string errMessag;
                string fstCol;
                if (ap.IsValid(out errMessag, out fstCol) == false)
                    return;

                // Before we set property a new value, we have to clean the entityref if property is a foreign key
                if ((e.Row as ICleanEntityRef) != null)
                {
                    (e.Row as ICleanEntityRef).CleanEntityRef(e.Column.FieldName);
                }

                //e.Row contains old values and new value is passed in e.Value => we need to assign  the value using reflection
                // grdProduct.View.CommitEditing();

                // animationManager.ProductInsertUpdate(ap);

                // (sender as TableView).Focus();
            }
        }

        private void TableView_RowCanceled(object sender, RowEventArgs e)
        {
            if (NewItemRow != null)
            {
                if (NewItemRow.Animation != null)
                    NewItemRow.Animation.AnimationProducts.Remove(NewItemRow);
                if (NewItemRow.Product != null)
                    NewItemRow.Product.AnimationProducts.Remove(NewItemRow);

                NewItemRow = null;
            }
        }

        private void TableView_ShownEditor(object sender, EditorEventArgs e)
        {
            if (e.Column.FieldName == "IDBrandAxe")
            {
                ComboBoxEdit edit = e.Editor as ComboBoxEdit;
                if (edit != null && e.Row != null)
                {
                    AnimationProduct ap = e.Row as AnimationProduct;

                    if (ap.IDSignature.HasValue && ap.IDSignature.Value != Guid.Empty)
                    {
                        // (e.Column.EditSettings as LookupComboBoxEditSettings).OriginalItemsSource = null;
                        (e.Column.EditSettings as ComboBoxEditSettings).ItemsSource = null;
                        DbDataContext Db = DbDataContext.GetInstance();

                        edit.ItemsSource = new BrandAxe[] { new BrandAxe() }.Union(BrandAxeManager.Instance.GetAllForAllocation(ap.IDSignature.Value));
                    }
                }
                else
                {
                    this.cboBrandAxes.ItemsSource = null;
                }
            }
            else if (e.Column.FieldName == "IDMultipleNormal" || e.Column.FieldName == "IDMultipleWarehouse")
            {
                ComboBoxEditSettings settings = e.Column.FieldName == "IDMultipleNormal" ? cboNormalMultiple : cboWarehouseMultiple;
                ComboBoxEdit edit = e.Editor as ComboBoxEdit;
                if (edit != null && e.Row != null)
                {
                    AnimationProduct ap = e.Row as AnimationProduct;
                    if (ap.Product != null)
                    {
                        // (settings as LookupComboBoxEditSettings).OriginalItemsSource = null;
                        (settings as ComboBoxEditSettings).ItemsSource = null;
                        IEnumerable<Multiple> multiples = new Multiple[]{new Multiple()}.Union(MultipleManager.Instance.MultipleGetToProduct(ap.Product.ID));
                        edit.ItemsSource = multiples;
                    }
                    else
                        settings.ItemsSource = null;
                }
                else
                    settings.ItemsSource = null;
            }
            else if (e.Column.FieldName == "MaterialCode")
            {
                ComboBoxEdit edit = e.Editor as ComboBoxEdit;
                if (edit != null)
                {
                    if (e.Value == null)
                        edit.ItemsSource = ProductManager.Instance.GetMaterialCodes();
                    else
                        edit.ItemsSource = ProductManager.Instance.GetMaterialCodes(e.Value.ToString());
                }
            }
            else if (e.Column.FieldName == "Description")
            {
                ComboBoxEdit edit = e.Editor as ComboBoxEdit;
                if (edit != null)
                {
                    if (e.Value == null)
                        edit.ItemsSource = ProductManager.Instance.GetDescriptions();
                    else
                        edit.ItemsSource = ProductManager.Instance.GetDescriptions(e.Value.ToString());
                }
            }
            else if (e.Column.FieldName == "ConfirmedMadMonthString")
            {
                ComboBoxEdit edit = e.Editor as ComboBoxEdit;
                if (edit != null)
                {
                    if (e.Value == null)
                        edit.ItemsSource = madMonths;
                    else
                        edit.ItemsSource = madMonths.Where(m => m.Contains(e.Value.ToString()));
                }
            }
        }

        private void TableView_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TableView tableView = sender as TableView;
            if (e.Key == Key.Delete && tableView.IsEditing == false && animationManager.Animation != null && animationManager.Animation.Status.HasValue)
            {
                if (tableView.SelectedRows.Count > 0)
                {
                    byte status = animationManager.Animation.Status.Value;
                    bool isOpenStatus = status == (byte)AnimationStatus.Open;
                    bool isActiveStatus = (status == (byte)AnimationStatus.Open) || (status == (byte)AnimationStatus.Locked) || (status == (byte)AnimationStatus.Draft) || (status == (byte)AnimationStatus.Published);

                    if (this.isDivisionAdmin == false && this.isMarketingUser == false)
                    {
                        //MessageBox.Show("Animation products can be deleted only by Markeing or Division Administrator");
                        MessageBox.Show(SystemMessagesManager.Instance.GetMessage("ProductAnimationDelete"));
                        return;
                    }

                    if ((isMarketingUser && isOpenStatus) || (isDivisionAdmin && isActiveStatus))
                    {
                        AnimationProduct ap = tableView.SelectedRows[0] as AnimationProduct;

                        // check if animation product has any customer/customer group allocations.
                        if (ap.AnimationProductDetails.Any(apd => apd.CustomerAllocations.Count > 0 || apd.CustomerGroupAllocations.Count > 0))
                        {
                            if (MessageBox.Show(SystemMessagesManager.Instance.GetMessage("ProductAnimationDeleteWithAllocation"), "Deleting an Animation Product", MessageBoxButton.YesNo) == MessageBoxResult.No)
                                return;
                        }

                        animationManager.ProductDelete(ap);
                        tableView.DeleteRow(tableView.GetSelectedRowHandles()[0]);

                        animationManager.Allocations = null;
                    }
                    else
                    {
                        // MessageBox.Show("Animation products cannot be deleted because of its Animation status.");
                        MessageBox.Show(SystemMessagesManager.Instance.GetMessage("ProductAnimationDeleteWithStatus"));
                    }
                }
            }
            else if (e.Key == Key.Up && animationManager.Animation != null && animationManager.Animation.Status.HasValue)
            {
                // grdProduct.View.MovePrevRow();
            }
        }

        private void TableView_ShowFilterPopup(object sender, FilterPopupEventArgs e)
        {
            
            if (e.Column.FieldName == "IDMultipleNormalxxx")
            {
                List<object> filterItems = new List<object>();
                foreach (object obj in e.ComboBoxEdit.ItemsSource)
                {
                    if (obj as CustomComboBoxItem != null)
                    {
                        filterItems.Add(obj);
                    }
                }

                //List<Multiple> multiples = animationManager.Products.Select(ap => ap.Multiple).ToList();
                //foreach (Multiple m in multiples)
                //{
                //    filterItems.Add(m);
                //}

                List<Guid?> normalMultiples = animationManager.Products.Select(ap => ap.IDMultipleNormal).ToList();
                foreach (Guid? guid in normalMultiples)
                {
                    if (guid != null)
                    {
                        Multiple multiple = MultipleManager.Instance.GetAll().Single(mm => mm.ID == guid.Value);
                        string displayValue = string.Format("{0} - {1}", multiple.Value, multiple.Product.MaterialCode);
                        if (filterItems.Any(item => (item as CustomComboBoxItem).DisplayValue.ToString() == displayValue.ToString()) == false)
                        {
                            filterItems.Add(new CustomComboBoxItem()
                            {
                                DisplayValue = displayValue,
                                EditValue = CriteriaOperator.Parse(string.Format("[IDMultipleNormal] = ?"), guid.Value)
                            });
                        }
                    }
                }

                e.ComboBoxEdit.ItemsSource = filterItems;
               
            }
        }
     
    }
}