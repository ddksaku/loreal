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

namespace LorealOptimiseGui.Lists
{
    /// <summary>
    /// Interaction logic for CustomerBrandExclusions.xaml
    /// </summary>
    public partial class CustomerBrandExclusions : BaseListUserControl<CustomerBrandExclusionManager, CustomerBrandExclusion>
    {
        public CustomerBrandExclusions()
            : base()
        {
            InitializeComponent();

            if (DesignerProperties.GetIsInDesignMode(this) == false)
            {
                Loaded += new RoutedEventHandler(CustomerBrandExclusions_Loaded);
                AssignEvents(grdCustomerBrandExclusions, true);
            }
        }

        protected override Hashtable Filters
        {
            get 
            {
                return new Hashtable();
            }
        }

        void CustomerBrandExclusions_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsVisible)
            {
                cboCustomer.ItemsSource = CustomerManager.Instance.GetAll().OrderBy(c=>c.Name);
                cboBrandAxe.ItemsSource = BrandAxeManager.Instance.GetAll();
            }
        }

        private void TableView_ShowingEditor(object sender, ShowingEditorEventArgs e)
        {
            string columnName = e.Column.FieldName;
            if (columnName == "IDCustomer" || columnName == "IDBrandAxe")
            {
                if (e.RowHandle >= 0)
                {
                    e.Cancel = true;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
        }
    }
}
