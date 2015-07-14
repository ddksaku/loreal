using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.Parameters;
using DevExpress.XtraReports.UI;
using System.Collections.Generic;


namespace LorealReports.Reports
{
    public partial class OrderFormReport : DevExpress.XtraReports.UI.XtraReport
    {
        public string ParameterSalesDrive { get; set; }
        public string ParameterAnimation { get; set; }
        public string ParameterSalesArea { get; set; }
        public string ParameterItemGroup { get; set; }

        public OrderFormReport()
        {
            InitializeComponent();
            ParametersRequestSubmit += OrderFormReport_ParametersRequestSubmit;
        }

        void OrderFormReport_ParametersRequestSubmit(object sender, ParametersRequestEventArgs e)
        {

            string paramSalesDrive = (string)e.ParametersInformation[0].Parameter.Value;

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

            string paramSalesArea = (string)e.ParametersInformation[2].Parameter.Value;

            string paramItemGroup = string.Empty;
            string collItemGroup = (string)e.ParametersInformation[3].Parameter.Value;
            string[] list2 = collItemGroup.Split(',');
            foreach (string s in list2)
            {
                paramItemGroup = string.IsNullOrEmpty(paramItemGroup) ? s : paramItemGroup + "," + s;
            }

            ParameterSalesDrive = string.IsNullOrEmpty(paramSalesDrive) ? null : paramSalesDrive;
            ParameterAnimation = string.IsNullOrEmpty(paramAnimation) ? null : paramAnimation;
            ParameterSalesArea = string.IsNullOrEmpty(paramSalesArea) ? null : paramSalesArea;
            ParameterItemGroup = string.IsNullOrEmpty(paramItemGroup) ? null : paramItemGroup;
            rep_OrderFormReportTableAdapter.Fill(lorealDataSet1.rep_OrderFormReport,
                                                 ParameterSalesDrive,
                                                 ParameterAnimation,
                                                 ParameterSalesArea,
                                                 ParameterItemGroup);
        }
    }
}
