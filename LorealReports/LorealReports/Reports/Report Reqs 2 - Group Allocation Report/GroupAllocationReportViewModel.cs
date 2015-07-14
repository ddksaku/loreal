using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using DevExpress.Xpf.Core.Commands;
using DevExpress.Xpf.Editors;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports;
using LorealReports.DataAccess;
using LorealReports.Reports.Base;
using LorealReports.Reports.ValueConverters;
using Microsoft.Office.Interop.Excel;

namespace LorealReports.Reports
{
    public class GroupAllocationReportViewModel : BaseViewModel
    {
        #region Properties and Members

        public List<SalesDrive> SalesDrives { get; set; }
        public List<Animation> Animations { get; set; }
        public List<SalesArea> SalesAreas { get; set; }
        public List<CustomerGroup> CustomerGroups { get; set; }
        public List<SalesEmployee> SalesEmployees { get; set; }
        public List<ItemGroup> ItemGroups { get; set; }
        public List<Product> Products { get; set; }


        #region SalesDrive

        public SalesDrive SelectedSalesDrive { get; set; }

        public List<SalesDrive> SalesDriveSource
        {
            get { return SalesDrives; }
        }

        #endregion

        #region Animation

        public List<Animation> SelectedAnimations { get; set; }

        public List<Animation> AnimationSource
        {
            get
            {
                if (SelectedSalesDrive == null)
                {
                    return new List<Animation>();
                }
                else
                {
                    return db.Animation.Where(x => x.IDSalesDrive == SelectedSalesDrive.ID && x.IDDivision == UserDivision.ID).ToList();
                }
            }
        }

        #endregion

        #region SalesArea

        public List<SalesArea> SelectedSalesAreas { get; set; }

        public List<SalesArea> SalesAreaSource
        {
            get
            {
                if (SelectedAnimations == null || SelectedAnimations.Count == 0)
                {
                    return new List<SalesArea>();
                }
                else
                {
                    List<Guid> selectedAnimationGuids = new List<Guid>();
                    selectedAnimationGuids.AddRange(SelectedAnimations.Select(x => x.ID));

                    var query = from salesArea in db.SalesArea
                                where (from animationProductDetail in db.AnimationProductDetail
                                       where (selectedAnimationGuids).Contains(animationProductDetail.AnimationProduct.IDAnimation)
                                       select animationProductDetail.IDSalesArea).Contains(salesArea.ID)
                                       && salesArea.IDDivision == UserDivision.ID
                                select salesArea;

                    return query.ToList();
                }
            }
        }

        #endregion

        #region CustomerGroups

        public List<CustomerGroup> SelectedCustomerGroups { get; set; }

        public List<CustomerGroup> CustomerGroupSource
        {
            get
            {
                if (SelectedAnimations == null || SelectedAnimations.Count == 0)
                {
                    return new List<CustomerGroup>();
                }
                else
                {
                    if (SelectedSalesAreas == null || SelectedSalesAreas.Count == 0)
                    {
                        List<Guid> selectedAnimationGuids = new List<Guid>();
                        selectedAnimationGuids.AddRange(SelectedAnimations.Select(x => x.ID));

                        var query = from customerGroup in db.CustomerGroup
                                    where (from salesArea in db.SalesArea
                                           where (from animationProductDetail in db.AnimationProductDetail
                                                  where (selectedAnimationGuids).Contains(animationProductDetail.AnimationProduct.IDAnimation)
                                                  select animationProductDetail.IDSalesArea).Contains(salesArea.ID)
                                                  && salesArea.IDDivision == UserDivision.ID
                                           select salesArea.ID).Contains(customerGroup.IDSalesArea)
                                    select customerGroup;
                        return query.ToList();
                    }
                    else
                    {
                        List<Guid> selectedSalesAreaGuids = new List<Guid>();
                        selectedSalesAreaGuids.AddRange(SelectedSalesAreas.Select(x => x.ID));

                        var query = from customerGroup in db.CustomerGroup
                                    where (selectedSalesAreaGuids).Contains(customerGroup.IDSalesArea)
                                    select customerGroup;
                        return query.ToList();
                    }
                }
            }
        }

        #endregion

        #region SalesEmployees

        public List<SalesEmployee> SelectedSalesEmployees { get; set; }

        public List<SalesEmployee> SalesEmployeeSource
        {
            get
            {
                if (SelectedAnimations == null || SelectedAnimations.Count == 0)
                {
                    return new List<SalesEmployee>();
                }
                else
                {
                    if (SelectedCustomerGroups == null || SelectedCustomerGroups.Count == 0)
                    {
                        List<Guid> selectedAnimationGuids = new List<Guid>();
                        selectedAnimationGuids.AddRange(SelectedAnimations.Select(x => x.ID));

                        var query = from salesEmployee in db.SalesEmployee
                                    where (from customer in db.Customer
                                           where (from salesArea in db.SalesArea
                                                  where (from animationProductDetail in db.AnimationProductDetail
                                                         where (selectedAnimationGuids).Contains(animationProductDetail.AnimationProduct.IDAnimation)
                                                         select animationProductDetail.IDSalesArea).Contains(salesArea.ID)
                                                  select salesArea.ID).Contains(customer.CustomerGroup.IDSalesArea)
                                           select customer.IDSalesEmployee).Contains(salesEmployee.ID)
                                           && salesEmployee.IDDivision == UserDivision.ID
                                    select salesEmployee;

                        return query.ToList();
                    }
                    else
                    {
                        List<Guid> selectedCustomerGroupGuids = new List<Guid>();
                        selectedCustomerGroupGuids.AddRange(SelectedCustomerGroups.Select(x => x.ID));

                        var query = from salesEmployee in db.SalesEmployee
                                    where (from customer in db.Customer
                                           where (selectedCustomerGroupGuids).Contains(customer.IDCustomerGroup)
                                           select customer.IDSalesEmployee).Contains(salesEmployee.ID)
                                    select salesEmployee;

                        return query.ToList();
                    }
                }
            }
        }

        #endregion

        #region ItemGroup

        public List<ItemGroup> SelectedItemGroups { get; set; }

        public List<ItemGroup> ItemGroupSource
        {
            get
            {
                if (SelectedAnimations == null || SelectedAnimations.Count == 0)
                {
                    return new List<ItemGroup>();
                }
                else
                {
                    if (SelectedCustomerGroups == null || SelectedCustomerGroups.Count == 0)
                    {
                        List<Guid> selectedAnimationGuids = new List<Guid>();
                        selectedAnimationGuids.AddRange(SelectedAnimations.Select(x => x.ID));

                        var query = from itemGroup in db.ItemGroup
                                    where (from animationProduct in db.AnimationProduct
                                           where (selectedAnimationGuids).Contains(animationProduct.IDAnimation)
                                           select animationProduct.IDItemGroup).Contains(itemGroup.ID)
                                           && itemGroup.IDDivision == UserDivision.ID
                                    select itemGroup;

                        return query.ToList();
                    }
                    else
                    {
                        List<Guid> selectedCustomerGroupGuids = new List<Guid>();
                        selectedCustomerGroupGuids.AddRange(SelectedCustomerGroups.Select(x => x.ID));

                        var query = from itemGroup in db.ItemGroup
                                    where (from animationProduct in db.AnimationProduct
                                           where (from animationProductDetail in db.AnimationProductDetail
                                                  where (from customerGroup in db.CustomerGroup
                                                         where (selectedCustomerGroupGuids).Contains(customerGroup.ID)
                                                         select customerGroup.IDSalesArea).Contains(animationProductDetail.IDSalesArea)
                                                  select animationProductDetail.IDAnimationProduct).Contains(animationProduct.ID)
                                           select animationProduct.IDItemGroup).Contains(itemGroup.ID)
                                    select itemGroup;
                        return query.ToList();
                    }
                }
            }
        }

        #endregion

        #region ProductName

        public List<Product> SelectedProductNames { get; set; }

        public List<Product> ProductNamesSource
        {
            get
            {
                if (SelectedAnimations == null || SelectedAnimations.Count == 0)
                {
                    return new List<Product>();
                }
                else
                {
                    if (SelectedItemGroups == null || SelectedItemGroups.Count == 0)
                    {
                        List<Guid> selectedAnimationGuids = new List<Guid>();
                        selectedAnimationGuids.AddRange(SelectedAnimations.Select(x => x.ID));

                        var query = from product in db.Product
                                    where (from animationProduct in db.AnimationProduct
                                           where (selectedAnimationGuids).Contains(animationProduct.IDAnimation)
                                           select animationProduct.IDProduct).Contains(product.ID)
                                           && product.IDDivision == UserDivision.ID
                                    select product;

                        return query.ToList();
                    }
                    else
                    {
                        List<Guid> selectedItemGroupGuids = new List<Guid>();
                        selectedItemGroupGuids.AddRange(SelectedItemGroups.Select(x => x.ID));

                        var query = from product in db.Product
                                    where (from animationProduct in db.AnimationProduct
                                           where (selectedItemGroupGuids).Contains(animationProduct.IDItemGroup)
                                           select animationProduct.IDProduct).Contains(product.ID)
                                    select product;
                        return query.ToList();
                    }
                }
            }
        }


        #endregion


        private ComboBoxEdit animationEditor;

        private ComboBoxEdit salesAreaEditor;

        private ComboBoxEdit customerGroupEditor;

        private ComboBoxEdit salesEmployeesEditor;

        private ComboBoxEdit itemGroupEditor;

        private ComboBoxEdit productNameEditor;

        #endregion

        #region Constructor

        public GroupAllocationReportViewModel()
            : base(new Guid("0DB8788D-59DE-4FC3-A65D-DF704500E9F3"))
        {
            ExportToCustomExcel = new DelegateCommand<object>(ExecuteExportCommand, CanExecuteExportCommand);

            LoadComboboxSources();

            SelectedAnimations = new List<Animation>();
            SelectedItemGroups = new List<ItemGroup>();
            SelectedSalesAreas = new List<SalesArea>();
            SelectedCustomerGroups = new List<CustomerGroup>();
            SelectedSalesEmployees = new List<SalesEmployee>();
            SelectedProductNames = new List<Product>();

            ReportPreviewModel.CustomizeParameterEditors += ReportPreviewModel_CustomizeParameterEditors;
        }

        #endregion

        #region Commanding

        public ICommand ExportToCustomExcel { get; private set; }

        bool CanExecuteExportCommand(object parameter)
        {
            return true;
        }

        void ExecuteExportCommand(object parameter)
        {
            Export();
        }

        #endregion

        #region Methods

        void ReportPreviewModel_CustomizeParameterEditors(object sender, DevExpress.Xpf.Printing.CustomizeParameterEditorsEventArgs e)
        {
            #region SalesDrive

            if (e.Parameter.Name == "SalesDrive")
            {
                var salesDriveCombo = new ComboBoxEdit()
                                          {
                                              DisplayMember = "Name",
                                              ItemsSource = SalesDriveSource,
                                              IsTextEditable = false
                                          };

                salesDriveCombo.SelectedIndexChanged += new RoutedEventHandler(salesDriveCombo_SelectedIndexChanged);

                e.Editor = salesDriveCombo;
                e.BoundDataMember = "EditValue";
                e.BoundDataConverter = new SingleSelectedConverter<SalesDrive>();
                e.BoundDataConverterParameter = SalesDrives;
            }

            #endregion

            #region Animations

            if (e.Parameter.Name == "Animations")
            {
                var animationCombo = new ComboBoxEdit()
                {
                    ItemsSource = AnimationSource,
                    DisplayMember = "Name",
                    IsTextEditable = false,
                    SeparatorString = ",",
                    StyleSettings = new CheckedComboBoxStyleSettings()
                };
                e.Editor = animationCombo;
                animationEditor = (ComboBoxEdit)animationCombo;
                animationCombo.SelectedIndexChanged += new RoutedEventHandler(animationCombo_SelectedIndexChanged);
                e.BoundDataMember = "EditValue";
                e.BoundDataConverter = new MultiSelectedConverter<Animation>();
                e.BoundDataConverterParameter = Animations;
            }

            #endregion

            #region SalesArea

            if (e.Parameter.Name == "SalesArea")
            {
                var combo = new ComboBoxEdit()
                                {
                                    DisplayMember = "Name",
                                    ItemsSource = SalesAreaSource,
                                    IsTextEditable = false,
                                    SeparatorString = ",",
                                    StyleSettings = new CheckedComboBoxStyleSettings()
                                };

                //combo.Validate += new DevExpress.Xpf.Editors.Validation.ValidateEventHandler(combo_Validate);
                e.Editor = combo;
                salesAreaEditor = (ComboBoxEdit)combo;
                salesAreaEditor.SelectedIndexChanged += new RoutedEventHandler(salesAreaEditor_SelectedIndexChanged);
                e.BoundDataMember = "EditValue";
                e.BoundDataConverter = new MultiSelectedConverter<SalesArea>();
                e.BoundDataConverterParameter = SalesAreas;
            }

            #endregion

            #region CustomerGroup

            if (e.Parameter.Name == "CustomerGroups")
            {
                var combo = new ComboBoxEdit()
                                {
                                    DisplayMember = "Name",
                                    ItemsSource = CustomerGroupSource,
                                    IsTextEditable = false,
                                    SeparatorString = ",",
                                    StyleSettings = new CheckedComboBoxStyleSettings()
                                };

                //combo.Validate += new DevExpress.Xpf.Editors.Validation.ValidateEventHandler(combo_Validate);
                e.Editor = combo;
                customerGroupEditor = (ComboBoxEdit)combo;
                customerGroupEditor.SelectedIndexChanged += new RoutedEventHandler(customerGroupEditor_SelectedIndexChanged);
                e.BoundDataMember = "EditValue";
                e.BoundDataConverter = new MultiSelectedConverter<CustomerGroup>();
                e.BoundDataConverterParameter = CustomerGroups;
            }

            #endregion

            #region SalesEmployees

            if (e.Parameter.Name == "SalesEmployees")
            {
                var combo = new ComboBoxEdit()
                                {
                                    DisplayMember = "Name",
                                    ItemsSource = SalesEmployeeSource,
                                    IsTextEditable = false,
                                    SeparatorString = ",",
                                    StyleSettings = new CheckedComboBoxStyleSettings()
                                };

                //combo.Validate += new DevExpress.Xpf.Editors.Validation.ValidateEventHandler(combo_Validate);
                e.Editor = combo;
                salesEmployeesEditor = (ComboBoxEdit)combo;
                e.BoundDataMember = "EditValue";
                e.BoundDataConverter = new MultiSelectedConverter<SalesEmployee>();
                e.BoundDataConverterParameter = SalesEmployees;
            }

            #endregion

            #region ItemGroups

            if (e.Parameter.Name == "ItemGroup")
            {
                var itemGroupCombo = new ComboBoxEdit()
                                         {
                                             DisplayMember = "Name",
                                             ItemsSource = ItemGroupSource,
                                             IsTextEditable = false,
                                             SeparatorString = ",",
                                             StyleSettings = new CheckedComboBoxStyleSettings()
                                         };
                //itemGroupCombo.Validate += new DevExpress.Xpf.Editors.Validation.ValidateEventHandler(itemGroupCombo_Validate);
                e.Editor = itemGroupCombo;
                itemGroupCombo.SelectedIndexChanged += new RoutedEventHandler(itemGroupCombo_SelectedIndexChanged);
                itemGroupEditor = itemGroupCombo;
                e.BoundDataMember = "EditValue";
                e.BoundDataConverter = new MultiSelectedConverter<ItemGroup>();
                e.BoundDataConverterParameter = ItemGroups;
            }

            #endregion

            #region ProductName

            if (e.Parameter.Name == "ProductName")
            {
                var productNameCombo = new ComboBoxEdit()
                {
                    DisplayMember = "Description",
                    ItemsSource = ProductNamesSource,
                    IsTextEditable = false,
                    SeparatorString = ",",
                    StyleSettings = new CheckedComboBoxStyleSettings()
                };
                //itemGroupCombo.Validate += new DevExpress.Xpf.Editors.Validation.ValidateEventHandler(itemGroupCombo_Validate);
                e.Editor = productNameCombo;
                productNameCombo.SelectedIndexChanged += new RoutedEventHandler(productNameCombo_SelectedIndexChanged);
                productNameEditor = productNameCombo;
                e.BoundDataMember = "EditValue";
                e.BoundDataConverter = new MultiSelectedConverter<Product>();
                e.BoundDataConverterParameter = Products;
            }

            #endregion
        }

        #region Validations

        #endregion

        private void LoadComboboxSources()
        {
            SalesDrives = db.SalesDrive.Where(x => x.IDDivision == UserDivision.ID).ToList();
            Animations = db.Animation.Where(x => x.IDDivision == UserDivision.ID).ToList();
            SalesAreas = db.SalesArea.Where(x => x.IDDivision == UserDivision.ID).ToList();
            CustomerGroups = db.CustomerGroup.ToList();
            SalesEmployees = db.SalesEmployee.Where(x => x.IDDivision == UserDivision.ID).ToList();
            ItemGroups = db.ItemGroup.Where(x => x.IDDivision == UserDivision.ID).ToList();
            Products = db.Product.Where(x => x.IDDivision == UserDivision.ID).ToList();
        }

        #region Event Handlers

        #region SalesDrive

        void salesDriveCombo_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            SelectedSalesDrive = (SalesDrive)(sender as ComboBoxEdit).SelectedItem;

            ClearSelectedAnimations();
            ClearSelectedSalesAreas();
            ClearSelectedCustomerGroups();
            ClearSelectedSalesEmployees();
            ClearSelectedItemGroups();
            ClearSelectedProductNames();
        }

        #endregion

        #region Animation

        void animationCombo_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            SelectedAnimations.Clear();
            SelectedAnimations.AddRange(((sender as ComboBoxEdit).SelectedItems).Cast<Animation>());

            ClearSelectedSalesAreas();
            ClearSelectedCustomerGroups();
            ClearSelectedSalesEmployees();
            ClearSelectedItemGroups();
            ClearSelectedProductNames();
        }

        #endregion

        #region SalesArea

        void salesAreaEditor_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            SelectedSalesAreas.Clear();
            SelectedSalesAreas.AddRange(((sender as ComboBoxEdit).SelectedItems).Cast<SalesArea>());

            ClearSelectedCustomerGroups();
            ClearSelectedSalesEmployees();
            ClearSelectedItemGroups();
            ClearSelectedProductNames();
        }

        #endregion

        #region CustomerGroup

        void customerGroupEditor_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            SelectedCustomerGroups.Clear();
            SelectedCustomerGroups.AddRange(((sender as ComboBoxEdit).SelectedItems).Cast<CustomerGroup>());

            ClearSelectedSalesEmployees();
            ClearSelectedItemGroups();
            ClearSelectedProductNames();
        }

        #endregion

        #region ItemGroup

        void itemGroupCombo_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            SelectedItemGroups.Clear();
            SelectedItemGroups.AddRange(((sender as ComboBoxEdit).SelectedItems).Cast<ItemGroup>());

            ClearSelectedProductNames();
        }

        #endregion

        #region ProductName

        void productNameCombo_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            SelectedProductNames.Clear();
            SelectedProductNames.AddRange(((sender as ComboBoxEdit).SelectedItems).Cast<Product>());
        }

        #endregion

        #endregion

        #region SelectedClearsMethod

        private void ClearSelectedAnimations()
        {
            if (animationEditor != null)
            {
                animationEditor.ItemsSource = AnimationSource;
                animationEditor.SelectedItems.Clear();
                animationEditor.ClearValue(ComboBoxEdit.SelectedItemProperty);
                SelectedAnimations.Clear();
            }
        }

        private void ClearSelectedSalesAreas()
        {
            if (salesAreaEditor != null)
            {
                salesAreaEditor.ItemsSource = SalesAreaSource;
                salesAreaEditor.SelectedItems.Clear();
                salesAreaEditor.ClearValue(ComboBoxEdit.SelectedItemProperty);
                SelectedSalesAreas.Clear();
            }
        }

        private void ClearSelectedCustomerGroups()
        {
            if (customerGroupEditor != null)
            {
                customerGroupEditor.ItemsSource = CustomerGroupSource;
                customerGroupEditor.SelectedItems.Clear();
                customerGroupEditor.ClearValue(ComboBoxEdit.SelectedItemProperty);
                SelectedCustomerGroups.Clear();
            }
        }

        private void ClearSelectedSalesEmployees()
        {
            if (salesEmployeesEditor != null)
            {
                salesEmployeesEditor.ItemsSource = SalesEmployeeSource;
                salesEmployeesEditor.SelectedItems.Clear();
                salesEmployeesEditor.ClearValue(ComboBoxEdit.SelectedItemProperty);
                SelectedSalesEmployees.Clear();
            }
        }

        private void ClearSelectedItemGroups()
        {
            if (itemGroupEditor != null)
            {
                itemGroupEditor.ItemsSource = ItemGroupSource;
                itemGroupEditor.SelectedItems.Clear();
                itemGroupEditor.ClearValue(ComboBoxEdit.SelectedItemProperty);
                SelectedItemGroups.Clear();
            }
        }

        private void ClearSelectedProductNames()
        {
            if (productNameEditor != null)
            {
                productNameEditor.ItemsSource = ProductNamesSource;
                productNameEditor.SelectedItems.Clear();
                productNameEditor.ClearValue(ComboBoxEdit.SelectedItemProperty);
                SelectedProductNames.Clear();
            }
        }

        #endregion

        protected override IReport CreateReport()
        {
            //return new GroupAllocationReportOld();
            return new GroupAllocationReport(UserDivision.ID);
        }

        #region ExportToCustomExcel

        private void Export()
        {

        }

        public static string RemoveSpecialCharacters(string str)
        {
            return Regex.Replace(str, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);
        }

        #endregion

        #endregion
    }
}
