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
using LorealOptimiseGui.Controls.StoresAndSales;
using LorealOptimiseData.Enums;

namespace LorealOptimiseGui.Lists
{
    /// <summary>
    /// Interaction logic for ItemTypes.xaml
    /// </summary>
    public partial class ItemTypes : BaseListUserControl<ItemTypeManager, ItemType>
    {
        public ItemTypes()
            : base()
        {
            InitializeComponent();

            if (LoggedUser.GetInstance().IsInRole(RoleEnum.DivisionAdmin) || LoggedUser.GetInstance().IsInRole(RoleEnum.SystemAdmin))
            {
                btnGenerate.IsEnabled = true;
            }
            else
            {
                btnGenerate.IsEnabled = false;
            }

            if (DesignerProperties.GetIsInDesignMode(this) == false)
            {
                Loaded += new RoutedEventHandler(ItemTypes_Loaded);
                AssignEvents(grdItemTypes);
            }
        }

        protected override Hashtable Filters
        {
            get 
            {
                return new Hashtable();
            }
        }

        void ItemTypes_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsVisible)
            {
                cboDivisions.ItemsSource = new Division[] { LoggedUser.LoggedDivision };
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        private void Generate_Click(object sender, RoutedEventArgs r)
        {
            AddCapacities generateCapacitiesCtrl = new AddCapacities();
            PopupWindow createProductDialog = new PopupWindow("Generating capacities");
            createProductDialog.Width = 950;
            createProductDialog.Height = 600;
            createProductDialog.AddControl(generateCapacitiesCtrl);
            createProductDialog.Show();
        }

    }
}
