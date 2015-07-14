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
using LorealOptimiseData.Enums;
using LorealOptimiseBusiness;
using DevExpress.Xpf.Grid;
using System.Collections;

namespace LorealOptimiseGui.Lists
{
    /// <summary>
    /// Interaction logic for SalesAreas.xaml
    /// </summary>
    public partial class SalesAreas : BaseListUserControl<SalesAreaManager, SalesArea>
    {
        public SalesAreas()
            : base()
        {
            InitializeComponent();
            if (DesignerProperties.GetIsInDesignMode(this) == false)
            {
                grdSalesAreas.View.PreviewKeyDown += new KeyEventHandler(View_PreviewKeyDown);
                Loaded += new RoutedEventHandler(SalesAreas_Loaded);
                AssignEvents(grdSalesAreas);
            }
        }

        void View_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (grdSalesAreas.View.IsEditing == false && e.Key == Key.Delete)
            {
                if (LoggedUser.GetInstance().IsInRole(RoleEnum.SystemAdmin) == false)
                {
                    e.Handled = true;
                }
            }
        }

        protected override Hashtable Filters
        {
            get 
            {
                return new Hashtable();
            }
        }

        void SalesAreas_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsVisible)
            {
                cboDivisions.ItemsSource = new Division[] { LoggedUser.LoggedDivision };
                cboDistributionChannels.ItemsSource = DistributionChannelManager.Instance.GetAll();
                cboSalesOrganizations.ItemsSource = SalesOrganizationManager.Instance.GetAll();

                if (LoggedUser.GetInstance().IsInRole(RoleEnum.SystemAdmin))
                {
                    grdSalesAreas.View.AllowEditing = true;
                    clmDivision.ReadOnly = false;
                    clmDisChannel.ReadOnly = false;
                    clmSalesOrg.ReadOnly = false;
                    clmCode.ReadOnly = false;
                    clmName.ReadOnly = false;

                }
                else
                {
                    (grdSalesAreas.View as TableView).NewItemRowPosition = NewItemRowPosition.None;
                    clmDivision.ReadOnly = true;
                    clmDisChannel.ReadOnly = true;
                    clmSalesOrg.ReadOnly = true;
                    clmCode.ReadOnly = true;
                    clmName.ReadOnly = true;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
        }
    }
}
