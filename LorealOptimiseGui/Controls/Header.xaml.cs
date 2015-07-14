using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using LorealOptimiseData;
using LorealOptimiseGui.Lists;
using LorealOptimiseBusiness;

namespace LorealOptimiseGui.Controls
{
    /// <summary>
    /// Interaction logic for Header.xaml
    /// </summary>
    public partial class Header : BaseUserControl
    {
        public Header()
        {
            InitializeComponent();

            if (DesignerProperties.GetIsInDesignMode(this) == false)
            {
                lblName.Content = string.Format("{0} ({1}, {2})", LoggedUser.GetInstance().Name,
                                                LoggedUser.GetInstance().Division.Name, LoggedUser.GetInstance().Division.Code);
                lblName.ToolTip = LoggedUser.GetInstance().RoleNames;
            }
        }

        private void btnClearDateContext_Click(object sender, RoutedEventArgs e)
        {
            //BaseManager.ResetDataContext();
        }
    }
}
