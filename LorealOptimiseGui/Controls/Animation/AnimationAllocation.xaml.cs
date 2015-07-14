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
using LorealOptimiseBusiness;
using LorealOptimiseData;
using LorealOptimiseData.Enums;

namespace LorealOptimiseGui.Controls
{
    /// <summary>
    /// Interaction logic for AnimationAllocation.xaml
    /// </summary>
    public partial class AnimationAllocation : BaseUserControl
    {
        public AnimationAllocation()
        {
            DataContext = AnimationManager.GetInstance();

            InitializeComponent();

            Loaded += new RoutedEventHandler(AnimationAllocation_Loaded);

        }

        void AnimationAllocation_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsVisible)
            {
                DataContext = AnimationManager.GetInstance();

                btnCopyFixedAllocation.Visibility = LoggedUser.GetInstance().IsInRole(RoleEnum.DivisionAdmin) ? Visibility.Visible : Visibility.Hidden;
            }
        }
    }
}
