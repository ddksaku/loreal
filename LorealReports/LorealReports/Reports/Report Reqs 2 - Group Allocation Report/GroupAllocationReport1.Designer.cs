namespace LorealReports.Reports
{
    partial class GroupAllocationReport1
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
            this.rep_GroupAllocationReportTableAdapter = new LorealReports.LorealDataSetTableAdapters.rep_GroupAllocationReportTableAdapter();
            this.lorealDataSet1 = new LorealReports.LorealDataSet();
            this.AnimName = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.CountDate = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.SapCode = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.ItemGroup = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.MatCode = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.Description = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.Barcode = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.Mult = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.xrPivotGridField1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField2 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField3 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            ((System.ComponentModel.ISupportInitialize)(this.lorealDataSet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPivotGrid1});
            this.Detail.HeightF = 105.2083F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrPivotGrid1
            // 
            this.xrPivotGrid1.DataAdapter = this.rep_GroupAllocationReportTableAdapter;
            this.xrPivotGrid1.DataMember = "rep_GroupAllocationReport";
            this.xrPivotGrid1.DataSource = this.lorealDataSet1;
            this.xrPivotGrid1.Fields.AddRange(new DevExpress.XtraPivotGrid.PivotGridField[] {
            this.AnimName,
            this.CountDate,
            this.SapCode,
            this.ItemGroup,
            this.MatCode,
            this.Description,
            this.Barcode,
            this.Mult,
            this.xrPivotGridField1,
            this.xrPivotGridField2,
            this.xrPivotGridField3});
            this.xrPivotGrid1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrPivotGrid1.Name = "xrPivotGrid1";
            this.xrPivotGrid1.OptionsChartDataSource.UpdateDelay = 300;
            this.xrPivotGrid1.OptionsDataField.AreaIndex = 0;
            this.xrPivotGrid1.OptionsView.ShowDataHeaders = false;
            this.xrPivotGrid1.OptionsView.ShowRowGrandTotalHeader = false;
            this.xrPivotGrid1.OptionsView.ShowRowGrandTotals = false;
            this.xrPivotGrid1.OptionsView.ShowRowTotals = false;
            this.xrPivotGrid1.SizeF = new System.Drawing.SizeF(1295.583F, 105.2083F);
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
            // AnimName
            // 
            this.AnimName.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea;
            this.AnimName.AreaIndex = 0;
            this.AnimName.Caption = "Animation Name";
            this.AnimName.FieldName = "AnimationName";
            this.AnimName.Name = "AnimName";
            this.AnimName.Width = 200;
            // 
            // CountDate
            // 
            this.CountDate.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea;
            this.CountDate.AreaIndex = 1;
            this.CountDate.CellFormat.FormatString = "dd/MM/yyyy";
            this.CountDate.CellFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.CountDate.FieldName = "OnCounterDate";
            this.CountDate.Name = "CountDate";
            // 
            // SapCode
            // 
            this.SapCode.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea;
            this.SapCode.AreaIndex = 2;
            this.SapCode.Caption = "SAP Animation Code";
            this.SapCode.FieldName = "SapAnimationCode";
            this.SapCode.Name = "SapCode";
            // 
            // ItemGroup
            // 
            this.ItemGroup.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea;
            this.ItemGroup.AreaIndex = 3;
            this.ItemGroup.Caption = "Item Group";
            this.ItemGroup.FieldName = "ItemGroupName";
            this.ItemGroup.Name = "ItemGroup";
            this.ItemGroup.Options.AllowFilter = DevExpress.Utils.DefaultBoolean.False;
            // 
            // MatCode
            // 
            this.MatCode.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea;
            this.MatCode.AreaIndex = 4;
            this.MatCode.Caption = "Material Code";
            this.MatCode.FieldName = "MaterialCode";
            this.MatCode.Name = "MatCode";
            this.MatCode.Options.AllowFilter = DevExpress.Utils.DefaultBoolean.False;
            // 
            // Description
            // 
            this.Description.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea;
            this.Description.AreaIndex = 5;
            this.Description.Caption = "Product Description";
            this.Description.FieldName = "ProductDescription";
            this.Description.Name = "Description";
            this.Description.Options.AllowFilter = DevExpress.Utils.DefaultBoolean.False;
            // 
            // Barcode
            // 
            this.Barcode.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea;
            this.Barcode.AreaIndex = 6;
            this.Barcode.Caption = "Ean Barcode";
            this.Barcode.FieldName = "EanBarcode";
            this.Barcode.Name = "Barcode";
            this.Barcode.Options.AllowFilter = DevExpress.Utils.DefaultBoolean.False;
            // 
            // Mult
            // 
            this.Mult.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea;
            this.Mult.AreaIndex = 7;
            this.Mult.Caption = "Multiple";
            this.Mult.FieldName = "Multiple";
            this.Mult.Name = "Mult";
            this.Mult.Options.AllowFilter = DevExpress.Utils.DefaultBoolean.False;
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
            this.BottomMargin.HeightF = 41.6667F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrPivotGridField1
            // 
            this.xrPivotGridField1.Area = DevExpress.XtraPivotGrid.PivotArea.ColumnArea;
            this.xrPivotGridField1.AreaIndex = 0;
            this.xrPivotGridField1.Name = "xrPivotGridField1";
            // 
            // xrPivotGridField2
            // 
            this.xrPivotGridField2.Area = DevExpress.XtraPivotGrid.PivotArea.ColumnArea;
            this.xrPivotGridField2.AreaIndex = 1;
            this.xrPivotGridField2.Name = "xrPivotGridField2";
            // 
            // xrPivotGridField3
            // 
            this.xrPivotGridField3.Area = DevExpress.XtraPivotGrid.PivotArea.ColumnArea;
            this.xrPivotGridField3.AreaIndex = 2;
            this.xrPivotGridField3.Name = "xrPivotGridField3";
            // 
            // GroupAllocationReport1
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin});
            this.Landscape = true;
            this.Margins = new System.Drawing.Printing.Margins(48, 100, 49, 42);
            this.PageHeight = 1169;
            this.PageWidth = 1654;
            this.PaperKind = System.Drawing.Printing.PaperKind.A3;
            this.Version = "11.1";
            ((System.ComponentModel.ISupportInitialize)(this.lorealDataSet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.XRPivotGrid xrPivotGrid1;
        private LorealDataSetTableAdapters.rep_GroupAllocationReportTableAdapter rep_GroupAllocationReportTableAdapter;
        private LorealDataSet lorealDataSet1;
        private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField AnimName;
        private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField ItemGroup;
        private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField MatCode;
        private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField Description;
        private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField Barcode;
        private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField Mult;
        private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField CountDate;
        private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField SapCode;
        private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField1;
        private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField2;
        private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField3;
    }
}
