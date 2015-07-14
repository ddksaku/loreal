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
using LorealOptimiseData;
using LorealOptimiseBusiness.Lists;

namespace LorealOptimiseGui.Lists
{
    /// <summary>
    /// Interaction logic for Lockouts.xaml
    /// </summary>
    public partial class Lockouts : UserControl
    {
        public Lockouts()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(Lockouts_Loaded);
        }

        public event EventHandler Close = null;

        void Lockouts_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsVisible)
            {
                dateStartDate.DateTime = DateTime.Now;
                txtStartTime.EditValue = DateTime.Now;
                dateEndDate.DateTime = dateStartDate.DateTime.AddHours(24);
                txtEndTime.EditValue = dateEndDate.DateTime;

                List<Lockout> upcomings = LockoutManager.Instance.GetExistingLockouts();
                lstFutureLockouts.ItemsSource = upcomings;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Lockout lockOut = new Lockout();

                DateTime startTime = DateTime.Parse(txtStartTime.EditValue.ToString());
                lockOut.Start = new DateTime(dateStartDate.DateTime.Year, dateStartDate.DateTime.Month, dateStartDate.DateTime.Day, startTime.Hour, startTime.Minute, startTime.Second);

                DateTime endTime = DateTime.Parse(txtEndTime.EditValue.ToString());
                lockOut.End = new DateTime(dateEndDate.DateTime.Year, dateEndDate.DateTime.Month, dateEndDate.DateTime.Day, endTime.Hour, endTime.Minute, endTime.Second);

                string errMsg;
                if (LockoutManager.Instance.CanAddLockout(lockOut, out errMsg))
                {
                    LockoutManager.Instance.InsertOrUpdate(lockOut);
                    MessageBox.Show(string.Format("New lock out schedule {0} has been added.", lockOut.ScheduleRange));
                }
                else
                {
                    if (MessageBox.Show(errMsg, "New lock out", MessageBoxButton.YesNo) == MessageBoxResult.No)
                    {
                        return;
                    }
                }

            }
            catch(Exception ex)
            {

            }
           

            if (Close != null)
                Close(this, new EventArgs());
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (Close != null)
                Close(this, new EventArgs());
        }
    }
}
