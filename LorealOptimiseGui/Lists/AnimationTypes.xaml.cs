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
using System.Windows.Shapes;
using LorealOptimiseBusiness.ViewMode;
using LorealOptimiseBusiness;
using DevExpress.XtraEditors.DXErrorProvider;
using DevExpress.Xpf.Grid;
using LorealOptimiseData;
using System.Collections.ObjectModel;
using LorealOptimiseBusiness.Lists;
using System.Collections;
using LorealOptimiseGui.Controls.StoresAndSales;
using LorealOptimiseData.Enums;

namespace LorealOptimiseGui.Lists
{
    /// <summary>
    /// Interaction logic for AnimationTypes.xaml
    /// </summary>
    public partial class AnimationTypes : BaseListUserControl<AnimationTypeManager, AnimationType>
    {
        public AnimationTypes()
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
                Loaded += new RoutedEventHandler(AnimationTypes_Loaded);
                AssignEvents(grdAnimationTypes);
            }
        }

        protected override Hashtable Filters
        {
            get 
            {
                return new Hashtable();
            }
        }

        void AnimationTypes_Loaded(object sender, RoutedEventArgs e)
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

        private void btnGenerate_Click(object sender, RoutedEventArgs e)
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
