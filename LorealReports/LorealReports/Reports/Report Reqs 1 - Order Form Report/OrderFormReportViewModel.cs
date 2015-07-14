using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using DevExpress.Xpf.Core.Commands;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Printing;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports;
using LorealReports.DataAccess;
using LorealReports.Reports.Base;
using LorealReports.Reports.ValueConverters;
using Microsoft.Office.Interop.Excel;

namespace LorealReports.Reports
{
    public class OrderFormReportViewModel : BaseViewModel
    {
        #region Properties and Members

        public List<SalesDrive> SalesDrives { get; set; }
        public List<Animation> Animations { get; set; }
        public List<SalesArea> SalesAreas { get; set; }
        public List<ItemGroup> ItemGroups { get; set; }

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
                    return db.Animation.Where(x => x.IDSalesDrive == SelectedSalesDrive.ID).ToList();
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
                    List<Guid> selectedAnimationGuids = new List<Guid>();
                    selectedAnimationGuids.AddRange(SelectedAnimations.Select(x => x.ID));

                    var query = from itemGroup in db.ItemGroup
                                where (from animationProduct in db.AnimationProduct
                                       where (selectedAnimationGuids).Contains(animationProduct.IDAnimation)
                                       select animationProduct.IDItemGroup).Contains(itemGroup.ID)
                                select itemGroup;

                    return query.ToList();
                }
            }
        }

        #endregion

        #region SalesArea

        public SalesArea SelectedSalesArea { get; set; }

        public List<SalesArea> SalesAreaSource
        {
            get
            {
                var query = from salesArea in db.SalesArea
                            select salesArea;

                return query.ToList();
            }
        }

        #endregion

        private ComboBoxEdit animationEditor;
        private ComboBoxEdit itemGroupEditor;
        private ComboBoxEdit salesAreaEditor;

        #endregion

        #region Constructor

        public OrderFormReportViewModel()
            : base(new Guid("0DB8788D-59DE-4FC3-A65D-DF704500E9F3"))
        {
            ExportToCustomExcel = new DelegateCommand<object>(ExecuteExportCommand, CanExecuteExportCommand);

            LoadComboboxSources();

            SelectedAnimations = new List<Animation>();
            SelectedItemGroups = new List<ItemGroup>();

            ReportPreviewModel.CustomizeParameterEditors += CustomizeParameterEditors;

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

        void CustomizeParameterEditors(object sender, CustomizeParameterEditorsEventArgs e)
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
                    IsTextEditable = false
                };

                combo.Validate += new DevExpress.Xpf.Editors.Validation.ValidateEventHandler(combo_Validate);
                e.Editor = combo;
                salesAreaEditor = (ComboBoxEdit)combo;
                salesAreaEditor.SelectedIndexChanged += new RoutedEventHandler(salesAreaEditor_SelectedIndexChanged);
                e.BoundDataMember = "EditValue";
                e.BoundDataConverter = new SingleSelectedConverter<SalesArea>();
                e.BoundDataConverterParameter = SalesAreas;
            }
            #endregion

            #region ItemGroups

            if (e.Parameter.Name == "ItemGroups")
            {
                var itemGroupCombo = new ComboBoxEdit()
                {
                    DisplayMember = "Name",
                    ItemsSource = ItemGroupSource,
                    IsTextEditable = false,
                    SeparatorString = ",",
                    StyleSettings = new CheckedComboBoxStyleSettings()
                };
                itemGroupCombo.Validate += new DevExpress.Xpf.Editors.Validation.ValidateEventHandler(itemGroupCombo_Validate);
                e.Editor = itemGroupCombo;
                itemGroupCombo.SelectedIndexChanged += new RoutedEventHandler(itemGroupCombo_SelectedIndexChanged);
                itemGroupEditor = itemGroupCombo;
                e.BoundDataMember = "EditValue";
                e.BoundDataConverter = new MultiSelectedConverter<ItemGroup>();
                e.BoundDataConverterParameter = ItemGroups;
            }

            #endregion
        }

        #region Validations

        void itemGroupCombo_Validate(object sender, ValidationEventArgs e)
        {
            IEnumerable<object> value = (IEnumerable<object>)e.Value;

            if (value != null && value.Count() > 0)
            {
                e.IsValid = true;
            }
            else
            {
                e.IsValid = false;
            }

            e.ErrorContent = "Must select at least one item";
        }

        void combo_Validate(object sender, ValidationEventArgs e)
        {
            SalesArea value = (SalesArea)e.Value;

            e.IsValid = value != null;

            e.ErrorContent = "Sales Area is Mandatory field";
        }

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

        #endregion

        #region Event Handlers

        void salesDriveCombo_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            SelectedSalesDrive = (SalesDrive)(sender as ComboBoxEdit).SelectedItem;

            ClearSelectedAnimations();
        }

        void animationCombo_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            SelectedAnimations.Clear();
            SelectedAnimations.AddRange(((sender as ComboBoxEdit).SelectedItems).Cast<Animation>());

            ClearSelectedItemGroups();
        }

        void salesAreaEditor_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            SelectedSalesArea = (SalesArea)(sender as ComboBoxEdit).SelectedItem;
        }

        void itemGroupCombo_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            SelectedItemGroups.Clear();
            SelectedItemGroups.AddRange(((sender as ComboBoxEdit).SelectedItems).Cast<ItemGroup>());
        }



        #endregion

        private void LoadComboboxSources()
        {
            SalesDrives = db.SalesDrive.ToList();
            Animations = db.Animation.ToList();
            SalesAreas = db.SalesArea.ToList();
            ItemGroups = db.ItemGroup.ToList();
        }

        protected override IReport CreateReport()
        {
            return new OrderFormReport();
        }

        #region ExportToCustomExcel

        private void Export()
        {
            string fileName = "Excel.xls";
            var rep = (ReportPreviewModel.Report as OrderFormReport);
            rep.ExportToXls(fileName, new XlsExportOptions());

            Microsoft.Office.Interop.Excel.Application ObjExcel = new Microsoft.Office.Interop.Excel.Application();
            Workbook ObjWorkBookGeneral;
            ObjWorkBookGeneral = ObjExcel.Workbooks.Open(Environment.CurrentDirectory + "\\" + fileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);


            if (File.Exists(Environment.CurrentDirectory + "\\" + fileName))
                File.Copy(Environment.CurrentDirectory + "\\" + fileName, Environment.CurrentDirectory + "\\Result_" + fileName, true);

            Workbook ObjWorkBookResult = ObjExcel.Workbooks.Open(Environment.CurrentDirectory + "\\Result_" + fileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            (ObjWorkBookResult.ActiveSheet as Worksheet).UsedRange.Clear();

            try
            {
                Worksheet sheet = ObjWorkBookGeneral.ActiveSheet;

                if (sheet != null)
                {
                    Range range = sheet.UsedRange;
                    if (range != null)
                    {
                        int nRows = range.Rows.Count;
                        int nCols = range.Columns.Count;

                        Range tempRange;

                        Range startCell = null;
                        Range endCell = null;
                        string animationName = string.Empty;

                        int currentSheetIndex = 0;
                        int k = 0;
                        for (int i = 1; i <= nRows + 1; i++)
                        {
                            string d = range[i, 1].value;
                            if (d == "xrStart" || i == nRows + 1)
                            {
                                k = 1;
                                if (startCell != null || i == nRows + 1)
                                {
                                    endCell = range.Cells[i - 1, nCols];
                                    tempRange = range.Range[startCell, endCell];


                                    currentSheetIndex = currentSheetIndex + 1;

                                    ((Worksheet)ObjWorkBookResult.Sheets[currentSheetIndex]).Select(Type.Missing);


                                    ObjWorkBookResult.ActiveSheet.Name = animationName;
                                    tempRange.Copy(Type.Missing);
                                    ObjWorkBookResult.ActiveSheet.PasteSpecial(XlPasteType.xlPasteColumnWidths);
                                    ObjWorkBookResult.ActiveSheet.PasteSpecial(XlPasteType.xlPasteAllUsingSourceTheme);

                                    (ObjWorkBookResult.ActiveSheet as Worksheet).PageSetup.Orientation = XlPageOrientation.xlLandscape;
                                    (ObjWorkBookResult.ActiveSheet as Worksheet).PageSetup.TopMargin = ObjExcel.CentimetersToPoints(1);
                                    (ObjWorkBookResult.ActiveSheet as Worksheet).PageSetup.BottomMargin = ObjExcel.CentimetersToPoints(1);
                                    (ObjWorkBookResult.ActiveSheet as Worksheet).PageSetup.HeaderMargin = 0;
                                    (ObjWorkBookResult.ActiveSheet as Worksheet).PageSetup.FooterMargin = 0;
                                    (ObjWorkBookResult.ActiveSheet as Worksheet).PageSetup.LeftMargin = ObjExcel.CentimetersToPoints(1);
                                    (ObjWorkBookResult.ActiveSheet as Worksheet).PageSetup.RightMargin = ObjExcel.CentimetersToPoints(1);


                                    startCell = range.Cells[i, 1];
                                    if (range.Cells[i + 1, 1].value != null)
                                    {
                                        animationName = range.Cells[i + 1, 1].value;
                                        animationName = RemoveSpecialCharacters(animationName);
                                        if (animationName.Length > 30)
                                            animationName = animationName.Substring(0, 30);

                                        ObjWorkBookResult.Sheets.Add(Type.Missing, ObjWorkBookResult.ActiveSheet,
                                                                     Type.Missing, Type.Missing);
                                    }
                                }
                                else
                                {
                                    startCell = range.Cells[i, 1];
                                    animationName = range.Cells[i + 1, 1].value;
                                    animationName = RemoveSpecialCharacters(animationName);
                                    if (animationName.Length > 30)
                                        animationName = animationName.Substring(0, 30);
                                }
                            }

                            Worksheet sh = (ObjWorkBookResult.ActiveSheet as Worksheet);
                            (sh.Rows[k] as Range).RowHeight = (range.Rows[i] as Range).RowHeight;
                            k++;

                        }
                    }
                }
            }
            finally
            {
                ObjWorkBookResult.Close(Microsoft.Office.Interop.Excel.XlSaveAction.xlSaveChanges, Type.Missing, Type.Missing);
                ObjWorkBookGeneral.Close(Microsoft.Office.Interop.Excel.XlSaveAction.xlSaveChanges, Type.Missing, Type.Missing);
                ObjExcel.Quit();
                File.Delete(fileName);
                File.Copy("Result_" + fileName, fileName);
                File.Delete("Result_" + fileName);
            }

            if (File.Exists(fileName))
                Process.Start(fileName);
        }

        public static string RemoveSpecialCharacters(string str)
        {
            return Regex.Replace(str, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);
        }

        #endregion

        #endregion
    }
}
