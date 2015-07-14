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

namespace LorealOptimiseGui.Controls
{
    /// <summary>
    /// Interaction logic for AdministrationTabs.xaml
    /// </summary>
    public partial class AdministrationTabs : UserControl
    {
        Dictionary<TabItem, Control> tabs = new Dictionary<TabItem, Control>();

        public AdministrationTabs()
        {
            InitializeComponent();

            if (this.grdMain != null)
            {
                if (grdMain.Children != null)
                {
                    grdMain.Children.Clear();
                }
            }
        }

        public Control VisibleList
        {
            get
            {
                return visibleList;
            }
            set
            {
                if (visibleList != null)
                {
                    grdMain.Children.Remove(visibleList);
                }
                visibleList = value;
                grdMain.Children.Add(visibleList);
            }
        }

        private Control visibleList = null;
    }
}
