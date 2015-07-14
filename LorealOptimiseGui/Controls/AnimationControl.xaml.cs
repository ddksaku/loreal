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
using System.Windows.Markup;
using LorealOptimiseBusiness;
using LorealOptimiseData;
using LorealOptimiseData.Enums;
using LorealOptimiseGui.Lists;
using LorealOptimiseBusiness.Lists;
using LorealOptimiseShared;
using LorealOptimiseGui.Enums;
using System.Collections;
using Microsoft.Reporting.WinForms;

namespace LorealOptimiseGui.Controls
{
    /// <summary>
    /// Interaction logic for Animation.xaml
    /// </summary>
    public partial class AnimationControl : BaseUserControl
    {
        AnimationManager manager = AnimationManager.GetInstance();
       
        public AnimationControl()
        {
            InitializeComponent();

            if (DesignerProperties.GetIsInDesignMode(this) == false)
            {
                Loaded += new RoutedEventHandler(AnimationControl_Loaded);
                Unloaded += new RoutedEventHandler(AnimationControl_Unloaded);
            }
        }

        void AnimationControl_Unloaded(object sender, RoutedEventArgs e)
        {
            manager.Animation = null;
        }

        private void btnAnimationReport_Click(object sender, RoutedEventArgs e)
        {
            ReportParameter[] p = new ReportParameter[3];

            ReportParameter rp = new ReportParameter("SalesDrive", manager.Animation.IDSalesDrive.ToString());
            ReportParameter rp2 = new ReportParameter("Animation", manager.Animation.ID.ToString());
            ReportParameter rp3 = new ReportParameter("DivisionID", LoggedUser.LoggedDivision.ID.ToString());

            p[0] = rp;
            p[1] = rp2;
            p[2] = rp3;

            Report reportCtrl = new Report(ReportType.AnimationReport, p);
            reportCtrl.Width = 800;
            reportCtrl.Height = 600;
            reportCtrl.Show();
        }

        private void OpenAllocation(object sender, EventArgs e)
        {
            tabAnimation.SelectedIndex = 3;
        }

        public void NewAnimation()
        {
            Animation newAnimation = new Animation();

            newAnimation.Status = (int)AnimationStatus.Open;
            newAnimation.RequestedDeliveryDate = null;
            newAnimation.PLVDeliveryDate = null;
            newAnimation.OnCounterDate = null;
            newAnimation.StockDate = null;
            newAnimation.PLVComponentDate = null;

            dateDefaultRequestedDeliveryDate.EditValue = null;
            dateDefaultRequestedDeliveryDate.Text = String.Empty;

            AnimationManager.GetInstance().Animation = newAnimation;
        }

        void AnimationControl_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = AnimationManager.GetInstance();

            AnimationManager.GetInstance().PropertyChanged += AnimationControl_PropertyChanged;

            ucProductDetail.OpenAllocations += OpenAllocation;

            if (!LoggedUser.GetInstance().IsInRole(RoleEnum.DivisionAdmin))
            {
                //rowDefUpperPart.Height = new GridLength(rowDefUpperPart.Height.Value - rowDefOrder.Height.Value);
                rowDefOrder.Height = new GridLength(0);
            }

            btnAnimationCheck.Visibility = ApplicationSettingManager.Instance.ShowValidationButton
                                               ? Visibility.Visible
                                               : Visibility.Hidden;

            if (manager.Animation != null)
            {
                ChangeDataEntryLocking((byte)manager.Animation.Status);
            }
            

            ucProductAnimation.DataContext = null;
            ucCustomers.DataContext = null;
            ucProductAnimation.DataContext = null;
            ucAnimationAllocation.DataContext = null;

            SalesDriveManager.Instance.Refresh();
            PriorityManager.Instance.Refresh();
            AnimationTypeManager.Instance.Refresh();
            DistributionChannelManager.Instance.Refresh();
            OrderTypeManager.Instance.Refresh();

            cmbSalesDrive.ItemsSource = SalesDriveManager.Instance.GetAll().OrderByDescending(sd => sd.YearAndName);
            cmbPriority.ItemsSource = PriorityManager.Instance.GetAll().OrderBy(p => p.Name);
            cmbAnimationType.ItemsSource = AnimationTypeManager.Instance.GetAll();
            cmbAnimationStatus.ItemsSource = Enum.GetNames(typeof(AnimationStatus));
            cmbDistributionChannel.ItemsSource = DistributionChannelManager.Instance.GetAll();
            cmbOrderType.ItemsSource = OrderTypeManager.Instance.GetAll();

            tabAnimation.SelectedIndex = 0;
        }


        void AnimationControl_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Animation")
            {
                if (manager.Animation != null)
                {
                    manager.Animation.PropertyChanged -= Animation_PropertyChanged;
                    manager.Animation.PropertyChanged += Animation_PropertyChanged;

                    if (manager.Animation.ID == Guid.Empty)
                        tabAnimation.Visibility = Visibility.Collapsed;
                    else
                        tabAnimation.Visibility = Visibility.Visible;

                    if (manager.Animation != null && manager.Animation.Status != null)
                    {
                        ChangeDataEntryLocking(manager.Animation.Status.Value);
                    }

                    if (AnimationManager.GetInstance().Animation.ID == Guid.Empty || (manager.Allocations == null && tabAnimation.SelectedIndex == 3))
                    {
                        tabAnimation.SelectedIndex = 0;
                    }
                }
            }
        }

        void Animation_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ID")
            {
                if (manager.Animation.ID != Guid.Empty)
                {
                    tabAnimation.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    tabAnimation.Visibility = System.Windows.Visibility.Hidden;
                }

            }
            else if (e.PropertyName == "Status")
            {
                if (manager.Animation != null && manager.Animation.Status != null)
                {
                    ChangeDataEntryLocking(manager.Animation.Status.Value);
                }
            }
        }

        void MakeControlsReadonly()
        {
            // Marketing users
            if (LoggedUser.GetInstance().IsInRole(RoleEnum.Marketing) || LoggedUser.GetInstance().IsInRole(RoleEnum.DivisionAdmin))
            {
                txtAnimationName.IsEnabled = true;
                cmbSalesDrive.IsEnabled = true;
                cmbPriority.IsEnabled = true;
                cmbAnimationType.IsEnabled = true;
                dateCounterDate.IsEnabled = true;
            }
            else
            {
                txtAnimationName.IsEnabled = false;
                cmbSalesDrive.IsEnabled = false;
                cmbPriority.IsEnabled = false;
                cmbAnimationType.IsEnabled = false;
                dateCounterDate.IsEnabled = false;
            }

            // Logistics users
            if (LoggedUser.GetInstance().IsInRole(RoleEnum.Logistics))
            {
                datePLVDeliveryDeadline.IsEnabled = true;
                datePLVComponentDeadline.IsEnabled = true;
                dateStockDeadline.IsEnabled = true;
            }
            else
            {
                datePLVDeliveryDeadline.IsEnabled = false;
                datePLVComponentDeadline.IsEnabled = false;
                dateStockDeadline.IsEnabled = false;
            }

            // Client care
            if (LoggedUser.GetInstance().IsInRole(RoleEnum.ClientCare) || LoggedUser.GetInstance().IsInRole(RoleEnum.DivisionAdmin))
            {
                txtAnimationCode.IsEnabled = true;
                txtSAPCode.IsEnabled = true;
                txtDefaultCustomerReference.IsEnabled = true;
            }
            else
            {
                txtAnimationCode.IsEnabled = false;
                txtSAPCode.IsEnabled = false;
                txtDefaultCustomerReference.IsEnabled = false;
            }

            // Div Admin
            if (LoggedUser.GetInstance().IsInRole(RoleEnum.DivisionAdmin))
            {
                cmbAnimationStatus.IsEnabled = true;
                dateDefaultRequestedDeliveryDate.IsEnabled = true;
            }
            else
            {
                cmbAnimationStatus.IsEnabled = false;
                dateDefaultRequestedDeliveryDate.IsEnabled = false;
            }
        }

        void ChangeDataEntryLocking(byte status)
        {
            switch ((AnimationStatus)status)
            {
                case AnimationStatus.Open:
                    txtAnimationName.IsEnabled = true;
                    txtAnimationCode.IsEnabled = true;
                    txtSAPCode.IsEnabled = true;
                    txtDefaultCustomerReference.IsEnabled = true;
                    cmbSalesDrive.IsEnabled = true;
                    cmbAnimationStatus.IsEnabled = true;
                    cmbPriority.IsEnabled = true;
                    cmbAnimationType.IsEnabled = true;
                    cmbOrderType.IsEnabled = true;
                    cmbDistributionChannel.IsEnabled = true;
                    dateDefaultRequestedDeliveryDate.IsEnabled = true;
                    dateCounterDate.IsEnabled = true;
                    datePLVDeliveryDeadline.IsEnabled = true;
                    datePLVComponentDeadline.IsEnabled = true;
                    dateStockDeadline.IsEnabled = true;
                    MakeControlsReadonly();
                    break;
                case AnimationStatus.Locked:
                case AnimationStatus.Draft:
                case AnimationStatus.Published:
                case AnimationStatus.Closed:
                case AnimationStatus.Cleared:
                    txtAnimationName.IsEnabled = false;
                    txtAnimationCode.IsEnabled = false;
                    txtSAPCode.IsEnabled = false;
                    txtDefaultCustomerReference.IsEnabled = false;
                    cmbSalesDrive.IsEnabled = false;
                    // cmbAnimationStatus.IsEnabled = false;
                    cmbDistributionChannel.IsEnabled = false;
                    cmbOrderType.IsEnabled = false;
                    cmbPriority.IsEnabled = false;
                    cmbAnimationType.IsEnabled = false;
                    dateDefaultRequestedDeliveryDate.IsEnabled = false;
                    dateCounterDate.IsEnabled = false;
                    datePLVDeliveryDeadline.IsEnabled = false;
                    datePLVComponentDeadline.IsEnabled = false;
                    dateStockDeadline.IsEnabled = false;
                    break;
            }
        }

        public void BindControl(Animation animation)
        {
            if (animation == null)
            {
                throw new ArgumentNullException("animation", "AnimationControl.BindControl -> Animation can not be null");
            }

            manager.Animation = animation;
        }

        #region Validation
        private void txtAnimationCode_Validate(object sender, DevExpress.Xpf.Editors.ValidationEventArgs e)
        {
            Animation currentAnimation = AnimationManager.GetInstance().Animation;
            if (e.Value != null && e.Value.ToString() != String.Empty && AnimationManager.GetInstance().AllAnimations.Any(ani => ani.ID != currentAnimation.ID && ani.Code == e.Value.ToString()))
            {
                e.SetError("There is already an animation with the same code");
            }
        }

        private void cmbAnimationStatus_Validate(object sender, DevExpress.Xpf.Editors.ValidationEventArgs e)
        {
            string errorMessage;
            if (manager.Animation != null && manager.Animation.ID != Guid.Empty && e.Value != null)
            {
                if (e.Value.ToString() != "Open")
                {
                    if (manager.Animation.IsValid(out errorMessage) == false)
                    {
                        e.SetError(errorMessage);
                        MessageBox.Show(errorMessage, "Validation error. Please correct following errors");
                    }
                }
            }
        }

        #endregion

        private void btnSave_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            // btn
        }

        private void Label_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            grdRow04.Height = grdRow04.Height.Value == 0 ? new GridLength(30) : new GridLength(0);
            grdRow05.Height = grdRow05.Height.Value == 0 ? new GridLength(30) : new GridLength(0);
            grdRow06.Height = grdRow06.Height.Value == 0 ? new GridLength(30) : new GridLength(0);
            grdRow07.Height = grdRow07.Height.Value == 0 ? new GridLength(30) : new GridLength(0);
            rowDefOrder.Height = rowDefOrder.Height.Value == 0 ? new GridLength(30) : new GridLength(0);

            lblShowHideDetails.Content = lblShowHideDetails.Content.ToString() == "Show Details" ? "Hide Details" : "Show Details";
        }

        private void cmbAnimationStatus_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            Animation ani = manager.Animation;

            // Only if an animation is loaded, visible and it is not the first setting of value. (OldValue)
            if (ani != null && ani.ID != Guid.Empty && e.OldValue != null && IsLoaded && IsVisible)
            {
                // check if SAP order is created
                if (ani.DateSAPOrderCreated != null && e.NewValue.ToString() == AnimationStatus.Closed.ToString())
                {
                    if (System.Windows.MessageBox.Show(SystemMessagesManager.Instance.GetMessage("CloseAnimationWithSAPOrder"), "", MessageBoxButton.YesNo) == MessageBoxResult.No)
                    {
                        if (e.OldValue != null)
                        {
                            cmbAnimationStatus.Text = e.OldValue.ToString();
                        }
                        else
                        {
                            cmbAnimationStatus.Text = AnimationStatus.Open.ToString();
                        }
                        return;
                    }
                }

                if (e.OldValue == null) return;

                if ((e.NewValue.ToString() == AnimationStatus.Draft.ToString() || e.NewValue.ToString() == AnimationStatus.Open.ToString() || e.NewValue.ToString() == AnimationStatus.Published.ToString() || e.NewValue.ToString() == AnimationStatus.Locked.ToString())
                    && (e.OldValue.ToString() == AnimationStatus.Cleared.ToString() || e.OldValue.ToString() == AnimationStatus.Closed.ToString()))
                {
                    if (manager.NeedToRecreateAllocation())
                    {
                        if (System.Windows.MessageBox.Show(SystemMessagesManager.Instance.GetMessage("ReactivateAnimation"), "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            manager.RecreateAllocations();
                            return;
                        }
                        else
                        {
                            cmbAnimationStatus.Text = e.OldValue.ToString();
                        }
                    }
                }

            }

        }

        private void cmbSalesDrive_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (manager.Animation != null && e.NewValue != null)
            {
                manager.Animation.CleanEntityRef("IDSalesDrive");
                manager.Animation.SalesDrive = DbDataContext.GetInstance().SalesDrives.Single(sd => sd.ID == (Guid)e.NewValue);
            }
        }

        private void cmbPriority_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (manager.Animation != null && e.NewValue != null && e.NewValue.Equals(e.OldValue) == false)
            {
                manager.Animation.CleanEntityRef("IDPriority");
                manager.Animation.Priority = DbDataContext.GetInstance().Priorities.Single(p => p.ID == (Guid)e.NewValue);
            }
        }

        private void cmbAnimationType_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (manager.Animation != null && e.NewValue != null && e.NewValue.Equals(e.OldValue) == false)
            {
                manager.Animation.CleanEntityRef("IDAnimationType");
                manager.Animation.AnimationType = DbDataContext.GetInstance().AnimationTypes.Single(at => at.ID == (Guid)e.NewValue);
            }
        }

        private void btnAnimationCheck_Click(object sender, RoutedEventArgs e)
        {
            int? countOfErrors = 0;
            string errorMessage = "";

            DbDataContext.GetInstance().up_animationCheck(manager.Animation.ID, ref countOfErrors, ref errorMessage);

            if (countOfErrors > 0)
            {
                MessageBox.Show(countOfErrors + " error/s \r\n " + errorMessage);
            }
            else
            {
                MessageBox.Show("No errors found.");
            }

        }

        private void dateCounterDate_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (e.OldValue != null && e.OldValue.Equals(e.NewValue) || e.NewValue == null)
                return;

            // default on counter date
            DateTime defaulOnCounterDate = Convert.ToDateTime(e.NewValue);
            double defaultRequestedDeliveryTiming = Convert.ToDouble(ApplicationSettingManager.Instance.GetAll().Where(c => c.IDDivision == LoggedUser.LoggedDivision.ID && c.SettingKey == "DefaultRequestedDeliveryTiming").FirstOrDefault().SettingValue);

            if (defaultRequestedDeliveryTiming != null && defaulOnCounterDate != DateTime.MinValue)
            {
                DateTime defaultRequestedDeliveryDate = defaulOnCounterDate.AddDays(-defaultRequestedDeliveryTiming);
                //dateDefaultRequestedDeliveryDate.EditValue = defaultRequestedDeliveryDate;
                manager.Animation.RequestedDeliveryDate = defaultRequestedDeliveryDate;
            }

            // defaul PLV delivery date
            double defaultPLVDeliveryTiming = Convert.ToDouble(ApplicationSettingManager.Instance.GetAll().Where(c => c.IDDivision == LoggedUser.LoggedDivision.ID && c.SettingKey == "DefaultPLVDeliveryTiming").FirstOrDefault().SettingValue);

            if (defaultPLVDeliveryTiming != null && defaulOnCounterDate != DateTime.MinValue)
            {
                DateTime defaultPLVDeliveryDate = defaulOnCounterDate.AddDays(-defaultPLVDeliveryTiming);
                //datePLVDeliveryDeadline.EditValue = defaultPLVDeliveryDate;
                manager.Animation.PLVDeliveryDate = defaultPLVDeliveryDate;
            }


            // default PLV component
            double defaultPLVComponentTiming = Convert.ToDouble(ApplicationSettingManager.Instance.GetAll().Where(c => c.IDDivision == LoggedUser.LoggedDivision.ID && c.SettingKey == "DefaultPLVComponentTiming").FirstOrDefault().SettingValue);

            if (defaultPLVComponentTiming != null && defaulOnCounterDate != DateTime.MinValue)
            {
                DateTime defaultPLVCompoenntDate = defaulOnCounterDate.AddDays(-defaultPLVComponentTiming);
                //datePLVComponentDeadline.EditValue = defaultPLVCompoenntDate;
                manager.Animation.PLVComponentDate = defaultPLVCompoenntDate;
            }


            // default Stock
            double defaultStockTiming = Convert.ToDouble(ApplicationSettingManager.Instance.GetAll().Where(c => c.IDDivision == LoggedUser.LoggedDivision.ID && c.SettingKey == "DefaultStockTiming").FirstOrDefault().SettingValue);

            if (defaultStockTiming != null && defaulOnCounterDate != DateTime.MinValue)
            {
                DateTime defaultStockDate = defaulOnCounterDate.AddDays(-defaultStockTiming);
                //dateStockDeadline.EditValue = defaultStockDate;
                manager.Animation.StockDate = defaultStockDate;
            }


            if (e.OldValue == null)
                return;

            foreach (AnimationCustomerGroup acg in manager.Animation.ObservableAnimationCustomerGroups.Where(acg => acg.IsOnCounterDateOverriden == false))
            {
                acg.OnCounterDate = (DateTime)e.NewValue;

            }
        }

        private void datePLVDeliveryDeadline_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (e.OldValue != null && e.OldValue.Equals(e.NewValue) || e.NewValue == null)
                return;

            foreach (AnimationCustomerGroup acg in manager.Animation.ObservableAnimationCustomerGroups.Where(acg => acg.IsPLVDeliveryDateOverriden == false))
            {
                acg.PLVDeliveryDate = (DateTime)e.NewValue;

            }
        }

        private void datePLVComponentDeadline_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (e.OldValue != null && e.OldValue.Equals(e.NewValue) || e.NewValue == null)
                return;

            foreach (AnimationCustomerGroup acg in manager.Animation.ObservableAnimationCustomerGroups.Where(acg => acg.IsPLVComponentDateOverriden == false))
            {
                acg.PLVComponentDate = (DateTime)e.NewValue;

            }
        }

        private void dateStockDeadline_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (e.OldValue != null && e.OldValue.Equals(e.NewValue) || e.NewValue == null)
                return;

            foreach (AnimationCustomerGroup acg in manager.Animation.ObservableAnimationCustomerGroups.Where(acg => acg.IsStockDateOverriden == false))
            {
                acg.StockDate = (DateTime)e.NewValue;

            }
        }

        private void txtSAPCode_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (e.OldValue != null && e.OldValue.Equals(e.NewValue) || e.NewValue == null)
                return;

            if (manager.Animation != null)
            {
                foreach (AnimationCustomerGroup acg in manager.Animation.ObservableAnimationCustomerGroups.Where(acg => acg.IsSapDespatchCodeOverriden == false))
                {
                    acg.SAPDespatchCode = e.NewValue.ToString();
                }
            }
        }
    }

    // bool to bool - f -> not(f)
    public class BTBConverter : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return !((bool)value);
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
