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
using LorealOptimiseData.Enums;
using DevExpress.Xpf.Grid;

namespace LorealOptimiseGui.Controls
{
    /// <summary>
    /// Interaction logic for AllocationResult.xaml
    /// </summary>
    public partial class AllocationResult : BaseUserControl
    {
        private Guid? idAllocationLog;

        public AllocationResult()
        {
            InitializeComponent();
        }

        void btnOpenLog_Click(object sender, RoutedEventArgs e)
        {
            string text;

            if (idAllocationLog.HasValue)
            {
                List<allocationLog> logs = DbDataContext.GetInstance().allocationLogs.Where(a => a.LogID == idAllocationLog.Value).ToList();

                StringBuilder sb = new StringBuilder(32000);

                foreach (allocationLog allocationlog in logs)
                {
                    sb.AppendLine(allocationlog.LogText);
                }

                text = sb.ToString();
            }
            else
            {
                text = "Not allocation log exists";
            }

            AllocationLog window = new AllocationLog();
            window.Bind(text);
            window.Show();
        }

        public void Bind(IEnumerable<LorealOptimiseData.CustomerAllocation> animations, Guid? idAllocationLog)
        {
            this.idAllocationLog = idAllocationLog;

            if (LoggedUser.GetInstance().IsInRole(RoleEnum.SystemAdmin))
            {
                btnOpenLog.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                btnOpenLog.Visibility = System.Windows.Visibility.Hidden;
            }

            grdEventLog.DataSource = animations;

            grdEventLog.BeginDataUpdate();

            try
            {
                grdEventLog.GroupBy(clmGroup);
                grdEventLog.GroupBy(clmProduct);

                grdEventLog.SortInfo.Add(new GridSortInfo(clmGroup.FieldName));
                grdEventLog.SortInfo.Add(new GridSortInfo(clmProduct.FieldName));
                grdEventLog.SortInfo.Add(new GridSortInfo(clmAccountNumber.FieldName));
            }
            finally
            {
                grdEventLog.EndDataUpdate();
            }
        }
    }
}
