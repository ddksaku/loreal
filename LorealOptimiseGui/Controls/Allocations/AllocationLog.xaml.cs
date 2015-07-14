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
using System.Windows.Shapes;

namespace LorealOptimiseGui.Controls
{
    /// <summary>
    /// Interaction logic for AllocationLog.xaml
    /// </summary>
    public partial class AllocationLog : Window
    {
        public AllocationLog()
        {
            InitializeComponent();
        }

        public void Bind(string text)
        {
            txtAllocationLog.Text = text;
        }
    }
}
