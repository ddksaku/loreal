namespace LorealReports.Reports
{
    partial class XtraReport1
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
            this.rep_GroupAllocationReportTestTableAdapter = new LorealReports.DataAccess.LorealDataSetTableAdapters.rep_GroupAllocationReportTestTableAdapter();
            this.lorealDataSet1 = new LorealReports.DataAccess.LorealDataSet();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
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
            this.fieldListPrice1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            ((System.ComponentModel.ISupportInitialize)(this.lorealDataSet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPivotGrid1});
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrPivotGrid1
            // 
            this.xrPivotGrid1.DataAdapter = this.rep_GroupAllocationReportTestTableAdapter;
            this.xrPivotGrid1.DataMember = "rep_GroupAllocationReportTest";
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
            this.fieldListPrice1});
            this.xrPivotGrid1.LocationFloat = new DevExpress.Utils.PointFloat(9.999974F, 0F);
            this.xrPivotGrid1.Name = "xrPivotGrid1";
            this.xrPivotGrid1.OptionsChartDataSource.UpdateDelay = 300;
            this.xrPivotGrid1.SizeF = new System.Drawing.SizeF(2119F, 100F);
            // 
            // rep_GroupAllocationReportTestTableAdapter
            // 
            this.rep_GroupAllocationReportTestTableAdapter.ClearBeforeFill = true;
            // 
            // lorealDataSet1
            // 
            this.lorealDataSet1.DataSetName = "LorealDataSet";
            this.lorealDataSet1.EnforceConstraints = false;
            this.lorealDataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // TopMargin
            // 
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
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
            // fieldListPrice1
            // 
            this.fieldListPrice1.Area = DevExpress.XtraPivotGrid.PivotArea.DataArea;
            this.fieldListPrice1.AreaIndex = 2;
            this.fieldListPrice1.Caption = "List Price";
            this.fieldListPrice1.FieldName = "ListPrice";
            this.fieldListPrice1.Name = "fieldListPrice1";
            // 
            // XtraReport1
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin});
            this.Landscape = true;
            this.PageHeight = 1654;
            this.PageWidth = 2339;
            this.PaperKind = System.Drawing.Printing.PaperKind.A2;
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
        private DataAccess.LorealDataSetTableAdapters.rep_GroupAllocationReportTestTableAdapter rep_GroupAllocationReportTestTableAdapter;
        private DataAccess.LorealDataSet lorealDataSet1;
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
        private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fieldListPrice1;
    }
}
