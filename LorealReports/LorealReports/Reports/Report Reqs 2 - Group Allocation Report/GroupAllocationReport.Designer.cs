namespace LorealReports.Reports
{
    partial class GroupAllocationReport
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrPivotGrid1 = new DevExpress.XtraReports.UI.XRPivotGrid();
            this.rep_GroupAllocationReportTableAdapter = new LorealReports.DataAccess.LorealDataSetTableAdapters.rep_GroupAllocationReportTableAdapter();
            this.lorealDataSet1 = new LorealReports.DataAccess.LorealDataSet();
            this.fieldAnimationName1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.fieldOnCounterDate1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.fieldSapAnimationCode1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.fieldItemGroupName1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.fieldMaterialCode1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.fieldProductDescription1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.fieldEanBarcode1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.fieldMultiple1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.fieldSalesAreaName1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.fieldSignOffStatus1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.fieldRRP1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.fieldLISTPRICE1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.fieldAvailableAllocation1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.fieldSalesDriveName1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.fieldDivisionName1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.fieldReportTitleText1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.fieldFooterComment1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.fieldRunDateTime1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.fieldRRP12 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.fieldLISTPRICE12 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.fieldCurrencySymbol1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.SalesDrive = new DevExpress.XtraReports.Parameters.Parameter();
            this.Animations = new DevExpress.XtraReports.Parameters.Parameter();
            this.SalesArea = new DevExpress.XtraReports.Parameters.Parameter();
            this.CustomerGroups = new DevExpress.XtraReports.Parameters.Parameter();
            this.HideCustWith0Alloc = new DevExpress.XtraReports.Parameters.Parameter();
            this.SalesEmployees = new DevExpress.XtraReports.Parameters.Parameter();
            this.ItemGroup = new DevExpress.XtraReports.Parameters.Parameter();
            this.HideProdWith0Alloc = new DevExpress.XtraReports.Parameters.Parameter();
            this.ProductName = new DevExpress.XtraReports.Parameters.Parameter();
            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.ReportFooter = new DevExpress.XtraReports.UI.ReportFooterBand();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            ((System.ComponentModel.ISupportInitialize)(this.lorealDataSet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPivotGrid1});
            this.Detail.HeightF = 78.125F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrPivotGrid1
            // 
            this.xrPivotGrid1.Appearance.Cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrPivotGrid1.Appearance.CustomTotalCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrPivotGrid1.Appearance.FieldHeader.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrPivotGrid1.Appearance.FieldValue.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrPivotGrid1.Appearance.FieldValueGrandTotal.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrPivotGrid1.Appearance.FieldValueTotal.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrPivotGrid1.Appearance.FilterSeparator.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrPivotGrid1.Appearance.GrandTotalCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrPivotGrid1.Appearance.HeaderGroupLine.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrPivotGrid1.Appearance.Lines.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrPivotGrid1.Appearance.TotalCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrPivotGrid1.DataAdapter = this.rep_GroupAllocationReportTableAdapter;
            this.xrPivotGrid1.DataMember = "rep_GroupAllocationReport";
            this.xrPivotGrid1.DataSource = this.lorealDataSet1;
            this.xrPivotGrid1.Fields.AddRange(new DevExpress.XtraPivotGrid.PivotGridField[] {
            this.fieldAnimationName1,
            this.fieldOnCounterDate1,
            this.fieldSapAnimationCode1,
            this.fieldItemGroupName1,
            this.fieldMaterialCode1,
            this.fieldProductDescription1,
            this.fieldEanBarcode1,
            this.fieldMultiple1,
            this.fieldSalesAreaName1,
            this.fieldSignOffStatus1,
            this.fieldRRP1,
            this.fieldLISTPRICE1,
            this.fieldAvailableAllocation1,
            this.fieldSalesDriveName1,
            this.fieldDivisionName1,
            this.fieldReportTitleText1,
            this.fieldFooterComment1,
            this.fieldRunDateTime1,
            this.fieldRRP12,
            this.fieldLISTPRICE12,
            this.fieldCurrencySymbol1});
            this.xrPivotGrid1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrPivotGrid1.Name = "xrPivotGrid1";
            this.xrPivotGrid1.OptionsChartDataSource.UpdateDelay = 300;
            this.xrPivotGrid1.OptionsPrint.PrintColumnHeaders = DevExpress.Utils.DefaultBoolean.False;
            this.xrPivotGrid1.OptionsPrint.PrintFilterHeaders = DevExpress.Utils.DefaultBoolean.False;
            this.xrPivotGrid1.OptionsPrint.PrintHeadersOnEveryPage = true;
            this.xrPivotGrid1.OptionsPrint.PrintRowHeaders = DevExpress.Utils.DefaultBoolean.True;
            this.xrPivotGrid1.OptionsView.ShowDataHeaders = false;
            this.xrPivotGrid1.OptionsView.ShowFilterHeaders = false;
            this.xrPivotGrid1.OptionsView.ShowFilterSeparatorBar = false;
            this.xrPivotGrid1.OptionsView.ShowRowGrandTotalHeader = false;
            this.xrPivotGrid1.OptionsView.ShowRowGrandTotals = false;
            this.xrPivotGrid1.OptionsView.ShowRowTotals = false;
            this.xrPivotGrid1.SizeF = new System.Drawing.SizeF(1073F, 78.125F);
            // 
            // rep_GroupAllocationReportTableAdapter
            // 
            this.rep_GroupAllocationReportTableAdapter.ClearBeforeFill = true;
            // 
            // lorealDataSet1
            // 
            this.lorealDataSet1.DataSetName = "LorealDataSet";
            this.lorealDataSet1.EnforceConstraints = false;
            this.lorealDataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // fieldAnimationName1
            // 
            this.fieldAnimationName1.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea;
            this.fieldAnimationName1.AreaIndex = 0;
            this.fieldAnimationName1.Caption = "Animation Name";
            this.fieldAnimationName1.FieldName = "AnimationName";
            this.fieldAnimationName1.Name = "fieldAnimationName1";
            // 
            // fieldOnCounterDate1
            // 
            this.fieldOnCounterDate1.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea;
            this.fieldOnCounterDate1.AreaIndex = 1;
            this.fieldOnCounterDate1.Caption = "On Counter Date";
            this.fieldOnCounterDate1.FieldName = "OnCounterDate";
            this.fieldOnCounterDate1.Name = "fieldOnCounterDate1";
            // 
            // fieldSapAnimationCode1
            // 
            this.fieldSapAnimationCode1.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea;
            this.fieldSapAnimationCode1.AreaIndex = 2;
            this.fieldSapAnimationCode1.Caption = "Sap Animation Code";
            this.fieldSapAnimationCode1.FieldName = "SapAnimationCode";
            this.fieldSapAnimationCode1.Name = "fieldSapAnimationCode1";
            // 
            // fieldItemGroupName1
            // 
            this.fieldItemGroupName1.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea;
            this.fieldItemGroupName1.AreaIndex = 3;
            this.fieldItemGroupName1.Caption = "Item Group Name";
            this.fieldItemGroupName1.FieldName = "ItemGroupName";
            this.fieldItemGroupName1.Name = "fieldItemGroupName1";
            // 
            // fieldMaterialCode1
            // 
            this.fieldMaterialCode1.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea;
            this.fieldMaterialCode1.AreaIndex = 4;
            this.fieldMaterialCode1.Caption = "Material Code";
            this.fieldMaterialCode1.FieldName = "MaterialCode";
            this.fieldMaterialCode1.Name = "fieldMaterialCode1";
            // 
            // fieldProductDescription1
            // 
            this.fieldProductDescription1.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea;
            this.fieldProductDescription1.AreaIndex = 5;
            this.fieldProductDescription1.Caption = "Product Description";
            this.fieldProductDescription1.FieldName = "ProductDescription";
            this.fieldProductDescription1.Name = "fieldProductDescription1";
            // 
            // fieldEanBarcode1
            // 
            this.fieldEanBarcode1.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea;
            this.fieldEanBarcode1.AreaIndex = 6;
            this.fieldEanBarcode1.Caption = "Ean Barcode";
            this.fieldEanBarcode1.FieldName = "EanBarcode";
            this.fieldEanBarcode1.Name = "fieldEanBarcode1";
            // 
            // fieldMultiple1
            // 
            this.fieldMultiple1.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea;
            this.fieldMultiple1.AreaIndex = 7;
            this.fieldMultiple1.Caption = "Multiple";
            this.fieldMultiple1.FieldName = "Multiple";
            this.fieldMultiple1.Name = "fieldMultiple1";
            // 
            // fieldSalesAreaName1
            // 
            this.fieldSalesAreaName1.Area = DevExpress.XtraPivotGrid.PivotArea.ColumnArea;
            this.fieldSalesAreaName1.AreaIndex = 0;
            this.fieldSalesAreaName1.Caption = "Sales Area Name";
            this.fieldSalesAreaName1.FieldName = "SalesAreaName";
            this.fieldSalesAreaName1.Name = "fieldSalesAreaName1";
            // 
            // fieldSignOffStatus1
            // 
            this.fieldSignOffStatus1.Area = DevExpress.XtraPivotGrid.PivotArea.DataArea;
            this.fieldSignOffStatus1.AreaIndex = 0;
            this.fieldSignOffStatus1.Caption = "Sign Off Status";
            this.fieldSignOffStatus1.FieldName = "SignOffStatus";
            this.fieldSignOffStatus1.Name = "fieldSignOffStatus1";
            // 
            // fieldRRP1
            // 
            this.fieldRRP1.Area = DevExpress.XtraPivotGrid.PivotArea.DataArea;
            this.fieldRRP1.AreaIndex = 1;
            this.fieldRRP1.Caption = "RRP";
            this.fieldRRP1.FieldName = "RRP";
            this.fieldRRP1.Name = "fieldRRP1";
            // 
            // fieldLISTPRICE1
            // 
            this.fieldLISTPRICE1.Area = DevExpress.XtraPivotGrid.PivotArea.DataArea;
            this.fieldLISTPRICE1.AreaIndex = 2;
            this.fieldLISTPRICE1.Caption = "LIST PRICE";
            this.fieldLISTPRICE1.FieldName = "LIST PRICE";
            this.fieldLISTPRICE1.Name = "fieldLISTPRICE1";
            // 
            // fieldAvailableAllocation1
            // 
            this.fieldAvailableAllocation1.Area = DevExpress.XtraPivotGrid.PivotArea.DataArea;
            this.fieldAvailableAllocation1.AreaIndex = 3;
            this.fieldAvailableAllocation1.Caption = "Available Allocation";
            this.fieldAvailableAllocation1.FieldName = "Available Allocation";
            this.fieldAvailableAllocation1.Name = "fieldAvailableAllocation1";
            // 
            // fieldSalesDriveName1
            // 
            this.fieldSalesDriveName1.AreaIndex = 0;
            this.fieldSalesDriveName1.Caption = "Sales Drive Name";
            this.fieldSalesDriveName1.FieldName = "SalesDriveName";
            this.fieldSalesDriveName1.Name = "fieldSalesDriveName1";
            // 
            // fieldDivisionName1
            // 
            this.fieldDivisionName1.AreaIndex = 1;
            this.fieldDivisionName1.Caption = "Division Name";
            this.fieldDivisionName1.FieldName = "DivisionName";
            this.fieldDivisionName1.Name = "fieldDivisionName1";
            // 
            // fieldReportTitleText1
            // 
            this.fieldReportTitleText1.AreaIndex = 2;
            this.fieldReportTitleText1.Caption = "Report Title Text";
            this.fieldReportTitleText1.FieldName = "ReportTitleText";
            this.fieldReportTitleText1.Name = "fieldReportTitleText1";
            // 
            // fieldFooterComment1
            // 
            this.fieldFooterComment1.AreaIndex = 3;
            this.fieldFooterComment1.Caption = "Footer Comment";
            this.fieldFooterComment1.FieldName = "FooterComment";
            this.fieldFooterComment1.Name = "fieldFooterComment1";
            // 
            // fieldRunDateTime1
            // 
            this.fieldRunDateTime1.AreaIndex = 4;
            this.fieldRunDateTime1.Caption = "Run Date Time";
            this.fieldRunDateTime1.FieldName = "RunDateTime";
            this.fieldRunDateTime1.Name = "fieldRunDateTime1";
            // 
            // fieldRRP12
            // 
            this.fieldRRP12.AreaIndex = 5;
            this.fieldRRP12.Caption = "RRP";
            this.fieldRRP12.FieldName = "RRP1";
            this.fieldRRP12.Name = "fieldRRP12";
            // 
            // fieldLISTPRICE12
            // 
            this.fieldLISTPRICE12.AreaIndex = 6;
            this.fieldLISTPRICE12.Caption = "LIST PRICE";
            this.fieldLISTPRICE12.FieldName = "LIST PRICE1";
            this.fieldLISTPRICE12.Name = "fieldLISTPRICE12";
            // 
            // fieldCurrencySymbol1
            // 
            this.fieldCurrencySymbol1.AreaIndex = 7;
            this.fieldCurrencySymbol1.Caption = "Currency Symbol";
            this.fieldCurrencySymbol1.FieldName = "CurrencySymbol";
            this.fieldCurrencySymbol1.Name = "fieldCurrencySymbol1";
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 48.95833F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 48.95833F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // SalesDrive
            // 
            this.SalesDrive.Description = "Sales Drive";
            this.SalesDrive.Name = "SalesDrive";
            // 
            // Animations
            // 
            this.Animations.Name = "Animations";
            // 
            // SalesArea
            // 
            this.SalesArea.Name = "SalesArea";
            // 
            // CustomerGroups
            // 
            this.CustomerGroups.Name = "CustomerGroups";
            // 
            // HideCustWith0Alloc
            // 
            this.HideCustWith0Alloc.Name = "HideCustWith0Alloc";
            this.HideCustWith0Alloc.Type = typeof(bool);
            // 
            // SalesEmployees
            // 
            this.SalesEmployees.Name = "SalesEmployees";
            // 
            // ItemGroup
            // 
            this.ItemGroup.Name = "ItemGroup";
            // 
            // HideProdWith0Alloc
            // 
            this.HideProdWith0Alloc.Name = "HideProdWith0Alloc";
            this.HideProdWith0Alloc.Type = typeof(bool);
            // 
            // ProductName
            // 
            this.ProductName.Name = "ProductName";
            // 
            // ReportHeader
            // 
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel4,
            this.xrLabel1});
            this.ReportHeader.HeightF = 88.54166F;
            this.ReportHeader.Name = "ReportHeader";
            // 
            // xrLabel4
            // 
            this.xrLabel4.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", this.lorealDataSet1, "rep_GroupAllocationReport.ReportTitleText")});
            this.xrLabel4.Font = new System.Drawing.Font("Times New Roman", 14F, System.Drawing.FontStyle.Bold);
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(617.7083F, 23F);
            this.xrLabel4.StylePriority.UseFont = false;
            this.xrLabel4.Text = "xrLabel4";
            // 
            // xrLabel1
            // 
            this.xrLabel1.Font = new System.Drawing.Font("Times New Roman", 14F, System.Drawing.FontStyle.Bold);
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 37.45832F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(415.625F, 23F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.Text = "CUSTOMER GROUP ALLOCATION REPORT";
            // 
            // ReportFooter
            // 
            this.ReportFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel3,
            this.xrLabel2});
            this.ReportFooter.HeightF = 71.25001F;
            this.ReportFooter.Name = "ReportFooter";
            // 
            // xrLabel3
            // 
            this.xrLabel3.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", this.lorealDataSet1, "rep_GroupAllocationReport.RunDateTime")});
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(973F, 48.25001F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrLabel3.Text = "xrLabel3";
            // 
            // xrLabel2
            // 
            this.xrLabel2.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", this.lorealDataSet1, "rep_GroupAllocationReport.FooterComment")});
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(910.4167F, 61.25002F);
            this.xrLabel2.Text = "xrLabel2";
            // 
            // GroupAllocationReport
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader,
            this.ReportFooter});
            this.Landscape = true;
            this.Margins = new System.Drawing.Printing.Margins(50, 46, 49, 49);
            this.PageHeight = 827;
            this.PageWidth = 1169;
            this.PaperKind = System.Drawing.Printing.PaperKind.A4;
            this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.SalesDrive,
            this.Animations,
            this.SalesArea,
            this.CustomerGroups,
            this.HideCustWith0Alloc,
            this.SalesEmployees,
            this.ItemGroup,
            this.HideProdWith0Alloc,
            this.ProductName});
            this.Version = "11.1";
            this.VerticalContentSplitting = DevExpress.XtraPrinting.VerticalContentSplitting.Smart;
            ((System.ComponentModel.ISupportInitialize)(this.lorealDataSet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.XRPivotGrid xrPivotGrid1;
        private DataAccess.LorealDataSetTableAdapters.rep_GroupAllocationReportTableAdapter rep_GroupAllocationReportTableAdapter;
        private DataAccess.LorealDataSet lorealDataSet1;
        private DevExpress.XtraReports.Parameters.Parameter SalesDrive;
        private DevExpress.XtraReports.Parameters.Parameter Animations;
        private DevExpress.XtraReports.Parameters.Parameter SalesArea;
        private DevExpress.XtraReports.Parameters.Parameter CustomerGroups;
        private DevExpress.XtraReports.Parameters.Parameter HideCustWith0Alloc;
        private DevExpress.XtraReports.Parameters.Parameter SalesEmployees;
        private DevExpress.XtraReports.Parameters.Parameter ItemGroup;
        private DevExpress.XtraReports.Parameters.Parameter HideProdWith0Alloc;
        private DevExpress.XtraReports.Parameters.Parameter ProductName;
        private DevExpress.XtraReports.UI.ReportHeaderBand ReportHeader;
        private DevExpress.XtraReports.UI.XRLabel xrLabel1;
        private DevExpress.XtraReports.UI.XRLabel xrLabel4;
        private DevExpress.XtraReports.UI.ReportFooterBand ReportFooter;
        private DevExpress.XtraReports.UI.XRLabel xrLabel3;
        private DevExpress.XtraReports.UI.XRLabel xrLabel2;
        private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fieldAnimationName1;
        private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fieldOnCounterDate1;
        private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fieldSapAnimationCode1;
        private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fieldItemGroupName1;
        private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fieldMaterialCode1;
        private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fieldProductDescription1;
        private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fieldEanBarcode1;
        private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fieldMultiple1;
        private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fieldSalesAreaName1;
        private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fieldSignOffStatus1;
        private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fieldRRP1;
        private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fieldLISTPRICE1;
        private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fieldAvailableAllocation1;
        private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fieldSalesDriveName1;
        private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fieldDivisionName1;
        private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fieldReportTitleText1;
        private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fieldFooterComment1;
        private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fieldRunDateTime1;
        private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fieldRRP12;
        private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fieldLISTPRICE12;
        private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fieldCurrencySymbol1;
    }
}
