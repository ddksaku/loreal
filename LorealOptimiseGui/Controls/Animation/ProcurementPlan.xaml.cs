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
using LorealOptimiseBusiness;
using LorealOptimiseData;
using System.Collections;

namespace LorealOptimiseGui.Controls
{
    /// <summary>
    /// Interaction logic for ProcurementType.xaml
    /// </summary>
    public partial class ProcurementPlan : BaseUserControl
    {
        private AnimationManager animationManager = AnimationManager.GetInstance();

        public ProcurementPlan()
        {
            InitializeComponent();
            animationManager.PropertyChanged +=new PropertyChangedEventHandler(animationManager_PropertyChanged);

            if (DesignerProperties.GetIsInDesignMode(this) == false)
            {
                Loaded += new RoutedEventHandler(ProcurementType_Loaded);
            }
        }

        void animationManager_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "AnimationEntity")
            {
                grdProcurement.DataSource = animationManager.ProcurementPlan;
                animationManager.ProcurementPlan.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(ProcurementPlan_CollectionChanged);
            }
            else if (e.PropertyName == "ProcurementPlan")
            {
                grdProcurement.DataSource = animationManager.ProcurementPlan;
            }
        }

        void ProcurementType_Loaded(object sender, RoutedEventArgs e)
        {

        }

        void ProcurementPlan_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            ;
        }
    }
}
