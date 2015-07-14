using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace LorealReports.Reports
{
    public partial class GroupAllocationReport : DevExpress.XtraReports.UI.XtraReport
    {
        public Guid UserDivision { get; set; }

        public string ParameterSalesDrive { get; set; }
        public string ParameterAnimation { get; set; }
        public string ParameterSalesArea { get; set; }
        public string ParameterCustomerGroup { get; set; }
        public bool ParameterHideCustomerWith0Alloc { get; set; }
        public string ParameterSalesEmployee { get; set; }
        public string ParameterItemGroup { get; set; }
        public bool ParameterHideProductWith0Alloc { get; set; }
        public string ParameterProductName { get; set; }

        public GroupAllocationReport(Guid userDivision)
        {
            InitializeComponent();
            UserDivision = userDivision;
            BeforePrint += XtraReport1_BeforePrint;
            ParametersRequestSubmit += GroupAllocationReport_ParametersRequestSubmit;

            xrPivotGrid1.BestFit(fieldAnimationName1);
            xrPivotGrid1.BestFit(fieldOnCounterDate1);
            xrPivotGrid1.BestFit(fieldSapAnimationCode1);
            xrPivotGrid1.BestFit(fieldSapAnimationCode1);
            xrPivotGrid1.BestFit(fieldItemGroupName1);
            xrPivotGrid1.BestFit(fieldMaterialCode1);
            xrPivotGrid1.BestFit(fieldProductDescription1);
            xrPivotGrid1.BestFit(fieldEanBarcode1);
            xrPivotGrid1.BestFit(fieldMultiple1);
        }

        void GroupAllocationReport_ParametersRequestSubmit(object sender, DevExpress.XtraReports.Parameters.ParametersRequestEventArgs e)
        {
            #region SalesDrive

            string paramSalesDrive = (string)e.ParametersInformation[0].Parameter.Value;

            #endregion

            #region Animation

            string paramAnimation = string.Empty;
            string collAnim = (string)e.ParametersInformation[1].Parameter.Value;
            if (!string.IsNullOrEmpty(collAnim))
            {
                string[] list1 = collAnim.Split(',');
                foreach (string s in list1)
                {
                    paramAnimation = string.IsNullOrEmpty(paramAnimation) ? s : paramAnimation + "," + s;
                }
            }

            #endregion

            #region SalesArea

            string paramSalesArea = string.Empty;
            string collSalesArea = (string)e.ParametersInformation[2].Parameter.Value;
            if (!string.IsNullOrEmpty(collSalesArea))
            {
                string[] list1 = collSalesArea.Split(',');
                foreach (string s in list1)
                {
                    paramSalesArea = string.IsNullOrEmpty(paramSalesArea) ? s : paramSalesArea + "," + s;
                }
            }

            #endregion

            #region CustomerGroup

            string paramCustomerGroup = string.Empty;
            string collCustomerGroup = (string)e.ParametersInformation[3].Parameter.Value;
            if (!string.IsNullOrEmpty(collCustomerGroup))
            {
                string[] list1 = collCustomerGroup.Split(',');
                foreach (string s in list1)
                {
                    paramCustomerGroup = string.IsNullOrEmpty(paramCustomerGroup) ? s : paramCustomerGroup + "," + s;
                }
            }

            #endregion

            #region ParameterHideCustomerWith0Alloc

            ParameterHideCustomerWith0Alloc = (bool)e.ParametersInformation[4].Parameter.Value;

            #endregion

            #region SalesEmployee

            string paramSalesEmployee = string.Empty;
            string collSalesEmployee = (string)e.ParametersInformation[5].Parameter.Value;
            if (!string.IsNullOrEmpty(collSalesEmployee))
            {
                string[] list1 = collSalesEmployee.Split(',');
                foreach (string s in list1)
                {
                    paramSalesEmployee = string.IsNullOrEmpty(paramSalesEmployee) ? s : paramSalesEmployee + "," + s;
                }
            }

            #endregion

            #region ItemGroup

            string paramItemGroup = string.Empty;
            string collItemGroup = (string)e.ParametersInformation[6].Parameter.Value;
            if (!string.IsNullOrEmpty(collItemGroup))
            {
                string[] list1 = collItemGroup.Split(',');
                foreach (string s in list1)
                {
                    paramItemGroup = string.IsNullOrEmpty(paramItemGroup) ? s : paramItemGroup + "," + s;
                }
            }

            #endregion

            #region HideProductWith0Alloc

            ParameterHideProductWith0Alloc = (bool)e.ParametersInformation[7].Parameter.Value;

            #endregion

            #region ProductName

            string paramProductName = string.Empty;
            string collProductName = (string)e.ParametersInformation[8].Parameter.Value;
            if (!string.IsNullOrEmpty(collProductName))
            {
                string[] list1 = collProductName.Split(',');
                foreach (string s in list1)
                {
                    paramProductName = string.IsNullOrEmpty(paramProductName) ? s : paramProductName + "," + s;
                }
            }

            #endregion

            ParameterSalesDrive = string.IsNullOrEmpty(paramSalesDrive) ? null : paramSalesDrive;
            ParameterAnimation = string.IsNullOrEmpty(paramAnimation) ? null : paramAnimation;
            ParameterSalesArea = string.IsNullOrEmpty(paramSalesArea) ? null : paramSalesArea;
            ParameterCustomerGroup = string.IsNullOrEmpty(paramCustomerGroup) ? null : paramCustomerGroup;
            ParameterSalesEmployee = string.IsNullOrEmpty(paramSalesEmployee) ? null : paramSalesEmployee;
            ParameterItemGroup = string.IsNullOrEmpty(paramItemGroup) ? null : paramItemGroup;
            ParameterProductName = string.IsNullOrEmpty(paramProductName) ? null : paramProductName;
            rep_GroupAllocationReportTableAdapter.Fill(lorealDataSet1.rep_GroupAllocationReport, UserDivision.ToString(),
                ParameterSalesDrive,
                ParameterAnimation,
                ParameterSalesArea,
                ParameterCustomerGroup,
                ParameterHideCustomerWith0Alloc,
                ParameterSalesEmployee,
                ParameterItemGroup,
                ParameterHideProductWith0Alloc,
                ParameterProductName);
        }


















        void XtraReport1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            //xrPivotGrid1.BestFit(fieldProductName);
        }

    }
}
