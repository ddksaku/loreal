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
using LorealOptimiseGui.Lists;
using LorealOptimiseBusiness.Lists;

namespace LorealOptimiseGui.Controls
{
    /// <summary>
    /// Interaction logic for Animation.xaml
    /// </summary>
    public partial class Animation : BaseUserControl
    {
        AnimationManager manager = new AnimationManager();
        public Animation()
        {
            InitializeComponent();

            cmbSalesDrive.ItemsSource = new SalesDriveManager().GetAll();
            cmbPriority.ItemsSource = new PriorityManager().GetAll();
            cmbAnimationType.ItemsSource = new AnimationTypeManager().GetAll();
            cmbAnimationStatus.ItemsSource = new int[3]{1,2,3};
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            LorealOptimiseData.Animation newAnimation = new LorealOptimiseData.Animation();
            
            newAnimation.Name = txtAnimationName.Text;
            newAnimation.IDSalesDrive = (Guid)cmbSalesDrive.SelectedValue;
            newAnimation.Code = txtAnimationCode.Text;
            newAnimation.SAPDespatchCode = txtSAPCode.Text;
            newAnimation.DefaultCustomerReference = txtDefaultCustomerReference.Text;
            newAnimation.Status = Convert.ToByte(cmbAnimationStatus.SelectedValue);
            newAnimation.RequestedDeliveryDate = dateDefaultRequestedDeliveryDate.DateTime;

            newAnimation.IDPriority = (Guid)cmbPriority.SelectedValue;
            newAnimation.IDAnimationType = (Guid)cmbAnimationType.SelectedValue;
            newAnimation.OnCounterDate = dateCounterDate.DateTime;
            newAnimation.PLVDeliveryDate = datePLVDeliveryDeadline.DateTime;
            newAnimation.PLVComponentDate = datePLVComponentDeadline.DateTime;
            newAnimation.StockDate = dateStockDeadline.DateTime;

            manager.InsertOrUpdate(newAnimation);

        }
    }
}
