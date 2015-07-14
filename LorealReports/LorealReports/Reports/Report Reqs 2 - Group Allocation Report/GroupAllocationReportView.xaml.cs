using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LorealReports.Reports
{
    /// <summary>
    /// Interaction logic for GroupAllocationReportView.xaml
    /// </summary>
    public partial class GroupAllocationReportView : UserControl
    {
        public GroupAllocationReportView()
        {
            InitializeComponent();
            DataContext = new GroupAllocationReportViewModel();
            //GroupAllocationReportOld rep = new GroupAllocationReportOld();
        }
    }
}
