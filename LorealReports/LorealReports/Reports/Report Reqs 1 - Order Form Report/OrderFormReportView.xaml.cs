using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DevExpress.Xpf.Printing;
using DevExpress.XtraPrinting;

using UserControl = System.Windows.Controls.UserControl;
using DevExpress.XtraReports.UI;

namespace LorealReports.Reports
{
    /// <summary>
    /// Interaction logic for OrderFormReport.xaml
    /// </summary>
    public partial class OrderFormReportView : UserControl
    {
        public OrderFormReportView()
        {
            InitializeComponent();
            DataContext = new OrderFormReportViewModel();
        }
    }
}
