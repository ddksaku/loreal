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
using LorealOptimiseBusiness.Lists;
using LorealOptimiseData;
using LorealOptimiseBusiness;
using DevExpress.Xpf.Grid;
using System.Collections;
using LorealOptimiseData.Enums;

namespace LorealOptimiseGui.Lists
{
    /// <summary>
    /// Interaction logic for DistributionChannels.xaml
    /// </summary>
    public partial class DistributionChannels : BaseListUserControl<DistributionChannelManager, DistributionChannel>
    {
        public DistributionChannels()
            : base()
        {
            InitializeComponent();
            if (DesignerProperties.GetIsInDesignMode(this) == false)
            {
                AssignEvents(grdDistributionChannels);

                Loaded += new RoutedEventHandler(DistributionChannels_Loaded);
                grdDistributionChannels.View.PreviewKeyDown += new KeyEventHandler(View_PreviewKeyDown);
            }
        }

        void View_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (grdDistributionChannels.View.IsEditing == false && e.Key == Key.Delete)
            {
                if (LoggedUser.GetInstance().IsInRole(RoleEnum.SystemAdmin) == false)
                {
                    e.Handled = true;
                }
            }            
        }

        void DistributionChannels_Loaded(object sender, RoutedEventArgs e)
        {
            if (LoggedUser.GetInstance().IsInRole(RoleEnum.SystemAdmin))
            {
                (grdDistributionChannels.View as TableView).NewItemRowPosition = NewItemRowPosition.Top;
                grdDistributionChannels.View.AllowEditing = true;
            }
            else
            {
                (grdDistributionChannels.View as TableView).NewItemRowPosition = NewItemRowPosition.None;
                grdDistributionChannels.View.AllowEditing = false;
            }
        }

        protected override Hashtable Filters
        {
            get 
            {
                return new Hashtable();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
        }
    }
}
