using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace LorealReports.Reports
{
    public partial class XtraReport1 : DevExpress.XtraReports.UI.XtraReport
    {
        public XtraReport1()
        {
            InitializeComponent();
            BeforePrint += new System.Drawing.Printing.PrintEventHandler(XtraReport1_BeforePrint);
        }

        void XtraReport1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            //xrPivotGrid1.BestFit(fieldProductName);
        }

    }
}
