namespace LorealReports.Reports
{
    partial class GroupAllocationReportOld
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
            this.xrPivot = new DevExpress.XtraReports.UI.XRPivotGrid();
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
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPivot});
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrPivot
            // 
            this.xrPivot.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrPivot.Name = "xrPivot";
            this.xrPivot.OptionsChartDataSource.UpdateDelay = 300;
            this.xrPivot.OptionsDataField.AreaIndex = 0;
            this.xrPivot.OptionsView.ShowColumnGrandTotals = false;
            this.xrPivot.OptionsView.ShowColumnTotals = false;
            this.xrPivot.OptionsView.ShowDataHeaders = false;
            this.xrPivot.OptionsView.ShowRowGrandTotals = false;
            this.xrPivot.OptionsView.ShowRowHeaders = false;
            this.xrPivot.OptionsView.ShowRowTotals = false;
            this.xrPivot.SizeF = new System.Drawing.SizeF(640F, 57.29167F);
            this.xrPivot.FieldValueDisplayText += new DevExpress.XtraPivotGrid.PivotFieldDisplayTextEventHandler(this.xrPivot_FieldValueDisplayText);
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
            // GroupAllocationReportOld
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin});
            this.Landscape = true;
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
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.XRPivotGrid xrPivot;
        private DevExpress.XtraReports.Parameters.Parameter SalesDrive;
        private DevExpress.XtraReports.Parameters.Parameter Animations;
        private DevExpress.XtraReports.Parameters.Parameter SalesArea;
        private DevExpress.XtraReports.Parameters.Parameter CustomerGroups;
        private DevExpress.XtraReports.Parameters.Parameter HideCustWith0Alloc;
        private DevExpress.XtraReports.Parameters.Parameter SalesEmployees;
        private DevExpress.XtraReports.Parameters.Parameter ItemGroup;
        private DevExpress.XtraReports.Parameters.Parameter HideProdWith0Alloc;
        private DevExpress.XtraReports.Parameters.Parameter ProductName;
    }
}
